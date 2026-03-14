using AVcontrol;
using System;

namespace Shared.Source.NetDriver.AB
{

    internal class PackageFragment
    {
        public enum Types
        {
            Request = 0,
            Answer = 1,
        }
        public readonly byte[] content;
        public readonly Types type;
        public int size {  get => 1 + 4 + content.Length; }
        public byte[] Pack { 
            get
            {
                byte[] msg = new byte[size];
                Buffer.BlockCopy(ToBinary.LittleEndian(content.Length), 0, msg, 0, 4);
                Buffer.BlockCopy(ToBinary.LittleEndian<Types>(type), 0, msg, 4, 1);
                Buffer.BlockCopy(content, 0, msg, 4 + 1, content.Length);
                return msg;
            } 
        }

        public PackageFragment(byte[] content, Types type)
        {
            if (content.Length > int.MaxValue - 4)
                throw new ArgumentException($"Content length exceeds maximum allowed {int.MaxValue} bytes.", nameof(content));

            this.content = content;
            this.type = type;
        }

        public static PackageFragment Decode(byte[] mesg)
        {
            int cl = FromBinary.LittleEndian<int>(mesg.AsSpan(0, 4).ToArray());
            Types type = FromBinary.LittleEndian<Types>(mesg.AsSpan(4, 1).ToArray());
            byte[] content = new byte[cl];
            Buffer.BlockCopy(mesg, 5, content, 0, cl);
            return new PackageFragment(content, type);
        }
    }
}
