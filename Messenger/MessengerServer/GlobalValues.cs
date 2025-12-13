using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MessengerServer
{
    
    public struct User
    {
        public Socket socket;
        public bool activeTask;
    }

    public enum DurationType
    {
        session, // длительная связь в задаче
        single   // ограничено одним приветствием и прощанием
    }

    public enum MessageType
    {
        greetings,
        sizeTransfer,
        communication,
        parting
    }

    public struct MessageSample
    {
        public DurationType durationType;
        public MessageType type;
        public byte[] content;
    }
}
