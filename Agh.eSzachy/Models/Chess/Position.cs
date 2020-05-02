namespace Agh.eSzachy.Models.Chess
{
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public const int LastColumn = 7;
        public const int LastRow = 7;
    }
}