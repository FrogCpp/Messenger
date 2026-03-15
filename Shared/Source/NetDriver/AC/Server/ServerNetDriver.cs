using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Source.NetDriver.AC.Server
{
    public class ServerNetDriver : NetDriverCore
    {
        public readonly Socket socket = new(
            AddressFamily.InterNetwork, 
            SocketType.Stream, 
            ProtocolType.Tcp
        );

        private readonly ConcurrentDictionary<Socket, Task> Users = new();
        public ServerNetDriver(Func<Request, Task<byte[]?>> Processor)
        {
            processor = Processor;
            InitalizeNetDriver();


            socket.Bind(new IPEndPoint(IPAddress.Any, 121221));
            socket.Listen();

            _backgroundTasks.Add(AceptingConnections());
        }

        private async Task AceptingConnections()
        {
            while (true)
            {
                var clientConnection = await socket.AcceptAsync();
                Users.TryAdd(clientConnection, ListeningSocket(clientConnection));
            }
        }
    }
}
