﻿using Agh.eSzachy.Models;

namespace Agh
{

    public class RoomUsers
    {
        public RoomEntity Room { get; set; }
        public string RoomId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}