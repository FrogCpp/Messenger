using System;

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

        static public void SendMessageToServer(string _uscMessage)
        {
           //  We just need to send a message to the server
           //  The message is fully ready, containing a usc - universal server comand
           //  
           //  Нам нужно просто отправить сообщение серверу
           //  Мы точно знаем что сообщение уже полностью готово
           //  (В сообщение уже включена usc - универсальная серверная команда)
        }
             //  Sending 1 message to the server
             //  Отправляем 1 сообщение серверу


        static public string ReceiveMessageFromServer()
        {
            //  We just need to receive the message from the server
            //  No decrypting, parsing, or whatever, just receive
            //
            //  Нам нужно просто получить сообщение от сервера
            //  Никаких дешифрований, парсингов соверщать не надо

            //
            //  Add the real code here
            //  This is just a placeholder
            //
            //  Добавь сюда настоящий код
            //  Это временный кусок куода

            return "lorem ipsum";
        }
             //  Receive 1 message from the server
             //  Получаем 1 сообщение от сервера
    }
}