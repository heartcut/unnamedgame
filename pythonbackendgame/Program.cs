using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using pythonbackendgame;
using pythonbackendgame.Models;
using SpawnDev.BlazorJS;
using SpawnDev.BlazorJS.JSObjects;
using System;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<BrowserService>();
builder.Services.AddSingleton<DataModel>();
builder.Services.AddSingleton<SoundService>();

//this is for peerjs spawndev p2p
//also need to add the script to index.html
//also need to import 
builder.Services.AddBlazorJSRuntime();

await builder.Build().RunAsync();

//things to change to work on github
//sed - i 's|href="/"|href="/unnamedgame/"|g' pythonbackendgame / wwwroot / index.html
//sed - i 's|navManager.NavigateTo("/game/0/"|navManager.NavigateTo("/unnamedgame/game/0/"|g' pythonbackendgame / Pages / LobbyPage / LobbyPage.razor
//sed - i 's|navManager.NavigateTo("/game/1/"|navManager.NavigateTo("/unnamedgame/game/1/"|g' pythonbackendgame / Pages / LobbyPage / LobbyPage.razor
//sed - i 's|url(/Assets/rainbowcurse.png)|url(/unnamedgame/Assets/rainbowcurse.png)|g' pythonbackendgame / wwwroot / css / app.css
//sed -i 's|url(/Assets/bcurs.png)|url(/unnamedgame/Assets/bcurs.png)|g' pythonbackendgame/wwwroot/css/app.css
//sed - i 's|url(/Assets/rcurs.png)|url(/unnamedgame/Assets/rcurs.png)|g' pythonbackendgame / wwwroot / css / app.css
//sed - i 's|url(/Assets/gcurs.png)|url(/unnamedgame/Assets/gcurs.png)|g' pythonbackendgame / wwwroot / css / app.css
//sed - i 's|url(/Assets/ycurs.png)|url(/unnamedgame/Assets/ycurs.png)|g' pythonbackendgame / wwwroot / css / app.css
//first publish the solution
//navigate to the wwwroot in terminal \repos\pythonbackendgame\pythonbackendgame\bin\Release\net8.0\browser-wasm\publish\wwwroot
//git add .
//git commit -m "Fix base href for GitHub Pages"
//git push origin gh-pages --force


//TODO:
//datamodel new and as a singleton
//doubling up and being one player higher lock or validation
//sound
//checkmark,lobby,gametimer, losing health timer
//dragcircle original-cleanup

