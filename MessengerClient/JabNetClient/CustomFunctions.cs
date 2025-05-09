//using System;
using System.Collections.Generic;

using static System.Console;


using static JabNetClient.GlobalVariables;
using static JabNetClient.InterfaceClasses;



namespace JabNetClient
{
    internal class CustomFunctions
    {

        static public ProgramTask GetUserTask()
        {
            
            //  Store the chosen task here
            //  Храним выбранную задачу здесь
            ProgramTask chosenTask = ProgramTask.None;


            while (chosenTask == ProgramTask.None)
            {
                Clear();

                //  Write all possible options (test)
                //  Выводим все возможные варианты (временно так мало)
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

                //  User input for the console interface
                //  Ввод пользователя для консольного интерфейса
                string userInput = ReadLine().Trim();


                //  Parse user input
                //  Transform it into a program task
                //  exit the loop
                //
                //  Прочитать ввод пользователя
                //  Преобразовать его в программную задачу
                //  Выйти из цикла
                switch (userInput)
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
            
            //  Return the chosen program task
            //  Возвращаем выбранную задачу для программы
            return chosenTask;
        }
             //  Getting a task from the user
             //  Получаем задачу от пользователя



        static public void RequestContacts(string uekRE, string usID, ulong staticUID)
        {
            //if(CheckForAuthorisation())
            //{
            string uscRequest = "";

            //  Create a universal server command to send it to the server
            //uscRequest = CreateContactsRequest(uekRE, usID, staticUID);

            //SendMessageToServer(uscRequest);

            List<string> userContacts = new List<string>();
            
            //string serverResponse = ReceiveMessageFromServer();


            //for (int i = 0; i < serverResponse.Length; i++)
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


        static public void RequestGroups(string uekRE, string usID, ulong staticUID)
        {
            //if(CheckForAuthorisation())
            //{
            string uscRequest = "";

            //  Create a universal server command to send it to the server
            //uscRequest = CreateGroupsRequest(uekRE, usID, staticUID);

            //SendMessageToServer(uscRequest);

            List<string> userGroups = new List<string>();

            string serverResponse;// = ReceiveMessageFromServer();


            //for (int i = 0; i < serverResponse.Length; i++)
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


        static public void RequestHistory(string uekRE, string usID, ulong staticUID, string selectedChat)
        {
            //if(CheckForAuthorisation())
            //{
                string uscRequest = "";

                //  Create a universal server command to send it to the server
                //uscRequest = CreateHistoryRequest(uekRE, usID, staticUID);

                //SendMessageToServer(uscRequest);

                List<JabNetMessage> latestMessages = new List<JabNetMessage>();

            string serverResponse;// = ReceiveMessageFromServer();


            //for(int i = 0; i < serverResponse.Length; i++)
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