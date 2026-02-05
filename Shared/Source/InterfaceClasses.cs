using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;


using DateTime4bLib;



namespace Shared.Source
{
    public class JN_Message(DateTime4b sentTime, string message, UInt64 authorSUID, UInt64 mesageSUID)
    {
        public DateTime4b sentTime = sentTime;
        protected string message = message;

        public UInt64 authorSUID = authorSUID;
        public UInt64 mesageSUID = mesageSUID;


        public virtual string MessageText { get; set; }
    }


    public class JN_Author(string name, string surname, string bio, ulong suid, ImageSource avatar)
    {
        protected string name    = name;
        protected string surname = surname;
        public string bio     = bio;

        public UInt64 suid = suid;
        protected ImageSource avatar = avatar;



        public string FullName => name + " " + surname;


        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual ImageSource Avatar { get; set; }
    }


    public class JN_Chat(List<UInt64> membersSUID, ImageSource chatAvatar, string chatName)
    {
        protected List<UInt64> membersSUID = membersSUID;
        protected string chatName = chatName;
        protected ImageSource chatAvatar = chatAvatar;

        public virtual List<UInt64> MembersSUID { get; set; }
        public virtual string ChatName { get; set; }
        public virtual ImageSource ChatAvatar { get; set; }

        public virtual void SendMessage(JN_Message msg) { }
        public virtual void RemoveMessage(UInt64 id) { }
        public virtual void ChangeMessage(UInt64 id, string text) { }

        //public List<JN_ChatTopic> topics = topics;
    }



    public class JN_ChatTopic(ImageSource topicAvatar, string topicTitle, Int32 topicID) // значительно позже. . .
    {
        public ImageSource topicAvatar = topicAvatar;

        public string topicTitle = topicTitle;
        public Int32  topicID    = topicID;
    }
}
