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

        private ConcurrentDictionary<int, User> _userList = null;
        private TaskHandler _taskEx = null;

        public MessageCatcher(ref TaskHandler tskEx, ref ConcurrentDictionary<int, User> usr)
        {
            _listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listenerSocket.Bind(new IPEndPoint(IPAddress.Any, 18742));
            _listenerSocket.Listen(10);

            _taskEx = tskEx;
            _userList = usr;
        }


        public void AcceptClients()
        {
            while (true)
            {
                try
                {
                    Socket clientSocket = _listenerSocket.Accept();

                    MessageSample msg = new();

                    if (!CommunicationManagement.ReadMessage(clientSocket, out msg))
                    {
                        clientSocket.Close();
                    }

                    _taskEx.StartTask(clientSocket, msg);
                }
                catch (SocketException)
                {
                    break;
                }
            }
        }
    }
}
