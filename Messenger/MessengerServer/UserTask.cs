using NetworkDriver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace MessengerServer
{
    internal class TaskHandler
    {
        public struct TaskSample
        {
            public TaskSample(Socket sckt, MessageSample mesg, Guid usrIndex)
            {
                socket = sckt;
                msg = mesg;
                userIndex = usrIndex;
            }

            public Socket socket { get; private set; }
            public MessageSample msg { get; private set; }
            public Guid userIndex { get; private set; }
        }


        private List<Task> _workers = null;
        private static ConcurrentDictionary<Guid, User> _userList = null;
        private static Channel<TaskHandler.TaskSample> _queue = null;



        public TaskHandler(ConcurrentDictionary<Guid, User> usr, Channel<TaskHandler.TaskSample> que, int count)
        {
            _userList = usr;
            _queue = que;
            _workers = new(count);

            for (int i = 0; i < count; i++)
            {
                _workers[i] = (Task.Run(async () => 
                {
                    while (await _queue.Reader.WaitToReadAsync())
                    {
                        if (_queue.Reader.TryRead(out TaskHandler.TaskSample tsk))
                        {
                            await TaskPerformer(tsk.socket, tsk.msg, tsk.userIndex);
                        }
                    }
                }));
            }
        }



        private static async Task TaskPerformer(Socket socket, MessageSample msg, Guid userIndex)
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
    }
}
