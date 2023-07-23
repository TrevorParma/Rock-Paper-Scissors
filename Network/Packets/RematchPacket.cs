using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors.Network.Packets
{
    public class RematchPacket : Packet
    {
        private const byte _rematchOffset = 2;

        public bool Rematch { get; init; }

        public RematchPacket(bool rematch) : base(PacketType.Rematch, 3)
        {
            Rematch = rematch;
        }

        public RematchPacket(byte[] data) : base(data)
        {
            if (data[_rematchOffset] == 0)
                Rematch = false;
            else
                Rematch = true;
        }

        public override byte[] ToByteArray()
        {
            byte[] array = base.ToByteArray();
            if (!Rematch)
                array[_rematchOffset] = 0;
            else
                array[_rematchOffset] = 1;

            return array;
        }
    }
}
