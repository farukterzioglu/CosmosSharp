using System.Collections.Generic;
using Newtonsoft.Json;

namespace CosmosSharp.Api
{
    public class Parts
    {
        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }
    }

    public class BlockId
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("parts")]
        public Parts Parts { get; set; }
    }

    public class Version
    {
        [JsonProperty("block")]
        public string Block { get; set; }

        [JsonProperty("app")]
        public string App { get; set; }
    }

    public class LastBlockId
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("parts")]
        public Parts Parts { get; set; }
    }

    public class Header
    {
        [JsonProperty("version")]
        public Version Version { get; set; }

        [JsonProperty("chain_id")]
        public string ChainId { get; set; }

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("num_txs")]
        public string NumTxs { get; set; }

        [JsonProperty("total_txs")]
        public string TotalTxs { get; set; }

        [JsonProperty("last_block_id")]
        public LastBlockId LastBlockId { get; set; }

        [JsonProperty("last_commit_hash")]
        public string LastCommitHash { get; set; }

        [JsonProperty("data_hash")]
        public string DatHash { get; set; }

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

    public class BlockMeta
    {
        [JsonProperty("block_id")]
        public BlockId BlockId { get; set; }

        [JsonProperty("header")]
        public Header Header { get; set; }
    }

    public class Data
    {
        [JsonProperty("txs")]
        public List<string> Txs { get; set; }
    }

    public class Evidence
    {
        [JsonProperty("evidence")]
        public object EvidenceInfo { get; set; }
    }

    public class PreCommit
    {
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("round")]
        public string Round { get; set; }

        [JsonProperty("block_id")]
        public BlockId BlockId { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("validator_address")]
        public string ValidatorAddress { get; set; }

        [JsonProperty("validator_index")]
        public string ValidatorIndex { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }

    public class LastCommit
    {
        [JsonProperty("block_id")]
        public BlockId block_id { get; set; }

        [JsonProperty("precommits")]
        public List<PreCommit> PreCommits { get; set; }
    }

    public class Block
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

    public class BlockInfo
    {
        [JsonProperty("block_meta")]
        public BlockMeta BlockMeta { get; set; }

        [JsonProperty("block")]
        public Block Block { get; set; }
    }
}