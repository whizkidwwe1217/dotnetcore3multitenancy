using System.Collections.Generic;

namespace HordeFlow.Core.Common
{
    public class ResponseData : IResponseData<object>
    {
        public IEnumerable<object> data { get; set; }
        public int? total { get; set; }
    }
}