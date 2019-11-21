namespace  HordeFlow.Models
{
    public interface ITenantEntity<TTenantKey, TKey> : IBaseEntity<TKey>
    {
        TTenantKey TenantId { get; set; }
    }
}