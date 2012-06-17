﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using HPD.Framework.Utils.Cryptography.Extensions;
namespace HPD.Framework.Utils.Cryptography
{
    public class Hashing
    {
        #region enum and fields

        //types of hashing available
        public enum HashingTypes
        {
            SHA, SHA256, SHA384, SHA512, MD5
        }

        private HashingTypes m_HashingType = HashingTypes.MD5;
        #endregion

        #region Constuctors
        public Hashing() : this(HashingTypes.MD5) { }

        public Hashing(HashingTypes hashingType)
        {
            m_HashingType = hashingType;
        }
        #endregion

        #region Hashing Engine

        /// <summary>
        ///		computes the hash code for a byte array
        /// </summary>
        /// <param name="inputData">input data to be hashed</param>
        /// <param name="hashingType">type of hashing to use</param>
        /// <returns>hashed byte array</returns>
        private byte[] ComputeHash(byte[] inputData)
        {
            //get appropriate Hashing algorithm and return byte array
            return GetHashAlgorithm().ComputeHash(inputData);
        }

        /// <summary>
        ///		computes the hash code and converts it to string
        /// </summary>
        /// <param name="inputText">input text to be hashed</param>
        /// <param name="hashingType">type of hashing to use</param>
        /// <returns>hashed string</returns>
        private string ComputeHash(string inputText)
        {
            //convert output byte array to a string
            byte[] byteArr = ByteArrayExtensions.ToByteArray(inputText);
            string hexString = ByteArrayExtensions.ToHexString(byteArr);
            return hexString.ToUpper();

            //F1
            //return ComputeHash(inputText.ToByteArray()).ToHexString().ToUpper();
        }

        /// <summary>
        ///		returns the specific hashing alorithm
        /// </summary>
        /// <param name="hashingType">type of hashing to use</param>
        /// <returns>HashAlgorithm</returns>
        private HashAlgorithm GetHashAlgorithm()
        {
            switch (m_HashingType)
            {
                case HashingTypes.SHA:
                    return SHA1CryptoServiceProvider.Create();
                case HashingTypes.SHA256:
                    return SHA256Managed.Create();
                case HashingTypes.SHA384:
                    return SHA384Managed.Create();
                case HashingTypes.SHA512:
                    return SHA512Managed.Create();
                default:
                    return MD5CryptoServiceProvider.Create();
            }
        }
        #endregion

        #region static members
        public static string Hash(String inputText)
        {
            return new Hashing().ComputeHash(inputText);
        }

        public static string Hash(String inputText, HashingTypes hashingType)
        {
            return new Hashing(hashingType).ComputeHash(inputText);
        }

        public static byte[] Hash(byte[] inputData)
        {
            return new Hashing().ComputeHash(inputData);
        }

        public static byte[] Hash(byte[] inputData, HashingTypes hashingType)
        {
            return new Hashing(hashingType).ComputeHash(inputData);
        }
        #endregion
    }
}
