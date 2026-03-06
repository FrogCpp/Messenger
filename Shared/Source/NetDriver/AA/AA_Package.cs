using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Source.NetDriver.AA
{
    public class Packet(Guid? id = null, Action<Packet>? callback = null)
    {
        public struct PacketFragment
        {
            public Guid packetID;
            public Int16 packetSize;
            public Int16 fragmentPosition;
            public Byte[] data;
        }

        public readonly List<PacketFragment> fragments = new(0);
        public readonly Guid packetID = id ?? Guid.NewGuid();
        public Int32 PacketSize { get { return fragments.Count * FragmentSize; } }
        public Int32 FragmentSize
        {
            get
            {
                if (fragments.Count == 0) return 0;
                return fragments[0].data.Length;
            }
        }
        public Action<Packet>? packetFinalized = callback;

        public bool Append(PacketFragment fragment)
        {
            if (fragment.packetID != packetID) return false;
            if (fragment.packetSize == fragments.Count) return false;
            fragments.Add(fragment);

            if (packetFinalized != null && fragments.Count == fragment.packetSize) packetFinalized(this);

            return true;
        }
    }



    public static class PacketBuilder
    {
        public static Packet ConvertToPacket(Byte[] rawData, Guid? id = null, ConnectionHandler? packetOwner = null)
        {
            var packet = new Packet(id, packetOwner != null ? packetOwner.OnPacketComplete : null);
            Int32 totalFragments = (Int32)Math.Ceiling(rawData.Length / (double)(128 * 1024));

            for (Int32 i = 0; i < totalFragments; i++)
            {
                Int32 startOffset = i * (128 * 1024);
                Int32 remainingBytes = rawData.Length - startOffset;
                Int32 fragmentBytes = Math.Min((128 * 1024), remainingBytes);

                Byte[] fragmentData = new Byte[fragmentBytes];
                Buffer.BlockCopy(rawData, startOffset, fragmentData, 0, fragmentBytes);

                var fragment = new Packet.PacketFragment
                {
                    packetID = packet.packetID,
                    packetSize = (Int16)totalFragments,
                    fragmentPosition = (Int16)i,
                    data = fragmentData
                };


                packet.Append(fragment);
            }
            return packet;
        }

        public static Byte[] ConvertToBytes(Packet packet)
        {
            var resultData = new Byte[packet.PacketSize];
            Int32 currentOffset = 0;

            packet.fragments.Sort((a, b) => a.fragmentPosition.CompareTo(b.fragmentPosition));

            for (Int32 i = 0; i < packet.fragments.Count; i++)
            {
                Buffer.BlockCopy(packet.fragments[i].data, 0, resultData, currentOffset, packet.FragmentSize);
                currentOffset += packet.FragmentSize;
            }

            return resultData;
        }
    }
}
