using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MessengerServer
{
    internal class MessageHandler
    {
        private Socket _listenerSocket;

        private ConcurrentQueue<UserTask> _taskList = null;
        private ConcurrentDictionary<string, User> _userList = null;

        public MessageHandler(ref ConcurrentQueue<UserTask> tsk, ref ConcurrentDictionary<string, User> usr)
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

                    var currentMessage = JsonSerializer.Deserialize<MessageSample>(Encoding.Unicode.GetString(buffer));

                    if (currentMessage.type == MessageType.sizeTransfer)
                    {
                        buffer = new byte[int.Parse(Encoding.Unicode.GetString(currentMessage.content))];
                        clientSocket.Send(new byte[] { 1 });
                    }
                    else
                    {
                        clientSocket.Send(new byte[] { 0 });
                    }

                    clientSocket.Receive(buffer);

                    currentMessage = JsonSerializer.Deserialize<MessageSample>(Encoding.Unicode.GetString(buffer));
                    User usr = new User();
                    usr.socket = clientSocket;
                    UserTask usrTsk = new UserTask(currentMessage, usr);

                    _taskList.Enqueue(usrTsk);
                }
                catch (SocketException)
                {
                    break;
                }
            }
        }
    }
}
