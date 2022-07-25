#nullable enable

using Finbuckle.MultiTenant;

namespace IdentityServer;

public class SampleTenantInfo : ITenantInfo
{
    public string? Id { get; set; }
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public string? ConnectionString { get; set; }
}