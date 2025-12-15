using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MessengerServer
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
        MSG_LENGHT,
    }

    public struct MessageSample()
    {
        public TaskType type;
        public byte[] content;
    }

    internal class UserTask
    {
        public Socket socket { get; private set; }
        public TaskType taskType { get; private set; }
        public byte[] content { get; private set; }

        public UserTask(Socket sckt, TaskType tsk, byte[] cnt)
        {
            socket = sckt;
            taskType = tsk;
            content = cnt;
        }

        
    }
}
