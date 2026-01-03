using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using static JabNet.NetDriver.Packet;

namespace JabNet
{
    public static class NetDriver
    {
        public class Packet
        {
            public struct PacketFragment
            {
                public Guid packetID;
                public Int16 packetSize;
                public Int16 fragmentPosition;
                public byte[] data;
            }

            public readonly List<PacketFragment> fragments = new(0);
            public readonly Guid packetID;
            public int packetSize { get { return fragments.Count * fragmentSize; } }
            public int fragmentSize
            {
                get
                {
                    if (fragments.Count == 0) return 0;
                    return fragments[0].data.Length;
                }
            }
            public Action<Packet> packetFinalized;

            public Packet(Guid? id = null, Action<Packet> callback = null)
            {
                packetID = id ?? Guid.NewGuid();
                packetFinalized = callback;
            }

            public bool Append(PacketFragment fragment)
            {
                if (fragment.packetID != packetID) return false;
                if (fragment.packetSize == fragments.Count) return false;
                fragments.Add(fragment);

                if (packetFinalized != null && fragments.Count == fragment.packetSize) packetFinalized(this);

                return true;
            }
        }



        public static class PacketBuilder
        {
            public static Packet ConvertToPacket(byte[] rawData, Guid? id = null, ConnectionHandler? packetOwner = null)
            {
                var packet = new Packet(id, packetOwner != null ? packetOwner.OnPacketComplete : null);
                int totalFragments = (int)Math.Ceiling(rawData.Length / (double)(128 * 1024));

                for (int i = 0; i < totalFragments; i++)
                {
                    int startOffset = i * (128 * 1024);
                    int remainingBytes = rawData.Length - startOffset;
                    int fragmentBytes = Math.Min((128 * 1024), remainingBytes);

                    byte[] fragmentData = new byte[fragmentBytes];
                    Buffer.BlockCopy(rawData, startOffset, fragmentData, 0, fragmentBytes);

                    var fragment = new Packet.PacketFragment();

                    fragment.packetID = packet.packetID;
                    fragment.packetSize = (Int16)totalFragments;
                    fragment.fragmentPosition = (Int16)i;
                    fragment.data = fragmentData;


                    packet.Append(fragment);
                }
                return packet;
            }

            public static byte[] ConvertToBytes(Packet packet)
            {
                var resultData = new byte[packet.packetSize];
                int currentOffset = 0;

                packet.fragments.Sort((a, b) => a.fragmentPosition.CompareTo(b.fragmentPosition));

                for (int i = 0; i < packet.fragments.Count; i++)
                {
                    Buffer.BlockCopy(packet.fragments[i].data, 0, resultData, currentOffset, packet.fragmentSize);
                    currentOffset += packet.fragmentSize;
                }

                return resultData;
            }
        }




        internal static class TaskCoordinator
        {
            public static async Task MonitorTasks(List<Task> taskList, Func<Task, Task> taskHandler)
            {
                while (taskList.Count != 0)
                {
                    var finishedTask = Task.WhenAny(taskList);

                    await taskHandler(finishedTask);
                }
            }
        }

        public class ConnectionCoordinator
        {
            public enum ConnectionMode
            {
                client,
                host,
            }

            private readonly ConnectionMode _currentMode;
            private ConcurrentBag<ConnectionHandler> _activeConnections;
            private readonly Action<byte[]> _dataProcessor;

            public ConnectionCoordinator(ConcurrentBag<ConnectionHandler> connections, Action<byte[]> processor, ConnectionMode mode)
            {
                _currentMode = mode;
                _activeConnections = connections;
                _dataProcessor = processor;
            }

            public async Task StartAcceptingClients()
            {
                if (_currentMode == ConnectionMode.client) return;

                Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listenerSocket.Bind(new IPEndPoint(IPAddress.Any, 121221));
                listenerSocket.Listen();

                while (true)
                {
                    var clientConnection = await listenerSocket.AcceptAsync();

                    _activeConnections.Add(new ConnectionHandler(clientConnection, _dataProcessor));
                    var clientHandler = _activeConnections.First(s => s.socket == clientConnection);
                    clientHandler.ReceiveTask = clientConnection.ReceiveAsync(clientHandler.receiveBuffer);
                }
            }


            public async Task ConnectToServer()
            {
                if (_currentMode == ConnectionMode.host) return;

                Socket serverConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                await serverConnection.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 121221));

                _activeConnections.Add(new ConnectionHandler(serverConnection, _dataProcessor));
                var serverHandler = _activeConnections.First(s => s.socket == serverConnection);
                serverHandler.ReceiveTask = serverConnection.ReceiveAsync(serverHandler.receiveBuffer);
            }

            public async Task StartListening()
            {
                await TaskCoordinator.MonitorTasks(_activeConnections.Select(conn => conn.ReceiveTask).ToList(), HandleIncomingMessage);
            }

            public async Task HandleIncomingMessage(Task receiveOperation)
            {
                var connection = _activeConnections.First(t => t.ReceiveTask == receiveOperation);

                PacketFragment receivedFragment = JsonSerializer.Deserialize<PacketFragment>(Encoding.Unicode.GetString(connection.receiveBuffer));

                var existingPacket = connection.pendingPackets.SingleOrDefault(p => p.packetID == receivedFragment.packetID);
                if (existingPacket.Equals(default(PacketFragment)))
                {
                    var newPacket = new Packet();
                    newPacket.Append(receivedFragment);
                    connection.pendingPackets.Add(newPacket);
                }
                else
                {
                    existingPacket.Append(receivedFragment);
                }
            }
        }


        public class ConnectionHandler(Socket socket, Action<byte[]> processor)
        {
            public readonly Socket socket = socket;
            public Task ReceiveTask;
            public readonly byte[] receiveBuffer = new byte[128 * 1024];

            public readonly List<Packet> pendingPackets = new();
            private readonly Action<byte[]> _processData = processor;

            public void OnPacketComplete(Packet packet)
            {
                pendingPackets.Remove(packet);
                _processData(PacketBuilder.ConvertToBytes(packet));
            }
        }
    }
}