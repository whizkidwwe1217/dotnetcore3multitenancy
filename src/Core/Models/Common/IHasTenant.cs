namespace HordeFlow.Core.Models.Common
{
    public interface IHasTenant
    {
        Tenant Tenant { get; set; }
    }
}