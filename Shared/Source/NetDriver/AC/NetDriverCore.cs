using Microsoft.Extensions.Logging;
using Shared.Source.tools;
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
        protected Func<Request, Task<byte[]?>>? processor;
        protected readonly ConcurrentDictionary<Guid, Request> _messageDict = new();
        protected readonly Channel<Request> _dispatchChannel = Channel.CreateUnbounded<Request>();
        protected readonly Channel<Request> _incomingChannel = Channel.CreateUnbounded<Request>();
        protected readonly ConcurrentBag<Task> _backgroundTasks = new();
        private readonly CancellationTokenSource _cts = new();

        public readonly string LOGFOLDER = "logs.txt";


        public async Task InitalizeNetDriver()
        {
            try
            {
                _backgroundTasks.Add(DispatchQueueController(_cts.Token));
                _backgroundTasks.Add(IncomingQueueController(_cts.Token));
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                DebugTool.Log(new DebugTool.log(DebugTool.log.Level.Error, ex.Message, LOGFOLDER));
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

                    if (sc.idSize != 16 || sc.contentSize > int.MaxValue)
                    {
                        continue;
                    }


                    var mainBuffer = new byte[sc.size + 4 + 4];
                    Buffer.BlockCopy(lenghtBuffer, 0, mainBuffer, 0, lenghtBuffer.Length);


                    read = 0;
                    while (read < mainBuffer.Length)
                    {
                        read += await sock.ReceiveAsync(mainBuffer.AsMemory(8 + read, mainBuffer.Length - (8 + read)));
                    }

                    var rq = new Request(new Message(mainBuffer), sock);
                    rq.appointment = Appointment.Read;



                    if (_messageDict.TryGetValue(rq.message.msgsuid, out var rqOut))
                    {
                        rqOut.GetAnswer(rq);
                    }
                    else
                    {
                        _incomingChannel.Writer.TryWrite(rq);
                    }
                }
            }
            catch (Exception ex)
            {
                DebugTool.Log(new DebugTool.log(DebugTool.log.Level.Error, ex.Message, LOGFOLDER));
                return ex;
            }
        }


        public async Task<Message?> SendReqMessageAsync(Socket sock, byte[] content)                 // ожидаем ответ
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));

            var tcs = new TaskCompletionSource<Request>();
            var rq = new Request(new Message(null, content), sock, tcs);
            if (!_messageDict.TryAdd(rq.message.msgsuid, rq))
            {
                DebugTool.Log(new DebugTool.log(DebugTool.log.Level.Error, "SendReqMessageAsync: can`t add message to dict", LOGFOLDER));
            }

            _dispatchChannel.Writer.TryWrite(rq);


            using (cts.Token.Register(() => tcs.TrySetCanceled()))
            {
                var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(-1, cts.Token));
                if (completedTask == tcs.Task)
                {
                    _messageDict.TryRemove(rq.message.msgsuid, out _);
                    return (await tcs.Task).message;
                }
                else
                {
                    _messageDict.TryRemove(rq.message.msgsuid, out _);
                    DebugTool.Log(new DebugTool.log(DebugTool.log.Level.Warning, "Response timeout", LOGFOLDER));
                    return null;
                }
            }
        }
        public void SendAnsMessageAsync(Socket sock, byte[] content, Guid msgsuid)                                // не ожидаем ответа
        {
            var rq = new Request(new Message(msgsuid, content), sock);

            _dispatchChannel.Writer.TryWrite(rq);
        }

        protected async Task DispatchQueueController(CancellationToken cancellationToken = default)
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
                    DebugTool.Log(new DebugTool.log(DebugTool.log.Level.Error, ex.Message, LOGFOLDER));
                }
            }
        }

        public async Task IncomingQueueController(CancellationToken cancellationToken = default)
        {
            var reader = _incomingChannel.Reader;

            await foreach (var req in reader.ReadAllAsync(cancellationToken))
            {
                try
                {
                    if (processor == null) continue;
                    var res = await processor(req);
                    if (res == null) continue;

                    SendAnsMessageAsync(req.socket, res, req.message.msgsuid);
                }
                catch (Exception ex)
                {
                    DebugTool.Log(new DebugTool.log(DebugTool.log.Level.Error, ex.Message, LOGFOLDER));
                }
            }
        }
    }
}
