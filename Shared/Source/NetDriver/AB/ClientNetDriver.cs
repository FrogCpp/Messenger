using AVcontrol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Shared.Source.NetDriver.AB
{
    public class ClientContorlHub
    {
        private readonly IPAddress _domain;
        private readonly UInt16 _port;
        private readonly Socket _serverSock = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly List<Task> tasks = new();
        private readonly ConcurrentQueue<WorkItem> _sendRequests = new();
        private readonly Action<byte[]> _processor;

        public ClientContorlHub(IPAddress domain, UInt16 port, Action<byte[]> processor)
        {
            _domain = domain;
            _port = port;
            _processor = processor;

            _ = InitalizeAsyncFunc();
        }

        private async Task InitalizeAsyncFunc()
        {
            await _serverSock.ConnectAsync(new IPEndPoint(_domain, _port));
            tasks.Add(GlobalHandlerAsync());
        }

        private async Task<Exception> GlobalHandlerAsync()
        {
            try
            {
                while (true)
                {

                    if (_sendRequests.TryDequeue(out var request))
                    {
                        PackageFragment pack = new(request.content, PackageFragment.Types.Request);
                        await _serverSock.SendAsync(pack.Pack);

                        byte[] response = await ReceiveMessageAsync();
                        request.TCS.SetResult(response);
                    }
                    else
                    {
                        _processor(await ReceiveMessageAsync());
                    }
                }
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public async Task<byte[]> SendMessageAsync(byte[] content)
        {
            var tcs = new TaskCompletionSource<byte[]>();
            _sendRequests.Enqueue(new WorkItem(content, tcs));

            return await tcs.Task;
        }
        private struct WorkItem(byte[] Content, TaskCompletionSource<byte[]> tcs)
        {
            public readonly byte[] content = Content;
            public readonly TaskCompletionSource<byte[]> TCS = tcs;
        }

        private async Task<byte[]> ReceiveMessageAsync()
        {
            byte[] bufferF = new byte[4];
            await _serverSock.ReceiveAsync(bufferF);

            byte[] bufferS = new byte[FromBinary.LittleEndian<int>(bufferF)];
            await _serverSock.ReceiveAsync(bufferS);


            return bufferS;
        }
    }
}
