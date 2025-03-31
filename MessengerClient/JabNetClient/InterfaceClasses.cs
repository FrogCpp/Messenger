//using System;
//using System.Dynamic;
//using System.Runtime.Remoting.Messaging;

namespace JabNetClient
{
    internal class InterfaceClasses
    {

        public class JabNetMessage
        {

            //  Message authors name
            //  Имя автора сообщения
            static string AuthorName;



            //  Time at what the message was send
            //  Время в которое сообщение было отправлено
            static ushort SendTime;



            //  This is the send date
            //  It is encoded to be as compact and easy to use as possible
            //
            //  Currently, every year has 366 days
            //  If a year actually doesnt have 366 days
            //  At the 1st march the Date will just skip 1 value
            //  (Straight ignoring 29th of february if it doesn exist)
            //
            //  This also means, that every 366 values we end a year and begin a new one
            //  So:        SendDate % 366              =    send date
            //  And:      (SendDate / 366) + 2025      =    send year
            //
            //  Due to ushort limitation of a 65'535 value,
            //  And the fact that we count years starting from 2025:
            //  The year 2025:   SendDate > 0 && SendDate < 367
            //  We can go 179 years in the future before the algorithm breaks
            //  And another Y2K event happens in the year 2204   XD
            //
            //
            //
            //  Это дата отправки сообщения, и она также хранит в себе год отправки
            //  Переменная закодирована, чтобы быть наиболее компактной и вмещать в себе наибольшее число вещей
            //
            //  На текущий момент, мы предполагаем что в каждом году 366 дней
            //  Если же в текущем году на самом деле не 366 дней
            //  То каждый раз при наступлении даты 1 марта, мы просто будет пропускать одно значение
            //  (Полностью игнорируя 29 февраля если его нету)
            //
            //  Это также означает, что каждые новые 366 значений будут начинать новый год
            //  Поэтому:       ДатаОтправки % 366             =  Отправленный день
            //  А такде:       (датаОтправки / 366) + 2025    =  отправленный год
            //
            //  Поскольку в ushort помещается всего 65'535 значений,
            //  И того оптимизирующего факта, что мы храним только года, начинающиеся с 2025
            //  Пример:   Если год отправки 2025 то:  ДатаОтправки будет > 0  и  < 367
            //  Таким образом, мы сможем хранить целых 179 лет прежде чем алгоритм сломает себя
            //  И снова случится Y2K, только уже в 2204 году    XD
            //  
            //  Вот так вот 
            static ushort SendDate;


            //  The sent message
            //  Отправленное сообщение
            static string Message;


            JabNetMessage(string iAuthorName, ushort iSendTime, ushort iSendDate, string iMessage)
            {
                //  i - as the prefix stands for Input
                //  i - как префикс обозначает сокращённо Input (ввод)

                //  Update the values in the cinstructor accordingly
                //  Обновляем значения в конструкторе

                AuthorName = iAuthorName;
                SendTime = iSendTime;
                SendDate = iSendDate;
                Message = iMessage;
            }
                 // Constructor
                 // Конструктор


            void EditMessage(ushort editTime, ushort editDate, string newMessage)
            {
                //  Pseudo logic for editing a message
                //  Псевдо логика для изменения сообщений

                if(editDate == SendDate || editDate == SendDate + 1)
                {
                    //  Check for the edit time
                    //  If it is within 24 to 48 hours => allow the edit
                    //
                    //  Проверка даты изменения сообщения
                    //  Если изменение произошло в промежутке от 24 до 48 часов
                    //  => мы разрешаем изменение сообщения
                    Message = newMessage;
                    SendDate = editDate;
                    SendTime = editTime;
                }
            }
                 //  Editing a sent message with basic error checking
                 //  Для изменения сообщения с упрощённой проверкой легитности изменения

            string GetAuthor()
            {
                //  Simple logic for returning the message authors name
                //  Простая логика для возвращения имени автора сообщения

                return AuthorName;
            }
                 //  Returning the message authors name
                 //  Возвращаем имя автора сообщения

            ushort GetSendTime()
            {
                //  Simple logic for returning the message send time
                //  Простая логика для возвращения времени отправки

                return SendTime;
            }
                 //  Returning the message send time
                 //  Возвращаем время отправки сообщения

            ushort GetSendDate()
            {
                //  Simple logic for returning the message send date (including the year)
                //  Простая логика для возвращения даты отправки сообщения (включая год отправки)

                return SendDate;
            }
                 //  Returning the message send date (including the send year)
                 //  Возвращаем дату отправки сообщения (включая год отправки)

            string GetMessage()
            {
                //  Simple logic for returning the sed message
                //  Простая логика для возвращения отправленного сообщения

                return Message;
            }
                 //  Returning the send message
                 //  Возвращаем отправленное сообщение
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