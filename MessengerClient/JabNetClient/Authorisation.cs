using System.Dynamic;
using System.Numerics;
using System.Security.Cryptography;


using static JabNetClient.CipherSource;
using static JabNetClient.GlobalSettings;
using static JabNetClient.ServerCommunication;

namespace JabNetClient
{
    internal class Authorisation
    {
        static public byte[] TryAuthorise(string _uekRE, ref ulong _staticUID)
        {
            //  _uekRE - unique RE encryption key
            //  It is known both by the user and the client to this point
            //  It will be used to encrypt the send and recieved messages between the server and the client
            //
            //  _uekRE - уникальный ключ шифрования РЕ
            //  На данный момент его знает и клиент и сервер
            //  Он будет использоваться для шифрования полученных и отправленных сообщений между клиентом и сервером


            //  _staticUID - short unique user ID that is persistent (doesn't change)
            //  It will be send to the client by the server to allow for easier communication
            //  It will not affect the encryption proccess of the messages
            //
            //  _staticUID - короткий уникальный ID пользователя который статичный
            //  (никогда не меняется после создания аккаунта)
            //  Он поможет упростить коммуникацию между пользователем и клиентом
            //  Он не будет влиять на процесс шифрования сообщений




            //  Encrypt the user login
            //  Зашифровываем полученный логин пользователя
            string _encryptedLogin = Encrypt(gCipherVersion, _login, _uekRE);

            //  Encrypt the user password
            //  Зашифровываем полученный пароль пользователя
            string _encryptedPassword = Encrypt(gCipherVersion, _password, _uekRE);


            //  Creating a special auth request message to send to the server
            //  Создаём специальный запрос на авторизацию для отправки серверу
            //
            //  Лёш эту функцию я создам сам
            string _uscMessage = CreateAuthRequest(_uekRE, _cipherProperties, _login, _password);

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
            SendMessageToServer(_uscMessage);

            //  Receive server response to our auth request
            //  Получить ответ сервера на наш запрос на вход
            //
            //  Лёш эта функция тоже для тебя
            //  С шифрованиием мудрить не надо
            //  Просто получить сообщение от сервера
            string _encryptedResponse = ReceiveMessageFromServer();

            //  Split the server response into different parts
            //  For easier using of it
            //
            //  Разбиваем ответ сервера на куски в массив,
            //  для более удобной работы с ним
            string[] _serverResponse = Decrypt(_encryptedResponse).Split("~");

            //  If we don't receive "Denied"   <-- Whatever chosen "access denied" message from the server
            //  Если мы не получили "Denied"   <-- Смотря какую ключевую фразу мы придумаем для "вход запрещё"
            if(_serverResponse[0] != "Denied")
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
                if (_serverResponse.Length == 3)
                {

                    //  Try to parse the server message at ID[1]
                    //  to a ulong value for the _staticUID 
                    //
                    //  Пытаемся преобразовать строку под индексом[1] в массиве сообщений от сервера
                    //  В числовое значение ulong для _staticUID
                    if (ulong.TryParse(_serverResponse[1], out _staticUID))
                    {

                        //  Store the future unique session ID here
                        //  Храним будующий уникальный ID сессии здесь
                        byte[] _usID = new byte[128];


                        if (TryParseUSID(_serverResponse[2], ref _usID))
                        {
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
                            //  And we successfully got the _staticUID and the _usID
                            //  We return the _usID through return, and the _staticUID through 'ref'
                            //
                            //  Если авторизации окажется успешной
                            //  (и мы успешно получили _staticUID и _udID)
                            //  Мы возвращаем _usID через return, и _staticUID через 'ref'
                            return _usID;
                        }
                    }
                }

                //  If an unexpected error happens - reset the static uID to 0
                //  So the program doesn't think the login was successfull when in reality it wasn't
                //
                //  Если случится непредвиденная ошибка с парсингом ответа сервера
                //  Нужно сбросить статичный ID в 0
                //  Это необходимо для того, чтобы программа не получила ложно положительный результат входа
                //  (чтобы не было ситуации - вход выполнен небыл, а программа считает что вход удался)
                _staticUID = 0;

                //  Return authorisation error for the _usID
                //  Возвращаем ошибку авторизации для  _usID
                return null;
            }
            else return null;
        }
             //  File an authorisation request and send it to the server
             //  If successfull - return _staticUID and _usID
             //  Else - 0 and Null   respectively
             //  
             //  Функция для создания и отправки специального запроса на авторизацию для сервера
             //  В случае успеха функция вернёт - _staticUID и _usID
             //  Иначе - 0 и null    соотвественно


        static public byte[] TryAutoAuthorise(string _uekRE, ref ulong _staticUID)
        {
            //  _uekRE - unique RE encryption key
            //  It is known both by the user and the client to this point
            //  It will be used to encrypt the send and recieved messages between the server and the client
            //
            //  _uekRE - уникальный ключ шифрования РЕ
            //  На данный момент его знает и клиент и сервер
            //  Он будет использоваться для шифрования полученных и отправленных сообщений между клиентом и сервером


            //  _staticUID - short unique user ID that is persistent (doesn't change)
            //  It will be send to the client by the server to allow for easier communication
            //  It will not affect the encryption proccess of the messages
            //
            //  _staticUID - короткий уникальный ID пользователя который статичный
            //  (никогда не меняется после создания аккаунта)
            //  Он поможет упростить коммуникацию между пользователем и клиентом
            //  Он не будет влиять на процесс шифрования сообщений


            string[] _storedAuthKeys = ExtractAuthKey(gPathForStoredAuthKey);


            //  For encryption:
            //     _storedAuthKeys[0] = cipher version for encryption
            //     _storedAuthKeys[1] = actual authentication key
            //     _storedAuthKeys[2] = special encryption key for the auth request
            string _encryptedAuthKey = Encrypt(_storedAuthKeys[0], _storedAuthKeys[1], _storedAuthKeys[2]);
            

            //  For the usc message:
            //     _storedAuthKeys[4] = stores the staticUID for the server 
            //        (for a faster decryption and authentication check)
            //
            //  Для usc сообщения:
            //     _storedAuthKeys[4] = хранит staticUID для того
            //          чтобы серверу ускорить процесс аутентификации
            string _uscMessage = CreateAutoAuthRequest(_storedAuthKeys[4], _encryptedAuthKey, true);


            //  Send an AUTO auth request message to the server
            //
            //  Отправляем сообщение серверу - АВТОМАТИЧЕСКИЙ запрос на вход
            //
            //
            //  Лёш эта функция тебе - нужно просто отправить серваку сообщение (строку)
            //  Сообщение уже зашифровано и содержит всю необходимую информацию
            //
            //  В свою очередь сервер должен будет:
            //      > Его расшифровать (специальным ключом для авто верификации)
            //      > Проверить специальный ключь верификации
            //      > В случае неудачи отправить "Denied" ну или на что мы там договорились
            //      > В случае удачи:
            //            > Сгенерировать 1024 битное число - уникальный код сессии для авторизации пользователя
            //            > Зашифровать сообщение: "разрешено~статичныйUID~1024код"
            //             > Если у клиента в запросе была просьба о повторной генерации специального ключа
            //                   > Дополнительно добавить в сообщение серию для специального ключа
            //                       (и не забыть зашифровать)
            //            > Отправить зашифрованное сообщение пользователю
            SendMessageToServer(_uscMessage);


            //  Receive server response to our auth request
            //  Получить ответ сервера на наш запрос на вход
            //
            //  Лёш эта функция тоже для тебя
            //  С шифрованиием мудрить не надо
            //  Просто получить сообщение от сервера
            string _encryptedResponse = ReceiveMessageFromServer();


            //  Split the server response into different parts
            //  For easier using of it
            //
            //  Разбиваем ответ сервера на куски в массив,
            //  для более удобной работы с ним
            string[] _serverResponse = Decrypt(_encryptedResponse).Split("~");

            //  If we don't receive "Denied"   <-- Whatever chosen "access denied" message from the server
            //  Если мы не получили "Denied"   <-- Смотря какую ключевую фразу мы придумаем для "вход запрещё"
            if (_serverResponse[0] != "Denied")
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
                if (_serverResponse.Length == 4)
                {

                    //  Try to parse the server message at ID[1]
                    //  to a ulong value for the _staticUID 
                    //
                    //  Пытаемся преобразовать строку под индексом[1] в массиве сообщений от сервера
                    //  В числовое значение ulong для _staticUID
                    if (ulong.TryParse(_serverResponse[1], out _staticUID))
                    {

                        //  Store the future unique session ID here
                        //  Храним будующий уникальный ID сессии здесь
                        byte[] _usID = new byte[128];


                        if (TryParseUSID(_serverResponse[2], ref _usID))
                        {
                            if (CheckForNewAuthKey(_serverResponse[3]))
                            {
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
                                //  And we successfully got the _staticUID and the _usID
                                //  We return the _usID through return, and the _staticUID through 'ref'
                                //
                                //  Если авторизации окажется успешной
                                //  (и мы успешно получили _staticUID и _udID)
                                //  Мы возвращаем _usID через return, и _staticUID через 'ref'
                                return _usID;
                            }
                        }
                    }
                }

                //  If an unexpected error happens - reset the static uID to 0
                //  So the program doesn't think the login was successfull when in reality it wasn't
                //
                //  Если случится непредвиденная ошибка с парсингом ответа сервера
                //  Нужно сбросить статичный ID в 0
                //  Это необходимо для того, чтобы программа не получила ложно положительный результат входа
                //  (чтобы не было ситуации - вход выполнен небыл, а программа считает что вход удался)
                _staticUID = 0;

                //  Return authorisation error for the _usID
                //  Возвращаем ошибку авторизации для  _usID
                return null;
            }
            else return null;




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

            byte[] _usID = new byte[128];
            _staticUID = 0;

            // Do the logic here

            return null;
        }
             //  File an automatic authorisation request and send it to the server
             //  If successfull - return _staticUID and _usID
             //  Else - 0 and Null   respectively
             //  
             //  Функция для создания и отправки специального запроса на автоматическую авторизацию для сервера
             //  В случае успеха функция вернёт - _staticUID и _usID
             //  Иначе - 0 и null    соотвественно


        static public void SecureConnectionWithServer(string _uekRE)
        {

            //  Temporary placeholder for the enryption key generator
            //  Временный код для будущей генерации временного ключа шифрования
            string _temporaryEncryptionKey = GenerateRandomSecureREkey(gCipherVersion, true, true, true, false);


            //  Encrypting our unique RE encryption key (_uekRE)
            //  With the new generated temporary encryption key
            //
            //  Зашифровываем наш уникальный ключ шифрования (_uek)
            //  С помощью нового временного сгенерированного ключа шифрования
            string _ourEcryptedMessage = Encrypt(gCipherVersion, _uekRE, _temporaryEncryptionKey);


            //  Create a message for the connection request for the server
            //  Создаём сообщения для запроса на подключения
            string _connectionRequestClientEncrypted = CreateConnectionRequestMessage(_ourEcryptedMessage);


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
            SendMessageToServer(_connectionRequestClientEncrypted);


            //  Receive server response to our connection request
            //  Получить ответ сервера на наш запрос на безопасное подключение
            //
            //  Лёш эта функция тоже для тебя
            //  С шифрованиием мудрить не надо
            //  Просто получить сообщение от сервера
            string _serverResponseEncryptedTwice = ReceiveMessageFromServer();


            //  Decrypting the twice encrypted server response
            //    The servers encryption is kept,
            //    because we need it and also don't know how to remove it)
            //
            //  Дешифруем полученный ответ, который уже дважды зашифрован
            //    Шифрование сервера мы не снимаем
            //    Потому что оно нам нужно, а также потому что мы не знаем как его снять
            string _serverEncryptedConnection = Decrypt(gCipherVersion, _serverResponseEncryptedTwice, _temporaryEncryptionKey);


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
            SendMessageToServer(_serverEncryptedConnection);
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