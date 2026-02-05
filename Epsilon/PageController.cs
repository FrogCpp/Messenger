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
            _user.name = newName;
        }

        public static void ChangeSurname(string newSurname)
        {
            _user.surname = newSurname;
        }

        public static void ChangeBio(string newBio)
        {
            _user.bio = newBio;
        }

        public static void ChangeAvatar(ImageSource newAvatar)
        {
            _user.avatar = newAvatar;
        }

        public static void AddChat(JN_Chat newChat)
        {
            _chatsList.Add(newChat.membersSUID[0],  newChat);
        }

        public static bool RemoveChat(JN_Chat chat)
        {
            try
            {
                _chatsList.Remove(chat.membersSUID[0]);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendMesage(JN_Chat chat, JN_Message message)
        {
            try
            {
                _chatsList[chat.membersSUID[0]].SendMessage(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RemoveMesage(JN_Chat chat, UInt64 messageID)
        {
            try
            {
                _chatsList[chat.membersSUID[0]].RemoveMessage(messageID);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ChangeMesage(JN_Chat chat, UInt64 messageID, string newMessageText)
        {
            try
            {
                _chatsList[chat.membersSUID[0]].ChangeMessage(messageID, newMessageText);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
