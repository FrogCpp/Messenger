using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using AVcontrol;

namespace JabNet
{
    public enum TaskType //задачи для всего, что только есть, не стесняямся писать, чем больше - тем удобнее.
    {
        SEND_MESSAGE,
        UPDATE_CHAT,
        GET_FIRST_USR_KEY,
        SEND_SECOND_USR_KEY,
        GET_SECOND_USR_KEY,
        CREAT_CHAT,
        REMOVE_CHAT,
        MSG_LENGHT, // низкий уровень
    }

    public struct MessageSample()
    {
        public TaskType type;
        public byte[] content;
    }

    public class NetDriver
    {
        public static bool SendMessage(MessageSample msg, Socket socket)
        {
            byte[] msgB = Encoding.Unicode.GetBytes(JsonSerializer.Serialize<MessageSample>(msg));

            MessageSample a = new();
            a.type = TaskType.MSG_LENGHT;
            a.content = ToBinary.LittleEndian(msgB.Length);

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
            message = new();

            byte[] buffer = new byte[1024];

            socket.Receive(buffer);

            MessageSample a = JsonSerializer.Deserialize<MessageSample>(Encoding.Unicode.GetString(buffer));

            if (a.type != TaskType.MSG_LENGHT)
            {
                return false;
            }

            buffer = new byte[FromBinary.LittleEndian<int>(a.content)];

            socket.Receive(buffer);

            message = JsonSerializer.Deserialize<MessageSample>(Encoding.Unicode.GetString(buffer));

            return true;
        }
    }
}
