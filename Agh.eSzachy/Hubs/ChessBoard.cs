namespace Agh.eSzachy.Hubs
{
    public enum BoardState
    {
        Started = 0,
        PlayerOneWins = 1,
        PlayerTwoWins = 2,
        Draw = 3,
    }
    
    public class ChessBoard
    {
        public Pawn[] Pawns { get; set; }
    }
}