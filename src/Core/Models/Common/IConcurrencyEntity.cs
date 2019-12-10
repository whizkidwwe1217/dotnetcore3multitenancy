using System;

namespace HordeFlow.Core.Models.Common
{
    public interface IConcurrencyEntity
    {
        byte[] ConcurrencyStamp { get; set; }
        DateTime? ConcurrencyTimeStamp { get; set; } // Used for Datbaase Providers that don't use rowversion data type.
    }
}