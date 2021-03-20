﻿using SeguraChain_Lib.Other.Object.SHA3;
using SeguraChain_Lib.Utility;
using System;
using System.Threading;

namespace SeguraChain_Lib.Algorithm
{
    public class ClassSha
    {
        private const int SizeSplitData = 1024;

        /// <summary>
        /// Make a big sha3-512 hash representation depending of the size of the data.
        /// Attempt to protect against extension length attacks.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string MakeBigShaHashFromBigData(byte[] data, CancellationTokenSource cancellation)
        {
            string hash = string.Empty;

            using (ClassSha3512DigestDisposable shaObject = new ClassSha3512DigestDisposable())
            {
                if (data.Length > SizeSplitData)
                {
                    long lengthProceed = 0;

                    while (lengthProceed < data.Length)
                    {
                        cancellation?.Token.ThrowIfCancellationRequested();

                        long lengthToProceed = SizeSplitData;

                        if (lengthToProceed + lengthProceed > data.Length)
                        {
                            lengthToProceed = data.Length - lengthProceed;
                        }

                        byte[] dataToProceed = new byte[lengthToProceed];

                        Array.Copy(data, lengthProceed, dataToProceed, 0, lengthToProceed);

                        hash += ClassUtility.GetHexStringFromByteArray(shaObject.Compute(dataToProceed));

                        lengthProceed += lengthToProceed;
                    }
                }
                else
                {
                    hash = ClassUtility.GetHexStringFromByteArray(shaObject.Compute(data));
                }

                shaObject.Reset();
            }

            return hash;
        }
    }
}
