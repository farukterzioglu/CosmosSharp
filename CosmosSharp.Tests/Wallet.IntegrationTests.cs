using NUnit.Framework;
using System;
using NBitcoin;
using NBitcoin.DataEncoders;
using CosmosSharp.Types;
using CosmosSharp.Sdk;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using CosmosSharp.Api;

namespace CosmosSharp.Tests
{
    public class IntegrationTests
    {
        string chainId = "gaia-13007";
        string httpEndpoint = "";

        CosmosApi api;
        CosmosSharp.Wallet wallet;
        private uint _lastSequence;

        [SetUp]
        public void Setup() 
        {
            Assert.True(!string.IsNullOrEmpty(chainId), "Should set chain id.");
            Assert.True(!string.IsNullOrEmpty(httpEndpoint), "Should set node url.");
            
            api = new CosmosApi(new CosmosConfigurator(){
                HttpEndpoint = httpEndpoint,
                ChainId = chainId
            });

            var wordList = "cloth tower certain forget city autumn better ankle that village bulk torch error edit cement dilemma palace soldier system around arrest similar lab luggage";
            wallet = CosmosSharp.Wallet.FromMnemonic(mnemonic: wordList);
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldGetNodeInfo()
        {   
            var nodeInfo = await api.GetNodeInfo(new CancellationToken());
            Assert.AreEqual(chainId, nodeInfo.node_info.network);
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldGetTxs()
        {   
            var txs = await api.GetTxs(295695,new CancellationToken());
            Assert.NotNull(txs);
            Assert.AreEqual(1, txs.Count);
            Assert.AreEqual(txs.Txs.First().TxHash, "E90FE5F065C762D224FC648FC6818F47899561BAB5B89310AAA1A29A6CB3B47E");
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldGetTx()
        {   
            var txs = await api.GetTx("E90FE5F065C762D224FC648FC6818F47899561BAB5B89310AAA1A29A6CB3B47E",new CancellationToken());
            Assert.NotNull(txs);
            Assert.AreEqual("295695",txs.Height);
            Assert.AreEqual("E90FE5F065C762D224FC648FC6818F47899561BAB5B89310AAA1A29A6CB3B47E", txs.TxHash);
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldGetBlock()
        {   
            var block = await api.GetBlock(1,new CancellationToken());
            Assert.NotNull(block);
            Assert.AreEqual("1", block.BlockMeta.Header.Height);
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldCreateTransaction()
        {   
            StdTx<MsgSend> atomTransfer = await api.CreateAtomTransaction(
                new AtomTransferRequest(){
                    From = "cosmos1vfknyaz4fgrkj8ef87ute57gfscmg3ukprjmqh", To =  "cosmos14wpr6c0k7p4f2vfdf7pgdchxxg550jzcv564u8", 
                    Memo= "111", ChainId= chainId, AmountUAtom= 200, Denom= DenomTypes.TestnetCoin.ToString()    
                }, new CancellationToken());

            StdTx expectedValue = new StdTx(){
                Msg = new List<Msg>(){
                    new Msg(){
                        Type = "cosmos-sdk/MsgSend",
                        Value = new MsgSend(){
                            FromAddress = "cosmos1vfknyaz4fgrkj8ef87ute57gfscmg3ukprjmqh",
                            ToAddress = "cosmos14wpr6c0k7p4f2vfdf7pgdchxxg550jzcv564u8",
                            Amount = new List<Types.Coin>(){
                                new Types.Coin(){
                                    Amount = "200",
                                    Denom = "umuon",
                                }
                            }
                        },
                    }
                },
                Fee = new StdFee(){
                    Amount = new List<Types.Coin>(){},
                    Gas = "200000"
                },
                Signatures = null,
                Memo = "111"
            };

            var expected = JsonConvert.SerializeObject(expectedValue);
            var actual = JsonConvert.SerializeObject(atomTransfer);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldGetAccount()
        {
            var account = await api.GetAccount("cosmos1vfknyaz4fgrkj8ef87ute57gfscmg3ukprjmqh",new CancellationToken());

            Assert.AreEqual("1897", account.AccountNumber);
            Assert.AreEqual("Au9kobJI1jZMSazacqDo8KF6EICtxkHH2WpE89wfEz9A", account.PublicKey.Value);
        }

        async Task<BaseAccount> GetAccount(string fromAddress)
        {
            var account = await api.GetAccount(fromAddress,new CancellationToken());
            if( account.Sequence <= _lastSequence) {
                await Task.Delay(TimeSpan.FromSeconds(15));
                account = await api.GetAccount(fromAddress,new CancellationToken());
            }
            _lastSequence = account.Sequence;
            return account;
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldBroadCast()
        {
            string fromAddress = "cosmos17yj45mrgvwezj9jlnhcgd2wr33ufqlcflj0xxv";

            // Create tx with api
            StdTx<MsgSend> atomTransfer = await api.CreateAtomTransaction(
                new AtomTransferRequest(){
                    From = fromAddress, To =  "cosmos1vfknyaz4fgrkj8ef87ute57gfscmg3ukprjmqh", 
                    Memo= "testing", ChainId= chainId, AmountUAtom= 100, Denom= DenomTypes.TestnetCoin.ToString()    
                }, new CancellationToken());

            // Get account details 
            BaseAccount account = await GetAccount(fromAddress);

            // Sign tx
            var wordList = "cloth tower certain forget city autumn better ankle that village bulk torch error edit cement dilemma palace soldier system around arrest similar lab luggage";
            CosmosSharp.Wallet wallet = CosmosSharp.Wallet.FromMnemonic(mnemonic: wordList);

            SignMeta meta = new SignMeta(){
                AccountNumber = account.AccountNumber,
                Sequence = account.Sequence,
                ChainId = chainId
            };
            StdSignMsg<MsgSend> signMsg = new StdSignMsg<MsgSend>(atomTransfer, meta){};
            StdTx<MsgSend> signed = wallet.Sign<MsgSend>(signMsg);

            var broadCastResponse = await api.BroadCastTx(signed, BroadcastMode.block, new CancellationToken());
            Assert.IsTrue(broadCastResponse.height > 0);
            Assert.IsTrue(broadCastResponse.code == 0);
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldBroadCastWithHelper()
        {
            string fromAddress = "cosmos17yj45mrgvwezj9jlnhcgd2wr33ufqlcflj0xxv";

            StdTx<MsgSend> atomTransfer = AtomHelper.CreateTx(
                fromAddress: fromAddress, toAddress: "cosmos1vfknyaz4fgrkj8ef87ute57gfscmg3ukprjmqh", 
                memo: "testing", amountUAtom: 1001, 
                feeUAtom: 750, Network.Testnet);
            BaseAccount account = await GetAccount(fromAddress);
            SignMeta meta = new SignMeta(){ AccountNumber = account.AccountNumber, Sequence = account.Sequence, ChainId = chainId };
            StdSignMsg<MsgSend> signMsg = new StdSignMsg<MsgSend>(atomTransfer, meta);
            StdTx<MsgSend> signed = wallet.Sign<MsgSend>(signMsg);

            var broadCastResponse = await api.BroadCastTx(signed, BroadcastMode.block, new CancellationToken());
            Assert.IsTrue(broadCastResponse.height > 0, broadCastResponse.raw_log);
            Assert.IsTrue(broadCastResponse.code == 0, broadCastResponse.raw_log);
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldBroadCastWithMasterKey()
        {
            wallet = CosmosSharp.Wallet.FromMasterKey("xprv9s21ZrQH143K3Ntj6s3Wx6bjrFPYW2dCu7GJtKo3rs4Z5MaN8AufRD27XZq5r3MyKgZACUKUugtrTAdTVKY7Kn99TdE5mBpuTagUxBbDxgr");
            string fromAddress = "cosmos17yj45mrgvwezj9jlnhcgd2wr33ufqlcflj0xxv";

            StdTx<MsgSend> atomTransfer = AtomHelper.CreateTx(
                fromAddress: fromAddress, 
                toAddress: "cosmos1vfknyaz4fgrkj8ef87ute57gfscmg3ukprjmqh", 
                memo: "testing", 
                amountUAtom: 999,
                feeUAtom: 750,
                Network.Testnet);
            BaseAccount account = await GetAccount(fromAddress);
            SignMeta meta = new SignMeta(){ AccountNumber = account.AccountNumber, Sequence = account.Sequence, ChainId = chainId };
            StdSignMsg<MsgSend> signMsg = new StdSignMsg<MsgSend>(atomTransfer, meta);
            StdTx<MsgSend> signed = wallet.Sign<MsgSend>(signMsg);

            var broadCastResponse = await api.BroadCastTx(signed, BroadcastMode.block, new CancellationToken());
            Assert.IsTrue(broadCastResponse.height > 0, broadCastResponse.raw_log);
            Assert.IsTrue(broadCastResponse.code == 0, broadCastResponse.raw_log);
        }

        [Test]
        [Category("Integration")]
        public async Task ShouldBroadCastWithGenericMsg()
        {
            string fromAddress = "cosmos17yj45mrgvwezj9jlnhcgd2wr33ufqlcflj0xxv";

            StdTx tx = new StdTx(){
                Msg = new List<Msg>(){
                    new Msg(){
                        Type = "cosmos-sdk/MsgSend",
                        Value = new MsgSend(){
                            FromAddress = fromAddress,
                            ToAddress = "cosmos1vfknyaz4fgrkj8ef87ute57gfscmg3ukprjmqh",
                            Amount = new List<Types.Coin>(){
                                new Types.Coin(){
                                    Denom = "umuon",
                                    Amount = "100"
                                }
                            }
                        }
                    }
                },
                Fee = new StdFee(){
                    Amount = new List<Types.Coin>(){ new Types.Coin(){ Amount = "750", Denom = "umuon"}},
                    Gas = "200000"
                },
                Memo = "testing",
            };

            BaseAccount account = await GetAccount(fromAddress);
            SignMeta meta = new SignMeta(){ AccountNumber = account.AccountNumber, Sequence = account.Sequence, ChainId = chainId };
            StdSignMsg signMsg = new StdSignMsg(tx,meta);
            StdTx signedTx = wallet.Sign(signMsg);

            var broadCastResponse = await api.BroadCastTx(signedTx, BroadcastMode.block, new CancellationToken());
            Assert.IsTrue(broadCastResponse.height > 0, broadCastResponse.raw_log);
            Assert.IsTrue(broadCastResponse.code == 0, broadCastResponse.raw_log);
        }
    }
}