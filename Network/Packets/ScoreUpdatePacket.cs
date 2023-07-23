using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors.Network.Packets
{
    public class ScoreUpdatePacket : Packet
    {
        public const byte Player1ScoreOffset = 2;
        public const byte Player2ScoreOffset = 3;
        
        public int Player1Score { get; init; }
        public int Player2Score { get; init; }

        public ScoreUpdatePacket(int player1Score, int player2Score) : base(PacketType.ScoreUpdate, 4)
        {
            Player1Score = player1Score;
            Player2Score = player2Score;
        }

        public ScoreUpdatePacket(byte[] data) : base(data)
        {
            Player1Score = data[Player1ScoreOffset];
            Player2Score = data[Player2ScoreOffset];
        }

        public override byte[] ToByteArray()
        {
            byte[] array = base.ToByteArray();
            array[Player1ScoreOffset] = (byte)Player1Score;
            array[Player2ScoreOffset] = (byte)Player2Score;

            return array;
        }
    }
}
