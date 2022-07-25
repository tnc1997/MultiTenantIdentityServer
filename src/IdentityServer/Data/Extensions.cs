using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data;

public static class Extensions
{
    public static MultiTenantEntityTypeBuilder AdjustKeys(this MultiTenantEntityTypeBuilder builder,
        ModelBuilder modelBuilder)
    {
        var keys = builder.Builder.Metadata
            .GetKeys()
            .Where(key => key.Properties.All(property => property.Name != "TenantId"))
            .ToList();

        foreach (var key in keys) builder.AdjustKey(key, modelBuilder);

        return builder;
    }
}