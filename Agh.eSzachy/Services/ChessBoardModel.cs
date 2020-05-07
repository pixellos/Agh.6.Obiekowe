using System;
using System.Collections.Generic;
using Agh.eSzachy.Models.Chess;

namespace Agh.eSzachy.Services
{
    public class ChessBoardModel
    {
        public Dictionary<Position, BasePawn> Board { get; set; }
        public DateTime Started { get; set; }
        public DateTime LastMove { get; set; }
    }
}