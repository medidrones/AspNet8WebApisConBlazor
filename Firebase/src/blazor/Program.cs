using blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var urlBaseApi = builder.Configuration.GetValue<string>("BaseUrl");
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(urlBaseApi!) });

await builder.Build().RunAsync();
