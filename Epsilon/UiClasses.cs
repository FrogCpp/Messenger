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
    internal class Accaunt : JN_Author, INotifyPropertyChanged
    {
        public Accaunt(string name, string surname, string bio, ulong suid, ImageSource avatar) : base(name, surname, bio, suid, avatar)
        {
            this.name = name;
            this.surname = surname;
            this.bio = bio;

            this.suid = suid;
            this.avatar = avatar;
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        public string Surname
        {
            get => surname;
            set
            {
                surname = value;
                OnPropertyChanged();
            }
        }
        public ImageSource Avatar
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


    internal class Chat : JN_Chat, INotifyPropertyChanged
    {
        private Dictionary<UInt64, JN_Message> _chatStory;
        public Chat(List<ulong> membersSUID, ImageSource chatAvatar) : base(membersSUID, chatAvatar)
        {
            this.membersSUID = membersSUID;
            this.chatAvatar = chatAvatar;
        }

        public Dictionary<UInt64, JN_Message> ChatStory
        {
            get => _chatStory;
        }

        public void SendMessage(JN_Message msg)
        {
            _chatStory.Add(ref msg.mesageSUID, msg);
            OnPropertyChanged();
        }

        public void RemoveMessage(UInt64 id)
        {
            _chatStory.Remove(id);
            OnPropertyChanged();
        }

        public void ChangeMesage(UInt64 id, string text)
        {
            _chatStory[id].message = text;
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class Message : JN_Message, INotifyPropertyChanged
    {
        public Message(DateTime4b sentTime, string message, ulong authorSUID, ulong mesageSUID) : base(sentTime, message, authorSUID, mesageSUID)
        {
            this.sentTime = sentTime;
            this.message = message;

            this.authorSUID = authorSUID;
            this.mesageSUID = mesageSUID;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
