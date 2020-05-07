using System;
using System.Collections.Generic;

namespace Agh.eSzachy.Hubs
{
    public class PawnPosition
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }
    public class ChessBoardHistory
    {
        public Dictionary<DateTime, ChessBoard> History { get; set; }
    }
}