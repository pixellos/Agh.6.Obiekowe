using System.ComponentModel.DataAnnotations.Schema;

namespace Agh
{
    public class GameEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string RoomId { get; set; }
        public RoomEntity Room { get; set; }
    }

    public class GameSnapshotEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
    
        
    }
}