using System;

using DateTime4bLib;

namespace JabNetClient
{
    internal class InterfaceClasses
    {

        public class Message
        {
            private readonly DateTime4b _sendDateTime;

            private readonly string _authorName;
            private string _message;


            public Message(string authorName, DateTime4b sendDateTime, string message)
            {
                _authorName = authorName;
                _sendDateTime = sendDateTime;
                _message = message;
            }


            public void EditMessage(DateTime4b editDateTime, string newMessage)
            {
                //  Pseudo logic for editing a message
                //  Псевдо логика для изменения сообщений

                if(editDateTime.PassedTotalMinutes - _sendDateTime.PassedTotalMinutes < 2880)
                {
					//  Check for the edit time being less than 2 days (2880 minutes)
					_message = newMessage;
                }
            }


            public DateTime4b SendDateTime => _sendDateTime;
            public string GetAuthor  => _authorName;
            public string GetMessage => _message;
        }


        static public class JabNetProfile
        {
            //static byte[] Avatar;  idk how to do it 
            static string name;
			static string bio;

			static UInt64 staticUID;

        }
    }
}