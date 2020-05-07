using Agh.eSzachy.Models;
using IdentityServer4.EntityFramework.Options;
using Innofactor.EfCoreJsonValueConverter;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

            builder.Entity<RoomEntity>().HasMany(x => x.Games).WithOne(x => x.Room).HasForeignKey(x => x.RoomId);

            builder.Entity<GameEntity>().HasOne(x => x.PlayerOne).WithMany().HasForeignKey(x => x.PlayerOneId).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<GameEntity>().HasOne(x => x.PlayerTwo).WithMany().HasForeignKey(x => x.PlayerTwoId).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);

            builder.AddJsonFields();
        }
    }
}
