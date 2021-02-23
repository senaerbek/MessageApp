using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalR.Models;

namespace SignalR.Context
{
    public class SignalRDbContext : IdentityDbContext<User>
    {
        public SignalRDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder){

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RoomUser>().HasKey(k=> new {k.RoomId,k.UserId});

        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<RoomUser> RoomUsers { get; set; }
    }
}