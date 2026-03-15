using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shared.Source.NetDriver.AC
{
    public class NetDriverCore
    {
        private readonly ConcurrentDictionary<Guid, Request> _messageDict = new();
        private readonly Channel<Request> _dispatchChannel = Channel.CreateUnbounded<Request>();
        private readonly List<Task> _backgroundTasks = new();
        private readonly CancellationTokenSource _cts = new();


        public async Task InitalizeNetDriver()
        {
            try
            {
                _backgroundTasks.Add(DispatchQueueController(_cts.Token));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Shutdown()
        {
            _cts.Cancel();
            _dispatchChannel.Writer.TryComplete();
        }



        public async Task<Exception> ListeningSocket(Socket sock)
        {
            try
            {
                while (true)
                {
                    var lenghtBuffer = new byte[8];
                    int read = 0;
                    while (read < lenghtBuffer.Length)
                    {
                        read += await sock.ReceiveAsync(lenghtBuffer.AsMemory(read, 8 - read));
                    }

                    var sc = Message.PartialParse(lenghtBuffer);

                    var mainBuffer = new byte[sc.size + 4 + 4];
                    Buffer.BlockCopy(lenghtBuffer, 0, mainBuffer, 0, lenghtBuffer.Length);


                    read = 0;
                    while (read < mainBuffer.Length)
                    {
                        read += await sock.ReceiveAsync(mainBuffer.AsMemory(read + 8, mainBuffer.Length - read));
                    }

                    var rq = new Request(new Message(mainBuffer), sock);
                    rq.appointment = Appointment.Read;



                    if (_messageDict.TryGetValue(rq.message.msgsuid, out var rqOut))
                    {
                        rqOut.GetAnswer(rq);
                    }
                    else if (!_messageDict.TryAdd(rq.message.msgsuid, rq))
                    {
                        throw new Exception("can`t add message to dict");
                    }
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        public async Task<Message> SendReqMessageAsync(Socket sock, byte[] content)                 // ожидаем ответ
        {
            var tcs = new TaskCompletionSource<Request>();
            var rq = new Request(new Message(Guid.NewGuid(), content), sock, tcs);
            if (!_messageDict.TryAdd(rq.message.msgsuid, rq))
            {
                throw new Exception("can`t add message to dict");
            }

            _dispatchChannel.Writer.TryWrite(rq);

            var msg = (await tcs.Task).message;
            if (!_messageDict.TryRemove(rq.message.msgsuid, out var a))
            {
                throw new Exception("can`t remove message from dict");
            }
            return msg;
        }
        public void SendAnsMessageAsync(Socket sock, byte[] content)                                // не ожидаем ответа
        {
            var rq = new Request(new Message(Guid.NewGuid(), content), sock);

            _dispatchChannel.Writer.TryWrite(rq);
        }

        public async Task DispatchQueueController(CancellationToken cancellationToken = default)
        {
            var reader = _dispatchChannel.Reader;

            await foreach (var req in reader.ReadAllAsync(cancellationToken))
            {
                try
                {
                    await req.socket.SendAsync(req.message.pack, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
