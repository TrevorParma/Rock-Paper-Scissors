﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors.Network.Packets
{
    public enum PacketType
    {
        Unknown,
        ThrowRequest,
        ThrowSend,
        ScoreUpdate,
        ScoreUpdateResponse,
        GameOver,
        GameOverResponse,
        Rematch
    }
}
