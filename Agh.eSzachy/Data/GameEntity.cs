using Agh.eSzachy.Models;
using Innofactor.EfCoreJsonValueConverter;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agh.eSzachy.Data
{
    public enum Player
    {
        One = 0,
        Two = 1
    }

    public class PositionEntity
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
    public class MoveJsonEntity
    {
        public Player Player { get; set; }
        public PositionEntity? From { get; set; } = new PositionEntity();
        public PositionEntity? To { get; set; } = new PositionEntity();
    }

    public class GameEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string RoomId { get; set; }
        public RoomEntity Room { get; set; }
        public string? PlayerOneId { get; set; }
        public ApplicationUser PlayerOne { get; set; }
        public string? PlayerTwoId { get; set; }
        public ApplicationUser PlayerTwo { get; set; }

        [JsonField]
        public List<MoveJsonEntity> Moves { get; set; }

        public GameState State { get; set; }
    }

    public enum GameState
    {
        Waiting = 0,
        InPlay = 1,
        Finished = 2
    }
}