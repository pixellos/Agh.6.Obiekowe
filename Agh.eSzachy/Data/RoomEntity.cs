using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agh.eSzachy.Data
{
    public class RoomEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<RoomUsers> ActiveUsers { get; set; }
        public ICollection<MessageEntity> Messages { get; set; }

        public virtual ICollection<GameEntity> Games { get; set; }
    }
}