namespace JabNetClient
{
    internal class GlobalVariables
    {
        //  Available user tasks,
        //  it is necessary so the client program understands what the user wants and what UI window to show
        //  Also will be used for communication between the client and the server

        //  Всевозможные действия пользователя,
        //  нужны для того чтобы программа клиента понимала какое окно показывать
        //  Также будет использоваться для упрощения комуникации между клиентом и сервером
        public enum ProgramTask
        {
            //  No task is currently in need to be executed,
            //  Used in the GetUserTask() function as a null value for the loop
            //
            //  Никаких действий на данный момент выполнять ненадо
            //  Используется в функции GetUserTask() как значения null для цикла
            None,

            //  Window task for the program to show the main menu
            //  Оконная задача для программы для показа главного окна
            ShowMenu,

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
    }
}