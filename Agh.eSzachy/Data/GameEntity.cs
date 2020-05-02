using System.ComponentModel.DataAnnotations.Schema;

namespace Agh.eSzachy.Data
{
    public class GameEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string RoomId { get; set; }
        public RoomEntity Room { get; set; }
    }
}