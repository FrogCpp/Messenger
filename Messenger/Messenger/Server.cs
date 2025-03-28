using Messenger;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

/*
 * важное уточнение:
 * переменные с заглавной буквы (Aboba) - переменные публичные
 * переменные с подчеркивания (_aboba) - переменные приватные
 * переменные без ничего (aboba) - переменные локальные и вспомогательные в масштабе одного цикла или функции (в частности a b c к ним тоже относятся)
 */

class Server(IPAddress StaticIpAdreesForHost, int Port)
{
    private IPAddress _address = StaticIpAdreesForHost;
    private Thread _mainListnerThread = null;
    private int _port = Port;
    private ConcurrentDictionary<UserIdentity, UserSocket> _connectionArray = new ConcurrentDictionary<UserIdentity, UserSocket>();

    public void Start()
    {
        Thread _mainListnerThread = new Thread(() => ListenConnectons());
        _mainListnerThread.Start();
    }

    public void End()
    {
        foreach (var i in _connectionArray.Keys)
        {
            i.MyThread.Abort();
            _connectionArray[i].Close();
        }
        _mainListnerThread.Abort();
    }

    private void ListenConnectons()
    {
        using (Socket mainListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            mainListenSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
            mainListenSocket.Listen();
            while (true)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs(); // не забудь переиминовать
                args.Completed += AcceptClients;
                if (!mainListenSocket.AcceptAsync(args))
                {
                    AcceptClients(mainListenSocket, args);
                }
            }
        }
    }

    private void AcceptClients(object sender, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            // a и b использются как логическая последовательность создания клиента
            var b = new UserSocket(args.AcceptSocket);

            Thread clientThread = new Thread(() => b.Communication());
            clientThread.Start();

            var a = new UserIdentity("", "", "", clientThread);
            _connectionArray.TryAdd(a, b);
        }
        args.AcceptSocket = null;
    }
}