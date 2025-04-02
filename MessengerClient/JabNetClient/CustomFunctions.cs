//using System;
using System.Collections.Generic;

using static System.Console;


using static JabNetClient.GlobalVariables;
using static JabNetClient.InterfaceClasses;
using static JabNetClient.ServerCommunication;



namespace JabNetClient
{
    internal class CustomFunctions
    {
        static public ProgramTask GetUserTask()
        {
            //  Store the chosen task here
            //  Храним выбранную задачу здесь
            ProgramTask _chosenTask = ProgramTask.None;

            //  User input for the console interface
            //  Ввод пользователя для консольного интерфейса
            string _userInput = ReadLine();

            while(_chosenTask == ProgramTask.None)
            {
                //  Parse user input
                //  Transform it into a program task
                //  exit the loop
                //
                //  Прочитать ввод пользователя
                //  Преобразовать его в программную задачу
                //  Выйти из цикла
            }
            
            //  Return the chosen program task
            //  Возвращаем выбранную задачу для программы
            return _chosenTask;
        }
        //  Getting a task from the user
        //  Получаем задачу от пользователя



        static public void RequestContacts(string _uekRE, byte[] _usID, ulong _staticUID)
        {
            //if(CheckForAuthorisation())
            //{
            string _uscRequest = "";

            //  Create a universal server command to send it to the server
            //_uscRequest = CreateContactsRequest(_uekRE, _usID, _staticUID);

            SendMessageToServer(_uscRequest);

            List<string> _userContacts = new List<string>();
            
            string _serverResponse = ReceiveMessageFromServer();


            for (int i = 0; i < _serverResponse.Length; i++)
            {
                //  Parse server response here
                //  From a string to either a list of strings 
                //  Or a custom class
                //
                //  Превращаем ответ сервера
                //  Из string либо в динамический массив string
                //  Либо в собственный класс
            }

            //return _userMessages;
            //}
            //return null;
        }


        static public void RequestGroups(string _uekRE, byte[] _usID, ulong _staticUID)
        {
            //if(CheckForAuthorisation())
            //{
            string _uscRequest = "";

            //  Create a universal server command to send it to the server
            //_uscRequest = CreateGroupsRequest(_uekRE, _usID, _staticUID);

            SendMessageToServer(_uscRequest);

            List<string> _userGroups = new List<string>();

            string _serverResponse = ReceiveMessageFromServer();


            for (int i = 0; i < _serverResponse.Length; i++)
            {
                //  Parse server response here
                //  From a string to either a list of strings 
                //  Or a custom class
                //
                //  Превращаем ответ сервера
                //  Из string либо в динамический массив string
                //  Либо в собственный класс
            }

            //return _userGroups;
            //}
            //return null;
        }


        static public void RequestHistory(string _uekRE, byte[] _usID, ulong _staticUID, string _selectedChat)
        {
            //if(CheckForAuthorisation())
            //{
                string _uscRequest = "";

                //  Create a universal server command to send it to the server
                //_uscRequest = CreateHistoryRequest(_uekRE, _usID, _staticUID);

                SendMessageToServer(_uscRequest);

                List<JabNetMessage> _latestMessages = new List<JabNetMessage>();

                string _serverResponse = ReceiveMessageFromServer();


                for(int i = 0; i < _serverResponse.Length; i++)
                {
                //  Parse server response here
                //  From a string to the custom class JabNetMessage
                //
                //  Превращаем ответ сервера
                //  Из string либо в собственный класс
            }

            //return _latestMessages;
            //}
            //return null;
        }


    }
}