using System;


using DateTime4bLib;



namespace JabNetClient
{
    public class InterfaceClasses
    {
        public class JN_Message
        {
            private readonly DateTime4b _sendDateTime;

            private readonly string[] _authors;
            private string _message;


            public JN_Message(string[] authors, DateTime4b sendDateTime, string message)
            {
                _authors = authors;
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
            public string[] Authors  => _authors;
            public string Message => _message;
        }


        static public class JN_Profile
        {
            //static byte[] Avatar;  idk how to do it 
            static string name;
			static string bio;

			static UInt64 staticUID;

        }
    }
}