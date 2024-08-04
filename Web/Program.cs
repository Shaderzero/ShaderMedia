using Blazored.LocalStorage;
using Common;
using Common.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web;
using Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var services = builder.Services;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorageAsSingleton();
services.AddSingleton<IStorage, WebStorage>();
services.AddWebServices();

await builder.Build().RunAsync();