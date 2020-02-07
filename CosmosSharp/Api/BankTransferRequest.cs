using System.Collections.Generic;
using CosmosSharp.Types;

namespace CosmosSharp.Api
{
    public class Amount
    {
        public string denom { get; set; }
        public string amount { get; set; }
    }

    public class BankTransferRequest
    {
        public BaseReq base_req { get; set; }
        public List<Amount> amount { get; set; }
    }
}