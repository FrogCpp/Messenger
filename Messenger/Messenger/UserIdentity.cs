using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    class UserIdentity(string name, string login, string password, UserSocket mySocket, CancellationToken myMainToken, CancellationTokenSource ThreadController)
    {
        public string Name = name;
        public CancellationToken MyThread = myMainToken;
        private CancellationTokenSource _myController = ThreadController;
        private string _password = password;
        private string _login = login;
        private UserSocket _mySocket = mySocket;

        public void mainAction()
        {
            try
            {
                string words;
                string answer;
                while (true)
                {
                    MyThread.ThrowIfCancellationRequested();

                    words = _mySocket.ReadMessage();

                    answer = ExecuteCommand(words);

                    _mySocket.Reply(answer);
                }
            }
            catch (OperationCanceledException)
            {
                _mySocket.Close();
            }
        }

        private string ExecuteCommand(string command)
        {
            Console.WriteLine(command.Equals("a"));
            if (command == "destroyUser")
            {
                Console.WriteLine("Destroy Client");
                _myController.Cancel();
            }
            return command;
        }
    }
}
