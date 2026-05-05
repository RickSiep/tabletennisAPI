using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TableTennisFrontEnd;
using TableTennisFrontEnd.Authentication;
using TableTennisFrontEnd.Components;
using TableTennisShared.DTO.Token;
using TableTennisShared.DTO.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/account/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorization();
//builder.Services.AddScoped<AuthState>();
//builder.Services.AddScoped<AuthService>();

builder.Services.AddHttpClient<ApiClient>("api", client =>
{
    client.BaseAddress = new("https://localhost:7149");
});

builder.Services.AddHttpClient<HttpClient>("local", client =>
{
    client.BaseAddress = new("https://localhost:7147");
});

//.AddHttpMessageHandler<TokenRefreshHandler>();

//builder.Services.AddScoped<TokenRefreshHandler>();

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

app.MapPost("/auth/login", async (HttpContext context, IHttpClientFactory factory, [FromForm] LoginRequestDto request) =>
{
    var http = factory.CreateClient("api");

    // call your API
    var response = await http.PostAsJsonAsync("auth/login", request);

    if (!response.IsSuccessStatusCode)
    {
        return Results.Unauthorized();
    }

    var result = await response.Content.ReadFromJsonAsync<UserInfoWithTokens>();

    if (result == null)
        return Results.Unauthorized();

    var claims = new List<Claim>
    {
        new(ClaimTypes.Name, result.FirstName),
        new(ClaimTypes.NameIdentifier, result.UserId.ToString())
    };

    var identity = new ClaimsIdentity(claims, "Cookies");
    var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync("Cookies", principal);

    return Results.Ok();
}).DisableAntiforgery();

//app.MapPost("/auth/logout", async (HttpContext context) =>
//{
//    await context.SignOutAsync("Cookies");
//    return Results.Ok();
//}).DisableAntiforgery();;

app.Run();