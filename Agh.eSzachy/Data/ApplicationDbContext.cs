using Agh.eSzachy.Models;
using Agh;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agh.eSzachy.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<GameEntity> Games { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<RoomUsers> RoomUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RoomUsers>().HasKey(x => new { x.RoomId, x.UserId });
            builder.Entity<RoomUsers>().HasOne(x => x.Room).WithMany(x => x.ActiveUsers).HasForeignKey(x => x.RoomId);
            builder.Entity<RoomUsers>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);

            builder.Entity<GameEntity>().HasOne(x => x.Room).WithMany(x => x.ArchivedGames).HasForeignKey(x => x.RoomId);
            builder.Entity<RoomEntity>().HasOne(x => x.ActualGame).WithOne();
        }
    }
}
