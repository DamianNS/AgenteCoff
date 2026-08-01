using Microsoft.AspNetCore.Mvc;

using AgenteCoff.ApiService.Data;
using AgenteCoff.ApiService.Data.Models;
using System.Threading.Tasks;
using System;

namespace AgenteCoff.ApiService.Controllers
{
    [Route("api/Webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        public class WebhookPayload
        {
            public string? PackageName { get; set; }
            public string? AppName { get; set; }
            public string? Title { get; set; }
            public string? Text { get; set; }
        }

        private readonly AppDbContext _db;

        public WebhookController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WebhookPayload payload)
        {
            Console.WriteLine(payload.PackageName);
            Console.WriteLine(payload.AppName);
            Console.WriteLine(payload.Title);
            Console.WriteLine(payload.Text);

            var entity = new NotifyDTO
            {
                PackageName = payload.PackageName ?? string.Empty,
                AppName = payload.AppName,
                Title = payload.Title,
                Text = payload.Text,
                ReceivedAt = DateTime.UtcNow
            };

            await _db.Notify.AddAsync(entity);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Webhook received and saved", data = entity });
        }
    }
}
