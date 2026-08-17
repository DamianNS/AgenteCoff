using AgenteCoff.ServiceDefaults.Models.Dragones;
using AgenteCoff.Web.Services;

public interface ICharacterService
{
    Task<List<Character>> GetCharactersAsync();
    Task SaveCharacterAsync(Character character);
    Task DeleteCharacterAsync(Character character);
}

public class CharacterService : ICharacterService
{
    private readonly ApiClient _apiClient;
    private List<Character> _characters = new List<Character>
    {
        new Character { Name = "Gandalf", Raze = "Elfo", Class = "Mago", Age = 200 },
        new Character { Name = "Frodo", Raze = "Humano", Class = "Guerrero", Age = 35 }
    };

    public CharacterService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<Character>> GetCharactersAsync()
    {
        return await _apiClient.SendGet<List<Character>>("/api/Personajes");
    }

    public async Task SaveCharacterAsync(Character character)
    {
        // Mocking the save logic
        if (character.Name != null)
        {
            _characters.Add(character);
            return;
        }
        throw new ArgumentException("Character name cannot be null.");
    }

    public async Task DeleteCharacterAsync(Character character)
    {
        // Mocking the delete logic
        _characters.Remove(character);
    }
}