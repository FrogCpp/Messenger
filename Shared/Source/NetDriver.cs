namespace JabNet
{
    public class Package : List<PackageDetail>
    {
        public Package() { unic_id = Guid.NewGuid(); }

        public Package(Guid id) { unic_id = id; }

        internal struct PackageDetail
        {
            public PackageDetail(uint16 sn, byte[] cntnt, Guid gd)
            {
                serialNumber = sn;
                packageOwnership = gd;
                content = cntnt;
                allElementsCnt = -1;
            }

            public uint16 serialNumber { get; private set; }
            public uint16 allElementsCnt;
            public Guid packageOwnership { get; private set; }
            public byte[] content { get; private set; } = new byte[128 * 1024];
        }

        public Guid unic_id { get; private set; }

        public byte[] Assemble()
        {
            var sorted = this.OrderBy(p => p.serialNumber).ToList();

            byte[] buffer = new(sorted.Sum(p => p.content.Length));
            int offset = 0;
            foreach (var item in sorted)
            {
                Buffer.BlockCopy(item.content, 0, buffer, offset, item.content.Length);
                offset += item.content.Length;
            }
            return buffer;
        }

        public bool Divide(byte[] content, bool force=false)
        {
            if (this.Count != 0)
            {
                if (!force) { return false; }
                this.Clear();
            }

            byte[] buffer = new(128 * 1024);

            int i = 0;
            int cnt = 0;
            for (var i in content)
            {
                buffer[i] = i;
                if (i == 128 * 1024)
                {
                    i = 0;
                    this.Add(new PackageDetail(cnt, buffer, unic_id));
                }
                else
                {
                    i++;
                }
            }

            if (cnt < 1)
            {
                this.Add(new PackageDetail(cnt, buffer, unic_id));
            }
            foreach (var i in this) { i.allElementsCnt = cnt; }
        }

        public byte[] ReceivePackage(uint16 numb)
        {
            PackageDetail a = this.Find(pd => pd.serialNumber == numb);

            return Encoding.Unicode.GetBytes(JsonSerializer.Serialize<PackageDetail>(a));
        }
    }



    public class PackageManager 
    {
        private readonly List<Package> _incoming = new(0);
        private readonly List<Package> _outcoming = new(0);

        public async Task GetPackage(byte[] bt)
        {
            Package.PackageDetail pkg = JsonSerializer.Deserialize<Package.PackageDetail>(Encoding.Unicode.GetString(bt));
            var a = _incoming.FindIndex(p => p.unic_id == pkg.serialNumber);
            if (a == -1)
            {
                _incoming.Add(new Package(pkg.serialNumber));
                a = _incoming.FindIndex(p => p.unic_id == pkg.serialNumber);
            }
            _incoming[a].Add(pkg);
        }

        public async Task PublishingPackages(Func<byte[], Task> F)
        {

        }
    }

    public class ConnectionManager
    {
        private Socket _socket;

        public ConnectionManager(Socket sck)
        {
            _socket = sck;
        }

        public async Task Listen(Func<byte[], Task> F)
        {

        }
    }
}