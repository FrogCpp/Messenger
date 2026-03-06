using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Source.NetDriver.AB
{
    public class ClientContorlHub
    {
        private readonly IPAddress _domain;
        private readonly UInt16 _port;
        private readonly Socket _serverSock = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly List<Task> tasks = new();
        private readonly Action<byte[]> _processor;

        public ClientContorlHub(IPAddress domain, UInt16 port, Action<byte[]> processor)
        {
            _domain = domain;
            _port = port;
            _processor = processor;

            _ = InitalizeAsyncFunc();
        }

        private async void InitalizeAsyncFunc()
        {
            await _serverSock.ConnectAsync(new IPEndPoint(_domain, _port));
        }

        private async Task RecivingMessageAsync()
        {
            while (true)
            {

            }
        }

        private async Task SendMessageAsync(byte[] content)
        {

        }
    }
}
