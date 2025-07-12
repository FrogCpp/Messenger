using System;

using static System.Console;


using static JabNetClient.GlobalVariables;



namespace JabNetClient
{
    internal class CustomFunctions
    {

        static public ProgramTask GetUserTask()
        {
            ProgramTask chosenTask = ProgramTask.None;


            while (chosenTask == ProgramTask.None)
            {
                Clear();
                Write("\n\n\n\n");
                Write("\n\t\t[?]  - Что вы хотите сделать?");
                Write("\n\t\t   > 1 <    - Отправить сообщение (тест)");
                Write("\n\t\t   > 2 <    - Получить все контакты (потом сделаю автоматом)");
                Write("\n\t\t   > 3 <    - Посмотреть конкретный чат (тесттест)\n");

                Write("\n\t\t   > 4 <    - Создать защищённый ключ");
                Write("\n\t\t   > 5 <    - Быстрый обмен новыми ключами");
                Write("\n\t\t   > 6 <    - Безопасный обмен новыми ключами\n");

                Write("\n\t\t   > 7 <    - Настройки");
                Write("\n\t\t   > 0 <    - Выход\n");

                Write("\n\t\t[->] Ваш выбор: ");


                switch (ReadLine().Trim())
                {
                    case "1": chosenTask = ProgramTask.SendMessage;       break;
                    case "2": chosenTask = ProgramTask.GetContacts;       break;
                    case "3": chosenTask = ProgramTask.ShowChat;          break;
                    case "4": chosenTask = ProgramTask.GenerateSecureKey; break;
                    case "5": chosenTask = ProgramTask.FastKeyExchange;   break;
                    case "6": chosenTask = ProgramTask.SecureKeyExchange; break;
                    case "7": chosenTask = ProgramTask.BrowseSettings;    break;
                    case "0": chosenTask = ProgramTask.Exit;              break;
                }
            }

            Write("\n\n\t\t[i]  - Выбрано: " + chosenTask);
            return chosenTask;
        }



        static public void RequestContacts(string reKey, string usID, UInt64 staticUID)
        {
            //if(CheckForAuthorisation())
            //{
            //string uscRequest = "";

            //  Create a universal server command to send it to the server
            //uscRequest = CreateContactsRequest(reKey, usID, staticUID);

            //SendMessageToServer(uscRequest);

            //List<string> userContacts = new List<string>();
            
            //string serverResponse = ReceiveMessageFromServer();


            //for (Int32 i = 0; i < serverResponse.Length; i++)
            //{
                //  Parse server response here
                //  From a string to either a list of strings 
                //  Or a custom class
                //
                //  Превращаем ответ сервера
                //  Из string либо в динамический массив string
                //  Либо в собственный класс
            //}

            //return userMessages;
            //}
            //return null;
        }


        static public void RequestGroups(string reKey, string usID, UInt64 staticUID)
        {
            //if(CheckForAuthorisation())
            //{
            //string uscRequest = "";

            //  Create a universal server command to send it to the server
            //uscRequest = CreateGroupsRequest(reKey, usID, staticUID);

            //SendMessageToServer(uscRequest);

            //List<string> userGroups = new List<string>();

            //string serverResponse;// = ReceiveMessageFromServer();


            //for (Int32 i = 0; i < serverResponse.Length; i++)
            //{
                //  Parse server response here
                //  From a string to either a list of strings 
                //  Or a custom class
                //
                //  Превращаем ответ сервера
                //  Из string либо в динамический массив string
                //  Либо в собственный класс
            //}

            //return userGroups;
            //}
            //return null;
        }


        static public void RequestHistory(string reKey, string usID, UInt64 staticUID, string selectedChat)
        {
            //if(CheckForAuthorisation())
            //{
                //string uscRequest = "";

                //  Create a universal server command to send it to the server
                //uscRequest = CreateHistoryRequest(reKey, usID, staticUID);

                //SendMessageToServer(uscRequest);

                //List<JabNetMessage> latestMessages = new List<JabNetMessage>();

            //string serverResponse;// = ReceiveMessageFromServer();


            //for(Int32 i = 0; i < serverResponse.Length; i++)
            //{
            //  Parse server response here
            //  From a string to the custom class JabNetMessage
            //
            //  Превращаем ответ сервера
            //  Из string либо в собственный класс
            //}

            //return latestMessages;
            //}
            //return null;
        }

    }
}