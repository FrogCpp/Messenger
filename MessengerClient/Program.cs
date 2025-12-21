using System;

using static System.Console;


using static JabNet.USC;
using static JabNetClient.CipherSource;

using static JabNetClient.MessagingLogic;
using static JabNetClient.CustomFunctions;

using static JabNetClient.GlobalSettings;
using static JabNetClient.GlobalVariables;

using static JabNetClient.ServerCommunication;





/*
 * И так, сейчас полностью напишу что требуется от тебя:
 *
 *
 *      static public void SendMessageToServer(string uscMessage)
 *      {
 *          ...
 *      }
 *      
 *      Для этой функции:
 *      - Точно будет входить один аргумент
 *      - Передаваемое сообщение полностью готово (зашифровано, и содержит USC)
 *      - Скорее всего функция не будет ничего возвращать (void)
 *      - Можно добавить ещё аргументы если необходимо
 *      
 *      
 *      
 *      
 *      static public string ReceiveMessageFromServer()
 *      {
 *          string receivedMessage;
 *          
 *          ...
 *      
 *          return receivedMessage;
 *      }
 *      
 *      Для этой функции:
 *      - Точно будет выплёвывать один аргумент (string)
 *      - Манипуляции с полученным сообщением делать не надо будет
 *      - Скорее всего функция не будет ничего принимать
 *      - Можно добавить ещё аргументы если необходимо
 *
 */

namespace JabNetClient
{
    internal class Program
    {
        static void Main()
        {
            Title = "JabNet Client pre-alpha";
            OutputEncoding = System.Text.Encoding.Unicode;

            bool exitFlag = false;


            //  reKey = unique RE encryption key
            string reKey;  //  temporary a string will later turn it into a struct

            //  usID = temporary unique session ID for this client
            //  usID = временный уникальный ключь доступа этой сессии для клиента
            string usID = "";

            //  Static user id (stored on the server for easier communication between the server and the client)
            //  Статический идентификатор пользователя (хранится на сервере для упрощения общения между сервером и клиентом)
            UInt64 staticUID = 0;


            //   Настройки для выбора случайного ключа шифрования
            bool parameters1 = true, parameters2 = true, parameters3 = true, parameters4 = gGenerateExtraUnicode;



            //  Generate a random secure RE key
            //
            //  Генерируем случайный надёжный ключ РЕ
            //  (Лёш эту функцию сделаю я)
            //reKey = GenerateRandomSecureREkey(gCipherVersion, parameters1, parameters2, parameters3, parameters4);



            //  Reload settings
            ResetSettings();
            ApplySettings();



            ProgramTask currentTask = ProgramTask.None;

            Int32 temporaryShift = 0;
            reKey = "1234567890 йцукенгшщзхъфывапролджэячсмитьбюёЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮЁ";
            usID = "35абоба";



            Write("\n\n\n\n");
            Write("\n\t\t[!]  - Нажми любую кнопку для начала демки ");


            while (currentTask != ProgramTask.Exit)
            {
                ReadKey();
                currentTask = GetUserTask();
                


                switch (currentTask)
                {
                    case ProgramTask.SendMessage:

                        //  The static UID of our chat partner
                        //  Статический UID нашего собеседника
                        UInt64 receiverUID = GetReceiverUID();


                        //  Console ui (temporary)
                        string message = CreateMessage(reKey);


                        //  Encrypting message
                        //  Зашифровываем наше сообщение
                        string encryptedMessage = ERE4(message, reKey, temporaryShift);

                        Write("\n\t\t[i]  - Зашифрованое сообщение: " + encryptedMessage);



                        //  Encrypting session ID
                        //  Зашифровываем свой ключ доступа
                        string encryptedusID = ERE4(usID, reKey, temporaryShift);

                        Write("\n\t\t[i]  - Зашифрованый usID: " + encryptedusID);



                        //  Creating a usc request for sendind a message to a user
                        //  staticUID and usID used to confirm our identity
                        //
                        //  Создаём usc реквест на отправку сообщения конкретному пользователю
                        //  staticUID и usID используется для подтверждения нашей личности
                        string uscSendMessage = "";
                        //SendMessageRequest(encryptedMessage, receiverUID, staticUID, encryptedusID);

                        Write("\n\t\t[i]  - USC: " + uscSendMessage);


                        Write("\n\t\t[!]  - Нажмите любую кнопку чтобы отправить сообщение ");
                        ReadKey();


                        
                        //  Temporary
                        SendAbstract(uscSendMessage);


                        break;


                    case ProgramTask.GetContacts:

                        break;


                    case ProgramTask.ShowChat:

                        break;
                }


            }


            /*  TryAutoAuthorisation(string path) Explained:

                * After a normal successful auth
                * If a setting option (auto authorise) is set to true
                * The server will generate a random authorisation key, 
                * and a random RE encryption key
                * 
                * And send the client: the auth key, and the encryption key
                *    >  Encrypted with the old reKey key
                *    
                * The client will store them both in a file at a chosen path(pathtostoredencryptedaccountdetails)
                * 
                * Then, after the program is closed and opened again
                * We will try to auto authorise:
                * 
                * Use the auto auth key to encrypt the auth key
                * Send the server the encrypted auth key
                * The server will try to decrypt it using the last specialy generated RE encryption key
                * 
                * If the decryption is successful 
                * {
                *    > Authorise the client
                *    > Exchange reKey
                *    
                *    > If the client has auto authorise enabled 
                *    {
                *         > Generate a new encryption key, and a new auth key
                *         >
                *         > Enrypt the auth key, and the encryption key with the reKey
                *         > Send the enrypted details back to the client
                *         >
                *         > Save the new encryption key and auth key
                *         >
                *         
                *         < The client will receive the details
                *         < The client will decrypt them with the reKey
                *         < The client will save the decrypted at the chosen path 
                *         < (pathtostoredencryptedaccountdetails)
                *    }     
                *}
            */
        }
    }
}
