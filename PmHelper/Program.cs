/* 
 * https://alexpotter.dev/net-6-with-vue-3/ 
 * https://www.meziantou.net/running-npm-tasks-when-building-a-dotnet-project.htm
*/

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

/*
var Configuration = builder.Configuration;

void ConfigureOpenIdConnect(OpenIdConnectOptions options)
{
    options.Authority = $"https://{Configuration["PmiAuth:Domain"]}";

    // Configure the Client ID and Client Secret
    options.ClientId = Configuration["PmiAuth:ClientId"];
    options.ClientSecret = Configuration["PmiAuth:ClientSecret"];

    // Set response type to code
    options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
    options.ResponseMode = OpenIdConnectResponseMode.FormPost;

    // Configure the scope
    options.Scope.Clear();
    options.Scope.Add("openid");

    // Set the callback path, so PmiAuth will call back to http://localhost:3000/callback
    // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
    options.CallbackPath = new PathString("/auth/SingIngCallback");

    // This saves the tokens in the session cookie
    options.SaveTokens = true;
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.HttpOnly = true;
    })
    .AddOpenIdConnect("PmiAuth", options =>
    {
        ConfigureOpenIdConnect(options);
    });
*/

// Set Authentication //
// Must have unique name (Scheme)
builder.Services
    .AddAuthentication(defaultScheme: "cookie")
    .AddCookie("cookie", options =>
    {
        options.Cookie.Name = "demo";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

        options.LoginPath = new PathString("/auth/index");
        options.AccessDeniedPath = new PathString("/auth/accessdenied");
    })
    .AddCookie("temp")
    .AddGoogle("google", options =>
    {
        options.ClientId = "";
        options.ClientSecret = "";

        options.CallbackPath = "/auth/signin-google";
        options.SignInScheme = "cookie";
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "api/{controller}/{action=Index}/{id?}"
    );
});
#pragma warning restore ASP0014 // Suggest using top level route registrations

if (app.Environment.IsDevelopment())
{
    app.UseSpa(spa =>
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:5173");
    });
}
else
{
    app.MapFallbackToFile("index.html");
}

app.Run();
