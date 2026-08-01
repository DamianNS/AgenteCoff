using Microsoft.EntityFrameworkCore;
using AgenteCoff.ApiService.Data.Models;

namespace AgenteCoff.ApiService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecastEntity> WeatherForecasts => Set<WeatherForecastEntity>();
        public DbSet<Aviso> Avisos { get; set; }
        public DbSet<NotifyDTO> Notify { get; set; }
    }
}
