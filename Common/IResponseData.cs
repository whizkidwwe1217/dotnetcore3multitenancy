using System.Collections.Generic;

namespace i21Apis.Common
{
    public interface IResponseData<T>
    {
        IEnumerable<T> data { get; set; }
        int? total { get; set; }
    }
}