using System;
using System.Collections.Generic;


using DateTime4bLib;



namespace Shared.Source
{
    public class JN_Message(DateTime4b sentTime, string message, UInt64 authorSUID)
    {
        public DateTime4b sentTime = sentTime;
        public string message = message;

        public UInt64 authorSUID = authorSUID;
    }


    public class JN_Author(string name, string surname, string bio, ulong suid, JN_ProfilePicture avatar)
    {
        public string name    = name;
        public string surname = surname;
        public string bio     = bio;

        public UInt64 suid = suid;
        public JN_ProfilePicture avatar = avatar;



        public string FullName => name + " " + surname;
    }


    public class JN_Chat(List<UInt64> membersSUID, JN_ProfilePicture chatAvatar, List<JN_ChatTopic> topics)
    {
        public List<UInt64>  membersSUID = membersSUID;
        public JN_ProfilePicture chatAvatar = chatAvatar;

        public List<JN_ChatTopic> topics = topics;
    }
    public class JN_ChatTopic(JN_ProfilePicture topicAvatar, string topicTitle, Int32 topicID)
    {
        public JN_ProfilePicture topicAvatar = topicAvatar;

        public string topicTitle = topicTitle;
        public Int32  topicID    = topicID;
    }


    public class JN_ProfilePicture(Byte[] smallAvatar, Byte[] mediumAvatar, Byte[] normalAvatar)
    {
        public Byte[] smallAvatar  = smallAvatar;
        public Byte[] mediumAvatar = mediumAvatar;
        public Byte[] normalAvatar = normalAvatar;
    }
}
