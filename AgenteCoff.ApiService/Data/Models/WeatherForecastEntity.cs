using System;
using System.ComponentModel.DataAnnotations;

namespace AgenteCoff.ApiService.Data.Models
{
    public class WeatherForecastEntity
    {
        [Key]
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public string? Summary { get; set; }
    }
}
