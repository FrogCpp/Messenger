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
    public class Accaunt : JN_Author, INotifyPropertyChanged
    {
        public Accaunt(string name, string surname, string bio, ulong suid, ImageSource avatar) : base(name, surname, bio, suid, avatar)
        {
            this.name = name;
            this.surname = surname;
            this.bio = bio;

            this.suid = suid;
            this.avatar = avatar;
        }

        public override string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public override string Surname
        {
            get => surname;
            set
            {
                surname = value;
                OnPropertyChanged();
            }
        }
        public override ImageSource Avatar
        {
            get => avatar;
            set
            {
                avatar = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class Chat : JN_Chat, INotifyPropertyChanged
    {
        public Dictionary<UInt64, JN_Message> ChatStory { get; private set; }
        public Chat(List<ulong> membersSUID, ImageSource chatAvatar, string chatName) : base(membersSUID, chatAvatar, chatName)
        {
            this.membersSUID = membersSUID;
            this.chatAvatar = chatAvatar;
            this.chatName = chatName;
        }
        public override List<UInt64> MembersSUID
        {
            get => membersSUID;
            set
            {
                membersSUID = value;
                OnPropertyChanged();
            }
        }
        public override string ChatName
        {
            get => base.ChatName;
            set
            {
                chatName = value;
                OnPropertyChanged();
            }
        }

        public override void SendMessage(JN_Message msg)
        {
            ChatStory.Add(msg.mesageSUID, msg);
            OnPropertyChanged();
        }

        public override void RemoveMessage(UInt64 id)
        {
            ChatStory.Remove(id);
            OnPropertyChanged();
        }

        public override void ChangeMessage(UInt64 id, string text)
        {
            ChatStory[id].MessageText = text;
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Message : JN_Message, INotifyPropertyChanged
    {
        public Message(DateTime4b sentTime, string message, ulong authorSUID, ulong mesageSUID) : base(sentTime, message, authorSUID, mesageSUID)
        {
            this.sentTime = sentTime;
            this.message = message;

            this.authorSUID = authorSUID;
            this.mesageSUID = mesageSUID;
        }

        public string StringSentTime { get => sentTime.CurrentToStandardString(); }

        public override string MessageText
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
