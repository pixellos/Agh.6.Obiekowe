using System;

namespace Agh.eSzachy.Models.Chess
{
    public class Position
    {
        public Position(int row, int col)
        {
            this.Row = row;
            this.Column = col;
        }
        public Position()
        {

        }
        public int Row { get; set; }
        public int Column { get; set; }

        public const int LastColumn = 7;
        public const int LastRow = 7;

        public override bool Equals(object? obj) => obj is Position position && this.Row == position.Row && this.Column == position.Column;
        public override int GetHashCode() => HashCode.Combine(this.Row, this.Column);
    }
}