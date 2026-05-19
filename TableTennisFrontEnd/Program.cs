using AspNet.Security.OAuth.Discord;
using AspNet.Security.OAuth.Twitch;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Components;
using TableTennisFrontEnd;
using TableTennisFrontEnd.Authentication;
using TableTennisFrontEnd.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddAuthenticationStateSerialization(options => options.SerializeAllClaims = true);

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/account/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    })
    .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        options.SignInScheme = "Cookies";
    })
    .AddDiscord(DiscordAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration["Authentication:Discord:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Discord:ClientSecret"]!;
        options.SignInScheme = "Cookies";
        options.Scope.Add("identify");
        options.Scope.Add("email");
    })
    .AddTwitch(TwitchAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.ClientId = builder.Configuration["Authentication:Twitch:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Twitch:ClientSecret"]!;
        options.SignInScheme = "Cookies";
    });

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthState>();

builder.Services.AddHttpClient<ApiClient>("api", client =>
{
    client.BaseAddress = new("https://localhost:7149");
});

builder.Services.AddHttpClient<HttpClient>("local", client =>
{
    client.BaseAddress = new("https://localhost:7147");
});

builder.Services.AddScoped<ApiClient>();
var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/Error/{0}");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorPages();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();