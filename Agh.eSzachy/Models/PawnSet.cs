using System.Collections.Generic;

namespace Agh
{
    public class PawnSet
    {
        public string OwnerId { get; set; }
        public ICollection<PawnWithPosition> Positions {get;set;}
    }
}