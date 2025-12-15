using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using NetworkDriver;

namespace MessengerServer
{
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
