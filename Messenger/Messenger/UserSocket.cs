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
            byte[] answer;
            string message;
            try
            {
                while (true)
                {
                    mainThreadToken.ThrowIfCancellationRequested();
                    br = _mainSocket.Receive(buffer);
                    message = Encoding.ASCII.GetString(buffer, 0, br);

                    answer = Encoding.ASCII.GetBytes(ExecuteCommand(message));

                    _mainSocket.Send(answer);
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
    }
}
