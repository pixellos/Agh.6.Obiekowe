using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agh
{
    public class Pawn
    {
        public bool IsWhile { get; set; }
        public bool Type { get; set; }
        public bool Position { get; set; }

    }

    public class ChessBoardHistory
    {
        public Dictionary<DateTime, ChessBoard> History { get; set; }
    }

    public class ChessBoard
    {
        public Pawn[] Pawns { get; set; }
    }

    public interface IGameClient
    {
        Task Refresh(string roomName, ChessBoard cb);
    }
}