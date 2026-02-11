using Microsoft.EntityFrameworkCore;
using RuangBooking.Models;

namespace RuangBooking.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Daftarkan model WeatherForecast agar jadi tabel di MySQL
    public DbSet<WeatherForecast> Forecasts { get; set; }
}