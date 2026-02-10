using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

using AVcontrol;



namespace Shared.Source
{
    public class JN_Message(DateTime4b sentTime, string message, UInt64 authorSUID)
    {
        public DateTime4b sentTime = sentTime;
        public string message = message;

        public UInt64 authorSUID = authorSUID;
    }


    public class JN_Author(string name, string surname, string bio, ulong suid, ImageSource avatar)
    {
        public string name    = name;
        public string surname = surname;
        public string bio     = bio;

        public UInt64 suid = suid;
        public ImageSource avatar = avatar;



        public string FullName => name + " " + surname;
    }


    public class JN_Chat(List<UInt64> membersSUID, ImageSource chatAvatar, List<JN_ChatTopic> topics)
    {
        public List<UInt64>  membersSUID = membersSUID;
        public ImageSource chatAvatar = chatAvatar;

        //public List<JN_ChatTopic> topics = topics;
    }
    public class JN_ChatTopic(ImageSource topicAvatar, string topicTitle, Int32 topicID) // значительно позже. . .
    {
        public ImageSource topicAvatar = topicAvatar;

        public string topicTitle = topicTitle;
        public Int32  topicID    = topicID;
    }
}
