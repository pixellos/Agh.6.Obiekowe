using System;
using System.Collections.Generic;
using Agh.eSzachy.Models.Chess;

namespace Agh.eSzachy.Services
{
    public enum GameStateModel
    {
        Waiting = 0,
        InPlay = 1,
        Finished = 2
    }

    public abstract class ChessBoardBase
    {
        public string PlayerOneName { get; set; }
        public string PlayerOneId { get; set; }

        public string PlayerTwoName { get; set; }
        public string PlayerTwoId { get; set; }
        public DateTime Started { get; set; }
        public DateTime LastMove { get; set; }
        public GameStateModel State { get; set; }

    }

    public class ChessBoardHistoryModel: ChessBoardBase
    {
        public Dictionary<DateTime, Dictionary<Position, BasePawn>> BoardInTime { get; set; } = new Dictionary<DateTime, Dictionary<Position, BasePawn>>();
    }

    public class ChessBoardModel : ChessBoardBase
    {
       
        public Player CurrentPlayer { get; set; }
        public Dictionary<Position, BasePawn> Board { get; set; } = new Dictionary<Position, BasePawn>();
    }
}