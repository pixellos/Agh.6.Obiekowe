using System.Collections.Generic;
using System.Diagnostics;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Agh.eSzachy.Models.Chess
{
    public abstract class BasePawn
    {
        public readonly Player player;
        private readonly bool isColiding;

        public BasePawn(Player player, bool isColiding = true)
        {
            this.player = player;
            this.isColiding = isColiding;
        }

        public abstract IEnumerable<Position> GetPositions(Position startingPosition);
    }
}