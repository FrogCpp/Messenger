using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    class UserIdentity(string name, string login, string password, CancellationToken myMainToken, CancellationTokenSource ThreadController)
    {
        public string Name = name;
        public CancellationToken MyThread = myMainToken;
        private CancellationTokenSource MyController = ThreadController;
        private string _password = password;
        private string _login = login;
    }
}
