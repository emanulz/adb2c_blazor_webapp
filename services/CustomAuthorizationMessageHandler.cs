using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace blazorADB2CWebApp;

public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
{
    private IConfiguration Configuration { get; set; }

    public CustomAuthorizationMessageHandler(
        IAccessTokenProvider provider,
        NavigationManager navigation,
        IConfiguration configuration
    ) : base(provider, navigation)
    {
        Configuration = configuration;
        ConfigureHandler(authorizedUrls: new[] { Configuration["Endpoints:DocumentsApi"]! });
    }
}
