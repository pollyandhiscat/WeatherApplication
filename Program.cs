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

    client.BaseAddress = new Uri($"http://api.weatherapi.com/v1/v1/");

});

var app = builder.Build();

if (app.Environment.IsProduction())
{

    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "PRODUCTION");
}

else
{

    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "DEVELOPMENT");
}

app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(

    name: "WeatherResult",
    pattern: "{controller=WeatherResultController}/{action=RetrieveWeather}/"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();