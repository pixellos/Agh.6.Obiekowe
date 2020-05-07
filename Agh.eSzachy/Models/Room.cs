using System;
using System.Collections.Generic;

namespace Agh.eSzachy.Models
{
    public class Room
    {
        public List<Message> Messages { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }
}