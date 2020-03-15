using NUnit.Framework;
using System;
using NBitcoin;
using NBitcoin.DataEncoders;
using CosmosSharp.Types;
using CosmosSharp.Sdk;
using System.Collections.Generic;
using System.Linq;

namespace CosmosSharp.Tests
{
    public class WalletTests
    {
        byte[] cosmosPrivateKey = new byte[]{
                13,
                110,
                241,
                146,
                147,
                67,
                140,
                75,
                50,
                230,
                95,
                50,
                143,
                22,
                234,
                206,
                129,
                111,
                10,
                48,
                75,
                132,
                185,
                212,
                21,
                128,
                43,
                44,
                208,
                85,
                198,
                109
            };
        byte[] cosmosPublicKey = new byte[]{
            3,
            60,
            164,
            119,
            53,
            116,
            177,
            167,
            2,
            1,
            131,
            86,
            33,
            29,
            56,
            87,
            99,
            24,
            244,
            100,
            113,
            178,
            114,
            89,
            143,
            95,
            177,
            157,
            201,
            73,
            191,
            110,
            121
        };

        string wordList = "rich banner swift brush fury tunnel pause forest dose color luggage length";
        string passPhrase = "";
        string expectedAddress = "cosmos1xkqfeym2enqah7mwww0wyksx8u5qplsnsy8nl7";

        Wallet wallet;

        [SetUp]
        public void Setup() 
        {
            wallet = Wallet.FromMnemonic(mnemonic: wordList);
        }

        [Test]
        public void ShouldCreateHdWallet()
        {
            var actualWallet = Wallet.FromMnemonic(mnemonic: wordList);
            
            Assert.AreEqual(cosmosPrivateKey, actualWallet.PrivateKey.ToBytes());
            Assert.AreEqual(cosmosPublicKey, actualWallet.PublicKey.ToBytes());
            Assert.AreEqual(expectedAddress, actualWallet.Address);
        }

        [Test]
        public void ShouldCreateHdWalletFromMasterKey()
        {
            string mnemonic = "alien athlete plug mom layer axis hockey liar clinic ignore hammer donkey across pulse file";
            Wallet walletMnemo = Wallet.FromMnemonic(mnemonic);

            var masterKey = "xprv9s21ZrQH143K2hPwTZhFvNjyyFL8YQV2gd54oNYKARjCKkxCe12rDMt4Cj95BmEBHx5q9ukBWkmV7Jh2fpZZwmkcnFzBWQvbgfwFnDtqLDP";
            Wallet walletMster = Wallet.FromMasterKey(masterKey);

            Assert.AreEqual(walletMnemo.PrivateKey, walletMster.PrivateKey);
            Assert.AreEqual(walletMnemo.PublicKey, walletMster.PublicKey);
        }

        [Test]
        [Ignore("Fails. Need to control")]
        public void ShouldSignWhenPropertiesNull()
        {
            // Arrange
            StdTx tx = new StdTx(){};
            var meta = new SignMeta(){};
            var signMsg = new StdSignMsg(tx,meta);

            // Act
            var signedTx = wallet.Sign(signMsg);

            // Assert
            Assert.AreEqual("+nJXdAjGYTVtMHXS9no5hCJy6KCdDz2CaMhyOS96oFo+IIQFfnALayt2yx6mvUqaTWhWm1MNVf6+7PTdKKqcCw==",signedTx.Signatures.First().SignatureInfo);
        }

        [Test]
        public void ShouldSignWhenFeeNull()
        {
            // Arrange
            StdTx tx = new StdTx(){
                Msg = new List<Msg>(){
                    new Msg(){
                        Type = "cosmos-sdk/MsgSend",
                        Value = new MsgSend(){
                            FromAddress = "cosmos1xkqfeym2enqah7mwww0wyksx8u5qplsnsy8nl7",
                            ToAddress = "cosmos1xkqfeym2enqah7mwww0wyksx8u5qplsnsy8nl7",
                            Amount = new List<Types.Coin>(){
                                new Types.Coin(){
                                    Denom = "stake",
                                    Amount = "1000000"
                                }
                            }
                        }
                    }
                },
                Fee = new StdFee(){
                    Gas = "100"
                },
                Memo = "This is a test.",
            };

            var meta = new SignMeta(){
                AccountNumber = "1",
                ChainId = "cosmos",
                Sequence = 0
            };

            var signMsg = new StdSignMsg(tx,meta);

            // Act
            var signedTx = wallet.Sign(signMsg);

            // Assert
            Assert.AreEqual("XOsOSK7bDMW0kUuqivWjjbpW1PElw5IqzZ2yFGYb0XMyboGq4dOlDFQUod/IIn/PYGZppBAbrMMw7zJQs3Mq2g==",signedTx.Signatures.First().SignatureInfo);


            // Arrange
            tx.Fee.Amount = new List<Types.Coin>();
            signMsg = new StdSignMsg(tx,meta);

            // Act 
            signedTx = wallet.Sign(signMsg);

            // Assert
            Assert.AreEqual("XOsOSK7bDMW0kUuqivWjjbpW1PElw5IqzZ2yFGYb0XMyboGq4dOlDFQUod/IIn/PYGZppBAbrMMw7zJQs3Mq2g==",signedTx.Signatures.First().SignatureInfo);
        }

        [Test]
        public void ShoıldSignTxWithGenericMsg()
        {
            StdTx tx = new StdTx(){
                Msg = new List<Msg>(){
                    new Msg(){
                        Type = "cosmos-sdk/MsgSend",
                        Value = new MsgSend(){
                            FromAddress = "cosmos1xkqfeym2enqah7mwww0wyksx8u5qplsnsy8nl7",
                            ToAddress = "cosmos1xkqfeym2enqah7mwww0wyksx8u5qplsnsy8nl7",
                            Amount = new List<Types.Coin>(){
                                new Types.Coin(){
                                    Denom = "stake",
                                    Amount = "1000000"
                                }
                            }
                        }
                    }
                },
                Fee = new StdFee(){
                    Amount = new List<Types.Coin>(){
                        new Types.Coin(){
                            Denom = "stake",
                            Amount = "1"
                        }
                    },
                    Gas = "100"
                },
                Memo = "This is a test.",
            };

            var meta = new SignMeta(){
                AccountNumber = "1",
                ChainId = "cosmos",
                Sequence = 0
            };

            var signMsg = new StdSignMsg(tx,meta);

            var signedTx = wallet.Sign(signMsg);
            Assert.AreEqual("zbaibf4Dh4wwM0spbZlnUWR9mGN8HwFUqyp29Mf7Ysoa0iVKUZuXrYAfTNP7pmwhMmAgp/3dolIiitQVt9tQIw==",signedTx.Signatures.First().SignatureInfo);
            Assert.AreEqual("tendermint/PubKeySecp256k1", signedTx.Signatures.First().PubKey.Type);
            Assert.AreEqual("AzykdzV0sacCAYNWIR04V2MY9GRxsnJZj1+xnclJv255", signedTx.Signatures.First().PubKey.PubKeySecp256k1Value);
        }

        [Test]
        public void ShouldSignTxWithTypedMsg()
        {
            StdTx<MsgSend> tx = new StdTx<MsgSend>(){
                Msg = new List<Msg<MsgSend>>(){
                    new Msg<MsgSend>(){
                        Type = "cosmos-sdk/MsgSend",
                        Value = new MsgSend(){
                            FromAddress = "cosmos1xkqfeym2enqah7mwww0wyksx8u5qplsnsy8nl7",
                            ToAddress = "cosmos1xkqfeym2enqah7mwww0wyksx8u5qplsnsy8nl7",
                            Amount = new List<Types.Coin>(){
                                new Types.Coin(){
                                    Denom = "stake",
                                    Amount = "1000000"
                                }
                            }
                        }
                    }
                },
                Fee = new StdFee(){
                    Amount = new List<Types.Coin>(){
                        new Types.Coin(){
                            Denom = "stake",
                            Amount = "1"
                        }
                    },
                    Gas = "100"
                },
                Memo = "This is a test.",
            };

            var meta = new SignMeta(){
                AccountNumber = "1",
                ChainId = "cosmos",
                Sequence = 0
            };

            var signMsg = new StdSignMsg<MsgSend>(tx,meta);

            var signedTx = wallet.Sign(signMsg);
            Assert.AreEqual("zbaibf4Dh4wwM0spbZlnUWR9mGN8HwFUqyp29Mf7Ysoa0iVKUZuXrYAfTNP7pmwhMmAgp/3dolIiitQVt9tQIw==",signedTx.Signatures.First().SignatureInfo);
            Assert.AreEqual("tendermint/PubKeySecp256k1", signedTx.Signatures.First().PubKey.Type);
            Assert.AreEqual("AzykdzV0sacCAYNWIR04V2MY9GRxsnJZj1+xnclJv255", signedTx.Signatures.First().PubKey.PubKeySecp256k1Value);
        }

        [Test]
        public void ShouldSignTxWithMasterKey()
        {
            var masterKey = "xprv9s21ZrQH143K3v6btBDpGAQL6k7PUAAjU8okx9RPkRgEhaDSkmw2YreCtNTfBQJ7K8eNzVRmYEknkEFvcGbAT7NJt2BwK6aYUvH6xGNXdAn";
            Wallet walletMster = Wallet.FromMasterKey(masterKey);

            StdTx<MsgSend> tx = new StdTx<MsgSend>(){
                Msg = new List<Msg<MsgSend>>(){
                    new Msg<MsgSend>(){
                        Type = "cosmos-sdk/MsgSend",
                        Value = new MsgSend(){
                            FromAddress = "cosmos1xkqfeym2enqah7mwww0wyksx8u5qplsnsy8nl7",
                            ToAddress = "cosmos1xkqfeym2enqah7mwww0wyksx8u5qplsnsy8nl7",
                            Amount = new List<Types.Coin>(){
                                new Types.Coin(){
                                    Denom = "stake",
                                    Amount = "1000000"
                                }
                            }
                        }
                    }
                },
                Fee = new StdFee(){
                    Amount = new List<Types.Coin>(){
                        new Types.Coin(){
                            Denom = "stake",
                            Amount = "1"
                        }
                    },
                    Gas = "100"
                },
                Memo = "This is a test.",
            };

            var meta = new SignMeta(){
                AccountNumber = "1",
                ChainId = "cosmos",
                Sequence = 0
            };

            var signMsg = new StdSignMsg<MsgSend>(tx,meta);

            var signedTx = walletMster.Sign(signMsg);
            Assert.AreEqual("zbaibf4Dh4wwM0spbZlnUWR9mGN8HwFUqyp29Mf7Ysoa0iVKUZuXrYAfTNP7pmwhMmAgp/3dolIiitQVt9tQIw==",signedTx.Signatures.First().SignatureInfo);
            Assert.AreEqual("tendermint/PubKeySecp256k1", signedTx.Signatures.First().PubKey.Type);
            Assert.AreEqual("AzykdzV0sacCAYNWIR04V2MY9GRxsnJZj1+xnclJv255", signedTx.Signatures.First().PubKey.PubKeySecp256k1Value);
        }
    }
}