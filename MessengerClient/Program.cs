using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessengerClient
{
    internal class Program
    {
        static void Main(string[] args) // тестовая работа, клиента потом перепишу
        {
            Socket mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                mainSocket.Connect(IPAddress.Parse("127.0.0.1"), 8000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
