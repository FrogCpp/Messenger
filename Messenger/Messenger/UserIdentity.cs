using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    class UserIdentity(string name, string login, string password, Thread myMainThread)
    {
        public string Name = name;
        public Thread MyThread = myMainThread;
        private string _password = password;
        private string _login = login;
    }
}
