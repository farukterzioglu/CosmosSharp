using System.Collections.Generic;

namespace CosmosSharp
{
    public class CosmosConfigurator
    {
        public string HttpEndpoint { get; set; }
        public string ChainId { get; set; }
        public Dictionary<string, string> HeaderKeyValues { get; set; }
    }
}