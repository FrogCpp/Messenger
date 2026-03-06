using AVcontrol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Source.NetDriver.AB
{

    internal class PackageFragment
    {
        public readonly UInt32 sn;
        public readonly byte[] uid;
        public readonly byte[] content;
        public UInt64 size {  get => (ushort)(2 + 2 + 4 + content.Length + uid.Length); }
        public byte[] pack { 
            get
            {
                byte[] msg = new byte[size];
                Buffer.BlockCopy(ToBinary.LittleEndian((UInt16)content.Length), 0, msg, 0, 2);
                Buffer.BlockCopy(ToBinary.LittleEndian((UInt16)uid.Length), 0, msg, 2, 2);
                Buffer.BlockCopy(ToBinary.LittleEndian(sn), 0, msg, 4, 4);
                Buffer.BlockCopy(uid, 0, msg, 8, uid.Length);
                Buffer.BlockCopy(content, 0, msg, 8 + uid.Length, content.Length);
                return msg;
            } 
        }

        public PackageFragment(Guid? uid, byte[] content, UInt32 sn)
        {
            if (content.Length > ushort.MaxValue)
                throw new ArgumentException($"Content length exceeds maximum allowed {ushort.MaxValue} bytes.", nameof(content));

            this.content = content;
            this.uid = uid.HasValue ? uid.Value.ToByteArray() : Guid.NewGuid().ToByteArray();
            this.sn = sn;
        }

        public static PackageFragment Decode(byte[] mesg)
        {
            UInt16 cl = FromBinary.LittleEndian<UInt16>(mesg.AsSpan(0, 2).ToArray());
            UInt16 uidl = FromBinary.LittleEndian<UInt16>(mesg.AsSpan(2, 2).ToArray());
            UInt32 sn = FromBinary.LittleEndian<UInt32>(mesg.AsSpan(4, 4).ToArray());
            byte[] uid = mesg.AsSpan(8, uidl).ToArray();
            byte[] content = mesg.AsSpan(8 + uidl, cl).ToArray();
            return new PackageFragment(new Guid(uid), content, sn);
        }
    }
}
