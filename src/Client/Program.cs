using System.IdentityModel.Tokens.Jwt;
using Client;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.ResponseType = "code";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
    });

builder.Services.AddMultiTenant<SampleTenantInfo>()
    .WithHostStrategy()
    .WithConfigurationStore()
    .WithPerTenantAuthentication();

var app = builder.Build();

app.UseMultiTenant();

if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        // This accessor does not have a multi-tenant context during sign-out.
        var accessor = context.RequestServices.GetRequiredService<IMultiTenantContextAccessor<SampleTenantInfo>>();

        return Task.CompletedTask;
    });

    // This accessor has a multi-tenant context during sign-out.
    var accessor = context.RequestServices.GetRequiredService<IMultiTenantContextAccessor<SampleTenantInfo>>();

    await next();
});

app.MapRazorPages().RequireAuthorization();

app.Run();