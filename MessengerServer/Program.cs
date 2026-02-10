using MessengerServer;
using System.Collections.Concurrent;
using static Shared.Source.NetDriver;

namespace JabServer
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            ConcurrentBag<ConnectionHandler> users = new ConcurrentBag<ConnectionHandler>();
            TaskHandler handler = new TaskHandler();
            var coordinator = new ConnectionCoordinator(users, handler.ProcessedTasks, ConnectionCoordinator.ConnectionMode.host);

            await coordinator.StartAcceptingClients();
            await coordinator.StartListening();
        }
    }
}