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

        public byte[] ReadMessage()
        {
            byte[] answer = new byte[0];
            int Br = 1024;
            while (Br == 1024)
            {
                byte[] reader = new byte[1024];
                Br = _mainSocket.Receive(reader);
                byte[] bts = new byte[answer.Length + reader.Length];
                Buffer.BlockCopy(answer, 0, bts, 0, answer.Length);
                Buffer.BlockCopy(reader, 0, bts, answer.Length, reader.Length);
                answer = bts;
            }

            return answer;
        }

        public void Reply(string message)
        {


            _mainSocket.Send(Encoding.UTF32.GetBytes(message));
        }

        public void Close()
        {
            _mainSocket.Close();
        }
    }
}
