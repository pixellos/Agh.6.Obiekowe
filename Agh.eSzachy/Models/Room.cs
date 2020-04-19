using System;
using System.Collections.Generic;

namespace Agh
{
    public class Message
    {
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
    }

    public class Room
    {
        public List<Message> Messages { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }

    }
}