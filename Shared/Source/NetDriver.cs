using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

using System.Text;
using System.Text.Json;

using System.Net;
using System.Net.Sockets;

using System.Threading.Tasks;


using static Shared.Source.NetDriver.Packet;
using System.Runtime.CompilerServices;



namespace Shared.Source
{
    public static class NetConst
    {
        public static int PORT = 23891;
        public static IPAddress IP = IPAddress.Parse("127.0.0.1");
    }



    public static class NetDriver
    {
        public class Packet(Guid? id = null, Action<NetDriver.Packet>? callback = null)
        {
            public struct PacketFragment
            {
                public Guid packetID;
                public Int16 packetSize;
                public Int16 fragmentPosition;
                public Byte[] data;
            }

            public readonly List<PacketFragment> fragments = new(0);
            public readonly Guid packetID = id ?? Guid.NewGuid();
            public Int32 PacketSize { get { return fragments.Count * FragmentSize; } }
            public Int32 FragmentSize
            {
                get
                {
                    if (fragments.Count == 0) return 0;
                    return fragments[0].data.Length;
                }
            }
            public Action<Packet>? packetFinalized = callback;

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
            public static Packet ConvertToPacket(Byte[] rawData, Guid? id = null, ConnectionHandler? packetOwner = null)
            {
                var packet = new Packet(id, packetOwner != null ? packetOwner.OnPacketComplete : null);
                Int32 totalFragments = (Int32)Math.Ceiling(rawData.Length / (double)(128 * 1024));

                for (Int32 i = 0; i < totalFragments; i++)
                {
                    Int32 startOffset = i * (128 * 1024);
                    Int32 remainingBytes = rawData.Length - startOffset;
                    Int32 fragmentBytes = Math.Min((128 * 1024), remainingBytes);

                    Byte[] fragmentData = new Byte[fragmentBytes];
                    Buffer.BlockCopy(rawData, startOffset, fragmentData, 0, fragmentBytes);

                    var fragment = new Packet.PacketFragment
                    {
                        packetID = packet.packetID,
                        packetSize = (Int16)totalFragments,
                        fragmentPosition = (Int16)i,
                        data = fragmentData
                    };


                    packet.Append(fragment);
                }
                return packet;
            }

            public static Byte[] ConvertToBytes(Packet packet)
            {
                var resultData = new Byte[packet.PacketSize];
                Int32 currentOffset = 0;

                packet.fragments.Sort((a, b) => a.fragmentPosition.CompareTo(b.fragmentPosition));

                for (Int32 i = 0; i < packet.fragments.Count; i++)
                {
                    Buffer.BlockCopy(packet.fragments[i].data, 0, resultData, currentOffset, packet.FragmentSize);
                    currentOffset += packet.FragmentSize;
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

        public class ConnectionCoordinator(ConcurrentBag<NetDriver.ConnectionHandler> connections, Action<Byte[]> processor, ConnectionCoordinator.ConnectionMode mode)
        {
            public enum ConnectionMode
            {
                client,
                host,
            }

            private readonly ConnectionMode _currentMode = mode;
            private readonly ConcurrentBag<ConnectionHandler> _activeConnections = connections;
            private readonly Action<Byte[]> _dataProcessor = processor;

            public async Task StartAcceptingClients()
            {
                if (_currentMode == ConnectionMode.client) return;

                Socket listenerSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listenerSocket.Bind(new IPEndPoint(IPAddress.Any, NetConst.PORT));
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

                Socket serverConnection = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                await serverConnection.ConnectAsync(new IPEndPoint(NetConst.IP, NetConst.PORT));

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


        public class ConnectionHandler(Socket socket, Action<Byte[]> processor)
        {
            public readonly Socket socket = socket;
            public Task ReceiveTask;
            public readonly Byte[] receiveBuffer = new Byte[128 * 1024];

            public readonly List<Packet> pendingPackets = [];
            private readonly Action<Byte[]> _processData = processor;

            public void OnPacketComplete(Packet packet)
            {
                pendingPackets.Remove(packet);
                _processData(PacketBuilder.ConvertToBytes(packet));
            }
        }
    }
}