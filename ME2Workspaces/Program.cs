using ME2Workspaces.Client.Pages;
using ME2Workspaces.Components;
using Me2Workspaces.ModulosME2.DBUser;
using Me2Workspaces.ModulosME2.Database;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar IHttpContextAccessor para uso no AuthenticationService
builder.Services.AddHttpContextAccessor();

// Registrar serviços Razor e outros serviços customizados
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ConnectionDB>();
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddMudServices();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ME2Workspaces.Client._Imports).Assembly);

app.Run();
