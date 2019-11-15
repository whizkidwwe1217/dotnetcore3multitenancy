namespace i21Apis.Multitenancy
{
    public interface ITenant<out TTenant>
    {
        TTenant Value { get; }
    }
}
