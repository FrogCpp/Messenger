using MessengerServer;
using System;
using System.Collections.Concurrent;
using NetworkDriver;
using System.Net.Sockets;

namespace MessengerServer
{
    internal class Program()
    {
        /*
         * прим. для себя: здесь называем переменные так: _a - приватные; a - публичные; A - функции и классы
         */

        public static ConcurrentDictionary<int, User> UserList = new();
        public TaskHandler taskEx = new(ref UserList);

        static void Main(string[] args)
        {

        }
    }

    struct User 
    {
        public Socket socket;
    }
}