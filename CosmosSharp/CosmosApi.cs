using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CosmosSharp.Api;
using CosmosSharp.Types;

namespace CosmosSharp
{
    public class CosmosApi
    {
        private CosmosConfigurator _config { get; set; }
        private IHttpHandler _httpHandler { get; set; }

        public CosmosApi(CosmosConfigurator config)
        {
           _config = config;
           _httpHandler = new HttpHandler();
        }

        public CosmosApi(CosmosConfigurator config, IHttpHandler httpHandler)
        {
           _config = config;
           _httpHandler = httpHandler;
        }

        public async Task<NodeInfoResponse> GetNodeInfo(CancellationToken cancellationToken) 
        {
            var url = string.Format("{0}/node_info", _config.HttpEndpoint);
            return await _httpHandler.GetJsonAsync<NodeInfoResponse>(url, cancellationToken);
        }

        public async Task<StdTx<MsgSend>> CreateAtomTransaction(AtomTransferRequest transferRequest, CancellationToken cancellationToken)
        {
            var url = string.Format("{0}/bank/accounts/{1}/transfers", _config.HttpEndpoint, transferRequest.To);
            var request = new BankTransferRequest(){
                base_req = new BaseReq(){
                    from = transferRequest.From,
                    memo = transferRequest.Memo,
                    chain_id = transferRequest.ChainId,
                    fees = new List<StdFee>(){},
                },
                amount = new List<Amount>(){
                    new Amount(){
                        amount = transferRequest.AmountUAtom.ToString(),
                        denom = transferRequest.Denom
                    }
                }
            };

            AtomTransferResponse response = await _httpHandler.PostJsonAsync<AtomTransferResponse, BankTransferRequest>(url, request);
            return response.Value;
        }

        public async Task<BaseAccount> GetAccount(string accountName, CancellationToken cancellationToken) 
        {
            var url = $"{_config.HttpEndpoint}/auth/accounts/{accountName}";
            var response = await _httpHandler.GetJsonAsync<ApiResponseWithHeight<ApiResponse<BaseAccount>>>(url, cancellationToken);
            return response?.Result?.Value;
        }

        public async Task<BroadcastTxResponse> BroadCastTx(StdTx signedTx, BroadcastMode mode, CancellationToken cancellationToken)
        {
            var url = $"{_config.HttpEndpoint}/txs";
            BroadcastTxRequest request = new BroadcastTxRequest(){
                Tx = signedTx,
                Mode = mode.ToString()
            };

            var response = await _httpHandler.PostJsonAsync<BroadcastTxResponse,BroadcastTxRequest>(url, request);
            return response;
        }

        public async Task<BroadcastTxResponse> BroadCastTx<TMsg>(StdTx<TMsg> signedTx, BroadcastMode mode, CancellationToken cancellationToken)
        {
            var url = $"{_config.HttpEndpoint}/txs";
            BroadcastTxRequest<TMsg> request = new BroadcastTxRequest<TMsg>(){
                Tx = signedTx,
                Mode = mode.ToString()
            };

            var response = await _httpHandler.PostJsonAsync<BroadcastTxResponse,BroadcastTxRequest<TMsg>>(url, request);
            return response;
        }

        public async Task<LatestBlockInfo> GetBlock(ulong blockHeight, CancellationToken cancellationToken)
        {
            var url = $"{_config.HttpEndpoint}//txs?tx.height={blockHeight}";
            var response = await _httpHandler.GetJsonAsync<LatestBlockInfo>(url, cancellationToken);
            return response;
        }

        public async Task<BlockInfo> GetLatestBlock(CancellationToken cancellationToken)
        {
            var url = $"{_config.HttpEndpoint}/blocks/latest";
            var response = await _httpHandler.GetJsonAsync<BlockInfo>(url, cancellationToken);
            return response;
        }

        public async Task<NewTransaction> GetTx(string txHash, CancellationToken cancellationToken)
        {
            var url = $"{_config.HttpEndpoint}/txs/{txHash}";
            var response = await _httpHandler.GetJsonAsync<NewTransaction>(url, cancellationToken);
            return response;
        }
    }
}