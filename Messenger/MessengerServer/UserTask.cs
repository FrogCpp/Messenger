using NetworkDriver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MessengerServer
{
    internal class TaskHandler
    {
        private List<Task> Processes = new List<Task>();
        private static ConcurrentDictionary<int, User> _userList = null;



        public TaskHandler(ref ConcurrentDictionary<int, User> usr)
        {
            _userList = usr;
        }

        public void StartTask(Socket socket, MessageSample msg)
        {
            Processes.Add(Task.Run(() => TaskPerformer(socket, msg)));
        }

        public void StartTask(Socket socket, MessageSample msg, int userIndex)
        {
            Processes.Add(Task.Run(() => TaskPerformer(socket, msg, userIndex)));
        }

        private static void TaskPerformer(Socket socket, MessageSample msg)
        {
            switch (msg.type)
            {
                case TaskType.SEND_MESSAGE:
                    break;

                case TaskType.UPDATE_CHAT:
                    break;

                case TaskType.CREAT_CHAT:
                    break;

                case TaskType.REMOVE_CHAT:
                    break;
            }
        }

        private static void TaskPerformer(Socket socket, MessageSample msg, int userIndex)
        {
            switch (msg.type)
            {
                case TaskType.GET_FIRST_USR_KEY:
                    var usr = _userList.GetValueOrDefault(userIndex);
                    break;

                case TaskType.SEND_SECOND_USR_KEY:
                    break;

                case TaskType.GET_SECOND_USR_KEY:
                    break;
            }
        }
    }
}
