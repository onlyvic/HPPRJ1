using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

using HPD.Framework.Utils.Cryptography.Extensions;
namespace HPD.Framework.Utils.Cryptography
{
    /// <summary>
    /// basic Encryption/decryption functionaility
    /// </summary>
    public class Encryption
    {
        #region enums, constants & fields

        //types of symmetric encyption
        public enum EncryptionTypes
        {
            DES,
            RC2,
            Rijndael,
            TripleDES
        }

        //direction fo the transform
        public enum TransformDirection
        {
            Encrypt,
            Decrypt
        }

        private const string DEFAULT_PASSWORD = "abcd!@#";
        private const EncryptionTypes DEFAULT_ALGORITHM = EncryptionTypes.Rijndael;

        private byte[] m_Key; // cryptographic secret key
        private byte[] m_IV; //initialization vector
        private byte[] m_SaltByteArray = { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }; //default salt

        private EncryptionTypes m_EncryptionType = DEFAULT_ALGORITHM;
        private string m_strPassword = DEFAULT_PASSWORD;
        private bool m_bCalculateNewKeyAndIV = true;
        #endregion

        #region Constructors
        public Encryption()
        {
        }

        public Encryption(EncryptionTypes type)
        {
            m_EncryptionType = type;
        }
        #endregion

        #region Props

        /// <summary>
        /// type of encryption / decryption used
        /// </summary>
        public EncryptionTypes EncryptionType
        {
            get { return m_EncryptionType; }
            set
            {
                if (m_EncryptionType != value)
                {
                    m_EncryptionType = value;
                    m_bCalculateNewKeyAndIV = true;
                }
            }
        }

        /// <summary>
        ///	Passsword Key Property.
        /// The password key used when encrypting / decrypting
        /// </summary>
        public string Password
        {
            get { return m_strPassword; }
            set
            {
                if (m_strPassword != value)
                {
                    m_strPassword = value;
                    m_bCalculateNewKeyAndIV = true;
                }
            }
        }

        /// <summary>
        /// The Salt that is used. This can only be set
        /// </summary>
        public byte[] Salt
        {
            set
            {
                if (m_SaltByteArray != value)
                {
                    m_SaltByteArray = value;
                    m_bCalculateNewKeyAndIV = true;
                }
            }
        }

        protected static string KeyValue()
        {
            DateTime day = DateTime.Now;
            return day.ToString("MMyyyydd");// today.Month.ToString("00") + today.Year.ToString("00") + today.Day.ToString("00");
        }
        #endregion

        #region Encryption

        /// <summary>
        /// Encrypts a byte array
        /// </summary>
        /// <param name="inputData">byte array to encrypt</param>
        /// <returns>an encrypted byte array</returns>
        public byte[] Encrypt(byte[] inputData)
        {
            return Transform(inputData, TransformDirection.Encrypt);
        }

        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="inputText">text to encrypt</param>
        /// <returns>an encrypted string</returns>
        public string Encrypt(string inputText)
        {
            byte[] byteArr = ByteArrayExtensions.ToByteArrayUTF8(inputText);
            string base64String = ByteArrayExtensions.ToBase64String(byteArr);
            return base64String;

            //F1
            //convert back to a string
            //return Encrypt(inputText.ToByteArrayUTF8()).ToBase64String();
        }

        /// <summary>
        /// Static encrypt method
        /// </summary>
        public static string EncryptText(string inputText)
        {
            return EncryptText(inputText, DEFAULT_ALGORITHM);
        }

        /// <summary>
        /// Static encrypt method
        /// </summary>
        public static string EncryptText(string inputText, EncryptionTypes type)
        {
            return new Encryption(type).Encrypt(inputText);
        }

        public static string DESEncrypt(string inputText)
        {

            byte[] EncodedKey;
            byte[] IV = { 0x98, 0x1B, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78 };
            byte[] Bytes;

            EncodedKey = Encoding.UTF8.GetBytes(KeyValue());
            DESCryptoServiceProvider CSP = new DESCryptoServiceProvider();

            Bytes = Encoding.UTF8.GetBytes(inputText);
            MemoryStream MS = new MemoryStream();

            try
            {
                CryptoStream CS = new CryptoStream(MS, CSP.CreateEncryptor(EncodedKey, IV), CryptoStreamMode.Write);

                CS.Write(Bytes, 0, Bytes.Length);
                CS.FlushFinalBlock();
                CS.Close();
                MS.Close();
            }
            catch (Exception ex)
            {

            }

            return Convert.ToBase64String(MS.ToArray());

        }
        #endregion

        #region Decryption

        /// <summary>
        ///		decrypts a string
        /// </summary>
        /// <param name="inputText">string to decrypt</param>
        /// <returns>a decrypted string</returns>
        public string Decrypt(string inputText)
        {
            byte[] byteArr = ByteArrayExtensions.ToByteArrayBase64(inputText);
            string utf8String = ByteArrayExtensions.ToUTF8String(byteArr);
            return utf8String;

            //F1
            //return ComputeHash(inputText.ToByteArray()).ToHexString().ToUpper();

            //F1
            //convert back to a string
            //return Decrypt(inputText.ToByteArrayBase64()).ToUTF8String();
        }

        /// <summary>
        /// decrypts a byte array
        /// </summary>
        /// <param name="inputData">byte array to decrypt</param>
        /// <returns>a decrypted byte array</returns>
        public byte[] Decrypt(byte[] inputData)
        {
            return Transform(inputData, TransformDirection.Decrypt);
        }

        /// <summary>
        /// Static Decrypt method
        /// </summary>
        public static string DecryptText(string inputText)
        {
            return DecryptText(inputText, DEFAULT_ALGORITHM);
        }

        /// <summary>
        /// Static Decrypt method
        /// </summary>
        public static string DecryptText(string inputText, EncryptionTypes type)
        {
            return new Encryption(type).Decrypt(inputText);
        }

        public static string DESDecrypt(string inputText)
        {

            byte[] EncodedKey;
            byte[] IV = { 0x98, 0x1B, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78 };
            byte[] Byte;
            EncodedKey = Encoding.UTF8.GetBytes(KeyValue());
            DESCryptoServiceProvider CSP = new DESCryptoServiceProvider();
            Byte = Convert.FromBase64String(inputText);
            MemoryStream MS = new MemoryStream();

            try
            {
                CryptoStream CS = new CryptoStream(MS, CSP.CreateDecryptor(EncodedKey, IV), CryptoStreamMode.Write);
                CS.Write(Byte, 0, Byte.Length);
                CS.FlushFinalBlock();
                MS.Close();
            }
            catch (Exception ex)
            {

            }

            return Encoding.UTF8.GetString(MS.ToArray());

        }
        #endregion

        #region Symmetric Engine

        /// <summary>
        ///		performs the actual enc/dec.
        /// </summary>
        /// <param name="inputBytes">input byte array</param>
        /// <param name="Encrpyt">wheather or not to perform enc/dec</param>
        /// <returns>byte array output</returns>
        private byte[] Transform(byte[] inputBytes, TransformDirection direction)
        {
            //get the correct transform
            ICryptoTransform transform = GetEncryptionTransform(direction);

            //memory stream for output
            MemoryStream memStream = new MemoryStream();

            try
            {
                //setup the cryption - output written to memstream
                CryptoStream cryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);

                //write data to cryption engine
                cryptStream.Write(inputBytes, 0, inputBytes.Length);

                //we are finished
                cryptStream.FlushFinalBlock();

                //get result
                byte[] output = memStream.ToArray();

                //finished with engine, so close the stream
                cryptStream.Close();

                return output;
            }
            catch (Exception e)
            {
                //throw an error
                throw new Exception("Error in symmetric engine. Error : " + e.Message, e);
            }
        }

        /// <summary>
        ///		returns the symmetric engine and creates the encyptor/decryptor
        /// </summary>
        /// <param name="encrypt">whether to return a encrpytor or decryptor</param>
        /// <returns>ICryptoTransform</returns>
        private ICryptoTransform GetEncryptionTransform(TransformDirection direction)
        {
            if (m_bCalculateNewKeyAndIV)
                CalculateNewKeyAndIV();
            if (direction == TransformDirection.Encrypt)
                return GetEncryptionAlgorithm().CreateEncryptor(m_Key, m_IV);
            else
                return GetEncryptionAlgorithm().CreateDecryptor(m_Key, m_IV);
        }
        /// <summary>
        ///		returns the specific symmetric algorithm acc. to the cryptotype
        /// </summary>
        /// <returns>SymmetricAlgorithm</returns>
        private SymmetricAlgorithm GetEncryptionAlgorithm()
        {
            switch (m_EncryptionType)
            {
                case EncryptionTypes.DES:
                    return DES.Create();
                case EncryptionTypes.RC2:
                    return RC2.Create();
                case EncryptionTypes.Rijndael:
                    return Rijndael.Create();
                default:
                    return TripleDES.Create(); //default
            }
        }

        /// <summary>
        ///		calculates the key and IV acc. to the symmetric method from the password
        ///		key and IV size dependant on symmetric method
        /// </summary>
        private void CalculateNewKeyAndIV()
        {
            //use salt so that key cannot be found with dictionary attack
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(m_strPassword, m_SaltByteArray);
            SymmetricAlgorithm algo = GetEncryptionAlgorithm();
            m_Key = pdb.GetBytes(algo.KeySize / 8);
            m_IV = pdb.GetBytes(algo.BlockSize / 8);
        }

        #endregion
    }
}
