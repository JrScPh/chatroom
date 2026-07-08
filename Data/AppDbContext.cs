using Microsoft.EntityFrameworkCore;
using ChatroomAPI.Models;

namespace ChatroomAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
    }
}
