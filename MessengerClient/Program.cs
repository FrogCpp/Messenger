using System;


//using System.Collections.Generic;       //  Пока не надо
//using System.Linq;                      //
//using System.Security.Cryptography;     //
//using System.Threading.Tasks;           //
//
//using System.Net;                       //
//using System.Net.Sockets;               //
//using System.Text;                      //




//using System.IO;                        //  In the future for the settings storing
using System.Security.Cryptography;            //  For authentication purposes
using System.Numerics;                         //
using static System.Console;              //  Для удобства, чтобы не писать Console. каждый раз

using static JabNetClient.DrawInterface;
using static JabNetClient.CustomProcedures;
using static JabNetClient.CustomFunctions;
using static JabNetClient.GlobalSettings;
using static JabNetClient.GlobalClasses;
using static JabNetClient.Authorisation;
using static JabNetClient.REcipherSource;




/*
 * важно уточнить, перед работой клиента каждый раз надо прокидывать базовую конфигуряцию клиента
 * комманды для конфигурации:
 * setName(
 * задает имя пользователя на стороне сервера
 * )
 * 
 * setLogin(
 * задает логин на стороне сервера
 * )
 * 
 * setPasword(
 * задает пароль на стороне сервера
 * )
 * 
 * если на стороне сервера имеется аккаунт удовлетворяющий всем трем параметрам выше, то дальше работа идет с ним
 * 
 * getHistory: ***(
 *  *** - аргумент названия чата историю которого полуаешь
 * после определения клиента клиенту копируется история чатов. Подробнее о синтасисе передачи в функция этому посвещенных
 * )
 * комманды для работы:
 * sendMessage: ***(
 * *** - это аргумент (сама мессага) с фотками будет прикол а с видео и подавно,  ууу, готовлюсь :/
 * )
 * 
 * updateChat(
 * обновление чата, выщитывается чуть ли не постоянно, но работает только если чат на стороне сервера изменен
 * )
 */

namespace JabNetClient
{
    internal class Program
    {

        //  This function helps generating random 1024 usID
        public static BigInteger GenerateRandom1024BitInteger()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[128];
                rng.GetBytes(bytes);
                return new BigInteger(bytes);
            }
        }



        //  User task, so the client program understands what the user wants
        //  Also will be used for communication between the client and the server

        //  Действие пользователя, нужно для того чтобы прога клиента понимала какое окно показывать
        //  Также будет использоваться для упрощения комуникации между клиентом и серваком
        enum ProgramTask
        {
            //  Window task for the program to show the settings menu
            //  Оконная задача для программы для показа окна с изменением настроек
            BrowseSettings,

            //  Window task for the program to show the user profile menu
            //  Оконная задача для программы для показа окна профиля пользователя
            ShowProfile,

            //  Window and communication task for the client
            //  to change the users login and send it to the server
            //
            //  Оконная и коммуникативная задача для программы клиента,
            //  которая вызывает окно изменения логина,
            //  и отправляет соответствующую комманду серверу
            ChangeLogin,

            //  Window and communication task for the client
            //  to change the users password and send it to the server
            //
            //  Оконная и коммуникативная задача для программы клиента,
            //  которая вызывает окно изменения пароля,
            //  и отправляет соответствующую комманду серверу
            ChangePassword,

            //  Communication task for the client
            //  to request the server the contacts for this user
            //
            //  Задача для программы клиента,
            //  которая заставляет клиент просить у сервера контакты пользователя
            GetContacts,

            //  Communication task for the client
            //  to request the server the groups that the user is in
            //
            //  Задача для программы клиента,
            //  которая заставляет клиент просить у сервера группы в которых пользователь состоит
            GetGroups,

            //  Communication task for the client
            //  to request the server the history of a chosen chat
            //
            //  Задача для программы клиента,
            //  которая заставляет клиент просить у сервера историю конкретного выбранного чата
            GetHistory,

            //  Communication task for the client
            //  to send the server a message from the user, and update the chat
            //
            //  Задача для программы клиента,
            //  которая заставляет клиент отправить серверу сообщение пользователя в конкретный чат
            //  и затем чтобы сервер обновил историю
            SendMessage,

            //  Communication task for the client
            //  to send the server a picture from the user, and update the chat
            //
            //  Задача для программы клиента,
            //  которая заставляет клиент отправить серверу картинку пользователя в конкретный чат
            //  и затем чтобы сервер обновил историю
            //  
            //  The reason I separated this task from the "SendFile" is:
            //  because most of the time pictures a relatively small 
            //  or even if they are "big" we can compress them
            //  and use the same secure RE protocol to quickly encrypt them
            //
            //  Причина почему я разделил эту команду от "отправить файл":
            //  картинки в большинстве случаев довольно маленькие,
            //  а даже если нет мы их можем сжать
            //  и использовать тот же надёжный метод шифровки РЕ для их шифрования
            SendPicture,

            //  Communication task for the client
            //  to send the server a file from the user, and update the chat
            //
            //  Задача для программы клиента,
            //  которая заставляет клиент отправить серверу файл пользователя в конкретный чат
            //  и затем чтобы сервер обновил историю
            SendFile,
        }


        /*static void Main()
        {
            /
            //  Small flag for exiting the program
            //  Упрощённая логика для закрытия программы
            bool exit = false;

            while (!exit)
            {
                //  Set encoding type to unicode (Needed for the correct encryption by the RE system)
                //  Переключить кодировку программы на юникод (нужно для корректной шифровки с помощью РЕ)
                OutputEncoding = System.Text.Encoding.Unicode;


                //  Update all the global settings variables according to the chosen settings
                //
                //  for example gAutoAuthorise for auto authorisation
                //  and some gParameters for the RE keys
                //
                //
                //
                //  Обновляем все глобальные переменные настроек в соответствии с выбранными настройками
                //
                //  например gAutoAuthorise для автоматической авторизации
                //  и некоторые gParameters для ключей РЕ
                //
                //  Алексей, я сам напишу эту функцию, она будет в файле GlobalSettings.cs
                ApplySettings(path_for_stored_settings);



                //  uEK = unique encryption key in the RE system
                //  uEK = уникальный ключь шифрования в системе РЕ
                string uekRE;

                //  usID = temporary unique session ID for this client
                //  usID = временный уникальный ключь доступа этой сессии для клиента
                byte[] usID = new byte[1024];

                //  Static user id (stored on the server for easier communication between the server and the client)
                //  Статический идентификатор пользователя (хранится на сервере для упрощения общения между сервером и клиентом)
                ulong staticUID = 0;


                //   Settings for the RE key generation function
                //   Настройки для выбора случайного ключа шифрования
                bool parameters1 = true, parameters2 = true, parameters3 = true, parameters4 = gGenerateExtraUnicode;


                //   cipher version for the RE key
                //   версия шифра для ключа РЕ
                const byte cipherVersion = 4;


                //  Generate a random secure RE key  (Лёш эту функцию сделаю я)
                //  Генерируем случайный надёжный ключ РЕ
                uekRE = GenerateRandomSecureREkey(cipherVersion, parameters1, parameters2, parameters3, parameters4);

                //  Connect to server and exchange RE encryption key with the server
                //  Подключаемся к серверу и обмениваемся ключом шифрования РЕ с сервером
                //
                //  Эта функция для тебя,
                //  в отдельном файле будет механизм работы который нужно реализовать
                SecureConnectionToServer(uekRE);


                //  gAutoAuthorise = global setting for auto authorisation
                //  gAutoAuthorise = глобальная настройка для автоматической авторизации

                //  Try to auto authorise the client
                //  Пробуем автоматическую авторизацию
                if (gAutoAuthorise)
                {
                    usID = TryAutoAuthorisation(path_to_stored_encrypted_account_details);

                    //  If the auto auth fails, try the manual authorisation
                    //  Если авто авторизация не удастся, пробуем авторизоваться вручную
                    if(usID == null) usID = TryAuthorise(uekRE, ref staticUID);
                }

                //  try to authorise manualy
                //  Пробуем авторизироваться вручную
                else usID = TryAuthorise(uekRE, ref staticUID);


                //  If the authorisation was successful, do the client logic
                //  Если мы успешно вошли, программа начинает выполнять запросы пользователя
                while (staticUID != 0 && !exit)
                {
                    ProgramTask userTask = GetUserTask();

                    switch(userTask)
                    {
                        //doWork();
                    }

                    //ShowUI();
                }
            }

        */



            /*  TryAutoAuthorisation(string path) Explained:

                * After a normal successful auth
                * If a setting option (auto authorise) is set to true
                * The server will generate a random authorisation key, 
                * and a random RE encryption key
                * 
                * And send the client: the auth key, and the encryption key
                *    >  Encrypted with the old uekRE key
                *    
                * The client will store them both in a file at a chosen path(path_to_stored_encrypted_account_details)
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
                *         < (path_to_stored_encrypted_account_details)
                *    }     
                *}
            */
    }
}
