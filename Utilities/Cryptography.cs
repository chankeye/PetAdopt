using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PetAdopt.Utilities
{
    /// <summary>
    /// 加解密
    /// </summary>
    public class Cryptography
    {
        #region SHA1
        /// <summary>
        /// 用SHA1的方式加密
        /// </summary>
        /// <param name="text">要被加密的字串</param>
        /// <returns>加密過後的字串</returns>
        public static string EncryptBySHA1(string text)
        {
            // string to byte[]
            var bytes = Encoding.UTF8.GetBytes(text);

            // encrypt
            var sha1 = new SHA1CryptoServiceProvider();
            var result = sha1.ComputeHash(bytes);

            // byte[] to string
            var resultString = Convert.ToBase64String(result);

            return resultString;
        }
        #endregion //SHA1

        #region AES
        /// <summary>
        /// 用AES的方式加密
        /// </summary>
        /// <param name="text">要被加密的字串</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>加密過後的字串</returns>
        public static string EncryptByAES(string text, string key, string iv)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var ivBytes = Encoding.UTF8.GetBytes(iv);

            var resultBytes = EncryptStringToBytes_Aes(text, keyBytes, ivBytes);
            var result = Convert.ToBase64String(resultBytes);

            return result;
        }

        /// <summary>
        /// 用AES的方式解密
        /// </summary>
        /// <param name="encryptedText">已被加密的字串</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>解密過後的字串</returns>
        public static string DecryptByAES(string encryptedText, string key, string iv)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var ivBytes = Encoding.UTF8.GetBytes(iv);

            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var result = DecryptStringFromBytes_Aes(encryptedBytes, keyBytes, ivBytes);

            return result;
        }

        #region copy from http://msdn.microsoft.com/zh-tw/library/system.security.cryptography.aes.aspx
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;

        }
        #endregion //copy from http://msdn.microsoft.com/zh-tw/library/system.security.cryptography.aes.aspx
        #endregion //AES

        #region MD5

        public static string EncryptByMD5(string text)
        {
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        #endregion  //MD5
    }
}