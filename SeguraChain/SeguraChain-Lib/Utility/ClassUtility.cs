﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LZ4;
using Newtonsoft.Json;
using SeguraChain_Lib.Blockchain.Setting;
using SeguraChain_Lib.Instance.Node.Network.Enum.P2P.Packet;
using SeguraChain_Lib.Other.Object.List;
using SeguraChain_Lib.Other.Object.SHA3;

namespace SeguraChain_Lib.Utility
{
    public class ClassUtility
    {
        #region Constant objects.

        private const string Base64Regex = @"^[a-zA-Z0-9\+/]*={0,3}$";

        private static readonly UTF8Encoding Utf8Encoding = new UTF8Encoding(false);

        private static readonly List<string> ListOfCharacters = new List<string>
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
            "v", "w", "x", "y", "z"

        };

        private static readonly List<string> ListOfOtherCharacters = new List<string>
        {
            "&", "é", "\"", "'", "(", "[", "|", "è", "_", "\\", "-", "ç", "à", ")", "=", "~", "#", "{", "[", "|", "`", "^", "@", "]", "}", "¨", "$", "£", "¤", "%", "ù", "*", "µ", ",", ";", ":", "!", "?", ".", "/", "§", "€", "\t", "\b", "*", "+"
        };

        public static readonly HashSet<char> ListOfHexCharacters = new HashSet<char>
        {
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9'
        };

        private static readonly HashSet<char> ListOfBase64Characters = new HashSet<char>
        {
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
            'a',
            'b',
            'c',
            'd',
            'e',
            'f',
            'g',
            'h',
            'i',
            'j',
            'k',
            'l',
            'm',
            'n',
            'o',
            'p',
            'q',
            'r',
            's',
            't',
            'u',
            'v',
            'w',
            'x',
            'y',
            'z',
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            '+',
            '/',
            '='
        };

        private static readonly List<string> ListOfNumbers = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        #endregion

        #region Static functions about SHA hash

        /// <summary>
        /// Generate a SHA 512 hash from a string.
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string GenerateSha3512FromString(string sourceString)
        {

            using (var hash = new ClassSha3512DigestDisposable())
            {

                hash.Compute(GetByteArrayFromStringAscii(sourceString), out byte[] hashedInputBytes);

                var hashedInputStringBuilder = new StringBuilder(BlockchainSetting.BlockchainSha512HexStringLength);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));

                string hashToReturn = hashedInputStringBuilder.ToString();

                #region Clear obsolete objects from memory

                Array.Clear(hashedInputBytes, 0, hashedInputBytes.Length);
                hashedInputStringBuilder.Clear();

                #endregion


                return hashToReturn;
            }

        }


        /// <summary>
        /// Generate a SHA 512 byte array from a byte array.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] GenerateSha512ByteArrayFromByteArray(byte[] source)
        {
            using (var hash = new ClassSha3512DigestDisposable())
            {
                return hash.Compute(source);
            }
        }

        #endregion

        #region Static functions about Random Secure RNG.

        /// <summary>
        ///  Generate a random long number object between a range selected.
        /// </summary>
        /// <param name="minimumValue"></param>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        public static long GetRandomBetweenLong(long minimumValue, long maximumValue)
        {
            using (RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[sizeof(long)];

                generator.GetBytes(randomNumber);

                var asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

                var multiplier = Math.Max(0, asciiValueOfRandomCharacter / 255d - 0.00000000001d);

                var range = maximumValue - minimumValue + 1;

                var randomValueInRange = Math.Floor(multiplier * range);

                return (long)(minimumValue + randomValueInRange);
            }
        }

        /// <summary>
        ///  Generate a random long number object between a range selected.
        /// </summary>
        /// <param name="minimumValue"></param>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        public static BigInteger GetRandomBetweenBigInteger(double minimumValue, double maximumValue)
        {
            using (RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[sizeof(double)];

                generator.GetBytes(randomNumber);

                var asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

                var multiplier = Math.Max(0, asciiValueOfRandomCharacter / 255d - 0.00000000001d);

                var range = maximumValue - minimumValue + 1;

                var randomValueInRange = Math.Floor(multiplier * range);

                return new BigInteger(minimumValue + randomValueInRange);
            }
        }

        /// <summary>
        ///  Generate a random int number object between a range selected.
        /// </summary>
        /// <param name="minimumValue"></param>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        public static int GetRandomBetweenInt(int minimumValue, int maximumValue)
        {
            using (RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[sizeof(int)];

                generator.GetBytes(randomNumber);

                var asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

                var multiplier = Math.Max(0, asciiValueOfRandomCharacter / 255d - 0.00000000001d);

                var range = maximumValue - minimumValue + 1;

                var randomValueInRange = Math.Floor(multiplier * range);

                return (int)(minimumValue + randomValueInRange);
            }
        }

        #endregion

        #region Static functions about content format. 

        /// <summary>
        /// Check if the string use only lowercase.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CheckStringUseLowercaseOnly(string value)
        {
            return value.All(t => !char.IsUpper(t));
        }

        /// <summary>
        /// Convert a string into a byte array object.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromStringAscii(string content)
        {
            return Encoding.ASCII.GetBytes(content);
        }

        /// <summary>
        /// Convert a string into a byte array object.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromStringUtf8(string content)
        {
            return Utf8Encoding.GetBytes(content);
        }

        private static readonly uint[] Lookup32 = CreateLookup32();

        /// <summary>
        /// Create a lookup conversation for accelerate byte array conversion into hex string.
        /// </summary>
        /// <returns></returns>
        private static uint[] CreateLookup32()
        {
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("X2");
                result[i] = s[0] + ((uint)s[1] << 16);
            }
            return result;
        }

        /// <summary>
        /// Convert a byte array to hex string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetHexStringFromByteArray(byte[] bytes)
        {
            var lookup32 = Lookup32;
            var result = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var val = lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result, 0, result.Length);
        }

        /// <summary>
        /// Return a long value from a hex string.
        /// </summary>
        /// <param name="hexContent"></param>
        /// <returns></returns>
        public static long GetLongFromHexString(string hexContent)
        {
            return Convert.ToInt64(hexContent, 16);
        }

        /// <summary>
        /// RemoveFromCache decimal point from decimal value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal RemoveDecimalPoint(decimal value)
        {
            string stringValue = value.ToString(CultureInfo.InvariantCulture);

            if (stringValue.Contains("."))
            {
                string[] stringValueArray = stringValue.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (!decimal.TryParse(stringValueArray[0], out value))
                {
                    value = 0;
                }
            }
            else if (stringValue.Contains(","))
            {
                string[] stringValueArray = stringValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (!decimal.TryParse(stringValueArray[0], out value))
                {
                    value = 0;
                }
            }

            return value;
        }

        /// <summary>
        /// Convert a hex string into byte array.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromHexString(string hex)
        {
            try
            {
                byte[] ret = new byte[hex.Length / 2];
                for (int i = 0; i < ret.Length; i++)
                {
                    int high = hex[i * 2];
                    int low = hex[i * 2 + 1];
                    high = (high & 0xf) + ((high & 0x40) >> 6) * 9;
                    low = (low & 0xf) + ((low & 0x40) >> 6) * 9;

                    ret[i] = (byte)((high << 4) | low);
                }

                return ret;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Check if the sha string contain only hex characters.
        /// </summary>
        /// <param name="shaHexString"></param>
        /// <returns></returns>
        public static bool CheckHexStringFormat(string shaHexString)
        {
            if (shaHexString.IsNullOrEmpty())
            {
                return false;
            }

            if (shaHexString.ToLower().Count(character => ListOfHexCharacters.Contains(character)) != shaHexString.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Remove every characters no-hex from a string.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static string RemoveNoHexCharacterFromString(string hexString)
        {
            string newHexString = string.Empty;

            if (!hexString.IsNullOrEmpty())
            {
                foreach (var hexCharacter in hexString.ToLower())
                {
                    if (ListOfHexCharacters.Contains(hexCharacter))
                    {
                        newHexString += hexCharacter;
                    }
                }
            }

            return newHexString;
        }

        /// <summary>
        /// Check if the string is a base64 string.
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static bool CheckBase64String(string base64String)
        {
            return (base64String.Length % 4 == 0) && Regex.IsMatch(base64String.Trim(), Base64Regex, RegexOptions.None);
        }

        public static bool CharIsABase64Character(char base64Character)
        {
            return ListOfBase64Characters.Contains(base64Character);
        }

        #endregion

        #region Static functions about random word.

        /// <summary>
        /// Random word.
        /// </summary>
        /// <param name="lengthTarget"></param>
        /// <returns></returns>
        public static string GetRandomWord(int lengthTarget)
        {
            string word = string.Empty;

            for (int i = 0; i < lengthTarget; i++)
            {
                var percent1 = GetRandomBetweenLong(1, 100); // Numbers.
                var percent2 = GetRandomBetweenLong(1, 100); // Letters.
                var percent3 = GetRandomBetweenLong(1, 100); // Special characters.

                if (percent1 >= percent2 && percent1 >= percent3) // Use numbers.
                {
                    word += ListOfNumbers[GetRandomBetweenInt(0, ListOfNumbers.Count - 1)];
                }
                else
                {
                    if (percent2 >= percent3) // Use letters.
                    {
                        percent1 = GetRandomBetweenLong(1, 100);
                        percent2 = GetRandomBetweenLong(1, 100);
                        if (percent2 >= percent1) // use Uppercase.
                        {
                            word += ListOfCharacters[GetRandomBetweenInt(0, ListOfCharacters.Count - 1)].ToUpper();
                        }
                        else // use normal lowercase.
                        {
                            word += ListOfCharacters[GetRandomBetweenInt(0, ListOfCharacters.Count - 1)];
                        }
                    }
                    else
                    {
                        if (percent3 >= percent1) // Use special characters.
                        {
                            word += ListOfOtherCharacters[GetRandomBetweenInt(0, ListOfOtherCharacters.Count - 1)];
                        }
                        else // Use numbers.
                        {
                            word += ListOfNumbers[GetRandomBetweenInt(0, ListOfNumbers.Count - 1)];
                        }
                    }
                }
            }

            return word;
        }

        /// <summary>
        /// Random word into byte array object.
        /// </summary>
        /// <param name="lengthTarget"></param>
        /// <returns></returns>
        public static byte[] GetRandomByteArrayWord(int lengthTarget)
        {
            string word = string.Empty;

            for (int i = 0; i < lengthTarget; i++)
            {
                var percent1 = GetRandomBetweenLong(1, 100);
                var percent2 = GetRandomBetweenLong(1, 100);
                if (percent1 >= percent2) // Use numbers.
                {
                    word += ListOfNumbers[GetRandomBetweenInt(0, ListOfNumbers.Count - 1)];
                }
                else // Use letters
                {
                    percent1 = GetRandomBetweenLong(1, 100);
                    percent2 = GetRandomBetweenLong(1, 100);
                    if (percent2 >= percent1) // use Uppercase.
                    {
                        word += ListOfCharacters[GetRandomBetweenInt(0, ListOfCharacters.Count - 1)].ToUpper();
                    }
                    else // use normal lowercase.
                    {
                        word += ListOfCharacters[GetRandomBetweenInt(0, ListOfCharacters.Count - 1)];
                    }
                }
            }

            return GetByteArrayFromStringAscii(word);
        }

        #endregion

        #region Static functions about timestamp.

        /// <summary>
        /// Return the current timestamp in seconds.
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTimestampInSecond()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        /// <summary>
        /// Return the current timestamp in millisecond.
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTimestampInMillisecond()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Check if the timestamp is not too late and not too early.
        /// </summary>
        /// <param name="timestampPacket"></param>
        /// <param name="maxDelay"></param>
        /// <param name="earlierDelay"></param>
        /// <returns></returns>
        public static bool CheckPacketTimestamp(long timestampPacket, int maxDelay, int earlierDelay)
        {
            long currentTimestamp = GetCurrentTimestampInSecond();

            if (timestampPacket + maxDelay >= currentTimestamp)
            {
                if (timestampPacket <= currentTimestamp + earlierDelay)
                {
                    return true;
                }
            }


            return false;
        }

        /// <summary>
        /// Convert a timestamp of seconds into a date.
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime GetDatetimeFromTimestamp(long unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();
        }

        #endregion

        #region Static functions about path.

        /// <summary>
        /// Convert the path for Linux/Unix system.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ConvertPath(string path)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        #endregion

        #region Static functions about the Blockchain.

        /// <summary>
        /// Insert a byte array on the first index
        /// </summary>
        /// <param name="baseByteArray"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static void InsertBlockchainVersionToByteArray(byte[] baseByteArray, out string result)
        {
            result = GetHexStringFromByteArray(baseByteArray);

            result = BlockchainSetting.BlockchainVersion + result;
        }

        #endregion

        #region Static function about Serialization/Deserialization.

        /// <summary>
        /// Try to deserialize an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="result"></param>
        /// <param name="handling"></param>
        public static bool TryDeserialize<T>(string content, out T result, ObjectCreationHandling handling = ObjectCreationHandling.Auto)
        {
            try
            {
                if (!content.IsNullOrEmpty())
                {
                    // Remover whitespaces.
                    content = content.Trim();
                    if ((content.StartsWith("{") && content.EndsWith("}")))
                    {
                        result = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings() { ObjectCreationHandling = handling });
                        return true;
                    }
                }
            }
            catch
            {
                // Ignored, the format is invalid or the JSON string is empty.
            }

            result = default;
            return false;
        }

        #endregion

        #region Static functions about LZ4 Compress/Decompress.

        public static byte[] CompressDataLz4(byte[] data)
        {
            return LZ4Codec.Wrap(data, 0, data.Length);
        }

        public static byte[] DecompressDataLz4(byte[] compressed)
        {
            return LZ4Codec.Unwrap(compressed);

        }
        #endregion

        #region Static functions about GC Collector

        /// <summary>
        /// Clean Garbage collector if it's possible.
        /// </summary>
        public static void CleanGc()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        /// <summary>
        /// Return the amount of ram allocated from the process.
        /// </summary>
        /// <returns></returns>
        public static long GetMemoryAllocationFromProcess()
        {
            return Process.GetCurrentProcess().WorkingSet64;
        }

        #endregion

        #region Static functions about Socket/TCPClient/Packets.

        /// <summary>
        /// Check if the tcp client socket is still connected.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static bool SocketIsConnected(TcpClient socket)
        {
            try
            {
                if (socket?.Client != null)
                {
                    try
                    {
                        //return !(socket.Client.Poll(1, SelectMode.SelectRead) && socket.Available == 0);

                        return !((socket.Client.Poll(10, SelectMode.SelectRead) && (socket.Client.Available == 0)) || !socket.Client.Connected);

                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Return each packet splitted received.
        /// </summary>
        /// <param name="packetDataToSplit"></param>
        /// <returns></returns>
        public static DisposableList<string> GetEachPacketSplitted(byte[] packetDataToSplit, CancellationTokenSource cancellation, out int countFilled)
        {
            countFilled = 0; // Default.
            DisposableList<string> listPacketDataSplitted = new DisposableList<string>();

            listPacketDataSplitted.Add(string.Empty);

            foreach (char packetDataChar in packetDataToSplit.GetStringFromByteArrayAscii())
            {
                cancellation.Token.ThrowIfCancellationRequested();

                if (packetDataChar != ClassPeerPacketSetting.PacketPeerSplitSeperator)
                {
                    listPacketDataSplitted[countFilled] += packetDataChar;
                }
                else
                {
                    listPacketDataSplitted.Add(string.Empty);
                    countFilled++;
                }
            }

            return listPacketDataSplitted;
        }


        #endregion

        #region Static functions about locking object.

        /// <summary>
        /// Lock and object to return
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectData"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static T LockReturnObject<T>(T objectData)
        {
            bool locked = false;
            T returnData = default;

            try
            {

                if (!Monitor.IsEntered(objectData))
                {
                    Monitor.TryEnter(objectData, ref locked);
                }
                else
                {
                    locked = true;
                }

                if (locked)
                {
                    returnData = objectData;
                }

            }
            finally
            {
                if (locked)
                {
                    if (Monitor.IsEntered(objectData))
                    {
                        Monitor.Exit(objectData);
                    }
                }
            }

            return returnData;
        }

        #endregion

        #region Others static functions.

        /// <summary>
        /// Return the maximum of available processor count
        /// </summary>
        /// <returns></returns>
        public static int GetMaxAvailableProcessorCount()
        {
            return Environment.ProcessorCount;
        }


        private static readonly BigInteger KhStart = 1000;
        private static readonly BigInteger MhStart = BigInteger.Multiply(KhStart, 100);
        private static readonly BigInteger ThStart = BigInteger.Multiply(MhStart, 100);
        private static readonly BigInteger PhStart = BigInteger.Multiply(ThStart, 100);
        private static readonly BigInteger EhStart = BigInteger.Multiply(PhStart, 100);


        /// <summary>
        /// Return the hashrate formatted by his power.
        /// </summary>
        /// <param name="hashrate"></param>
        /// <returns></returns>
        public static string GetFormattedHashrate(BigInteger hashrate)
        {
            if (hashrate < KhStart)
            {
                return hashrate + " H/s";
            }
            if (hashrate >= KhStart && hashrate < MhStart)
            {
                return ((double)hashrate / (double)KhStart).ToString("N2", CultureInfo.InvariantCulture) + " KH/s";
            }
            if (hashrate >= MhStart && hashrate < ThStart)
            {
                return ((double)hashrate / (double)MhStart).ToString("N2", CultureInfo.InvariantCulture) + " MH/s";
            }
            if (hashrate >= ThStart && hashrate < PhStart)
            {
                return ((double)hashrate / (double)ThStart).ToString("N2", CultureInfo.InvariantCulture) + " TH/s";
            }
            if (hashrate >= PhStart && hashrate < EhStart)
            {
                return ((double)hashrate / (double)PhStart).ToString("N2", CultureInfo.InvariantCulture) + " PH/s";

            }
            return ((double)hashrate / (double)EhStart).ToString("N2", CultureInfo.InvariantCulture) + " EH/s";
        }

        /// <summary>
        /// Convert bytes count into megabytes.
        /// </summary>
        /// <param name="bytesCount"></param>
        /// <returns></returns>
        public static string ConvertBytesToMegabytes(long bytesCount)
        {
            if (bytesCount > 0)
            {
                return Math.Round((((double)bytesCount / 1024) / 1024), 2) + " MB(s)";
            }
            return "0 MB(s)";
        }



        #endregion
    }

    #region Extensions class's of generic objects.

    /// <summary>
    /// String extension class.
    /// </summary>
    public static class ClassUtilityStringExtension
    {
        /// <summary>
        /// Copy a base58 string.
        /// </summary>
        /// <param name="base58String"></param>
        /// <param name="isWalletAddress">Use limits of wallet base58 string length limits stated.</param>
        /// <returns></returns>
        public static string CopyBase58String(this string base58String, bool isWalletAddress)
        {
            string base58StringCopy = string.Empty;
            bool isValid = false;

            foreach (var character in base58String)
            {
                if (ClassBase58.CharacterIsInsideBase58CharacterList(character))
                {
                    base58StringCopy += character;
                }

                if (isWalletAddress)
                {
                    if (base58StringCopy.Length >= BlockchainSetting.WalletAddressWifLengthMin && base58StringCopy.Length <= BlockchainSetting.WalletAddressWifLengthMax)
                    {
                        if (ClassBase58.DecodeWithCheckSum(base58StringCopy, true) != null)
                        {
                            isValid = true;
                            break;
                        }
                    }
                }
            }

            if (isValid)
            {
                return base58StringCopy;
            }

            if (!isWalletAddress)
            {
                if (ClassBase58.DecodeWithCheckSum(base58StringCopy, false) != null)
                {
                    return base58StringCopy;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Deep copy of string, to keep out the string copied linked to the original one.
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        public static string DeepCopy(this string srcString)
        {
            string copiedString = string.Empty;

            foreach (char character in srcString)
            {
                copiedString += character;
            }
            return copiedString;
        }

        /// <summary>
        /// Extended unsafe function who permit to clear a string.
        /// </summary>
        /// <param name="s"></param>
        public static unsafe void Clear(this string s)
        {
            if (s != null)
            {
                fixed (char* ptr = s)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        ptr[i] = '\0';
                    }
                }
            }
        }

        /// <summary>
        /// Attempt to split faster a string.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="seperatorStr"></param>
        /// <param name="countGetLimit"></param>
        /// <returns></returns>
        public static DisposableList<string> DisposableSplit(this string src, string seperatorStr, int countGetLimit = 0)
        {
            DisposableList<string> listSplitted = new DisposableList<string>();


            if (src.Length > 0)
            {

                foreach (var word in src.Split(new[] { seperatorStr }, StringSplitOptions.RemoveEmptyEntries))
                {
                    listSplitted.Add(word);
                }

                #region Old split way. Too slow, around 236ms per block without tx's splitted.

                /*
                var word = new StringBuilder();
                char seperator = seperatorStr.ToCharArray(0, seperatorStr.Length)[0];

                bool seperatorFound = false;

                int countFound = 0;

                foreach (var character in src)
                {
                    if (character != seperator)
                    {
                        word.Append(character);
                    }
                    else
                    {
                        string wordComplete = word.ToString();
                        word.Clear();

                        if (!wordComplete.IsNullOrEmpty())
                        {
                            listSplitted.Add(wordComplete);
                            seperatorFound = true;
                            countFound++;
                            if (countGetLimit > 0)
                            {
                                if (countFound >= countGetLimit)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!seperatorFound)
                {
                    string wordComplete = word.ToString();
                    if (!wordComplete.IsNullOrEmpty())
                    {
                        listSplitted.Add(wordComplete);
                    }
                }

                word.Clear();

                */
                #endregion
            }


            return listSplitted;
        }

        /// <summary>
        /// Check if the string is empty.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            if (str == null) return true;
            if (str.Length == 0) return true;
            if (str == "") return true;

            if (string.IsNullOrEmpty(str.Trim()))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get a specific string between two strings.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="firstString"></param>
        /// <param name="lastString"></param>
        /// <returns></returns>
        public static string GetStringBetweenTwoStrings(this string str, string firstString, string lastString)
        {
            int pos1 = str.IndexOf(firstString, 0, StringComparison.Ordinal) + firstString.Length;
            int pos2 = str.IndexOf(lastString, 0, StringComparison.Ordinal);
            return str.Substring(pos1, pos2 - pos1);
        }
    }

    /// <summary>
    /// Byte array extension class
    /// </summary>
    public static class ClassUtilityByteArrayExtension
    {
        private static readonly ASCIIEncoding _asciiEncoding = new ASCIIEncoding();
        /// <summary>
        /// Get a string from a byte array object.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetStringFromByteArrayAscii(this byte[] content)
        {
            if (content != null)
            {
                if (content.Length > 0)
                {
                    return _asciiEncoding.GetString(content);
                }
            }
            return null;
        }

        /// <summary>
        /// Compare the source array with another one.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool CompareArray(this byte[] source, byte[] compare)
        {
            if (source != null)
            {
                if (compare == null)
                {
                    return false;
                }

                if (source.Length != compare.Length)
                {
                    return false;
                }

                return !source.Where((t, i) => t != compare[i]).Any();
            }

            if (compare != null)
            {
                return false;
            }
            return true;
        }
    }


    public static class ClassUtilityNetworkStreamExtension
    {
        /// <summary>
        /// Try to split a packet data to send and try to send it.
        /// </summary>
        /// <param name="networkStream"></param>
        /// <param name="packetBytesToSend"></param>
        /// <param name="cancellation"></param>
        /// <param name="packetMaxSize"></param>
        /// <returns></returns>
        public static async Task<bool> TrySendSplittedPacket(this NetworkStream networkStream, byte[] packetBytesToSend, CancellationTokenSource cancellation, int packetMaxSize)
        {
            bool sendStatus = true;
            try
            {
                if (packetBytesToSend.Length > 0)
                {

                    if (packetBytesToSend.Length > packetMaxSize)
                    {
                        int packetLength = packetBytesToSend.Length;
                        int countPacketSendLength = 0;
                        while (true)
                        {
                            cancellation?.Token.ThrowIfCancellationRequested();

                            int packetSize = packetMaxSize;

                            if (countPacketSendLength + packetSize > packetLength)
                            {
                                packetSize = packetLength - countPacketSendLength;
                            }

                            if (packetSize <= 0)
                            {
                                break;
                            }

                            byte[] dataBytes = new byte[packetSize];

                            Array.Copy(packetBytesToSend, countPacketSendLength, dataBytes, 0, packetSize);

                            await networkStream.WriteAsync(dataBytes, 0, dataBytes.Length, cancellation.Token);

                            countPacketSendLength += packetSize;

                            if (countPacketSendLength >= packetLength)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        await networkStream.WriteAsync(packetBytesToSend, 0, packetBytesToSend.Length, cancellation.Token);
                    }

                    await networkStream.FlushAsync(cancellation.Token);
                }
            }
            catch
            {
                sendStatus = false;
            }

            if (packetBytesToSend.Length > 0)
            {
                Array.Clear(packetBytesToSend, 0, packetBytesToSend.Length);
                GC.SuppressFinalize(packetBytesToSend);
            }

            return sendStatus;
        }
    }

    public static class ClassUtilityMemoryStreamExtension
    {
        /// <summary>
        /// Extension function who clear and reset the memory stream.
        /// </summary>
        /// <param name="memoryStream"></param>
        public static void Clear(this MemoryStream memoryStream)
        {
            if (memoryStream?.Length > 0)
            {
                try
                {
                    byte[] buffer = memoryStream.GetBuffer();
                    if (buffer != null)
                    {
                        if (buffer.Length > 0)
                        {
                            Array.Clear(buffer, 0, buffer.Length);
                        }
                    }
                }
                catch
                {
                    // Ignored, can be null or the length equal of 0
                }
                try
                {
                    memoryStream.Position = 0;
                    memoryStream.SetLength(0);
                    memoryStream.Capacity = 0;
                }
                catch
                {
                    // ignored, the capacity can change.
                }
            }
        }
    }

    #endregion


}
