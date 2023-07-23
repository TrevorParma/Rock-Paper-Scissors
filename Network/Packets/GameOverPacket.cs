using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors.Network.Packets
{
    public class GameOverPacket : Packet
    {
        public const int GameOverOffset = 2;
        public const int WinnerOffset = 3;

        public bool GameOver { get; init; }
        public byte Winner { get; init; }

        public GameOverPacket(bool gameOver, byte winner) : base(PacketType.GameOver, 4)
        {
            GameOver = gameOver;
            Winner = winner;
        }

        public GameOverPacket(byte[] data) : base(data)
        {
            byte gameOverByte = data[GameOverOffset];
            if (gameOverByte == 0)
                GameOver = false;
            else
                GameOver = true;

            Winner = data[WinnerOffset];
        }

        public override byte[] ToByteArray()
        {
            byte[] array = base.ToByteArray();
            if (!GameOver)
                array[GameOverOffset] = 0;
            else
                array[GameOverOffset] = 1;
            array[WinnerOffset] = Winner;

            return array;
        }
    }
}
