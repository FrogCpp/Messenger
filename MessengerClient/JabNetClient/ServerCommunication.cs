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
            //  We just need to send a message to the server
            //  The message is fully ready, containing a usc - universal server comand
            //  
            //  Нам нужно просто отправить сообщение серверу
            //  Мы точно знаем что сообщение уже полностью готово
            //  (В сообщение уже включена usc - универсальная серверная команда)
            _myPersonalSocket.Send(Encoding.ASCII.GetBytes(_uscMessage)); // магия -- вещь удивительно крохотная XD

        }


        private byte[] ReceiveMessageFromServer()
        {
            //  We just need to receive the message from the server
            //  No decrypting, parsing, or whatever, just receive
            //
            //  Нам нужно просто получить сообщение от сервера
            //  Никаких дешифрований, парсингов соверщать не надо

            byte[] answer = new byte[0]; // не спрашивай, просто так надо
            int Br = 1024;
            while (Br == 1024)
            {
                byte[] reader = new byte[1024];
                Br = _myPersonalSocket.Receive(reader);
                byte[] bts = new byte[answer.Length + reader.Length];
                Buffer.BlockCopy(answer, 0, bts, 0, answer.Length);
                Buffer.BlockCopy(reader, 0, bts, answer.Length, reader.Length);
                answer = bts;
            }

            return answer;
        }

        public string ExecuteCommand(string _uscMessage)
        {
            SendMessageToServer(_uscMessage);

            string response = Encoding.ASCII.GetString(ReceiveMessageFromServer());
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