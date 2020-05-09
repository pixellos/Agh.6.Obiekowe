using Agh.eSzachy.Models;
using Agh.eSzachy.Models.Chess;

namespace Agh.eSzachy.Hubs
{

    public class PlayerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class ChessBoardDto
    {
        public PlayerDto PlayerOne { get; set; } = new PlayerDto();
        public PlayerDto PlayerTwo { get; set; } = new PlayerDto();
        public Client Client { get; set; }

        public Player Player { get; set; }
        public BoardState State { get; set; }
        public Pawn[] Pawns { get; set; }
    }
}