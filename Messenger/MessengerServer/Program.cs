using MessengerServer;
using System;
using System.Collections.Concurrent;

namespace MessengerServer
{
    internal class Program()
    {
        /*прим. для себя: здесь называем переменные так: _a - приватные; a - публичные; A - функции и классы*/

        public static readonly ConcurrentQueue<UserTask> TaskList = new();
        public static readonly ConcurrentDictionary<int, User> UserList = new();

        static void Main(string[] args)
        {

        }
    }

    struct User 
    {
        public int a;
    }
}