using System;
using System.Text;


using static System.Console;



namespace JabNetClient
{
    internal class MessagingLogic
    {

        static public string CreateMessage(string reKey)
        {
            string message = "";

            while (message == "" || message == null)
            {
                Clear();

                Write("\n\n\n\n");
                Write("\n\t\t[?]  - Какое сообщение вы хотите передать?");
                Write("\n\t\t[!]  - > Нельзя включать следующие спецсимволы: ~☺$");
                Write("\n\t\t       > Не должно быть пустым");
                Write("\n\t\t       > Сообщение должно быть валидным в utf-8\n");
                Write("\n\t\t     !!!!! Временно: можно использовать только русские букавы + цифры\n");

                Write("\n\t\t[->] - Сообщение: ");


                message = ReadLine().Trim();


                //  Basic logic for handling banned characters (temporary)
                //  Простая логика для избавления от спецсимволов (времменно)
                message = message.Replace("~", "").Replace("☺", "").Replace("$", "");


                //  Check the message for being UTF8 valid
                //  Проверка сообщения на соответствие кодировке UTF8
                if (!IsValidUTF8(message)) message = "";


                //  TEMPORARY checking for containing non supported reKey characters 
                //  ВРЕМЕННАЯ проверка на наличие символов не имеющихся в reKey
                //  - Это проверка на наличие не русский сиволов, потому что на данный момент reKey содержит только:
                //    "1234567890 йцукенгшщзхъфывапролджэячсмитьбюёЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮЁ"
                if (message != "" && message != null)
                {
                    for (Int32 i = 0; i < message.Length; i++)
                    {
                        if (reKey.IndexOf(message[i]) == -1)
                        {
                            i += message.Length;
                            message = "";
                        }
                    }
                }
            }

            //  Write the chosen message (temporary)
            //  Выводим выбранное сообщение (временно)
            Write("\n\t\t[i]  - Выбранное сообщение: " + message);
            return message;
        }


        static public bool IsValidUTF8(string input)
        {
            var encoder = Encoding.GetEncoding(
                "UTF-8",
                new EncoderExceptionFallback(),
                new DecoderExceptionFallback()
            );


            //  Try to encode the input string
            try
            {
                byte[] bytes = encoder.GetBytes(input);


                //  Encoding was successfull
                return true;
            }


            //  If utf-8 encoding failed - return error
            catch (Exception e)
            {
                Write("\n\t\t[!]  - Сообщение не валидно в UTF-8");
                Write("\n\t\t       Код ошибки: " + e);

                return false;
            }
        }
             //  Check a string for being valid in UTF-8
             //  Проверка строки на соответствие кодировке UTF-8


        static public UInt64 GetReceiverUID()
        {
            UInt64 chosenReceiverUID = 0;
            string userInput;

            while (chosenReceiverUID == 0)
            {
                Clear();

                Write("\n\n\n\n");
                Write("\n\t\t[->] - Введите статичный ID собеседника: ");

                userInput = ReadLine().Trim();
                

                //  Try parsing into a 64 bit number
                //  Пытаемся преобразовать в 64 битное число
                UInt64.TryParse(userInput, out chosenReceiverUID);
            }

            Write("\n\t\t[i]  - Выбранный ID собеседника: " + chosenReceiverUID);
            ReadKey();

            return chosenReceiverUID;
        }
             //  TEMPORARY logic for getting the chat partner ID
             //  ВРЕМЕННАЯ логика получения ID собеседника
    }
}