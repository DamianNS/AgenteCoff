using AgenteCoff.ApiService.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgenteCoff.ApiService.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecastEntity> WeatherForecasts => Set<WeatherForecastEntity>();
        public DbSet<Aviso> Avisos { get; set; }
        public DbSet<NotifyDTO> Notify { get; set; }
    }
}
