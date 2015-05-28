//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************



using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Common
{
    public interface IRandom
    {
        int Next(int n);
    }

    public class SystemRandom : IRandom
    {
        private System.Random gen = new System.Random();
            
        public void Init(int seed)
        {
            gen = new System.Random(seed);
        }

        /// <summary>
        /// Returns a non-negative random number less than n.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        public int Next(int n)
        {
            if (n <= 0) throw new ArgumentException("argument must be positive.");
            return gen.Next(n);
        }
    }

    public class CryptoRandom : IRandom
    {
        private RandomNumberGenerator gen = new RNGCryptoServiceProvider();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        public int Next(int n)
        {
            if (n <= 0) throw new ArgumentException("argument must be positive.");

            // Create a byte array to hold the random value.
            byte[] bytes = new byte[4];

            // Fill the array with a random value.
            gen.GetBytes(bytes);

            // Convert the byte to an integer value to make the modulus operation easier.
            int randomNumber = BitConverter.ToInt32(bytes, 0);

            return Math.Abs(randomNumber % n);
        }
    }

}
