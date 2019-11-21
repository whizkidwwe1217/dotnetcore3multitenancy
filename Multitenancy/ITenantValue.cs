namespace HordeFlow.Multitenancy
{
    public interface ITenantValue<out TTenant>
    {
        TTenant Value { get; }
    }
}
