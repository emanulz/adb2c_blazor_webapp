using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using blazorADB2CWebApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CustomAuthorizationMessageHandler>();

builder.Services
    .AddHttpClient(
        "documentsAPI",
        client => client.BaseAddress = new Uri(builder.Configuration["Endpoints:DocumentsApi"]!)
    )
    .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient(
    "localHttp",
    client => client.BaseAddress = new Uri(builder.Configuration["Endpoints:LocalApi"]!)
);

builder.Services.AddScoped(
    sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("documentsAPI")
);

builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>()!.CreateClient("localHttp"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.LoginMode = "redirect";

    var add = options.ProviderOptions.DefaultAccessTokenScopes.Add;

    add("openid");
    add("offline_access");
    add(builder.Configuration["Endpoints:ReadScope"]!);
});

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
