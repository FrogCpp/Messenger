using Shared.Source;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Epsilon
{
    public static class PageController
    {
        private static Dictionary<UInt64, JN_Chat> _chatsList = null;
        private static JN_Author _user = null;

        public static void Init(JN_Author usr, Dictionary<UInt64, JN_Chat> chats)
        {
            if (_user == null || _chatsList == null)
            {
                _user = usr;
                _chatsList = chats;
            }
        }

        public static void ChangeName(string newName)
        {
            _user.Name = newName;
        }

        public static void ChangeSurname(string newSurname)
        {
            _user.Surname = newSurname;
        }

        public static void ChangeBio(string newBio)
        {
            _user.bio = newBio;
        }

        public static void ChangeAvatar(ImageSource newAvatar)
        {
            _user.Avatar = newAvatar;
        }

        public static void AddContact(JN_Chat newChat)
        {
            _chatsList.Add(newChat.membersSUID[0],  newChat);
        }

        public static bool RemoveContact(JN_Chat chat)
        {
            if (!_chatsList.Contains(chat)) return false;
            _chatsList.Remove(chat);
            return true;
        }

        public static bool SendMesage(JN_Chat contact, JN_Message message)
        {
            if (!_chatsList.Contains(contact)) return false;
            _chatsList.FirstOrDefault(contact).MessageText.Add(message);
            return true;
        }

        public static bool RemoveMesage(MainPage.Contact contact, JN_Message message)
        {
            if (!_chatsList.Contains(contact)) return false;
            if (!_chatsList.FirstOrDefault(contact).Messages.Contains(message)) return false;
            _chatsList.FirstOrDefault(contact).Messages.Remove(message);
            return true;
        }

        public static bool ChangeMesage(MainPage.Contact contact, JN_Message oldMessage, JN_Message newMessage)
        {
            if (!_chatsList.Contains(contact)) return false;
            if (!_chatsList.FirstOrDefault(contact).Messages.Contains(oldMessage)) return false;
            //_contactsList.FirstOrDefault(contact).Messages.FirstOrDefault(oldMessage) = newMessage;
            return true;
        }
    }
}
