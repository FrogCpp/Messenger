using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


//using System.Collections.Generic;       //  Will be necessary soon (probably)
//using System.Linq;                      //  Скоро понадобятся (наверное)
//using System.Security.Cryptography;     //
//using System.Threading.Tasks;           //

//using System.Net;                       //
//using System.Net.Sockets;               //
//using System.Text;                      //



namespace JabNetClient
{
    internal class ServerCommunication
    {
        private IPAddress serverAdress;
        private int serverPort;
        static private Socket myPersonalSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public ServerCommunication(IPAddress StaticIpAdreesForHost, int Port)
        {
            serverAdress = StaticIpAdreesForHost;
            serverPort = Port; 
        }
          
        static private void SendMessageToServer(string uscMessage)
        {
            byte[] message = Encoding.UTF32.GetBytes(uscMessage);
            int len = message.Length;

            myPersonalSocket.Send(Encoding.UTF32.GetBytes(len.ToString()));

            myPersonalSocket.Send(BitConverter.GetBytes(len));
        }

        static public void SendAbstract(string uscMessage)
        {
            //  Temporary
            //  Временно, пока ты нормально не сделаешь отправку
            SendMessageToServer(uscMessage);
        }


        public byte[] ReceiveMessageFromServer()
        {
            byte[] answer = new byte[myPersonalSocket.ReceiveBufferSize];
            return answer;
        }

        public string ExecuteCommand(string uscMessage)
        {
            SendMessageToServer(uscMessage);


            string response = Encoding.UTF32.GetString(ReceiveMessageFromServer());


            return response;
        }

        public void Start() // необходимо вызвать при создании
        {
            try
            {
                myPersonalSocket.Connect(serverAdress, serverPort);
            }
            catch (Exception e)
            {
                Console.WriteLine($"connection failed, reason: {e}");
            }
        }
    }
}