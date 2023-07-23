using Rock_Paper_Scissors.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors.Network.Packets
{
    public class ThrowPacket : Packet
    {
        public const int ThrowOffset = 2;   // 3rd byte is the throw

        public PlayerThrow Throw { get; init; }

        public ThrowPacket(PlayerThrow pThrow) : base(PacketType.ThrowSend, 3)
        {
            Throw = pThrow;
        }

        public ThrowPacket(byte[] data) : base(data)
        {
            Throw = (PlayerThrow)data[ThrowOffset];
        }

        public override byte[] ToByteArray()
        {
            byte[] array = base.ToByteArray();
            array[ThrowOffset] = (byte)Throw;

            return array;
        }
    }
}
