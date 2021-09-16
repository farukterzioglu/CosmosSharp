using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CosmosSharp.StargateApiModel
{
    public partial class BlockInfo
    {
        [JsonProperty("block_id")]
        public BlockId BlockId { get; set; }

        [JsonProperty("block")]
        public Block Block { get; set; }
    }

    public partial class Block
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("evidence")]
        public Evidence Evidence { get; set; }

        [JsonProperty("last_commit")]
        public LastCommit LastCommit { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("txs")]
        public object[] Txs { get; set; }
    }

    public partial class Evidence
    {
        [JsonProperty("evidence")]
        public object[] EvidenceEvidence { get; set; }
    }

    public partial class Header
    {
        [JsonProperty("version")]
        public Version Version { get; set; }

        [JsonProperty("chain_id")]
        public string ChainId { get; set; }

        [JsonProperty("height")]
        public ulong Height { get; set; }

        [JsonProperty("time")]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("last_block_id")]
        public BlockId LastBlockId { get; set; }

        [JsonProperty("last_commit_hash")]
        public string LastCommitHash { get; set; }

        [JsonProperty("data_hash")]
        public string DataHash { get; set; }

        [JsonProperty("validators_hash")]
        public string ValidatorsHash { get; set; }

        [JsonProperty("next_validators_hash")]
        public string NextValidatorsHash { get; set; }

        [JsonProperty("consensus_hash")]
        public string ConsensusHash { get; set; }

        [JsonProperty("app_hash")]
        public string AppHash { get; set; }

        [JsonProperty("last_results_hash")]
        public string LastResultsHash { get; set; }

        [JsonProperty("evidence_hash")]
        public string EvidenceHash { get; set; }

        [JsonProperty("proposer_address")]
        public string ProposerAddress { get; set; }
    }

    public partial class BlockId
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("part_set_header")]
        public PartSetHeader PartSetHeader { get; set; }
    }

    public partial class PartSetHeader
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }
    }

    public partial class Version
    {
        [JsonProperty("block")]
        public long Block { get; set; }

        [JsonProperty("app")]
        public long App { get; set; }
    }

    public partial class LastCommit
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("round")]
        public long Round { get; set; }

        [JsonProperty("block_id")]
        public BlockId BlockId { get; set; }
    }
}