namespace  HordeFlow.Core
{
    public interface ITenantEntity<TTenantKey, TKey> : IBaseEntity<TKey>
    {
        TTenantKey TenantId { get; set; }
    }
}