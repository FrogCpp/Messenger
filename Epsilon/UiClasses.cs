using DateTime4bLib;
using Observables.Specialized.Extensions;
using Shared.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Epsilon
{
    public class Accaunt : JN_Author
    {

        public Accaunt(string name, string surname, string bio, ulong suid, ImageSource avatar) : base(name, surname, bio, suid, avatar)
        {
            this.name = name;
            this.surname = surname;
            this.bio = bio;

            this.suid = suid;
            this.avatar = avatar;
        }
    }


    public class Chat : JN_Chat
    {
        public Dictionary<UInt64, JN_Message> ChatStory { get; private set; }
        public Chat(List<ulong> membersSUID, ImageSource chatAvatar, string chatName) : base(membersSUID, chatAvatar, chatName)
        {
            this.membersSUID = membersSUID;
            this.chatAvatar = chatAvatar;
            this.chatName = chatName;
        }

        public override void SendMessage(JN_Message msg)
        {
            ChatStory.Add(msg.mesageSUID, msg);
        }

        public override void RemoveMessage(UInt64 id)
        {
            ChatStory.Remove(id);
        }

        public override void ChangeMessage(UInt64 id, string text)
        {
            ChatStory[id].message = text;
        }
    }

    public class Message : JN_Message
    {
        public Message(DateTime4b sentTime, string message, ulong authorSUID, ulong mesageSUID) : base(sentTime, message, authorSUID, mesageSUID)
        {
            this.sentTime = sentTime;
            this.message = message;

            this.authorSUID = authorSUID;
            this.mesageSUID = mesageSUID;
        }

        public string StringSentTime { get => sentTime.CurrentToStandardString(); }
    }
}
