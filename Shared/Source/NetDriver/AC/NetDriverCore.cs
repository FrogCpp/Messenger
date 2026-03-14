using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shared.Source.NetDriver.AC
{
    public class NetDriverCore
    {
        private readonly ConcurrentDictionary<Guid, Request> _messageDict = new();
        private readonly ConcurrentQueue<Request> _dispatchQueue = new();
        private readonly List<Task> _backgroundTasks = new();


        public async Task InitalizeNetDriver()
        {
            try
            {
                _backgroundTasks.Add(DispatchQueueController());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        public async Task<Exception> ListeningSocket(Socket sock)
        {
            try
            {
                while (true)
                {
                    var lenghtBuffer = new byte[8];
                    await sock.ReceiveAsync(lenghtBuffer);

                    var sc = Message.PartialParse(lenghtBuffer);

                    var mainBuffer = new byte[sc.size + 4 + 4];
                    Buffer.BlockCopy(lenghtBuffer, 0, mainBuffer, 0, lenghtBuffer.Length);
                    int offset = 1024;                       // здесь выберем отступ, с которым будем читать пакеты
                    while (sc.size % offset != 0)
                    {
                        offset--;
                    }
                    int step = 0;
                    while (step * offset < sc.size)
                    {
                        var smallBuffer = new byte[offset];
                        await sock.ReceiveAsync(smallBuffer);
                        Buffer.BlockCopy(smallBuffer, 0, mainBuffer, (step * offset) + 4 + 4, offset);
                        step++;
                    }

                    var rq = new Request(new Message(mainBuffer), sock);
                    rq.appointment = Appointment.Read;
                    if (_messageDict.TryGetValue(rq.message.msgsuid, out var rqOut))
                    {
                        rq.GetAnswer(rqOut);
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

            _dispatchQueue.Enqueue(rq);

            return (await tcs.Task).message;
        }
        public void SendAnsMessageAsync(Socket sock, byte[] content)                                // не ожидаем ответа
        {
            var rq = new Request(new Message(Guid.NewGuid(), content), sock);

            _dispatchQueue.Enqueue(rq);
        }

        public async Task DispatchQueueController()
        {
            while (true)
            {
                if (_dispatchQueue.TryDequeue(out var req))
                {
                    await req.socket.SendAsync(req.message.pack);
                }
            }
        }
    }
}
