﻿using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using SeguraChain_Lib.Algorithm;
using SeguraChain_Lib.Blockchain.Setting;
using SeguraChain_Lib.Blockchain.Wallet.Function;
using SeguraChain_Lib.Instance.Node.Setting.Object;
using SeguraChain_Lib.Utility;

namespace SeguraChain_Lib.Instance.Node.Network.Database.Object
{
    public class ClassPeerCryptoStreamObject
    {
        /// <summary>
        /// Encryption/Decryption streams.
        /// </summary>
        private RijndaelManaged _aesManaged;
        private ICryptoTransform _encryptCryptoTransform;
        private ICryptoTransform _decryptCryptoTransform;

        /// <summary>
        /// Handle multithreading access.
        /// </summary>
        private SemaphoreSlim _semaphoreUpdateCryptoStream;
        private SemaphoreSlim _semaphoreDoEncryption;
        private SemaphoreSlim _semaphoreDoDecryption;

        private string _privateKey;
        private ECPrivateKeyParameters _ecPrivateKeyParameters;
        private string _publicKey;
        private ECPublicKeyParameters _ecPublicKeyParameters;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="publicKey"></param>
        /// <param name="privateKey"></param>
        /// 
        public ClassPeerCryptoStreamObject(byte[] key, byte[] iv, string publicKey, string privateKey, CancellationTokenSource cancellation)
        {
            _publicKey = string.Empty;
            _privateKey = string.Empty;
            _semaphoreUpdateCryptoStream = new SemaphoreSlim(1, 1);
            _semaphoreDoEncryption = new SemaphoreSlim(1, ClassUtility.GetMaxAvailableProcessorCount());
            _semaphoreDoDecryption = new SemaphoreSlim(1, ClassUtility.GetMaxAvailableProcessorCount());
            InitializeAesAndEcdsaSign(key, iv, publicKey, privateKey, true, cancellation);
        }

        /// <summary>
        /// Update the crypto stream informations.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="publicKey"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public void UpdateEncryptionStream(byte[] key, byte[] iv, string publicKey, string privateKey, CancellationTokenSource cancellation)
        {
            InitializeAesAndEcdsaSign(key, iv, publicKey, privateKey, false, cancellation);
        }

        /// <summary>
        /// Initialize AES.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="publicKey"></param>
        /// <param name="privateKey"></param>
        private void InitializeAesAndEcdsaSign(byte[] key, byte[] iv, string publicKey, string privateKey, bool fromInitialization, CancellationTokenSource cancellation)
        {
            bool semaphoreUsed = false;

            try
            {
                _semaphoreUpdateCryptoStream.Wait(cancellation.Token);
                semaphoreUsed = true;

                try
                {
                    if (fromInitialization || _aesManaged == null)
                    {
                        _aesManaged = new RijndaelManaged()
                        {
                            KeySize = ClassAes.EncryptionKeySize,
                            BlockSize = ClassAes.EncryptionBlockSize,
                            Key = key,
                            IV = iv,
                            Mode = CipherMode.CFB,
                            Padding = PaddingMode.PKCS7
                        };
                    }
                    else
                    {

                        try
                        {
                            _aesManaged?.Dispose();
                        }
                        catch
                        {
                            // Ignored.
                        }

                        _aesManaged = new RijndaelManaged()
                        {
                            KeySize = ClassAes.EncryptionKeySize,
                            BlockSize = ClassAes.EncryptionBlockSize,
                            Key = key,
                            IV = iv,
                            Mode = CipherMode.CFB,
                            Padding = PaddingMode.PKCS7
                        };
                    }

                    if (fromInitialization || _encryptCryptoTransform == null)
                    {
                        _encryptCryptoTransform = _aesManaged.CreateEncryptor(key, iv);
                    }
                    else
                    {

                        try
                        {
                            _encryptCryptoTransform?.Dispose();
                        }
                        catch
                        {
                            // Ignored.
                        }

                        _encryptCryptoTransform = _aesManaged.CreateEncryptor(key, iv);

                    }

                    if (fromInitialization || _decryptCryptoTransform == null)
                    {
                        _decryptCryptoTransform = _aesManaged.CreateDecryptor(key, iv);
                    }
                    else
                    {

                        try
                        {
                            _decryptCryptoTransform?.Dispose();
                        }
                        catch
                        {
                            // Ignored.
                        }
                        _decryptCryptoTransform = _aesManaged.CreateDecryptor(key, iv);

                    }
                    if (!publicKey.IsNullOrEmpty() && !privateKey.IsNullOrEmpty())
                    {
                        _privateKey = privateKey;
                        _ecPrivateKeyParameters = new ECPrivateKeyParameters(new BigInteger(ClassBase58.DecodeWithCheckSum(privateKey, true)), ClassWalletUtility.ECDomain);
                        _publicKey = publicKey;
                        _ecPublicKeyParameters = new ECPublicKeyParameters(ClassWalletUtility.ECParameters.Curve.DecodePoint(ClassBase58.DecodeWithCheckSum(publicKey, false)), ClassWalletUtility.ECDomain);
                    }
                }
                catch
                {
                    // Ignored.
                }
            }
            finally
            {
                if (semaphoreUsed)
                {
                    _semaphoreUpdateCryptoStream.Release();
                }
            }

        }

        /// <summary>
        /// Encrypt data.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<byte[]> EncryptDataProcess(byte[] content, CancellationTokenSource cancellation)
        {
            byte[] result = null;
            bool semaphoreUsed = false;
            try
            {
                await _semaphoreDoEncryption.WaitAsync(cancellation.Token);
                semaphoreUsed = true;

                try
                {
                    if (content.Length > 0)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, _encryptCryptoTransform, CryptoStreamMode.Write))
                            {
                                await cryptoStream.WriteAsync(content, 0, content.Length, cancellation.Token);

                                if (!cryptoStream.HasFlushedFinalBlock)
                                {
                                    cryptoStream.FlushFinalBlock();
                                }

                                if (!cancellation.IsCancellationRequested)
                                {
                                    if (memoryStream.Length > 0)
                                    {
                                        result = memoryStream.ToArray();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception error)
                {
#if DEBUG
                    Debug.WriteLine("Error on encrypt data from a peer. Exception: " + error.Message);
#endif
                    result = null;
                }
            }
            finally
            {
                if (semaphoreUsed)
                {
                    _semaphoreDoEncryption.Release();
                }
            }

            return result;
        }

        /// <summary>
        /// Decrypt data.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<Tuple<byte[], bool>> DecryptDataProcess(byte[] content, CancellationTokenSource cancellation)
        {
            byte[] result = null;
            bool decryptStatus = false;
            bool semaphoreUsed = false;

            try
            {
                await _semaphoreDoDecryption.WaitAsync(cancellation.Token);
                semaphoreUsed = true;

                try
                {
                    if (content.Length > 0)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, _decryptCryptoTransform, CryptoStreamMode.Write))
                            {
                                await cryptoStream.WriteAsync(content, 0, content.Length, cancellation.Token);


                                if (!cryptoStream.HasFlushedFinalBlock)
                                {
                                    cryptoStream.FlushFinalBlock();
                                }


                                if (!cancellation.IsCancellationRequested)
                                {
                                    if (memoryStream.Length > 0)
                                    {
                                        result = memoryStream.ToArray();

                                        if (result.Length > 0)
                                        {
                                            decryptStatus = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception error)
                {
#if DEBUG
                    Debug.WriteLine("Error on decrypt data from a peer. Exception: " + error.Message);
#endif
                    result = null;
                    decryptStatus = false;
                }
            }
            finally
            {
                if (semaphoreUsed)
                {
                    _semaphoreDoDecryption.Release();
                }
            }

            return new Tuple<byte[], bool>(result, decryptStatus);
        }

        /// <summary>
        /// Generate a signature.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public string DoSignatureProcess(string hash, string privateKey)
        {
            string signature = string.Empty;

            if (privateKey != null)
            {
                var _signerDoSignature = SignerUtilities.GetSigner(BlockchainSetting.SignerName);

                if (privateKey != _privateKey || _ecPrivateKeyParameters == null)
                {
                    _privateKey = privateKey;
                    _ecPrivateKeyParameters = new ECPrivateKeyParameters(new BigInteger(ClassBase58.DecodeWithCheckSum(privateKey, true)), ClassWalletUtility.ECDomain);
                }

                _signerDoSignature.Init(true, _ecPrivateKeyParameters);

                _signerDoSignature.BlockUpdate(ClassUtility.GetByteArrayFromHexString(hash), 0, hash.Length / 2);


                signature = Convert.ToBase64String(_signerDoSignature.GenerateSignature());

                // Reset.
                _signerDoSignature.Reset();
            }

            return signature;
        }

        /// <summary>
        /// Check a signature.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="signature"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public bool CheckSignatureProcess(string hash, string signature, string publicKey)
        {
            bool result = false;

            if (publicKey != null)
            {
                var _signerCheckSignature = SignerUtilities.GetSigner(BlockchainSetting.SignerName);

                if (publicKey != _publicKey || _ecPublicKeyParameters == null)
                {
                    _publicKey = publicKey;
                    _ecPublicKeyParameters = new ECPublicKeyParameters(ClassWalletUtility.ECParameters.Curve.DecodePoint(ClassBase58.DecodeWithCheckSum(publicKey, false)), ClassWalletUtility.ECDomain);
                }

                _signerCheckSignature.Init(false, _ecPublicKeyParameters);

                _signerCheckSignature.BlockUpdate(ClassUtility.GetByteArrayFromHexString(hash), 0, hash.Length / 2);

                 result = _signerCheckSignature.VerifySignature(Convert.FromBase64String(signature));

                // Reset.
                _signerCheckSignature.Reset();
            }

            return result;
        }

    }
}
