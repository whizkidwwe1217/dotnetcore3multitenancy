namespace HordeFlow.Infrastructure.Multitenancy
{
    public interface ITenantInfo<out TTenant>
    {
        TTenant Value { get; }
    }
}
