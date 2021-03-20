﻿#if DEBUG
using System.Diagnostics;
#endif
using System;
using System.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using SeguraChain_Lib.Blockchain.Setting;
using SeguraChain_Lib.Blockchain.Wallet.Object.Wallet;
using SeguraChain_Lib.Other.Object.SHA3;
using SeguraChain_Lib.Utility;
using BigInteger = Org.BouncyCastle.Math.BigInteger;
using System.Threading;

namespace SeguraChain_Lib.Blockchain.Wallet.Function
{
    public class ClassWalletUtility
    {
        public static readonly X9ECParameters ECParameters = SecNamedCurves.GetByName(BlockchainSetting.CurveName);
        public static readonly ECDomainParameters ECDomain = new ECDomainParameters(ECParameters.Curve, ECParameters.G, ECParameters.N, ECParameters.H);
        private const string BaseHexCurve = "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
        private static ISigner _signer = SignerUtilities.GetSigner(BlockchainSetting.SignerName);



        #region Functions for generate a wallet.

        /// <summary>
        /// Generate a new wallet.
        /// </summary>
        /// <param name="baseWords"></param>
        /// <param name="fastGenerator">Use the fast generator way for generate a private key.</param>
        /// <returns>Return a complete wallet generated.</returns>
        public static ClassWalletGeneratorObject GenerateWallet(string baseWords, bool fastGenerator = false)
        {
#if DEBUG
            Stopwatch watchProcessTimespend = new Stopwatch();
            watchProcessTimespend.Start();
#endif
            ClassWalletGeneratorObject walletObject;

            string privateKeyWallet = GenerateWalletPrivateKey(baseWords, fastGenerator);
            string publicKeyWallet = GenerateWalletPublicKeyFromPrivateKey(privateKeyWallet);
            string walletAddress = GenerateWalletAddressFromPublicKey(publicKeyWallet);

            walletObject = new ClassWalletGeneratorObject
            {
                WalletAddress = walletAddress,
                WalletPublicKey = publicKeyWallet,
                WalletPrivateKey = privateKeyWallet
            };

#if DEBUG
            watchProcessTimespend.Stop();
            Debug.WriteLine("Amount of time spend for generate the whole wallet: " + watchProcessTimespend.ElapsedMilliseconds + " ms.");
#endif

            return walletObject;
        }

        /// <summary>
        /// Generate a private key.
        /// </summary>
        /// <param name="baseWords"></param>
        /// <param name="fastGenerator">Use the fast generator way for generate a private key.</param>
        /// <returns>Return a private key WIF.</returns>
        public static string GenerateWalletPrivateKey(string baseWords, bool fastGenerator = false)
        {
            /* Was usefull on testing SECT571R1 signer and ECDSA-SHA512 performances.
#if DEBUG
            Stopwatch watchProcessTimespend = new Stopwatch();

            watchProcessTimespend.Start();
#endif
            */

            // Not really much secure..
            if (!fastGenerator)
            {
                bool useBaseWord = !baseWords.IsNullOrEmpty(); // If false, use random word process.

                byte[] privateKeyWifByteArray = new byte[BlockchainSetting.WalletPrivateKeyWifByteArrayLength];
                if (useBaseWord)
                {
                    using (ClassSha3512DigestDisposable sha3512Digest = new ClassSha3512DigestDisposable())
                    {
                        sha3512Digest.Compute(ClassUtility.GetByteArrayFromStringAscii(baseWords), out privateKeyWifByteArray);
                    }
                }
                else
                {
                    using (RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider())
                    {
                        rngCrypto.GetBytes(privateKeyWifByteArray, 0, BlockchainSetting.WalletPrivateKeyWifByteArrayLength);
                    }
                    using (ClassSha3512DigestDisposable sha3512Digest = new ClassSha3512DigestDisposable())
                    {
                        sha3512Digest.Compute(privateKeyWifByteArray, out privateKeyWifByteArray);
                    }
                }

                #region Input Blockchain version and generate the private key WIF.

                Array.Resize(ref privateKeyWifByteArray, BlockchainSetting.WalletPrivateKeyWifByteArrayLength);

                ClassUtility.InsertBlockchainVersionToByteArray(privateKeyWifByteArray, out var finalPrivateKeyHex);

                // Clean up.
                Array.Clear(privateKeyWifByteArray, 0, privateKeyWifByteArray.Length);

                string privateKeyWif = ClassBase58.EncodeWithCheckSum(ClassUtility.GetByteArrayFromHexString(finalPrivateKeyHex));

                #endregion

                /*
#if DEBUG
                watchProcessTimespend.Stop();
                Debug.WriteLine("Timespend to generate a private key: " + watchProcessTimespend.ElapsedMilliseconds + " ms.");
#endif
                */

                return privateKeyWif;
            }
            else
            {
                string hexPrivateKey = string.Empty;
                for (int i = 0; i < BaseHexCurve.Length; i++)
                {
                    int indexChar = ClassUtility.GetRandomBetweenInt(0, ClassUtility.ListOfHexCharacters.Count - 1);
                    hexPrivateKey += ClassUtility.ListOfHexCharacters.ElementAt(indexChar);
                }

                byte[] privateKeyWifByteArray = ClassUtility.GetByteArrayFromHexString(hexPrivateKey);

                Array.Resize(ref privateKeyWifByteArray, BlockchainSetting.WalletPrivateKeyWifByteArrayLength);

                /*
                using (RNGCryptoServiceProvider randomProvider = new RNGCryptoServiceProvider())
                {
                    randomProvider.GetBytes(privateKeyWifByteArray, 0, privateKeyWifByteArray.Length);
                }*/


                ClassUtility.InsertBlockchainVersionToByteArray(privateKeyWifByteArray, out var finalPrivateKeyHex);

                /*
#if DEBUG
                Debug.WriteLine("Private key Hex generated: " + finalPrivateKeyHex);
#endif
                */

                // Clean up.
                Array.Clear(privateKeyWifByteArray, 0, privateKeyWifByteArray.Length);

                string privateKeyWif = ClassBase58.EncodeWithCheckSum(ClassUtility.GetByteArrayFromHexString(finalPrivateKeyHex));

                /* Was usefull on testing SECT571R1 signer and ECDSASHA512 performances.
#if DEBUG
                watchProcessTimespend.Stop();
                Debug.WriteLine("Timespend to generate a private key: " + watchProcessTimespend.ElapsedMilliseconds + " ms.");
#endif
                */

                return privateKeyWif;
            }
        }

        /// <summary>
        /// Generate a public key from a private key.
        /// </summary>
        /// <param name="privateKeyWif"></param>
        /// <param name="blockReward"></param>
        /// <returns>Return a public key WIF.</returns>
        public static string GenerateWalletPublicKeyFromPrivateKey(string privateKeyWif, bool blockReward = false)
        {
            if (privateKeyWif.IsNullOrEmpty())
            {
                return null;
            }

            if (!blockReward)
            {
                if (privateKeyWif.Length != BlockchainSetting.WalletPrivateKeyWifLength)
                {
                    return string.Empty;
                }
            }
            else
            {
                if (privateKeyWif.Length < BlockchainSetting.WalletAddressWifLengthMin || privateKeyWif.Length > BlockchainSetting.WalletAddressWifLengthMax)
                {
                    return string.Empty;
                }
            }

            /* Was usefull on testing SECT571R1 signer and ECDSASHA512 performances.
#if DEBUG
            Stopwatch watchProcessTimespend = new Stopwatch();

            watchProcessTimespend.Start();
#endif
            */



            Org.BouncyCastle.Math.EC.ECPoint publicKeyOne = ECParameters.G.Multiply(new BigInteger(ClassBase58.DecodeWithCheckSum(privateKeyWif, true)));


            /* Was usefull on testing SECT571R1 signer and ECDSASHA512 performances.
#if DEBUG
            watchProcessTimespend.Stop();
            Debug.WriteLine("Timespend to generate a public key: " + watchProcessTimespend.ElapsedMilliseconds + " ms.");
#endif
            */

            return ClassBase58.EncodeWithCheckSum(new ECPublicKeyParameters(publicKeyOne, ECDomain).Q.GetEncoded());


        }

        /// <summary>
        /// Generate a wallet address from the public key.
        /// </summary>
        /// <param name="publicKeyWif"></param>
        /// <returns>Return a wallet address WIF.</returns>
        public static string GenerateWalletAddressFromPublicKey(string publicKeyWif)
        {
            if (publicKeyWif.IsNullOrEmpty())
            {
                return null;
            }

            if (publicKeyWif.Length != BlockchainSetting.WalletPublicKeyWifLength)
            {
                return string.Empty;
            }

            // Was usefull on testing SECT571R1 signer and ECDSASHA512 performances.
#if DEBUG
            Stopwatch watchProcessTimespend = new Stopwatch();

            watchProcessTimespend.Start();
#endif


            byte[] walletAddressByteArray = new byte[BlockchainSetting.WalletAddressByteArrayLength - 1];

            using (ClassSha3512DigestDisposable sha512 = new ClassSha3512DigestDisposable())
            {

                sha512.Compute(ClassBase58.DecodeWithCheckSum(publicKeyWif, false), out walletAddressByteArray);
                sha512.Reset();
            }


            ClassUtility.InsertBlockchainVersionToByteArray(walletAddressByteArray, out var finalWalletAddressHexWif);


            // Convert wallet address into Base58 Format.
            string walletAddressWif = ClassBase58.EncodeWithCheckSum(ClassUtility.GetByteArrayFromHexString(finalWalletAddressHexWif));


            /*// Was usefull on testing SECT571R1 signer and ECDSASHA512 performances.
#if DEBUG
            watchProcessTimespend.Stop();
            Debug.WriteLine("Timespend to generate a wallet address: " + watchProcessTimespend.ElapsedMilliseconds + " ms.");
#endif
            */

            // Cleanup.
            Array.Clear(walletAddressByteArray, 0, walletAddressByteArray.Length);


            return walletAddressWif;
        }

        #endregion

        #region Function about transaction/messages signatures.


        /// <summary>
        /// Sign the transaction with the private key of the wallet.
        /// </summary>
        /// <param name="privateKeyWif"></param>
        /// <param name="hash"></param>
        /// <returns>Return the signature of a transaction.</returns>
        public static string WalletGenerateSignature(string privateKeyWif, string hash)
        {
            if (privateKeyWif.IsNullOrEmpty() || hash.IsNullOrEmpty())
            {
                return null;
            }

            /* Was usefull on testing SECT571R1 signer and ECDSASHA512 performances.
#if DEBUG
            Stopwatch stopwatch =new Stopwatch(); 
            stopwatch.Start();
#endif
            */


            var signer = SignerUtilities.GetSigner(BlockchainSetting.SignerName);

            signer.Init(true, new ECPrivateKeyParameters(new BigInteger(ClassBase58.DecodeWithCheckSum(privateKeyWif, true)), ECDomain));

            signer.BlockUpdate(ClassUtility.GetByteArrayFromHexString(hash), 0, hash.Length / 2);

            string signature = Convert.ToBase64String(signer.GenerateSignature());

            // Reset.
            signer.Reset();

            return signature;
        }

        /// <summary>
        /// Check the transaction signature with a transaction signed and it's public key.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="signature"></param>
        /// <param name="publicKeyWif"></param>
        /// <returns>Return check result from a signature.</returns>
        public static bool WalletCheckSignature(string hash, string signature, string publicKeyWif)
        {
            if (hash.IsNullOrEmpty() || publicKeyWif.IsNullOrEmpty() || signature.IsNullOrEmpty())
            {
                return false;
            }

            var signer = SignerUtilities.GetSigner(BlockchainSetting.SignerName);
            signer.Init(false, new ECPublicKeyParameters(ECParameters.Curve.DecodePoint(ClassBase58.DecodeWithCheckSum(publicKeyWif, false)), ECDomain));

            signer.BlockUpdate(ClassUtility.GetByteArrayFromHexString(hash), 0, hash.Length / 2);

            bool result = signer.VerifySignature(Convert.FromBase64String(signature));

            // Reset.
            signer.Reset();


            return result;
        }

        #endregion

        #region Function about to check wallet address and public key format.

        /// <summary>
        /// Check a wallet address format.
        /// </summary>
        /// <param name="walletAddress"></param>
        /// <returns></returns>
        public static bool CheckWalletAddress(string walletAddress)
        {
            if (walletAddress.IsNullOrEmpty())
            {
                return false;
            }

            #region Check WIF length.

            if (walletAddress.Length < BlockchainSetting.WalletAddressWifLengthMin || walletAddress.Length > BlockchainSetting.WalletAddressWifLengthMax)
            {
                return false;
            }

            #endregion

            #region Check WIF encoding.

            if (ClassBase58.DecodeWithCheckSum(walletAddress, true) == null)
            {
                return false;
            }

            #endregion

            return true;
        }

        /// <summary>
        /// Check a wallet public key format.
        /// </summary>
        /// <param name="walletPublicKey"></param>
        /// <returns></returns>
        public static bool CheckWalletPublicKey(string walletPublicKey)
        {
            if (walletPublicKey.IsNullOrEmpty())
            {
                return false;
            }

            #region Check WIF length.

            if (walletPublicKey.Length != BlockchainSetting.WalletPublicKeyWifLength)
            {
                return false;
            }

            #endregion

            #region Check WIF encoding.

            if (ClassBase58.DecodeWithCheckSum(walletPublicKey, false) == null)
            {
                return false;
            }

            #endregion

            return true;
        }

        #endregion
    }
}
