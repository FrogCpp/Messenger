using static System.Console;


using static JabNetClient.GlobalVariables;

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

    }
}