using System.ComponentModel;

namespace HordeFlow.Core.Models.Common
{
    public interface ICanActivate
    {
        [DefaultValue(true)]
        bool Active { get; set; }
    }
}