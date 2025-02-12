using System.Security.Cryptography;
using System.Text;

namespace PDL.Authentication.Security.DataSecurity
{
    public class AesGcmCrypto
    {
        public static string Encrypt(string data, string plainSymmetricKey)
        {
            string _salt = Utils.RandomString(16);
            byte[] salt = Encoding.UTF8.GetBytes(_salt);
            string _iv = Utils.RandomString(12);
            byte[] iv = Encoding.UTF8.GetBytes(_iv);

            byte[] aesKeyFromPassword = GetAESKeyFromPassword(plainSymmetricKey, salt);

            using (AesGcm aesGcm = new AesGcm(aesKeyFromPassword))
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(data);
                byte[] cipherText = new byte[plainTextBytes.Length];
                byte[] tag = new byte[16];

                aesGcm.Encrypt(iv, plainTextBytes, cipherText, tag);

                byte[] encryptedData = new byte[cipherText.Length + iv.Length + salt.Length + tag.Length];
                iv.CopyTo(encryptedData, 0);
                salt.CopyTo(encryptedData, iv.Length);
                cipherText.CopyTo(encryptedData, iv.Length + salt.Length);
                tag.CopyTo(encryptedData, iv.Length + salt.Length + cipherText.Length);

                return Convert.ToBase64String(encryptedData);
            }
        }        

        public static string Decrypt(string data, string plainSymmetricKey)
        {
            byte[] decode = Convert.FromBase64String(data);
            
            byte[] iv = new byte[12];
            byte[] salt = new byte[16];
            byte[] authTag = new byte[16];
            byte[] cipherText = new byte[decode.Length - iv.Length - salt.Length - 16];
             
            Array.Copy(decode, 0, iv, 0, iv.Length);
            Array.Copy(decode, iv.Length, salt, 0, salt.Length);
            Array.Copy(decode, iv.Length + salt.Length, cipherText, 0, cipherText.Length);
            Array.Copy(decode, decode.Length - authTag.Length, authTag, 0, authTag.Length);

            byte[] aesKey = GetAESKeyFromPassword(plainSymmetricKey, salt);

            using (AesGcm aesGcm = new AesGcm(aesKey))
            {
                byte[] decryptedData = new byte[cipherText.Length];

                aesGcm.Decrypt(iv, cipherText, authTag, decryptedData);

                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        private static byte[] GetAESKeyFromPassword(string password, byte[] salt)
        {
            using (Rfc2898DeriveBytes keyDerivationFunction = new Rfc2898DeriveBytes(password, salt, 65536, HashAlgorithmName.SHA256))
            {
                return keyDerivationFunction.GetBytes(32); // 128 bits
            }
        }

    }
}
