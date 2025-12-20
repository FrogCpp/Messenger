using NetworkDriver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace MessengerServer
{
    internal class MessageCatcher
    {
        private Socket _listenerSocket;

        private ConcurrentDictionary<Guid, User> _userList = null;
        private static Channel<TaskHandler.TaskSample> _queue = null;

        public MessageCatcher(ConcurrentDictionary<Guid, User> usr, Channel<TaskHandler.TaskSample> que)
        {
            _listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listenerSocket.Bind(new IPEndPoint(IPAddress.Any, 18742));
            _listenerSocket.Listen(10);

            _userList = usr;
            _queue = que;
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

                    var gd = Guid.NewGuid();
                    if (_userList.TryAdd(gd, new User())) {

                        _queue.Writer.WriteAsync(new TaskHandler.TaskSample(clientSocket, msg, gd));
                    }
                }
                catch (SocketException)
                {
                    break;
                }
            }
        }
    }
}
