using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Source.NetDriver.AC
{
    internal class MassiveContentBuilder
    {
        private static readonly string swapDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads");
        public readonly string pathToFolder;
        public readonly Guid FileGuid;
        public readonly int expectedQuantity = -1;
        private readonly Dictionary<int, Message> _hash = new();
        public MassiveContentBuilder(Guid fileGuid, int expectedQuantity, string fileName)
        {
            Directory.CreateDirectory(swapDir);
            FileGuid = fileGuid;
            this.expectedQuantity = expectedQuantity;
            pathToFolder = Path.Combine(swapDir, fileName);
            File.Create(pathToFolder).Dispose();
        }
        public async Task WritePackage(Message msg)
        {
            if (expectedQuantity + 1 == msg.serialNumber)
            {
                File.AppendAllBytes(pathToFolder, msg.content);
            }
            else
            {
                _hash.Add(msg.serialNumber, msg);
            }
            if (_hash.TryGetValue(expectedQuantity + 1, out var oMsg))
            {
                await WritePackage(oMsg);
            }
        }
    }
}
