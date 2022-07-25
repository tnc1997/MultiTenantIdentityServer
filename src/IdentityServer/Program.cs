using System.Reflection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IdentityServer;
using IdentityServer.Data;
using IdentityServerHost;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddIdentityServer()
    .AddConfigurationStore<MultiTenantConfigurationDbContext>(options =>
    {
        options.ResolveDbContextOptions = (provider, builder) =>
        {
            var connectionString = provider.GetRequiredService<IConfiguration>().GetConnectionString("IdentityServer");

            builder.UseNpgsql(connectionString,
                builder => { builder.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name); });
        };
    })
    .AddOperationalStore(options =>
    {
        options.ResolveDbContextOptions = (provider, builder) =>
        {
            var connectionString = provider.GetRequiredService<IConfiguration>().GetConnectionString("IdentityServer");

            builder.UseNpgsql(connectionString,
                builder => { builder.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name); });
        };
    })
    .AddTestUsers(TestUsers.Users);

builder.Services.AddMultiTenant<SampleTenantInfo>()
    .WithHostStrategy()
    .WithConfigurationStore();

var app = builder.Build();

app.UseMultiTenant();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await scope.ServiceProvider.GetRequiredService<MultiTenantConfigurationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

    app.Use(async (context, next) =>
    {
        var db = context.RequestServices.GetRequiredService<MultiTenantConfigurationDbContext>();

        if (!await db.Clients.AnyAsync())
        {
            var client = new Client
            {
                ClientId = "2ec71004-25d5-4e61-87a6-acb8955e689a",
                ClientSecrets = new List<Secret> { new("secret".Sha512()) },
                ClientName = "Client",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string> { "openid", "profile", "email" },
                RedirectUris = new List<string> { "https://one.example.local:5002/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://one.example.local:5002/signout-callback-oidc" }
            }.ToEntity();

            db.Entry(client).Property("TenantId").CurrentValue = "one";

            await db.Clients.AddAsync(client);
            await db.SaveChangesAsync();
        }

        if (!await db.IdentityResources.AnyAsync())
        {
            await db.IdentityResources.AddRangeAsync(
                new IdentityResources.OpenId().ToEntity(),
                new IdentityResources.Profile().ToEntity(),
                new IdentityResources.Email().ToEntity());
            await db.SaveChangesAsync();
        }

        await next();
    });
}

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();