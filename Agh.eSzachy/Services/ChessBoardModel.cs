using System;
using System.Collections.Generic;
using Agh.eSzachy.Models.Chess;

namespace Agh.eSzachy.Services
{
    public class ChessBoardModel
    {
        public object State { get; set; }
        public Dictionary<Position, BasePawn> Player { get; set; }
        public Dictionary<Position, BasePawn> Enemy { get; set; }
        public DateTime Started { get; set; }
        public DateTime LastMove { get; set; }
    }
}