using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace HoldemHotshots.Utilities
{
    
    /// <summary>
    /// Used to encrypt and decrypt strings
    /// </summary>
    class Encryptor
    {
        
        private RijndaelManaged cryptoManager;
        private ICryptoTransform encyptorCipher;
        private ICryptoTransform decyptorCipher;

        /// <summary>
        /// Constructor for Encryptor class,
        /// if no key and iv are passed in they will be generated
        /// </summary>
        public Encryptor()
        {
            cryptoManager = new RijndaelManaged();
            cryptoManager.GenerateKey();
            cryptoManager.GenerateIV();

            encyptorCipher = cryptoManager.CreateEncryptor(cryptoManager.Key, cryptoManager.IV);
            decyptorCipher = cryptoManager.CreateDecryptor(cryptoManager.Key, cryptoManager.IV);
           
        }

        /// <summary>
        /// Constructor for encyptor class,
        /// takes a premade key and initalization vector
        /// </summary>
        /// <param name="key">The encryption key</param>
        /// <param name="iv">The initalization vector</param>
        public Encryptor(byte[] key,byte[] iv)
        {
            cryptoManager = new RijndaelManaged();
            cryptoManager.Key = key;
            cryptoManager.IV = iv;

            encyptorCipher = cryptoManager.CreateEncryptor(cryptoManager.Key, cryptoManager.IV);
            decyptorCipher = cryptoManager.CreateDecryptor(cryptoManager.Key, cryptoManager.IV);

        }

        /// <summary>
        /// Encypts a string
        /// </summary>
        /// <param name="text">The string to be encrypted</param>
        /// <returns>The encrypted string</returns>
        public string EncyptString(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            MemoryStream memStream = new MemoryStream();
            CryptoStream encryptSteam = new CryptoStream(memStream,encyptorCipher,CryptoStreamMode.Write);
            StreamWriter streamWriter = new StreamWriter(encryptSteam);

            streamWriter.Write(text);

            return Convert.ToBase64String(memStream.ToArray());
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="text">The string to be decrypted</param>
        /// <returns>The decrypted string</returns>
        public String DecryptString(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");


            var cipher = Convert.FromBase64String(text);

            MemoryStream memStream = new MemoryStream(cipher);
            CryptoStream decryptSteam = new CryptoStream(memStream, decyptorCipher, CryptoStreamMode.Read);
            StreamReader streamReader = new StreamReader(decryptSteam);

            string decryptedText = streamReader.ReadToEnd();

            return decryptedText;
        }

        public byte[] getKey()
        {
            return cryptoManager.Key;
        }

        public byte[] getIV()
        {
            return cryptoManager.IV;
        }

        


    }
}
