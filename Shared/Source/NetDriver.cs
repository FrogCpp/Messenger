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
        /*
        public class PackageOperator(Action<SimplePackage> F)
        {
            private readonly List<SimplePackage> _incoming = new(0);
            private readonly List<SimplePackage> _outcoming = new(0);
            private readonly Action<SimplePackage> _f = F;

            public void AcceptPkg(SimplePackage sp) // будет добавлено делегату
            {
                // вот здесь переделать из пакета в таску (чуть позже)

                _f(sp);
                sp.packed -= AcceptPkg;
                _incoming.Remove(sp);
            }

            public void CollectPkg(byte[] inf)
            {
                SimplePackage.PkgPice sp = JsonSerializer.Deserialize<SimplePackage.PkgPice>(inf);
                var a = _incoming.FindIndex(p => p.packageID == sp.unicPkgID);
                if (a == -1)
                {
                    _incoming.Add(new SimplePackage(sp.unicPkgID));
                    a = _incoming.FindIndex(p => p.packageID == sp.unicPkgID);
                    _incoming[a].packed += AcceptPkg;
                }

                _incoming[a].Add(sp);
            }
        }

        public class SimplePackage
        {
            public SimplePackage() { packageID = Guid.NewGuid(); }
            public SimplePackage(Guid ID) { packageID = ID; }

            public Action<SimplePackage> packed;
            public readonly Guid packageID;
            private List<PkgPice> _package = new();

            public struct PkgPice
            {
                public PkgPice(Guid ID, int lnght, int pose, byte[] cntnt)
                {
                    unicPkgID = ID;
                    pkgLength = lnght;
                    pieceIndex = pose;
                    content = cntnt;
                }

                public Guid unicPkgID { get; }
                public int pkgLength { get; }
                public int pieceIndex { get; }
                public byte[] content { get; }
            }

            public void Add(PkgPice pps)
            {
                _package.Add(pps);
                _package.Sort((a, b) => a.pieceIndex.CompareTo(b.pieceIndex));

                if (_package.Count == _package[0].pkgLength)
                {
                    packed?.Invoke(this);
                }
            }

            public static SimplePackage SplitIntoPieces(byte[] data)
            {
                var pkg = new SimplePackage();
                int totalPieces = (int)Math.Ceiling(data.Length / (double)(128 * 1024));

                for (int i = 0; i < totalPieces; i++)
                {
                    int offset = i * (128 * 1024);
                    int remaining = data.Length - offset;
                    int chunkSize = Math.Min((128 * 1024), remaining);

                    byte[] chunk = new byte[chunkSize];
                    Buffer.BlockCopy(data, offset, chunk, 0, chunkSize);

                    pkg.Add(new PkgPice(pkg.packageID, totalPieces, i, chunk));
                }
                return pkg;
            }
        }


        public class SocketHandler
        {
            private List<User> _userList = new(0);

            public void Add(User usr)
            {
                _userList.Add(usr);
                usr.buffer = new byte[128 * 1024];
                usr.ListenTask = usr.socket.ReceiveAsync(usr.buffer);
            }

            private async Task Listen() // стартовать внутри основной проги
            {
                while (true)
                {
                    var a = await Task.WhenAny(_userList.Select(s => s.ListenTask));
                    var usr = _userList.First(p => p.ListenTask == a);
                    var pkgOp = usr.packageOp;
                    if (pkgOp == null) { continue; }
                    pkgOp.CollectPkg(usr.buffer);
                }
            }

            public static bool Send(Socket sckt, SimplePackage pkg)
            {
                return false; // заменить на норм код
            }
        }

        public interface User
        {
            Socket socket { get; set; }
            Task<int> ListenTask { get; set; }
            byte[] buffer { get; set; }

            // здесь же хранятся ключи кодирования и все остальное (все, что нужно для шифрования), как это хранить позже придумаю

            PackageOperator packageOp { get; set; }
        }
        */




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
            private ConcurrentBag<User> _userList;

            public ConnectionController(ConcurrentBag<User> usrlst, c_state state = c_state.user)
            {
                _state = state;
                _userList = usrlst;
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

                    _userList.Add(new User(clientSocket));
                    var cl = _userList.First(s => s.socket == clientSocket);
                    cl.ListenTask = clientSocket.ReceiveAsync(cl.buffer);
                }
            }

            public async Task ListenAsync()
            {
                await EventController.Listen(_userList.Select(user => user.ListenTask).ToList(), ReciveMessage);
            }

            public async Task ReciveMessage(Task reciving)
            {
                var usr = _userList.First(t => t.ListenTask == reciving);

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


        public class User(Socket sk)
        {
            public readonly Socket socket = sk;
            public Task ListenTask;
            public readonly byte[] buffer = new byte[128 * 1024];

            public readonly List<Package> incoming = new();
            public readonly List<Package> outgoing = new();
        }
    }
}