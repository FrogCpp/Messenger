using MessengerServer;
using System;
using System.Collections.Concurrent;
using NetworkDriver;
using System.Net.Sockets;
using System.Threading.Channels;
using System.Collections;

namespace MessengerServer
{
    internal class Program()
    {
        /*
         * прим. для себя: здесь называем переменные так: _a - приватные; a - публичные; A - функции и классы
         */

        public static ConcurrentDictionary<Guid, User> userList = new();
        public static Channel<TaskHandler.TaskSample> queue= null;
        public TaskHandler taskEx = new( userList, queue, 5);

        static void Main(string[] args)
        {
            queue = Channel.CreateUnbounded<TaskHandler.TaskSample>();

            var a = new MessageCatcher(userList, queue);
            var b = new TaskHandler(userList, queue, 100);
        }
    }

    struct User
    {
        public User()
        {
            Listner = Task.Run(async () =>
            {

            });
        }

        public Socket socket;

        public Task Listner;
    }
}