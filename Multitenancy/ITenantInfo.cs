namespace HordeFlow.Multitenancy
{
    public interface ITenantInfo<out TTenant>
    {
        TTenant Value { get; }
    }
}
