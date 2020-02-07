using System.Collections.Generic;
using CosmosSharp.Types;

namespace CosmosSharp.Api
{
    public class BaseReq
    {
        public string from { get; set; }
        public string memo { get; set; }
        public string chain_id { get; set; }
        public List<StdFee> fees { get; set; }
    }
}