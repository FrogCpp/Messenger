using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
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
            string message;
            byte[] buffer = new byte[1024];
            int br = 0;
            while (true)
            {
                message = Console.ReadLine();

                mainSocket.Send(Encoding.ASCII.GetBytes(message));

                buffer = new byte[0];
                br = 1024;
                while (br == 1024)
                {
                    byte[] reader = new byte[1024];
                    br = mainSocket.Receive(reader);
                    byte[] bts = new byte[buffer.Length + reader.Length];
                    Buffer.BlockCopy(buffer, 0, bts, 0, buffer.Length);
                    Buffer.BlockCopy(reader, 0, bts, buffer.Length, reader.Length);
                    buffer = bts;
                }
                Console.WriteLine(Encoding.ASCII.GetString(buffer));
            }
        }
    }
}
