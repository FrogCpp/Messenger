using System;
using System.Net.Sockets;
using System.Text.Json;

namespace JabNet
{
    public static class NetDriver
    {
        public class PackageOperator(Action<SimplePackage> F)
        {
            private readonly List<SimplePackage> _incoming = new(0);
            private readonly List<SimplePackage> _outcoming = new(0);
            private readonly Action<SimplePackage> _f = F;

            public void AcceptPkg(SimplePackage sp) // будет добавлено делегату
            {
                // вот здесь переделать из пакета в таску (чуть позже)

                _f(sp);
                _incoming.Remove(sp);
                sp.packed -= AcceptPkg;
            }

            public void CollectPkg(byte[] inf) // будет делегатом для слушателя
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
                    pkgLenght = lnght;
                    accountPosition = pose;
                    content = cntnt;
                }

                public Guid unicPkgID { get; }
                public int pkgLenght { get; }
                public int accountPosition { get; }
                public byte[] content { get; }
            }

            public void Add(PkgPice pps)
            {
                _package.Add(pps);
                _package.Sort((a, b) => a.accountPosition.CompareTo(b.accountPosition));

                if (_package.Count == _package[0].pkgLenght)
                {
                    packed.Invoke(this);
                }
            }
        }


        public class SocketHandler
        {
            private List<User> _userList;
            private List<Task<>> _taskList;

            public void Add(User usr)
            {
                _userList.Add(usr);
            }

            private async Task Listen()
            {

            }
        }

        public interface User
        {
            Socket socket { get; set; }
            Task ListenTask { get; set; }

            // здесь же хранятся ключи кодирования и все остальное (все, что нужно для шифрования), как это хранить позже придумаю

            PackageOperator packageOp { get; set; }
        }
    }
}