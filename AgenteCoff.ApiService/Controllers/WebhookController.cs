using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public IActionResult Post(WebhookPayload payload)
        {
            Console.WriteLine(payload.PackageName);
            Console.WriteLine(payload.AppName);
            Console.WriteLine(payload.Title);
            Console.WriteLine(payload.Text);
            return Ok(new { message = "Webhook received successfully", data = payload });
        }
    }
}
