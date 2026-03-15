using System;
using AVcontrol;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices.Marshalling;

namespace Shared.Source.NetDriver.AC
{
    public class Message
    {
        public readonly Guid msgsuid;
        public readonly byte[] content;
        public int size 
        { 
            get 
            {
                return 4 + 4 + msgsuid.ToByteArray().Length + content.Length;
            }
        }
        public byte[] pack
        {
            get
            {
                byte[] p = new byte[size];

                Buffer.BlockCopy(ToBinary.LittleEndian(content.Length), 0, p, 0, 4);                    // запись длинны контента
                Buffer.BlockCopy(ToBinary.LittleEndian(msgsuid.ToByteArray().Length), 0, p, 4, 4);      // запись длинны айдишнника
                Buffer.BlockCopy(content, 0, p, 4 + 4, content.Length);
                Buffer.BlockCopy(msgsuid.ToByteArray(), 0, p, 4 + 4 + content.Length, msgsuid.ToByteArray().Length);

                return p;
            }
        }


        public Message(Guid? suid, byte[] content)
        {
            if (content.Length > int.MaxValue - 8)
                throw new Exception($"content size ({content.Length}) bigger then limit ({int.MaxValue - 8})");

            msgsuid = suid.HasValue ? suid.Value : Guid.NewGuid();
            this.content = content;
        }
        public Message(byte[] pack)
        {
            int contentSize = FromBinary.LittleEndian<int>(pack.AsSpan(0, 4).ToArray());
            int idSize = FromBinary.LittleEndian<int>(pack.AsSpan(4, 4).ToArray());
            var idBuffer = new byte[idSize];
            Buffer.BlockCopy(pack, 4 + 4 + contentSize, idBuffer, 0, idSize);
            msgsuid = new Guid(idBuffer);
            content = new byte[contentSize];
            Buffer.BlockCopy(pack, 4 + 4, content, 0, contentSize);
        }
        public static sizeConf PartialParse(byte[] pack)
        {
            var sc = new sizeConf();
            sc.contentSize = FromBinary.LittleEndian<int>(pack.AsSpan(0, 4).ToArray());
            sc.idSize = FromBinary.LittleEndian<int>(pack.AsSpan(4, 4).ToArray());

            return sc;
        }
        public struct sizeConf
        {
            public int idSize;
            public int contentSize;
            public int size
            {
                get
                {
                    return idSize + contentSize;
                }
            }
        }
    }
    public enum Appointment
    {
        Send,
        Read
    }
}
