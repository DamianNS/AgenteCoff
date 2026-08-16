using AgenteCoff.Web.Components.Pages;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace AgenteCoff.Web.Components.Pages
{
    public partial class Chat : ComponentBase
    {
        private string userInput = "";

        private async Task SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(userInput))
            {
                // In a real application, this would call an API service
                Console.WriteLine($"Sending message: {userInput}");
                // Logic to add user message to chat history
                userInput = "";
            }
        }
    }
}