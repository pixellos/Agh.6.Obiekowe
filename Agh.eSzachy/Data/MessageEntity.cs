using Agh.eSzachy.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agh.eSzachy.Data
{
    public class MessageEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ClientId { get; set; }
        public virtual ApplicationUser Client { get; set; }
        public DateTime Date { get; set; }
        [StringLength(128)]
        public string Message { get; set; }
    }
}