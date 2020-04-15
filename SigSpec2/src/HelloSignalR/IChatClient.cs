using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace HelloSignalR
{
    public class Board
    {
        internal bool CanMove(Point pos) => throw new NotImplementedException();
        internal bool IsEnemy(Point pos) => throw new NotImplementedException();
    }

    public class Tower : IPawn
    {

        public IEnumerable<Point> Moves(Point position, Board board)
        {
            var pos = new Point(position.X, position.Y);
            do
            {
                pos = new Point(position.X + 1, position.Y);
                yield return pos;
                if (board.IsEnemy(pos))
                {
                    break;
                }
            }
            while (board.CanMove(pos));
        }


    }

    public interface IPawn
    {
        IEnumerable<Point> Moves(Point position, Board board);
    }

    public class PawnSet
    {
        public string OwnerId { get; set; }
        public ICollection<PawnWithPosition> Positions {get;set;}
    }

    public class BoardState
    {
        public PawnSet Whites { get; set; }
        public PawnSet Blacks { get; set; }
    }

    public class Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }


    public enum MessageType
    {
        Leave,
        Win,
        Joined,
    }

    public interface IRoomClient
    {
        Task RoomStatus(string message, MessageType messageType);
    }
}