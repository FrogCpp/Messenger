using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Source.NetDriver.AC
{
    public class NetDriverCore
    {
        private readonly ConcurrentDictionary<Socket, Message> _messageDict = new();

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
                    int offset = 256;                       // здесь выберем отступ, с которым будем читать пакеты
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


                    if (!_messageDict.TryAdd(sock, new Message(mainBuffer)))
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
    }
}
