namespace HordeFlow.Core.Models.Common
{
    public interface ICanSoftDelete
    {
        bool Deleted { get; set; }
    }
}