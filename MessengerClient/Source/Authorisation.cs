﻿using System;
using System.Numerics;
using System.Security.Cryptography;


using static JabNetClient.CipherSource;
using static JabNetClient.GlobalSettings;


namespace JabNetClient
{
    internal class Authorisation
    {
        static public string TryAuthorise(string reKey, ref UInt64 staticUID)
        {
            //  reKey - unique RE encryption key
            //  It is known both by the user and the client to this point
            //  It will be used to encrypt the send and recieved messages between the server and the client
            //
            //  reKey - уникальный ключ шифрования РЕ
            //  На данный момент его знает и клиент и сервер
            //  Он будет использоваться для шифрования полученных и отправленных сообщений между клиентом и сервером


            //  staticUID - short unique user ID that is persistent (doesn't change)
            //  It will be send to the client by the server to allow for easier communication
            //  It will not affect the encryption proccess of the messages
            //
            //  staticUID - короткий уникальный ID пользователя который статичный
            //  (никогда не меняется после создания аккаунта)
            //  Он поможет упростить коммуникацию между пользователем и клиентом
            //  Он не будет влиять на процесс шифрования сообщений


            //  Get the inputed login for the future auth request
            //  Получаем логин (его вводит пользователь) для будующего запроса на авторизацию
            string login;// = GetLoginFromInput();


            //  Get the inputed password for the future auth request
            //  Получаем пароль (его вводит пользователь) для будующего запроса на авторизацию
            string password;// = GetPasswordFromInput();


            //  Encrypt the user login
            //  Зашифровываем полученный логин пользователя
            string encryptedLogin;// = Encrypt(gCipherVersion, login, reKey);

            //  Encrypt the user password
            //  Зашифровываем полученный пароль пользователя
            string encryptedPassword;// = Encrypt(gCipherVersion, password, reKey);


            //  Creating a special auth request message to send to the server
            //  Создаём специальный запрос на авторизацию для отправки серверу
            //
            //  Лёш эту функцию я создам сам
            string uscMessage;// = CreateAuthRequest(encryptedLogin, encryptedPassword, cipherProperties);

            //  Send an auth request message to the server
            //
            //  Отправляем сообщение серверу - запрос на вход
            //
            //
            //  Лёш эта функция тебе - нужно просто отправить серваку сообщение (строку)
            //  Сообщение уже зашифровано и содержит всю необходимую информацию
            //
            //  В свою очередь сервер должен будет:
            //      > Его расшифровать
            //      > Проверить логин и пароль
            //      > В случае неудачи отправить "Denied" ну или на что мы там договорились
            //      > В случае удачи:
            //            > Сгенерировать 1024 битное число - уникальный код сессии для авторизации пользователя
            //            > Зашифровать сообщение: "разрешено~статичныйUID~1024код"
            //            > Отправить зашифрованное сообщение пользователю
            //SendMessageToServer(uscMessage);

            //  Receive server response to our auth request
            //  Получить ответ сервера на наш запрос на вход
            //
            //  Лёш эта функция тоже для тебя
            //  С шифрованиием мудрить не надо
            //  Просто получить сообщение от сервера
            string encryptedResponse;// = ReceiveMessageFromServer();

            //  Split the server response into different parts
            //  For easier using of it
            //
            //  Разбиваем ответ сервера на куски в массив,
            //  для более удобной работы с ним
            string[] serverResponse = { "" };// = Decrypt(encryptedResponse).Split("~");

            //  If we don't receive "Denied"   <-- Whatever chosen "access denied" message from the server
            //  Если мы не получили "Denied"   <-- Смотря какую ключевую фразу мы придумаем для "вход запрещё"
            if(serverResponse[0] != "Denied")
            {

                //  If the server message array has a realistic length after splitting
                //  We are excpecting the server to send us either:
                //  "Denied"
                //  or "Allowed~staticUID~usID"
                //
                //  Если после разделения строки на массив у него реалистичная длина
                //  Мы ожидаем получить от сервера:
                //  либо "Denied"  (доступ запрещён)
                //  либо "Allowed~staticUID~usID"
                if (serverResponse.Length == 3)
                {

                    //  Try to parse the server message at ID[1]
                    //  to a UInt64 value for the staticUID 
                    //
                    //  Пытаемся преобразовать строку под индексом[1] в массиве сообщений от сервера
                    //  В числовое значение UInt64 для staticUID
                    if (UInt64.TryParse(serverResponse[1], out staticUID))
                    {

                        //  Store the future unique session ID here
                        //  Храним будующий уникальный ID сессии здесь
                        string usID = "";


                        //if (TryParseUSID(serverResponse[2], ref usID))
                        //{
                            //  TryParseUSID is a custom function that I will make
                            //  It will try to parse a string number to an array of 128 bytes
                            //  (Or a 1024 bit binary integer)
                            //
                            //  If the parsing fails, the function will return false = parsing error
                            //
                            //
                            //  TryParseUSID это не системная функция которую мне предстоит сделать
                            //  Она будет пытаться преобразовать строку в массив 128 байт
                            //  (Или в двоичное число состоящее из 1024 бит)
                            //
                            //  Если преобразование не удастся, функия вернёт false = ошибка преобразования




                            //  If the authorisation process was successfull
                            //  And we successfully got the staticUID and the usID
                            //  We return the usID through return, and the staticUID through 'ref'
                            //
                            //  Если авторизации окажется успешной
                            //  (и мы успешно получили staticUID и udID)
                            //  Мы возвращаем usID через return, и staticUID через 'ref'
                            //return usID;
                        //}
                    }
                }

                //  If an unexpected error happens - reset the static uID to 0
                //  So the program doesn't think the login was successfull when in reality it wasn't
                //
                //  Если случится непредвиденная ошибка с парсингом ответа сервера
                //  Нужно сбросить статичный ID в 0
                //  Это необходимо для того, чтобы программа не получила ложно положительный результат входа
                //  (чтобы не было ситуации - вход выполнен небыл, а программа считает что вход удался)
                staticUID = 0;

                //  Return authorisation error for the usID
                //  Возвращаем ошибку авторизации для  usID
                return null;
            }
            else return null;
        }
             //  File an authorisation request and send it to the server
             //  If successfull - return staticUID and usID
             //  Else - 0 and Null   respectively
             //  
             //  Функция для создания и отправки специального запроса на авторизацию для сервера
             //  В случае успеха функция вернёт - staticUID и usID
             //  Иначе - 0 и null    соотвественно


        static public string TryAutoAuthorise(string reKey, ref UInt64 staticUID)
        {
            ////  reKey - unique RE encryption key
            ////  It is known both by the user and the client to this point
            ////  It will be used to encrypt the send and recieved messages between the server and the client
            ////
            ////  reKey - уникальный ключ шифрования РЕ
            ////  На данный момент его знает и клиент и сервер
            ////  Он будет использоваться для шифрования полученных и отправленных сообщений между клиентом и сервером


            ////  staticUID - short unique user ID that is persistent (doesn't change)
            ////  It will be send to the client by the server to allow for easier communication
            ////  It will not affect the encryption proccess of the messages
            ////
            ////  staticUID - короткий уникальный ID пользователя который статичный
            ////  (никогда не меняется после создания аккаунта)
            ////  Он поможет упростить коммуникацию между пользователем и клиентом
            ////  Он не будет влиять на процесс шифрования сообщений


            //string[] storedAuthKeys = ExtractAuthKey(gPathForStoredAuthKey);


            ////  For encryption:
            ////     storedAuthKeys[0] = cipher version for encryption
            ////     storedAuthKeys[1] = actual authentication key
            ////     storedAuthKeys[2] = special encryption key for the auth request
            //string encryptedAuthKey = Encrypt(storedAuthKeys[0], storedAuthKeys[1], storedAuthKeys[2]);


            ////  For the usc message:
            ////     storedAuthKeys[4] = stores the staticUID for the server 
            ////        (for a faster decryption and authentication check)
            ////
            ////  Для usc сообщения:
            ////     storedAuthKeys[4] = хранит staticUID для того
            ////          чтобы серверу ускорить процесс аутентификации
            //string uscMessage = CreateAutoAuthRequest(storedAuthKeys[4], encryptedAuthKey, true);


            ////  Send an AUTO auth request message to the server
            ////
            ////  Отправляем сообщение серверу - АВТОМАТИЧЕСКИЙ запрос на вход
            ////
            ////
            ////  Лёш эта функция тебе - нужно просто отправить серваку сообщение (строку)
            ////  Сообщение уже зашифровано и содержит всю необходимую информацию
            ////
            ////  В свою очередь сервер должен будет:
            ////      > Его расшифровать (специальным ключом для авто верификации)
            ////      > Проверить специальный ключь верификации
            ////      > В случае неудачи отправить "Denied" ну или на что мы там договорились
            ////      > В случае удачи:
            ////            > Сгенерировать 1024 битное число - уникальный код сессии для авторизации пользователя
            ////            > Зашифровать сообщение: "разрешено~статичныйUID~1024код"
            ////             > Если у клиента в запросе была просьба о повторной генерации специального ключа
            ////                   > Дополнительно добавить в сообщение серию для специального ключа
            ////                       (и не забыть зашифровать)
            ////            > Отправить зашифрованное сообщение пользователю
            //SendMessageToServer(uscMessage);


            ////  Receive server response to our auth request
            ////  Получить ответ сервера на наш запрос на вход
            ////
            ////  Лёш эта функция тоже для тебя
            ////  С шифрованиием мудрить не надо
            ////  Просто получить сообщение от сервера
            //string encryptedResponse = ReceiveMessageFromServer();


            ////  Split the server response into different parts
            ////  For easier using of it
            ////
            ////  Разбиваем ответ сервера на куски в массив,
            ////  для более удобной работы с ним
            //string[] serverResponse = Decrypt(encryptedResponse).Split("~");

            ////  If we don't receive "Denied"   <-- Whatever chosen "access denied" message from the server
            ////  Если мы не получили "Denied"   <-- Смотря какую ключевую фразу мы придумаем для "вход запрещё"
            //if (serverResponse[0] != "Denied")
            //{

            //    //  If the server message array has a realistic length after splitting
            //    //  We are excpecting the server to send us either:
            //    //  "Denied"
            //    //  or "Allowed~staticUID~usID"
            //    //
            //    //  Если после разделения строки на массив у него реалистичная длина
            //    //  Мы ожидаем получить от сервера:
            //    //  либо "Denied"  (доступ запрещён)
            //    //  либо "Allowed~staticUID~usID"
            //    if (serverResponse.Length == 4)
            //    {

            //        //  Try to parse the server message at ID[1]
            //        //  to a UInt64 value for the staticUID 
            //        //
            //        //  Пытаемся преобразовать строку под индексом[1] в массиве сообщений от сервера
            //        //  В числовое значение UInt64 для staticUID
            //        if (UInt64.TryParse(serverResponse[1], out staticUID))
            //        {

            //            //  Store the future unique session ID here
            //            //  Храним будующий уникальный ID сессии здесь
            //            string usID = "";


            //            if (TryParseUSID(serverResponse[2], ref byte[] usID))
            //            {
            //                if (CheckForNewAuthKey(serverResponse[3]))
            //                {
            //                    //  TryParseUSID is a custom function that I will make
            //                    //  It will try to parse a string number to an array of 128 bytes
            //                    //  (Or a 1024 bit binary integer)
            //                    //
            //                    //  If the parsing fails, the function will return false = parsing error
            //                    //
            //                    //
            //                    //  TryParseUSID это не системная функция которую мне предстоит сделать
            //                    //  Она будет пытаться преобразовать строку в массив 128 байт
            //                    //  (Или в двоичное число состоящее из 1024 бит)
            //                    //
            //                    //  Если преобразование не удастся, функия вернёт false = ошибка преобразования




            //                    //  If the authorisation process was successfull
            //                    //  And we successfully got the staticUID and the usID
            //                    //  We return the usID through return, and the staticUID through 'ref'
            //                    //
            //                    //  Если авторизации окажется успешной
            //                    //  (и мы успешно получили staticUID и udID)
            //                    //  Мы возвращаем usID через return, и staticUID через 'ref'
            //                    return usID;
            //                }
            //            }
            //        }
            //    }

            //    //  If an unexpected error happens - reset the static uID to 0
            //    //  So the program doesn't think the login was successfull when in reality it wasn't
            //    //
            //    //  Если случится непредвиденная ошибка с парсингом ответа сервера
            //    //  Нужно сбросить статичный ID в 0
            //    //  Это необходимо для того, чтобы программа не получила ложно положительный результат входа
            //    //  (чтобы не было ситуации - вход выполнен небыл, а программа считает что вход удался)
            //    staticUID = 0;

                //  Return authorisation error for the usID
                //  Возвращаем ошибку авторизации для  usID
                return null;
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

            //byte[] usID = new byte[128];
            //staticUID = 0;

            // Do the logic here

            //return null;
        //}
             //  File an automatic authorisation request and send it to the server
             //  If successfull - return staticUID and usID
             //  Else - 0 and Null   respectively
             //  
             //  Функция для создания и отправки специального запроса на автоматическую авторизацию для сервера
             //  В случае успеха функция вернёт - staticUID и usID
             //  Иначе - 0 и null    соотвественно


        static public void SecureConnectionWithServer(string reKey)
        {

            //  Temporary placeholder for the enryption key generator
            //  Временный код для будущей генерации временного ключа шифрования
            string temporaryEncryptionKey = GenerateRandomSecureREkey(gCipherVersion, true, true, true, false);


            //  Encrypting our unique RE encryption key (reKey)
            //  With the new generated temporary encryption key
            //
            //  Зашифровываем наш уникальный ключ шифрования (uek)
            //  С помощью нового временного сгенерированного ключа шифрования
            string ourEcryptedMessage;// = Encrypt(gCipherVersion, reKey, temporaryEncryptionKey);


            //  Create a message for the connection request for the server
            //  Создаём сообщения для запроса на подключения
            string connectionRequestClientEncrypted;// = CreateConnectionRequestMessage(ourEcryptedMessage);


            //  Send an connection request message to the server
            //
            //  Отправляем сообщение серверу - запрос на безопасное соединение
            //
            //
            //  Лёш эта функция тебе - нужно просто отправить серваку сообщение (строку)
            //  Сообщение уже зашифровано и содержит всю необходимую информацию
            //
            //  В свою очередь сервер должен будет:
            //      > Сгенерировать случайный ключ шифрования
            //      > Зашифровать полученное сообщение
            //      > Отправить его обратно
            //SendMessageToServer(connectionRequestClientEncrypted);


            //  Receive server response to our connection request
            //  Получить ответ сервера на наш запрос на безопасное подключение
            //
            //  Лёш эта функция тоже для тебя
            //  С шифрованиием мудрить не надо
            //  Просто получить сообщение от сервера
            string serverResponseEncryptedTwice;// = ReceiveMessageFromServer();


            //  Decrypting the twice encrypted server response
            //    The servers encryption is kept,
            //    because we need it and also don't know how to remove it)
            //
            //  Дешифруем полученный ответ, который уже дважды зашифрован
            //    Шифрование сервера мы не снимаем
            //    Потому что оно нам нужно, а также потому что мы не знаем как его снять
            string serverEncryptedConnection;// = Decrypt(gCipherVersion, serverResponseEncryptedTwice, temporaryEncryptionKey);


            //  Send an connection request message to the server (final stage)
            //
            //  Отправляем сообщение серверу - запрос на безопасное соединение (финальная стадия)
            //
            //
            //  Лёш эта функция тебе - нужно просто отправить серваку сообщение (строку)
            //  Сообщение уже зашифровано и содержит всю необходимую информацию
            //
            //  В свою очередь сервер должен будет:
            //      > Вспомнить свой случайный ключ шифрования
            //      > Расшифровать полученное сообщение
            //      > Запомнить полученный в сообщении ключ шифрования
            //      (записав его в себе к таблицу - к соединениям - к этому устройству)
            //SendMessageToServer(serverEncryptedConnection);
        }
             //  Perform a cryptographycally safe key exchange with the server
             //  Криптографически надёжно обмениваемся ключом шифрования с сервером


        public static BigInteger GenerateRandom1024BitInteger()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[128];
                rng.GetBytes(bytes);
                return new BigInteger(bytes);
            }
        }
             //  This function helps generating random secure 1024 bit integers for the usID
             //  It will be used on the server side and in a modified way
             //  So this is just a placeholder
             //  
             //  Эта функция помогает генерировать случайные 1024 битные числа для usID
             //  Она скорее всего будет использоваться со стороны сервера
             //  Но даже тогда я уверен что она притерпит много изменений,
             //  так что этот код точно временный

        public static BigInteger GenerateRandom512BitInteger()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[64];
                rng.GetBytes(bytes);
                return new BigInteger(bytes);
            }
        }
             //  This function helps generating random secure 512 bit integers for the usID
             //  It will be used on the server side and in a modified way
             //  So this is just a placeholder
             //  
             //  Эта функция помогает генерировать случайные 512 битные числа для usID
             //  Она скорее всего будет использоваться со стороны сервера
             //  Но даже тогда я уверен что она притерпит много изменений,
             //  так что этот код точно временный

        public static BigInteger GenerateRandom256BitInteger()
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[32];
                rng.GetBytes(bytes);
                return new BigInteger(bytes);
            }
        }
             //  This function helps generating random secure 256 bit integers for the usID
             //  It will be used on the server side and in a modified way
             //  So this is just a placeholder
             //  
             //  Эта функция помогает генерировать случайные 256 битные числа для usID
             //  Она скорее всего будет использоваться со стороны сервера
             //  Но даже тогда я уверен что она притерпит много изменений,
             //  так что этот код точно временный
    }
}