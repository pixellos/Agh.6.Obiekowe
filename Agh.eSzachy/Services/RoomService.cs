using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agh.eSzachy.Data;
using Agh.eSzachy.Models;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Agh.eSzachy.Services
{
    public class RoomService : IRoomService
    {
        public RoomService(ApplicationDbContext dbContext, IUserStore<ApplicationUser> userStore)
        {
            this.DbContext = dbContext;
            this.UserStore = userStore;
        }

        public ApplicationDbContext DbContext { get; }
        public IUserStore<ApplicationUser> UserStore { get; }

        public Result<Room> Create(Client c, Room r)
        {
            var user = this.UserStore.FindByIdAsync(c.Id, CancellationToken.None).Result;
            var existing = this.DbContext.Rooms.FirstOrDefault(x => x.Title == r.Name);
            if (existing == null)
            {
                var re = new RoomEntity
                {
                    Title = r.Name,
                    CreateDate = DateTime.Now,
                    ActiveUsers = new List<RoomUsers>()
                };
                this.DbContext.Rooms.Add(re);
                re.ActiveUsers.Add(new RoomUsers
                {
                    User = user
                });
                try
                {
                    this.DbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    throw;
                }
                return new Result<Room>(Map(re));
            }
            else
            {
                return new Result<Room>(new Exception($"{nameof(Room)} with given {nameof(Room.Name)} already exists."));
            }
        }

        public Result<Room> Join(Client c, string roomName)
        {
            var room = this.DbContext.Rooms.Include(x => x.ActiveUsers).FirstOrDefault(x => x.Title == roomName);
            if (room != null)
            {
                var user = this.UserStore.FindByIdAsync(c.Id, CancellationToken.None).Result;
                if (room.ActiveUsers.All(x => x.UserId != user.Id))
                {
                    room.ActiveUsers.Add(user.ToRoomUsers());
                }

                this.DbContext.SaveChanges();
                return Map(room);
            }
            else
            {
                return new Result<Room>(new ArgumentException(this.GetType().FullName, nameof(c)));
            }
        }

        public Result<Room> Left(Client c, string roomName)
        {
            var room = this.DbContext.Rooms.FirstOrDefault(x => x.Title == roomName);
            if (room != null)
            {
                var user = this.UserStore.FindByIdAsync(c.Id, CancellationToken.None).Result;
                var isRemoved = room.ActiveUsers.Remove(user.ToRoomUsers());
                if (isRemoved)
                {
                    this.DbContext.SaveChanges();
                    return Map(room);
                }
            }
            return new Result<Room>(new ArgumentException(this.GetType().FullName, nameof(c)));
        }

        public Result<Room> SendMessage(Room r, Client c, string m)
        {
            var room = this.DbContext.Rooms.Include(x => x.Messages).FirstOrDefault(x => x.Title == r.Id);
            var user = this.UserStore.FindByIdAsync(c.Id, CancellationToken.None).Result;
            if (room != null)
            {
                room.Messages.Add(new MessageEntity
                {
                    Date = DateTime.Now,
                    ClientId = user.Id,
                    Message = m
                });
                this.DbContext.SaveChanges();
                var updateRoom = this.DbContext.Rooms.Single(x => x.Id == room.Id);
                var result = Map(updateRoom);
                return new Result<Room>(result);
            }
            else
            {
                return new Result<Room>(new ArgumentException(this.GetType().FullName, nameof(c)));
            }
        }

        public async Task<Result<Room[]>> Status(Client c)
        {
            var withUsers = this.DbContext.Rooms.Include(x => x.ActiveUsers);
            var user = await this.UserStore.FindByIdAsync(c.Id, CancellationToken.None);
            if (user == null)
            {
                return new Result<Room[]>(new Room[] { });
            }
            var activeRooms = withUsers.Where(x => x.ActiveUsers.Any(u => u.UserId == user.Id)).Include(x => x.Messages);
            var results = activeRooms.Select(x => Map(x)).ToArray();
            return new Result<Room[]>(results);
        }

        private static Room Map(RoomEntity x) => new Room()
        {
            Id = x.Id,
            Name = x.Title,
            Created = x.CreateDate,
            Messages = x.Messages?.Select(x => new Message()
            {
                UserId = x.Client.Id,
                Created = x.Date,
                Text = x.Message
            }).ToList() ?? new List<Message>()
        };

        public Result<string[]> GetAllRoomNames()
        {
            var results = this.DbContext.Rooms.Select(x => x.Title).ToArray();
            return results;
        }

        public async Task<Room> Get(string id) => Map(this.DbContext.Rooms.First(x => x.Title == id));
    }
}