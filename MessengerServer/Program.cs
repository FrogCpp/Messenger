using MessengerServer;
using Shared.Source;
using System.Collections.Concurrent;
using System.Net;
using static MessengerServer.SQLiteHandler;
using static Shared.Source.NetConst;
using static Shared.Source.NetDriver;

namespace JabServer
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Start!");
            
            InitAsync();
            await DebugPrintAsync();

            foreach (var i in TableList)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine(await RemoveTableAsync(TableList[1]));
            await DebugPrintAsync();

            //PORT = 11111;                         это для клиента надо.
            //IP = IPAddress.Parse("127.0.0.1");    указать айпи\домен сервера.

            ConcurrentBag<ConnectionHandler> users = new ConcurrentBag<ConnectionHandler>();
            TaskHandler handler = new TaskHandler();
            var coordinator = new ConnectionCoordinator(users, handler.ProcessedTasks, ConnectionCoordinator.ConnectionMode.host);

            await coordinator.StartAcceptingClients();
            await coordinator.StartListening();
        }
    }
}