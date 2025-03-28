using Messenger;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Console;

namespace CharTest_csharp
{
    internal class MainProgram
    {
        static void Main(string[] args)
        {
            //Server aboba = new Server(IPAddress.Parse("127.0.0.1"), 8000);
            //aboba.Start();
            //Console.ReadLine();
            //aboba.End();
            var a = new SQLiteUser();
            a.Start();
        }
    }
}