﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using SeguraChain_Lib.Blockchain.Setting;

namespace SeguraChain_Lib.Utility
{
    public class ClassBase58
    {

        private const string Digits = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        private const char FixedDigit = '1';

        private static readonly Dictionary<char, int> Base58DictionaryChar = GenerateBase58DictionaryChar();
        private static readonly Dictionary<int, char> Base58DictionaryIndex = GenerateBase58DictionaryIndex();


        /// <summary>
        /// Generate base 58 dictionary characters, indexed by character.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<char, int> GenerateBase58DictionaryChar()
        {
            Dictionary<char, int> dictionary = new Dictionary<char, int>();

            int index = 0;
            foreach (var character in Digits)
            {
                if (!dictionary.ContainsKey(character))
                {
                    dictionary.Add(character, index);
                    index++;
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Generate base 58 dictionary characters, indexed by their index.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<int, char> GenerateBase58DictionaryIndex()
        {
            Dictionary<int, char> dictionary = new Dictionary<int, char>();

            int index = 0;
            foreach (var character in Digits)
            {
                if (!dictionary.ContainsValue(character))
                {
                    dictionary.Add(index, character);
                    index++;
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Encode the data with the constant checksum size into a base58 string.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncodeWithCheckSum(byte[] data)
        {
            return Encode(AddCheckSum(data));
        }

        /// <summary>
        /// Check if the character is inside the list of character used by base58 string.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static bool CharacterIsInsideBase58CharacterList(char character)
        {
            return Base58DictionaryChar.ContainsKey(character);
        }

        /// <summary>
        /// Encode the data.
        /// </summary>
        /// <param name="dataToEncode"></param>
        /// <returns></returns>
        private static string Encode(byte[] dataToEncode)
        {

            BigInteger intData = dataToEncode.Aggregate<byte, BigInteger>(0, (current, t) => current * 256 + t);

            // Encode BigInteger to Base58 string
            string result = string.Empty;
            while (intData > 0)
            {
                int remainder = (int)(intData % 58);
                intData /= 58;
                result = Base58DictionaryIndex[remainder] + result;
                if (intData <= 0)
                {
                    break;
                }
            }

            // Append `1` for each leading 0 byte
            for (int i = 0; i < dataToEncode.Length && dataToEncode[i] == 0; i++)
            {
                result = FixedDigit + result;
            }
            return result;
        }

        /// <summary>
        /// Decode the base58 with a checksum included, if their is no checksum or if this one is wrong, the function return a null byte array.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="useBlockchainVersion"></param>
        /// <returns></returns>
        public static byte[] DecodeWithCheckSum(string s, bool useBlockchainVersion)
        {
            return VerifyAndRemoveCheckSum(Decode(s, useBlockchainVersion));
        }

        /// <summary>
        /// Decode the base58.
        /// </summary>
        /// <param name="base58Content"></param>
        /// <param name="useBlockchainVersion"></param>
        /// <returns></returns>
        private static byte[] Decode(string base58Content, bool useBlockchainVersion)
        {
            if (base58Content.IsNullOrEmpty())
            {
                return null;
            }
            try
            {
                BigInteger intData = 0;
                foreach (var character in base58Content)
                {
                    if (Base58DictionaryChar.ContainsKey(character))
                    {
                        int digit = Base58DictionaryChar[character];
                        intData = intData * 58 + digit;
                    }
                    else
                    {
                        return null;
                    }
                }
                int leadingZeroCount = base58Content.TakeWhile(c => c == FixedDigit).Count();
                var leadingZeros = Enumerable.Repeat((byte)0, leadingZeroCount);
                var bytesWithoutLeadingZeros = intData.ToByteArray().Reverse().SkipWhile(b => b == 0);//strip sign byte
                var result = leadingZeros.Concat(bytesWithoutLeadingZeros).ToArray();

                if (useBlockchainVersion)
                {
                    string resultHex = ClassUtility.GetHexStringFromByteArray(result);
                    if (!resultHex.StartsWith(BlockchainSetting.BlockchainVersion))
                    {
                        return null;
                    }
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Include check in the data to encode.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] AddCheckSum(byte[] data)
        {
            return ArrayHelpers.ConcatArrays(data, GetCheckSum(data));
        }

        //Check and remove the check, returns null if the checksum is invalid.
        public static byte[] VerifyAndRemoveCheckSum(byte[] data)
        {
            if (data != null)
            {
                try
                {
                    byte[] result = ArrayHelpers.SubArray(data, 0, data.Length - BlockchainSetting.BlockchainChecksum);
                    byte[] givenCheckSum = ArrayHelpers.SubArray(data, data.Length - BlockchainSetting.BlockchainChecksum);

                    if (givenCheckSum != null)
                    {
                        byte[] correctCheckSum = GetCheckSum(result);

                        if (correctCheckSum != null)
                        {
                            if (givenCheckSum.Where((t, i) => t != correctCheckSum[i]).Any())
                            {
                                return null;
                            }

                            return result;
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Check the checkum of the data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] GetCheckSum(byte[] data)
        {
            if (data != null)
            {
                using (SHA256 sha256 = new SHA256Managed())
                {
                    byte[] hash1 = sha256.ComputeHash(data);
                    byte[] hash2 = sha256.ComputeHash(hash1);

                    var result = new byte[BlockchainSetting.BlockchainChecksum];
                    Array.Copy(hash2, 0, result, 0, result.Length);

                    // Clean up.
                    Array.Clear(hash1, 0, hash1.Length);
                    Array.Clear(hash2, 0, hash2.Length);
                    return result;
                }
            }
            return null;
        }
    }

    public class ArrayHelpers
    {
        public static T[] ConcatArrays<T>(params T[][] arrays)
        {
            var result = new T[arrays.Sum(arr => arr.Length)];
            int offset = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                var arr = arrays[i];
                Array.Copy(arr, 0, result, offset, arr.Length);

                offset += arr.Length;
            }
            return result;
        }

        public static T[] ConcatArrays<T>(T[] arr1, T[] arr2)
        {
            var result = new T[arr1.Length + arr2.Length];
            Array.Copy(arr1, 0, result, 0, arr1.Length);
            Array.Copy(arr2, 0, result, arr1.Length, arr2.Length);
            return result;
        }

        public static T[] SubArray<T>(T[] arr, int start, int length)
        {
            var result = new T[length];
            Array.Copy(arr, start, result, 0, length);
            return result;
        }

        public static T[] SubArray<T>(T[] arr, int start)
        {
            return SubArray(arr, start, arr.Length - start);
        }
    }
}

