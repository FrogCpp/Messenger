using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MessengerServer
{
    internal class CommunicationManagement
    {
        public static bool SendMessage(MessageSample msg, Socket socket)
        {
            byte[] msgB = Encoding.Unicode.GetBytes(JsonSerializer.Serialize<MessageSample>(msg));

            MessageSample a = new();
            a.type = TaskType.MSG_LENGHT;
            a.content = BitConverter.GetBytes(msgB.Length);

            socket.Send(Encoding.Unicode.GetBytes(JsonSerializer.Serialize<MessageSample>(a)));

            byte[] buffer = new byte[1];

            socket.Receive(buffer);

            if (buffer[0] == 0)
            {
                return false;
            }

            socket.Send(msgB);

            return true;
        }

        public static bool ReadMessage(Socket socket, out MessageSample message)
        {
            byte[] buffer = new byte[1024];

            socket.Receive(buffer);


        }
    }
}
