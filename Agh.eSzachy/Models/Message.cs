using System;

namespace Agh.eSzachy.Models
{
    public class Message
    {
        public string Text { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public string? UserName { get; internal set; }
    }
}