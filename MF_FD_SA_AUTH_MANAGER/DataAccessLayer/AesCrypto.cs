using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER.DataAccessLayer
{
    public class AesCrypto
    {
        public async static Task<string> EncryptAES(string sText, string sAesKey, string sAesIV)
        {
            // AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = sAesKey.Length * 16;// 8;---256 Bit encryption
            aes.IV = Encoding.UTF8.GetBytes(sAesIV);
            aes.Key = Encoding.UTF8.GetBytes(sAesKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Convert string to byte array
            byte[] src = Encoding.Unicode.GetBytes(sText);

            // encryption
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                // Convert byte array to Base64 strings
                return Convert.ToBase64String(dest);
            }
        }

        public async static Task<string> DecryptAES(string sText, string sAesKey, string sAesIV)
        {
            // AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = sAesKey.Length * 8;
            aes.IV = Encoding.UTF8.GetBytes(sAesIV);
            aes.Key = Encoding.UTF8.GetBytes(sAesKey);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;


            // Convert Base64 strings to byte array
            byte[] src = System.Convert.FromBase64String(sText);

            // decryption
            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            {
                byte[] dest = decryptor.TransformFinalBlock(src, 0, src.Length);

                return Encoding.Unicode.GetString(dest);
            }
        }

        public async static Task<string> EncodeString(string sData)
        {
            try
            {
                byte[] encData_byte = new byte[sData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                //_objErrorLog.LogError(ex);
                return string.Empty;
            }
        }

        public async static Task<string> DecodeString(string sData)
        {
            try
            {
                sData = sData.Trim('\0');
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception ex)
            {
                //_objErrorLog.LogError(ex);
                return string.Empty;
            }
        }

        public async static Task<string> EncryptString(string InputText, string Key)
        {
            // "SecretKey" string variable is nothing but the key(your secret key) value which is sent from the front end.
            // "InputText" string variable is the actual text sent from the page.
            // We are now going to create an instance of the
            // Rihndael class.
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            // First we need to turn the input strings into a byte array.
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
            // We are using Salt to make it harder to guess our key
            // using a dictionary attack.
            byte[] Salt = Encoding.ASCII.GetBytes(Key.Length.ToString());
            // The (Secret Key) will be generated from the specified
            // password and Salt.
            //PasswordDeriveBytes -- It Derives a key from a password
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Key, Salt);
            // Create a encryptor from the existing SecretKey bytes.
            // We use 32 bytes for the secret key
            // (the default Rijndael key length is 256 bit = 32 bytes) and
            // then 16 bytes for the IV (initialization vector),
            // (the default Rijndael IV length is 128 bit = 16 bytes)
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            // Create a MemoryStream that is going to hold the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();
            // Create a CryptoStream through which we are going to be processing our data.
            // CryptoStreamMode.Write means that we are going to be writing data
            // to the stream and the output will be written in the MemoryStream
            // we have provided. (always use write mode for encryption)
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            // Start the encryption process.
            cryptoStream.Write(PlainText, 0, PlainText.Length);
            // Finish encrypting.
            cryptoStream.FlushFinalBlock();
            // Convert our encrypted data from a memoryStream into a byte array.
            byte[] CipherBytes = memoryStream.ToArray();
            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();
            // Convert encrypted data into a base64-encoded string.
            // A common mistake would be to use an Encoding class for that.
            // It does not work, because not all byte values can be
            // represented by characters. We are going to be using Base64 encoding
            // That is designed exactly for what we are trying to do.
            string EncryptedData = Convert.ToBase64String(CipherBytes);
            SHAEncryption();
            // Return encrypted string.
            return EncryptedData;
        }

        public async static Task<string> DecryptString(string InputText, string Key)
        {
            try
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();
                byte[] EncryptedData = Convert.FromBase64String(InputText);
                byte[] Salt = Encoding.ASCII.GetBytes(Key.Length.ToString());
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Key, Salt);
                // Create a decryptor from the existing SecretKey bytes.
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);
                // Create a CryptoStream. (always use Read mode for decryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold EncryptedData;
                // DecryptedData is never longer than EncryptedData.
                byte[] PlainText = new byte[EncryptedData.Length];
                // Start decrypting.
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                memoryStream.Close();
                cryptoStream.Close();
                // Convert decrypted data into a string.
                string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
                // Return decrypted string.
                return DecryptedData;
            }
            catch (Exception exception)
            {
                return (exception.Message);
            }
        }

        public async Task<RijndaelManaged> GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        public byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        /// <summary>
        /// Encrypts plaintext using AES 128bit key and a Chain Block Cipher and returns a base64 encoded string
        /// </summary>
        /// <param name="plainText">Plain text to encrypt</param>
        /// <param name="key">Secret key</param>
        /// <returns>Base64 encoded string</returns>
        public async Task <String> Encrypt(String plainText, String key)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, await GetRijndaelManaged(key)));
        }

        /// <summary>
        /// Decrypts a base64 encoded string using the given key (AES 128bit key and a Chain Block Cipher)
        /// </summary>
        /// <param name="encryptedText">Base64 Encoded String</param>
        /// <param name="key">Secret Key</param>
        /// <returns>Decrypted String</returns>
        public async Task<String> Decrypt(String encryptedText, String key)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, await GetRijndaelManaged(key)));
        }

        public async static Task<string> SHAEncryption()
        {

            string textToEncrypt = "TEST STRING";

            int keySize = 256;
            string hashAlgorithm = "SHA384";// "SHA256";// "SHA1";
            string passPhrase = "AH!PSB0%FGHR$";
            string saltValue = "LRT%YUR#VBNL@1";
            string initVector = "HR$2pIjHR$2pIjol";



            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(textToEncrypt);

            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, 2);

            byte[] keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();

            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            MemoryStream memoryStream = new MemoryStream();

            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;

        }


        public async static Task<string> HashPassword(string plainMessage)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainMessage);
            using (HashAlgorithm sha = new SHA256Managed())
            {
                byte[] encryptedBytes = sha.TransformFinalBlock(data, 0, data.Length);
                return  Convert.ToBase64String(sha.Hash);
            }
        }
    }
}
