using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MessengerServer
{
    internal class UserTask
    {
        private User _user;
        public DurationType durationType;
        public MessageSample currentMessage;

        public UserTask(MessageSample msg, User usr)
        {
            _user = usr;
            durationType = msg.durationType;
            currentMessage = msg;
            _user.activeTask = true;
        }

        public bool Read()
        {
            byte[] buffer = new byte[1024];
            _user.socket.Receive(buffer);

            currentMessage = JsonSerializer.Deserialize<MessageSample>(Encoding.Unicode.GetString(buffer));

            if (currentMessage.type == MessageType.sizeTransfer)
            {
                buffer = new byte[int.Parse(Encoding.Unicode.GetString(currentMessage.content))];
                _user.socket.Send(new byte[] { 1 });
            }
            else
            {
                _user.socket.Send(new byte[] { 0 });
                return false;
            }

            _user.socket.Receive(buffer);

            currentMessage = JsonSerializer.Deserialize<MessageSample>(Encoding.Unicode.GetString(buffer));

            return true;
        }

        public bool Send(MessageSample msg)
        {
            byte[] sndBts = Encoding.Unicode.GetBytes(JsonSerializer.Serialize<MessageSample>(msg));
            MessageSample msgL = new MessageSample();
            msgL.durationType = durationType;
            msgL.type = MessageType.sizeTransfer;
            msgL.content = Encoding.Unicode.GetBytes(sndBts.Length.ToString());

            byte[] buffer = new byte[1];
            _user.socket.Receive(buffer);
            if (buffer[0] == 1)
            {
                _user.socket.Send(sndBts);
                return true;
            }
            return false;
        }
    }
}
