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
    private int _port = Port;
    private CancellationTokenSource _myMainThreadController = new CancellationTokenSource();
    private CancellationToken _mainListnerThreadToken;
    private ConcurrentDictionary<UserIdentity, UserSocket> _connectionArray = new ConcurrentDictionary<UserIdentity, UserSocket>();

    public void Start()
    {
        _mainListnerThreadToken = _myMainThreadController.Token;
        Task.Run(() => ListenConnectons(_mainListnerThreadToken), _mainListnerThreadToken);
    }

    public void End()
    {
        _myMainThreadController.Cancel();
    }

    private void ListenConnectons(CancellationToken threadToken)
    {
        using (Socket mainListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            mainListenSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
            mainListenSocket.Listen();
            try
            {
                while (true)
                {
                    threadToken.ThrowIfCancellationRequested();

                    SocketAsyncEventArgs args = new SocketAsyncEventArgs(); // не забудь переиминовать
                    args.Completed += AcceptClients;
                    if (!mainListenSocket.AcceptAsync(args))
                    {
                        AcceptClients(mainListenSocket, args);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                mainListenSocket.Close();
            }
        }
    }

    private void AcceptClients(object sender, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            // a и b использются как логическая последовательность создания клиента
            var b = new UserSocket(args.AcceptSocket);

            CancellationTokenSource clientThread = new CancellationTokenSource();

            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(clientThread.Token, _myMainThreadController.Token);
            CancellationToken linkedToken = linkedCts.Token;

            //Task.Run(() => b.Communication(linkedToken), linkedToken);

            var a = new UserIdentity("", "", "", b, linkedToken, clientThread);
            _connectionArray.TryAdd(a, b);

            Task.Run(() => a.mainAction(), linkedToken);

            Console.WriteLine("new client!");
        }
        args.AcceptSocket = null;
    }
}