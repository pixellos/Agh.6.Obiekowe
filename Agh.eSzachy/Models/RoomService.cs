using Agh.eSzachy.Data;
using Agh.eSzachy.Models;
using LanguageExt;
using LanguageExt.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agh
{
    public class RoomEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<ApplicationUser> ActiveUsers { get; set; }
        public virtual ICollection<MessageEntity> Messages { get; set; }
    }

    public class MessageEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public virtual ApplicationUser ClientId { get; set; }
        public DateTime Date { get; set; }
        [StringLength(128)]
        public string Message { get; set; }
    }

    public class RoomService : IRoomService
    {
        public RoomService(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; }

        public Result<Unit> Join(Client c, Room r) => throw new System.NotImplementedException();
        public Result<Unit> Left(Client c, Room r) => throw new System.NotImplementedException();
        public Result<Client[]> Members(Room r) => throw new System.NotImplementedException();
        public Result<Unit> SendMessage(Room r, Client c, string m) => throw new System.NotImplementedException();
        public Result<Room[]> Status(Client c)
        {
            var basicRooms = new[]
            {
                new Room()
                {
                    Id = "Global",
                    Name = "Global",
                    Created = DateTime.Parse("10/10/2020")
                }
            };
            var activeRooms = this.DbContext.Rooms.Where(x => x.ActiveUsers.Any(u => u.Id == c.Id));
            var results = activeRooms.Select(x => new Room()
            {
                Id = x.Id,
                Name = x.Title,
                Created = x.CreateDate,

            }).ToArray().Concat(basicRooms).ToArray();
            return new Result<Room[]>(results);
        }
    }
}