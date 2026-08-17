using Microsoft.AspNetCore.Components;

namespace AgenteCoff.Web.Components.Pages.Dragones
{
    public partial class Home : ComponentBase
    {
        private int tabActivo = 0;

        private void CambiarTab(int index)
        {
            tabActivo = index;
        }
    }
}
