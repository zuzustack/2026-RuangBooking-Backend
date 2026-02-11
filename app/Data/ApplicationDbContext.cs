using Microsoft.EntityFrameworkCore;
using RuangBooking.Models;

namespace RuangBooking.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BookRoom> BookedRooms { get; set; }

}