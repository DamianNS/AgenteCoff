using AgenteCoff.ApiService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgenteCoff.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonajesController(UserService userService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPersonajes()
        {
            var user = await userService.GetUser();
            if (user == null)
            {
                return Unauthorized();
            }

            // Aquí iría la lógica para obtener los personajes desde la base de datos o cualquier otra fuente de datos.
            // Por ahora, devolveremos una lista de ejemplo.
            var personajes = new List<object>
            {
                new { Id = 1, Nombre = user.UserName, Descripcion = "Descripción del personaje 1" },
                new { Id = 2, Nombre = "Personaje 2", Descripcion = "Descripción del personaje 2" },
                new { Id = 3, Nombre = "Personaje 3", Descripcion = "Descripción del personaje 3" }
            };
            return Ok(personajes);
        }
    }
}
