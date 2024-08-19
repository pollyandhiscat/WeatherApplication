using Azure.Identity;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Azure.Core;
using Azure.Security.KeyVault.Secrets;
var builder = WebApplication.CreateBuilder(args);

/*
 * Code Citations:
 * Citation #18 
 * Citation #19
*/

// Add services to the container.
builder.Services.AddControllersWithViews();

// Create the client for connecting to the WeatherAPI
builder.Services.AddHttpClient("weatherAPIClient", client =>
{

    client.BaseAddress = new Uri("http://api.weatherapi.com/v1/");

});

SecretClientOptions options = new SecretClientOptions()
{
    Retry =
        {
            Delay= TimeSpan.FromSeconds(2),
            MaxDelay = TimeSpan.FromSeconds(16),
            MaxRetries = 5,
            Mode = RetryMode.Exponential
         }
};

// Citation #36, Citation #37
var client = new SecretClient(new Uri("https://weather-app-key-vault.vault.azure.net/"), new DefaultAzureCredential(), options);

KeyVaultSecret secret = client.GetSecret("WeatherApplicationSecret");

string secretValue = secret.Value;

var app = builder.Build();

// Citation #9, Citation #10, Citation #11
// If the app is running in development, we use local settings such as environment variables to authenticate with Azure.
if (app.Environment.IsDevelopment())
{
    // TODO: We may need to restore this logic once we deploy to Azure, but make this happen for Production, not Dev.

    //var keyVaultEndpoint = new Uri(builder.Configuration["AZURE_KEY_VAULT_URI"]);
    //var azureSecret = builder.Configuration["AZURE_CLIENT_SECRET"];
    //var azureClientID = builder.Configuration["AZURE_CLIENT_ID"];
    //var azureTenantID = builder.Configuration["AZURE_TENANT_ID"];
    //builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new ClientSecretCredential(azureTenantID, azureClientID, azureSecret));
    //var weatherAPIKey = builder.Configuration["WeatherApplicationSecret"];
    
}

// If the app is running in production, we do a different authentication to Azure Key Vault.
else

{ 
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    // Code to authenticate with Azure a different way.
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(

    name: "WeatherResult",
    pattern: "{controller=WeatherResultController}/{action=RetrieveWeather}/");

// Citation #36
app.MapGet("/", () => secretValue);

app.Run();