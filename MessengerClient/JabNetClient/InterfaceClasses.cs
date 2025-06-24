//using System;
//using System.Dynamic;
//using System.Runtime.Remoting.Messaging;

using static CompactDateTimeLibrary.CompactType;


namespace JabNetClient
{
    internal class InterfaceClasses
    {

        public class JabNetMessage
        {
            //  CDT Type is finished, only use logic is not finished yet
            private readonly CompactDateTime _sendDateTime;

            private readonly string _authorName;
            private string _message;


            public JabNetMessage(string authorName, CompactDateTime sendDateTime, string message)
            {
                _authorName = authorName;
                _sendDateTime = sendDateTime;
                _message = message;
            }


            public void EditMessage(CompactDateTime editDateTime, string newMessage)
            {
                //  Pseudo logic for editing a message
                //  Псевдо логика для изменения сообщений

                if(editDateTime.PassedDays - _sendDateTime.PassedDays >= 0 &&
                    editDateTime.PassedDays - _sendDateTime.PassedDays < 2)
                {
                    //  Check for the edit time
                    //  If it is within 24 to 48 hours => allow the edit
                    //
                    //  Проверка даты изменения сообщения
                    //  Если изменение произошло в промежутке от 24 до 48 часов
                    //  => мы разрешаем изменение сообщения
                    _message = newMessage;
                }
            }
            //  Editing a sent message with basic error checking
            //  Для изменения сообщения с упрощённой проверкой легитности изменения


            public CompactDateTime SendDateTime => _sendDateTime;
            public string GetAuthor  => _authorName;
            public string GetMessage => _message;
        }


        static public class JabNetProfile
        {
            static byte[] Avatar;
            static string Pseudoname;
            static ulong StaticUID;
            static string Message;

        }
    }
}