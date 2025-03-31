using System;

using static System.Console;


namespace JabNetClient
{
    internal class DrawInterface
    {

        static public class JabNetMessage
        {
            static string AuthorName;
            static ushort SendTime;
            static byte   Date;
            static string Message;

        }

        static public class JabNetProfile
        {
            static byte[] Avatar;
            static string Pseudoname;
            static ulong  StaticUID;
            static string Message;

        }

        static public void ShowUI(string _sortedChats[], )
        {

            Write("Insert console UI output");


            //  Placeholder for the UI
        }
    }
}