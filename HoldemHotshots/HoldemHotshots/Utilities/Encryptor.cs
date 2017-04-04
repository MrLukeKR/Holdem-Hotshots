using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace HoldemHotshots.Utilities
{
    class Encryptor
    {
        
        private RijndaelManaged cryptoManager;
        private ICryptoTransform encyptorCipher;
        private ICryptoTransform decyptorCipher;

        public Encryptor()
        {
            cryptoManager = new RijndaelManaged();
            cryptoManager.GenerateKey();
            cryptoManager.GenerateIV();

            encyptorCipher = cryptoManager.CreateEncryptor(cryptoManager.Key, cryptoManager.IV);
            decyptorCipher = cryptoManager.CreateDecryptor(cryptoManager.Key, cryptoManager.IV);
           
        }

        public Encryptor(byte[] key,byte[] iv)
        {
            cryptoManager = new RijndaelManaged();
            cryptoManager.Key = key;
            cryptoManager.IV = iv;

            encyptorCipher = cryptoManager.CreateEncryptor(cryptoManager.Key, cryptoManager.IV);
            decyptorCipher = cryptoManager.CreateDecryptor(cryptoManager.Key, cryptoManager.IV);

        }

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

        


    }
}
