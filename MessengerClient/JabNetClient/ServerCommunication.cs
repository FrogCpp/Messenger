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
        private IPAddress _serverAdress;
        private int _serverPort;
        private Socket _myPersonalSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public ServerCommunication(IPAddress StaticIpAdreesForHost, int Port)
        {
            _serverAdress = StaticIpAdreesForHost;
            _serverPort = Port; 
        }
          
        private void SendMessageToServer(string _uscMessage)
        {
            byte[] message = Encoding.UTF32.GetBytes(_uscMessage);
            int len = message.Length;

            _myPersonalSocket.Send(Encoding.UTF32.GetBytes(len.ToString()));

            _myPersonalSocket.Send(BitConverter.GetBytes(len));
        }


        public byte[] ReceiveMessageFromServer()
        {
            byte[] answer = new byte[_myPersonalSocket.ReceiveBufferSize];
            return answer;
        }

        public string ExecuteCommand(string _uscMessage)
        {
            SendMessageToServer(_uscMessage);


            string response = Encoding.UTF32.GetString(ReceiveMessageFromServer());


            return response;
        }

        public void Start() // необходимо вызвать при создании
        {
            try
            {
                _myPersonalSocket.Connect(_serverAdress, _serverPort);
            }
            catch (Exception e)
            {
                Console.WriteLine($"connection failed, reason: {e}");
            }
        }
    }
}