using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using pythonbackendgame;
using SpawnDev.BlazorJS;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<BrowserService>();

//this is for peerjs spawndev p2p
//also need to add the script to index.html
//also need to import 
builder.Services.AddBlazorJSRuntime();

await builder.Build().RunAsync();

//navigate to the wwwroot in terminal
//git add .
//git commit -m "Fix base href for GitHub Pages"
//git push origin gh-pages --force