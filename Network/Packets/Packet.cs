using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors.Network.Packets
{
    public class Packet
    {
        public const byte SizeOffset = 0;   // First byte in a packet represents the size in bytes.
        public const byte TypeOffset = 1;   // Second byte in a packet represents the packet type.

        public byte Size { get; init; }
        public PacketType Type { get; init; }

        public Packet()
        {
            Size = 2;
            Type = PacketType.Unknown;
        }

        public Packet(PacketType type)
        {
            Size = 2;
            Type = type;
        }

        public Packet(PacketType type, byte size)
        {
            Size = size;
            Type = type;
        }

        public Packet(byte[] data)
        {
            Size = data[SizeOffset];
            Type = (PacketType)data[TypeOffset];
        }

        public virtual byte[] ToByteArray()
        {
            byte[] array = new byte[Size];
            array[SizeOffset] = Size;
            array[TypeOffset] = (byte)Type;

            return array;
        }
    }
}
