using CosmosSharp.Types;
using Newtonsoft.Json;

namespace CosmosSharp.Api
{
    public class AtomTransferRequest
    {
        public ulong AmountUAtom { get; set; }
        public string Denom { get; set; }
        public string ChainId { get; set; }
        public string Memo { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public ulong FeeUAtom {get; set;}
    }

    public class AtomTransferResponse: ApiResponse<StdTx<MsgSend>> {}
}