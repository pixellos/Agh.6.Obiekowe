using System.ComponentModel.DataAnnotations.Schema;

namespace Agh.eSzachy.Data
{
    public class GameSnapshotEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }


    }
}