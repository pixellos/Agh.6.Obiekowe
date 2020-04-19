using System.ComponentModel.DataAnnotations.Schema;

namespace Agh
{
    public class GameEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
    }
}