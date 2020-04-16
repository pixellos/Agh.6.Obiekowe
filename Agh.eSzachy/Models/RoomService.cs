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
        public string ClientId { get; set; }
        public virtual ApplicationUser Client { get; set; }
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
        public Result<Unit> SendMessage(Room r, Client c, string m)
        {
            var room = this.DbContext.Rooms.FirstOrDefault(x => x.Id == r.Id);
            if (room != null)
            {
                room.Messages.Add(new MessageEntity
                {
                    Date = DateTime.Now,
                    ClientId = c.Id,
                    Message = m
                });
                this.DbContext.SaveChanges();
                return new Result<Unit>();
            }
            return new Result<Unit>(new ArgumentException(this.GetType().FullName, nameof(c)));
        }

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
                Messages = x.Messages.Select(x => new Message()
                {
                    UserId = x.Client.Id,
                    Created = x.Date,
                    Text = x.Message
                }).ToList()
            }).ToArray().Concat(basicRooms).ToArray();
            return new Result<Room[]>(results);
        }
    }
}