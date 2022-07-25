using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Entities;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data;

public class MultiTenantConfigurationDbContext : ConfigurationDbContext<MultiTenantConfigurationDbContext>,
    IMultiTenantDbContext
{
    public MultiTenantConfigurationDbContext(DbContextOptions<MultiTenantConfigurationDbContext> options,
        IMultiTenantContextAccessor<SampleTenantInfo> multiTenantContextAccessor) : base(options)
    {
        MultiTenantContextAccessor = multiTenantContextAccessor;
    }

    private IMultiTenantContextAccessor<SampleTenantInfo> MultiTenantContextAccessor { get; }

    public ITenantInfo TenantInfo => MultiTenantContextAccessor.MultiTenantContext!.TenantInfo!;

    public TenantMismatchMode TenantMismatchMode => TenantMismatchMode.Throw;

    public TenantNotSetMode TenantNotSetMode => TenantNotSetMode.Throw;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<Client>()
            .Property(client => client.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ClientClaim>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<ClientClaim>()
            .Property(clientClaim => clientClaim.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ClientCorsOrigin>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<ClientCorsOrigin>()
            .Property(clientCorsOrigin => clientCorsOrigin.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ClientGrantType>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<ClientGrantType>()
            .Property(clientGrantType => clientGrantType.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ClientIdPRestriction>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<ClientIdPRestriction>()
            .Property(clientIdPRestriction => clientIdPRestriction.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ClientPostLogoutRedirectUri>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<ClientPostLogoutRedirectUri>()
            .Property(clientPostLogoutRedirectUri => clientPostLogoutRedirectUri.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ClientProperty>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<ClientProperty>()
            .Property(clientProperty => clientProperty.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ClientRedirectUri>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<ClientRedirectUri>()
            .Property(clientRedirectUri => clientRedirectUri.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ClientScope>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<ClientScope>()
            .Property(clientScope => clientScope.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ClientSecret>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<ClientSecret>()
            .Property(clientSecret => clientSecret.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<IdentityProvider>()
            .IsMultiTenant()
            .AdjustKeys(modelBuilder)
            .AdjustIndexes();
        modelBuilder.Entity<IdentityProvider>()
            .Property(identityProvider => identityProvider.Id)
            .ValueGeneratedOnAdd();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.EnforceMultiTenant();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        this.EnforceMultiTenant();

        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}