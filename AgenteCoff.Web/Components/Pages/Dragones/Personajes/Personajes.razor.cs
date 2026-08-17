using Microsoft.AspNetCore.Components;

namespace AgenteCoff.Web.Components.Pages.Dragones.Personajes;

public partial class Personajes: ComponentBase
{
    [Inject]
    private CharacterService CharacterService { get; set; } = default!;

    private List<ServiceDefaults.Models.Dragones.Character> Characters = new List<ServiceDefaults.Models.Dragones.Character>();

    protected override void OnInitialized()
    {
        LoadCharacters();
    }

    private void LoadCharacters()
    {
        CharacterService.GetCharactersAsync().ContinueWith(task =>
        {
            if (task.Exception == null)
            {
                Characters = task.Result;
                InvokeAsync(StateHasChanged);
            }
            else
            {
                // Manejar el error aquí, por ejemplo, mostrar un mensaje de error
                Console.WriteLine($"Error al cargar los personajes: {task.Exception.Message}");
            }
        });
    }

    private void EditCharacter(ServiceDefaults.Models.Dragones.Character personaje)
    {
        // Lógica para editar el personaje
        Console.WriteLine($"Editar personaje: {personaje.Name}");
    }

    private void DeleteCharacter(ServiceDefaults.Models.Dragones.Character personaje)
    {
        // Lógica para eliminar el personaje
        Console.WriteLine($"Eliminar personaje: {personaje.Name}");
    }
}

