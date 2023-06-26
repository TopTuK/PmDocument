/* 
 * https://alexpotter.dev/net-6-with-vue-3/ 
 * https://www.meziantou.net/running-npm-tasks-when-building-a-dotnet-project.htm
*/

using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using PmHelper.Domain.Repository;
using PmHelper.Domain.Services.Users;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

/*
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
*/

internal class Program
{
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var Configuration = builder.Configuration;

        // Set Authentication //
        // Must have unique name (Scheme)
        builder.Services
            .AddAuthentication(defaultScheme: Configuration["AuthDefaultSchemeName"]!)
            .AddCookie(Configuration["AuthDefaultSchemeName"]!, options =>
            {
                options.Cookie.Name = Configuration["AuthDefaultCookieName"];
                options.ExpireTimeSpan = TimeSpan.FromDays(1.0);

                options.Cookie.HttpOnly = false;

                options.LoginPath = new PathString("/login");
            })
            .AddCookie(Configuration["AuthTempCookieName"]!)
            .AddGoogle(Configuration["GoogleAuth:Name"]!, options =>
            {
                options.ClientId = Configuration["GoogleAuth:ClientId"]!;
                options.ClientSecret = Configuration["GoogleAuth:ClientSecret"]!;

                options.CallbackPath = Configuration["GoogleAuth:Callback"]!;
                options.SignInScheme = Configuration["AuthTempCookieName"]!;
            })
            .AddOpenIdConnect(Configuration["OidcAuth:Name"]!, options =>
            {
                options.Authority = Configuration["OidcAuth:Authority"];

                options.ClientId = Configuration["OidcAuth:ClientId"];
                options.ClientSecret = Configuration["OidcAuth:ClientSecret"];

                // Set the callback path, so it will call back to.
                options.CallbackPath = new PathString(Configuration["OidcAuth:Callback"]);

                // Set response type to code
                options.ResponseType = OpenIdConnectResponseType.Code;
                // options.ResponseMode = OpenIdConnectResponseMode.Query;

                // Configure the scope
                options.Scope.Clear();
                options.Scope.Add("openid");

                options.SaveTokens = true;

                options.Events.OnAuthorizationCodeReceived = async (context) =>
                {
                    var request = context.HttpContext.Request;
                    var redirectUri = context.Properties?.Items[OpenIdConnectDefaults.RedirectUriForCodePropertiesKey] ?? "/";
                    var code = context.ProtocolMessage.Code;

                    using var client = new HttpClient();
                    var discoResponsee = await client.GetDiscoveryDocumentAsync(options.Authority);

                    var tokenResponse = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                    {
                        Address = discoResponsee.TokenEndpoint,
                        ClientId = options.ClientId!,
                        ClientSecret = options.ClientSecret,
                        Code = code,
                        RedirectUri = redirectUri,
                    });

                    if (tokenResponse.IsError)
                    {
                        // Error handler
                        throw new Exception("Bad auth. Can't exchange code for access token and id token");
                    }

                    var accessToken = tokenResponse.AccessToken ?? string.Empty;
                    var idToken = tokenResponse.IdentityToken ?? string.Empty;

                    context.HandleCodeRedemption(accessToken, idToken);
                };

                //options.GetClaimsFromUserInfoEndpoint = true;
                options.MapInboundClaims = false;

                options.SignInScheme = "temp";
            });

        // Add AppDbContext
        builder.Services.AddDbContext<AppDbContext>();

        // Configure application services
        ConfigureServices(builder.Services);

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
    }
}