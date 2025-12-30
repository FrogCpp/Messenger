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
                SimplePackage.PkgPice sp = JsonSerizlizer.Serialize<SimplePackage.PkgPice>(inf);
                var a = _incoming.FindIndex(p => p.packageID == sp.unicPkgID);
                if (a == -1) { _incoming.Add(new SimplePackage(sp.unicPkgID)); a = _incoming.FindIndex(p => p.packageID == sp.unicPkgID); }

                _incoming[a].Add()
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
                public PkgPice(Guid ID, uint16 lnght, uint16 pose, byte[] cntnt)
                {
                    unicPkgID = ID;
                    pkgLenght = lnght;
                    accountPosition = pose;
                    content = cntnt;
                }

                public Guid unicPkgID { get; }
                public uint16 pkgLenght { get; }
                public uint16 accountPosition { get; }
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
    }
}