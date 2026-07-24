using Microsoft.AspNetCore.Mvc;

namespace AgenteCoff.ApiService.Controllers
{
    [Route("api/Webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        public class WebhookPayload
        {
            public string titulo { get; set; }
            public string body { get; set; }
        }

        [HttpPost]
        public IActionResult Post(WebhookPayload payload)
        {
            Console.WriteLine(payload.titulo);
            Console.WriteLine(payload.body);
            return Ok(new { message = "Webhook received successfully", data = payload });
        }
    }
}
