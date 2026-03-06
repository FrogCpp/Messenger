using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

using System.Text;
using System.Text.Json;

using System.Net;
using System.Net.Sockets;

using System.Threading.Tasks;



namespace Shared.Source.NetDriver.AA
{
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

    public class ConnectionCoordinator(ConcurrentBag<ConnectionHandler> connections, Action<Byte[]> processor, ConnectionCoordinator.ConnectionMode mode)
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

            Socket serverConnection = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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

            Packet.PacketFragment receivedFragment = JsonSerializer.Deserialize<Packet.PacketFragment>(Encoding.Unicode.GetString(connection.receiveBuffer));

            var existingPacket = connection.pendingPackets.SingleOrDefault(p => p.packetID == receivedFragment.packetID);
            if (existingPacket.Equals(default(Packet.PacketFragment)))
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