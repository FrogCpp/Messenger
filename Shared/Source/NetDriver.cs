using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static JabNet.NetDriver.Package;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JabNet
{
    public static class NetDriver
    {
        public class Package
        {
            public struct PackageElement
            {
                public Guid packageID;
                public Int16 packageLength;
                public Int16 elementIndex;
                public byte[] content;
            }

            public readonly List<PackageElement> pack = new(0);
            public readonly Guid packageID;
            public int packageLength { get { return pack.Count * elementLength; } }
            public int elementLength { get 
                {
                    if (pack.Count == 0) return 0;
                    return pack[0].content.Length; 
                } }
            public bool packageСompleteness { get 
                {
                    if (pack.Count == 0) return false;

                    return pack.Count == pack[0].packageLength;
                } }

            public Package(Guid? id = null)
            {
                packageID = id ?? Guid.NewGuid();
            }

            public bool Add(PackageElement pkge)
            {
                if (pkge.packageID != packageID) return false;
                if (pkge.packageLength == pack.Count) return false;
                pack.Add(pkge);

                return true;
            }
        }



        public static class PackConstructor
        {
            public static Package Serealize(byte[] data)
            {
                var pkg = new Package();
                int totalPieces = (int)Math.Ceiling(data.Length / (double)(128 * 1024));

                for (int i = 0; i < totalPieces; i++)
                {
                    int offset = i * (128 * 1024);
                    int remaining = data.Length - offset;
                    int chunkSize = Math.Min((128 * 1024), remaining);

                    byte[] chunk = new byte[chunkSize];
                    Buffer.BlockCopy(data, offset, chunk, 0, chunkSize);

                    var packEl = new Package.PackageElement();

                    packEl.packageID = pkg.packageID;
                    packEl.packageLength = (Int16)totalPieces;
                    packEl.elementIndex = (Int16)i;
                    packEl.content = chunk;


                    pkg.Add(packEl);
                }
                return pkg;
            }

            public static byte[] Desrealize(Package pack)
            {
                var data = new byte[pack.packageLength];
                int offset = 0;

                pack.pack.Sort((a, b) => a.elementIndex.CompareTo(b.elementIndex));

                for (int i = 0; i < pack.pack.Count; i++)
                {
                    Buffer.BlockCopy(pack.pack[i].content, 0, data, offset, pack.elementLength);
                    offset += pack.elementLength;
                }

                return data;
            }
        }




        internal static class EventController
        {
            public static async Task Listen(List<Task> tasks, Func<Task, Task> act)
            {
                while (tasks.Count != 0)
                {
                    var tsk= Task.WhenAny(tasks);

                    await act(tsk);
                }
            }
        }

        public class ConnectionController
        {
            public enum c_state
            {
                user,
                server,
            }

            private readonly c_state _state;
            private ConcurrentBag<ConnectionAvatar> _avatarConnectionList;

            public ConnectionController(ConcurrentBag<ConnectionAvatar> usrlst, c_state state = c_state.user)
            {
                _state = state;
                _avatarConnectionList = usrlst;
            }

            public async Task AsyncAcceptNewClients()
            {
                if (_state == c_state.user) return;

                Socket listner = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listner.Bind(new IPEndPoint(IPAddress.Any, 121221));
                listner.Listen();

                while (true)
                {
                    var clientSocket = await listner.AcceptAsync();

                    _avatarConnectionList.Add(new ConnectionAvatar(clientSocket));
                    var cl = _avatarConnectionList.First(s => s.socket == clientSocket);
                    cl.ListenTask = clientSocket.ReceiveAsync(cl.buffer);
                }
            }


            public async Task AsyncConnectServer()
            {
                if (_state == c_state.server) return;

                Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                await serverSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 121221));

                _avatarConnectionList.Add(new ConnectionAvatar(serverSocket));
                var sr = _avatarConnectionList.First(s => s.socket == serverSocket);
                sr.ListenTask = serverSocket.ReceiveAsync(sr.buffer);
            }

            public async Task ListenAsync()
            {
                await EventController.Listen(_avatarConnectionList.Select(user => user.ListenTask).ToList(), ReciveMessage);
            }

            public async Task ReciveMessage(Task reciving)
            {
                var usr = _avatarConnectionList.First(t => t.ListenTask == reciving);

                PackageElement pkge = JsonSerializer.Deserialize<PackageElement>(Encoding.Unicode.GetString(usr.buffer));

                var pack = usr.incoming.SingleOrDefault(p => p.packageID == pkge.packageID);
                if (pack.Equals(default(PackageElement)))
                {
                    var pkg = new Package();
                    pkg.Add(pkge);
                    usr.incoming.Add(pkg);
                }
                else
                {
                    pack.Add(pkge);
                }
            }
        }


        public class ConnectionAvatar(Socket sk)
        {
            public readonly Socket socket = sk;
            public Task ListenTask;
            public readonly byte[] buffer = new byte[128 * 1024];

            public readonly List<Package> incoming = new();
            public readonly List<Package> outgoing = new();

            public async Task WaitComplitePackage()
            {

            }
        }
    }
}