using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using NetworkDriver;

namespace MessengerServer
{
    internal class MessageCatcher
    {
        private Socket _listenerSocket;

        private ConcurrentQueue<UserTask> _taskList = null;
        private ConcurrentDictionary<int, User> _userList = null;

        public MessageCatcher(ref ConcurrentQueue<UserTask> tsk, ref ConcurrentDictionary<int, User> usr)
        {
            _listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listenerSocket.Bind(new IPEndPoint(IPAddress.Any, 111111));
            _listenerSocket.Listen(10);


            _taskList = tsk;
            _userList = usr;
        }


        public void AcceptClients()
        {
            while (true)
            {
                try
                {
                    Socket clientSocket = _listenerSocket.Accept();

                    byte[] buffer = new byte[1024];
                    clientSocket.Receive(buffer);



                }
                catch (SocketException)
                {
                    break;
                }
            }
        }
    }
}
