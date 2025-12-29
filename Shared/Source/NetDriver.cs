namespace JabNet
{
    internal class Package : List<PackageDetail>
    {
        private struct PackageDetail
        {
            public PackageDetail(uint16 sn, byte[] cntnt, Guid gd)
            {
                serialNumber = sn;
                packageOwnership = gd;
                content = cntnt;
            }

            public uint16 serialNumber { get; private set; }
            public Guid packageOwnership { get; private set; }
            public byte[] content { get; private set; } = new byte[128 * 1024];
        }

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
            }

            Guid id = Guid.NewGuid();

            byte[] buffer = new(128 * 1024);

            int i = 0;
            int cnt = 0;
            for (var i in content)
            {
                buffer[i] = i;
                if (i == 128 * 1024)
                {
                    i = 0;
                    this.Add(new PackageDetail(cnt, buffer, id));
                }
                else
                {
                    i++;
                }
            }

            if (cnt < 1)
            {
                this.Add(new PackageDetail(cnt, buffer, id));
            }
        }

        public byte[] ReceivePackage(uint16 numb)
        {
            PackageDetail a = this.Find(pd => pd.serialNumber == numb);

            return Encoding.Unicode.GetBytes(JsonSerializer.Serialize<PackageDetail>(a));
        }
    }
}