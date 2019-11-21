using System.Collections.Generic;

namespace HordeFlow.Common
{
    public interface IResponseData<T>
    {
        IEnumerable<T> data { get; set; }
        int? total { get; set; }
    }
}