//using System;
//using System.Net;

using static System.Console;              //  Для удобства, чтобы не писать Console. каждый раз



//using static JabNetClient.DataManipulation;
//using static JabNetClient.InterfaceClasses;

//using static JabNetClient.FullTestLogic;
//using static JabNetClient.DrawInterface;
//using static JabNetClient.Authorisation;

using static JabNetClient.USC;
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
 *          //  Function logic
 *          //  Логика функции
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
 *          //  Function logic
 *          //  Логика функции
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

            //  Set the title of the console window
            //  Устанавливаем заголовок консольного окна
            Title = "JabNet Client pre-alpha";


            //  Set encoding type to unicode (Needed for the correct encryption by the RE system)
            //  Переключить кодировку программы на юникод (нужно для корректной шифровки с помощью РЕ)
            OutputEncoding = System.Text.Encoding.UTF8;


            //  Small flag for exiting the program
            //  Упрощённая логика для закрытия программы
            bool exit = false;


            //  uEK = unique encryption key in the RE system
            //  uEK = уникальный ключь шифрования в системе РЕ
            string uekRE;

            //  usID = temporary unique session ID for this client
            //  usID = временный уникальный ключь доступа этой сессии для клиента
            string usID = "";

            //  Static user id (stored on the server for easier communication between the server and the client)
            //  Статический идентификатор пользователя (хранится на сервере для упрощения общения между сервером и клиентом)
            ulong staticUID = 0;


            //   Settings for the RE key generation function
            //   Temporary placeholder,
            //   it will definetely change when I will finish the GenerateRandomSecureREkey function
            //
            //   Настройки для выбора случайного ключа шифрования
            //   Временно выглядит непонятно я их точно переделаю
            //   когда доделаю функцию GenerateRandomSecureREkey 
            bool parameters1 = true, parameters2 = true, parameters3 = true, parameters4 = gGenerateExtraUnicode;



            //  Generate a random secure RE key
            //
            //  Генерируем случайный надёжный ключ РЕ
            //  (Лёш эту функцию сделаю я)
            uekRE = GenerateRandomSecureREkey(gCipherVersion, parameters1, parameters2, parameters3, parameters4);


            //  Reset the settings to default parameters
            //  Сбросить настройки до начальных / системных
            ResetSettings();


            //  Update all the settings for the user chosen options
            //
            //  Обновляем все настройки на пользовательские
            //
            //  Алексей, я сам допишу эту функцию, она будет в файле GlobalSettings.cs
            ApplySettings();


            // это тестовая часть, ее потом снеси, ок?
            /*var a = new ServerCommunication(IPAddress.Parse("127.0.0.1"), 8000);
            a.Start();
            string msg;
            while (true)
            {
                msg = Console.ReadLine();
                Console.WriteLine(a.ExecuteCommand(msg));
            }*/



            ProgramTask currentTask = ProgramTask.None;

            int temporaryShift = 0;
            uekRE = "1234567890 йцукенгшщзхъфывапролджэячсмитьбюёЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮЁ";
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
                        ulong receiverUID = GetReceiverUID();


                        //  Our message to the chat partner
                        //  Наше сообщение для собеседника
                        string message = CreateMessage(uekRE);


                        //  Encrypting our message
                        //  Зашифровываем наше сообщение
                        string encryptedMessage = ERE4(message, uekRE, temporaryShift);

                        Write("\n\t\t[i]  - Зашифрованое сообщение: " + encryptedMessage);



                        //  Encrypting our unique session ID
                        //  Зашифровываем свой клю доступа
                        string encryptedusID = ERE4(usID, uekRE, temporaryShift);

                        Write("\n\t\t[i]  - Зашифрованый usID: " + encryptedusID);



                        //  Creating a usc request for sendind a message to a user
                        //  staticUID and usID used to confirm our identity
                        //
                        //  Создаём usc реквест на отправку сообщения конкретному пользователю
                        //  staticUID и usID используется для подтверждения нашей личности
                        string uscSendMessage = CreateSendMessageRequest(encryptedMessage, receiverUID, staticUID, encryptedusID);

                        Write("\n\t\t[i]  - USC: " + uscSendMessage);


                        Write("\n\t\t[!]  - Нажмите любую кнопку чтобы отправить сообщение ");
                        ReadKey();


                        
                        //  Temporary function to send the message
                        //  Времменная заглушка отвечающая за отправку сообщения
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
                *    >  Encrypted with the old uekRE key
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
                *    > Exchange uekRE
                *    
                *    > If the client has auto authorise enabled 
                *    {
                *         > Generate a new encryption key, and a new auth key
                *         >
                *         > Enrypt the auth key, and the encryption key with the uekRE
                *         > Send the enrypted details back to the client
                *         >
                *         > Save the new encryption key and auth key
                *         >
                *         
                *         < The client will receive the details
                *         < The client will decrypt them with the uekRE
                *         < The client will save the decrypted at the chosen path 
                *         < (pathtostoredencryptedaccountdetails)
                *    }     
                *}
            */
        }
    }
}
