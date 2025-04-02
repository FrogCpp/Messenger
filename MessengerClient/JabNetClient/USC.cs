using System;


namespace JabNetClient
{
    internal class USC
    {
        /*  This is the file (mini library/ almost framework)
         *  Here I will write how the USC (universal server commands) will look like 
         * 
         * 
         *  1)  Authorisation request (AuthRequestUSC)
         *  
         *  Message for the server:  
         *                    AR0~Login~Password
         *               or   ARUID~Login~Password
         *               
         *  Components:
         *             AR   lastSavedUID   encryptedLogin   encryptedPassword
         *             
         *             (AR - authorisation request)
         *             (0  - the client program doesn't for sure know the user for the authorisation)
         *             [!] - Login and password are encrypted
         * 
         *    SavedUID - last remembered static UID of the logged in account
         *    
         *    
         *    
         *  2)  Auto authorisation request (or special auth request / AutoAuthRequestUSC)
         *  
         *  Message for the server:
         *                   
         *  
         *  
         */

        static public string CreateAuthRequest(string _encrLogin, string _encrPass, ulong _staticUID = 0)
        {
            //  Creating the usc,
            //  Currently pretty simple, probably will add more stuff later
            //  Although the simpler the request - the better
            //  (As long as we dont lose in security of course)
            //
            //  Создаём usc
            //  Очень простой запрос, скорее всего в будущем что-то добавиться
            //  Но при этом чем проще запрос тем лучше
            //  (конечно пока мы не начинаем терять в безопасности)
            string _usc = "AR~" + _staticUID.ToString() + "~" + _encrLogin + "~" + _encrPass;


            //  AR - authorisation request
            //
            //  StaticUID - needed for the faster auth check for the server
            //  With it the server only checks for one selected user
            //  Instead of checking login and password for all of them
            //  (it doesn't affect the encryption security, only the decryption speed)
            //  By setting staticUID to 0 - we search for a math in the login and password for multiple users
            //  
            //  Splitting the request by ~, bcs it is disallowed in the login, 
            //  So we can correctly distinquish the login from the password
            //
            //
            //  AR - Запрос на авторизацию
            //  staticUID - нужно для ускорения процесса проверки логина и пароля пользователя сервером
            //  С помощью него сервер проверяет логин и пароль на соответствие только у конкретного аккаунта
            //  Который указан под номером staticUID,
            //  вместо того чтобы проверять все зарегесрированные аккаунты в датасете
            //  (это не влияет на безопасность шифрования, только ускоряет процесс дешифрования для сервера)
            //
            //  Пока что разделяем запрос на части благодаря спец символу ~,
            //  потому что он запрещён в логине пользователя
            //  поэтому мы и можем корректно отличать логин от пароля



            //  Return the created usc
            //  Возвращаем созданный usc
            return _usc;
        }
             //  Create a USC for a standart authorisation request
             //  Создаём  usc для стандартного запроса на авторизацию

    }
}