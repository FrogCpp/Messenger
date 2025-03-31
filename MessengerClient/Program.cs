﻿using System;





using static System.Console;              //  Для удобства, чтобы не писать Console. каждый раз

//using static JabNetClient.DrawInterface;
//using static JabNetClient.CustomProcedures;
using static JabNetClient.CustomFunctions;
using static JabNetClient.GlobalSettings;
using static JabNetClient.GlobalVariables;
using static JabNetClient.Authorisation;
using static JabNetClient.CipherSource;
//using static JabNetClient.DataManipulation;




/*
 * И так, сейчас полностью напишу что требуется от тебя:
 *
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
        static void Main()
        {
            //  Small flag for exiting the program
            //  Упрощённая логика для закрытия программы
            bool exit = false;


            //  Reset the settings to default parameters
            //  Сбросить настройки до начальных / системных
            ResetSettings();


            //  Update all the settings for the user chosen options
            //
            //  Обновляем все настройки на пользовательские
            //
            //  Алексей, я сам допишу эту функцию, она будет в файле GlobalSettings.cs
            ApplySettings();


            while (!exit)
            {
                //  Set encoding type to unicode (Needed for the correct encryption by the RE system)
                //  Переключить кодировку программы на юникод (нужно для корректной шифровки с помощью РЕ)
                OutputEncoding = System.Text.Encoding.Unicode;



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

                //  Connect to server and exchange RE encryption key with the server
                //  Подключаемся к серверу и обмениваемся ключом шифрования РЕ с сервером
                //
                //  Эта функция для тебя,
                //  в отдельном файле будет механизм работы который нужно реализовать
                SecureConnectionWithServer(uekRE);


                //  gAutoAuthorise = global setting for auto authorisation
                //  gAutoAuthorise = глобальная настройка для автоматической авторизации

                //  Try to auto authorise the client
                //  Пробуем автоматическую авторизацию
                if (gAutoAuthorise)
                {
                    //  Trying to automaticaly authorise the user (without any user input)
                    //  Saving the unique session ID,
                    //  if the authorisation fails, the function will return 0 to the static user ID
                    //
                    //  Пытаемся автоматически авторизовать пользователя (без какого-либо ввода с его стороны)
                    //  В результате получим уникальный ключ сессии,
                    //  если авторизация не удастся, функция вернёт 0 в статический ID пользователя
                    usID = TryAutoAuthorise(gPathForStoredAuthKey, ref staticUID);

                    //  If the auto auth fails, try to manually authorise the user
                    //  Если авто авторизация не удастся, пробуем авторизовать пользователя вручную
                    if (staticUID == 0) usID = TryAuthorise(uekRE, ref staticUID);
                }

                //  Try to authorise manually
                //  Пробуем авторизироваться вручную
                else usID = TryAuthorise(uekRE, ref staticUID);


                //  If the authorisation was successful, do the client logic
                //  Если мы успешно вошли, программа начинает выполнять запросы пользователя
                while (staticUID != 0 && !exit)
                {
                    //  This function will help the client program
                    //  To understand what the user currently wants
                    //
                    //  Эта фунция поможет программе клиента
                    //  Понять что хочет сделать пользователь в данный момент
                    //  (Её напишу я)
                    ProgramTask userTask = GetUserTask();

                    switch (userTask)
                    {
                        //doWork();
                    }

                    //ShowUI();
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
}
