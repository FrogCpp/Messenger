using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/*
 * как выглядит логика общения:
 * 1) клиент --> сервер (комманда)
 * 2) сервер --> клиент (результат выполнения комманды)
 * 3) повторить по новой
 * никак по другому
 */

namespace Messenger
{
    class UserSocket(Socket mainSocket)
    {
        private Socket _mainSocket = mainSocket;

        public void Communication(CancellationToken mainThreadToken)
        {
            int br = 0;
            byte[] buffer = new byte[1024];
            string answer;
            string message;
            try
            {
                while (true)
                {
                    mainThreadToken.ThrowIfCancellationRequested();
                    br = _mainSocket.Receive(buffer);
                    message = Encoding.ASCII.GetString(buffer, 0, br);
                    if (message == "Start")
                    {
                        _mainSocket.Send(Encoding.ASCII.GetBytes("Ready"));

                        byte[] ans = new byte[0];
                        int Br = 1024;
                        while (Br == 1024)
                        {
                            byte[] reader = new byte[1024];
                            Br = _mainSocket.Receive(reader);
                            byte[] bts = new byte[ans.Length + reader.Length];
                            Buffer.BlockCopy(ans, 0, bts, 0, ans.Length);
                            Buffer.BlockCopy(reader, 0, bts, ans.Length, reader.Length);
                            ans = bts;
                        }

                        answer = ExecuteCommand(Encoding.ASCII.GetString(ans));

                        _mainSocket.Send(Encoding.ASCII.GetBytes(answer));
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _mainSocket.Close();
            }
        }

        private string ExecuteCommand(string command)
        {
            return command;
        }

        public void Close()
        {
            _mainSocket.Close();
        }
    }
}
