namespace CosmosSharp.Api
{
    public class ProtocolVersion
    {
        public string p2p { get; set; }
        public string block { get; set; }
        public string app { get; set; }
    }

    public class Other
    {
        public string tx_index { get; set; }
        public string rpc_address { get; set; }
    }

    public class NodeInfo
    {
        public ProtocolVersion protocol_version { get; set; }
        public string id { get; set; }
        public string listen_addr { get; set; }
        public string network { get; set; }
        public string version { get; set; }
        public string channels { get; set; }
        public string moniker { get; set; }
        public Other other { get; set; }
    }

    public class ApplicationVersion
    {
        public string name { get; set; }
        public string server_name { get; set; }
        public string client_name { get; set; }
        public string version { get; set; }
        public string commit { get; set; }
        public string build_tags { get; set; }
        public string go { get; set; }
    }

    public class NodeInfoResponse
    {
        public NodeInfo node_info { get; set; }
        public ApplicationVersion application_version { get; set; }
    }
}