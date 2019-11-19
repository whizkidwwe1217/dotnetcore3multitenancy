namespace  i21Apis.Models
{
    public interface ITenantEntity<TTenantKey, TKey> : IBaseEntity<TKey>
    {
        TTenantKey TenantId { get; set; }
    }
}