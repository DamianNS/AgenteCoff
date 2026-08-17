using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgenteCoff.ApiService.Controllers
{
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        [HttpGet("/")]
        public IActionResult GetRoot()
        {
            return Ok("API service is running. Navigate to /weatherforecast to see sample data.");
        }

        [Authorize]
        [HttpGet("/weatherforecast")]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]
                ))
                .ToArray();
            return forecast;
        }

        public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
        {
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        }
    }
}
