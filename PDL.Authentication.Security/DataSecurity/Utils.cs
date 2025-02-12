using System.Security.Cryptography;
using System.Text;

namespace PDL.Authentication.Security.DataSecurity
{
    public class Utils
    {

        public static string publicKeyPath = @"D:\\LOSDOC\\ProteanCertificates\\proteanProductionPublicKey.pem";
        public static string pdlPrivateKeyPath = @"D:\\LOSDOC\\ProteanCertificates\\pDLProductionPrivateKey.pem";

        public static string GetProteanPublicKey()
        {
            return File.ReadAllText(publicKeyPath);
        }
        public static string GetPdlPrivateKey()
        {
            return File.ReadAllText(pdlPrivateKeyPath);
        }


        //        //PROTEAN UAT PUBLIC KEY
        //        public static string publicKey = @"-----BEGIN PUBLIC KEY-----
        //MIIBITANBgkqhkiG9w0BAQEFAAOCAQ4AMIIBCQKCAQBw7Zq8McjphWnzjTN8T/0H
        //ukitNqWKSTIu6RWQP7OcuEuNQKTLE4Y5Cv+6gPoQslixD1KxHehJ7rqrm0lgGfL3
        //DVv5ljzNSzp+mYHwRaBghplXqjasE2BrI5uHwNMXgaZXbL8UZbNUrTjsdsSjcFrI
        //5XUhrsUPimlgO+4p2lh6w5vvlmSAZKCddOwCxvRrZ3IG7/aVPfftSTsLCU8Lkezt
        //crqSwTTq3MrO46kRsW9vX/VJLr9VShfbdV1VHPPDXKIhut2jlNmDpXWczssWQ311
        //h13+ZAVs9uKH/O7t88hwloSZDI77avbF2X4HmRYgRDfXBDe7JW6c0eeF8S8AGCSZ
        //AgMBAAE=
        //-----END PUBLIC KEY-----";

        //        // PDL UAT PRIVATE KEY
        //        public static string privateKey = @"-----BEGIN PRIVATE KEY-----
        //MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDUZBGN6J+rQLBK
        //v/gqSq0+E9ZN0IRHMCLZz6IoVuc/30CDBEM6mY8Xe/JJLamjw5eWW2vG5vUCnfwa
        //ta539A/KIX65YuqD0YczxSc1JuwXkn7QDG1hu+t6sf4myl2G9c/UqgSkwyfP7XCj
        //uUDsx3vi2ASzEikFjqyrSODh/BEB6r6Jv0Tu4W8wP/vrM6TJRH25hMG7oiewlp6Y
        //yXfx6amO10gSbFpop0nrLOfRsPCmsrDVG38H4s0N88lphBCpXvJICNQ7dXhXR5BI
        //gsKzHk9hdVpI5+pBgvP3yMHSDV+qQdf4rCEfc7u0m8twjOCDeQXp3383vHrjg5fF
        //+Xe3nhH9AgMBAAECggEAO6jUSYFjgG5OVuDvq6mIWlymIPuGfJyn3Xj2etBWatmi
        //PGsxGz/RMu6NE0MxWJ/zb7fEYX20qwSHsVUBag5zdPrNpvODHn33tXIW3lZNkR2E
        //Y5pqCz8HGVLwKiND/EoGFB2h6korX7u5nTPHaftq8EBvqYFbZoU6OW7iRHhSqq+r
        //jzsgl/i1ryiDCgzYkdyoQeAzlNYRq3oigIbtFbs6q1ZL7rxRroZ9fW/zUCudmG12
        //JWNFb8fIS0jE4Zxx3YhJteg35EkvXlLcntJZ+9gq1+XdLvM+2UMYq/edZ8TLoKm7
        //X42FJSrkuWkJ6cuPmkpdrV1nm9D+ERgacNNHrBvVfQKBgQDbG/Te/tAS6NGSEiWL
        //ySPaU+VVBYzSzEpF5XAsJ/QC71P7+Y0kmRTL/YquJ3s/sHqVc/Nd/i9Rj0fKksmc
        //jnaifgy/CD4YHlhDUlFjxEK3X3kdbVr6QWdjhVjviOQdmNjOvW4VJV+RiAu655fE
        //DwtcvAPa2gpCPLWFm6HQwL52RwKBgQD4JooVkn0OILaudBVcrB3iF8FdCdTiD3O3
        //IPAdWMlFPmRk0m7s5w63VZAUwYufBasP1uUlJ0bRaBnk6Yn4QaXrfpYHF5VjCc2l
        //905FTRWZ3srs0N1VEF1ltlZFPkwvXyQk34Ee6+hjr6HjTOA7e0/4suJKI6SmgnrA
        //UHMKY3ljmwKBgBPK0rPSELmkjknDmeCmqrZuHakwdygTjCIEN73FXiVluRBp2nYT
        //3e0PWhehOWDncCtP7gvvihaz+qgx2kRqGg1TlZMsC2/iTdbG+NMqR1yJI0elOTKh
        //9dTnlsEMfI6v3+XM9sSyO4/J2kVn5i2vrjcSRMbgK00QbtVC2bshrhLBAoGBAPB3
        //yE/nSLAsG1squpi3Ya36/zI9mMIH30aS8Jw/sascLwVUtpFzxtr6W0kB1V8giwgq
        //YpuCWCoNCyq89SpR04tFe/UbcXQrx9EQuhSGAmy9bT/XluQbm6Y475jiKcmuFMR2
        //ohVo3iXbyLEPiiuZ74E9N/RIXgHNZ059pz1l7/gJAoGAfvEWTZ/Lj/vTRwscNa8r
        //uPH0IEEv76cbLkEq3IP2bUhRAZ6HqpaQjpKP4qk9ecUUvXlurNobM44oLdsL8Efa
        //Y6rkLxqRL9MVuJ+ImKjqFMvXwx6xwWXKIvpXU+CUAo+0qg2IAxtcIx0WnsvDOyQw
        //SonrSuOHQURAJCMhUUSPR4w=
        //-----END PRIVATE KEY-----";


        public static dynamic data { get; set; }

        public static byte[] Encode(byte[] base64Decoded)
        {
            try
            {
                byte[] base64Encoded = null;
                if (base64Decoded.Length <= 0) { return base64Encoded; }
                string data = System.Convert.ToBase64String(base64Decoded);
                base64Encoded = System.Text.ASCIIEncoding.ASCII.GetBytes(data);
                return base64Encoded;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] Decode(byte[] base64Encoded)
        {
            try
            {
                byte[] base64Decoded = null;
                if (base64Encoded.Length <= 0) { return base64Decoded; }
                string data = System.Text.ASCIIEncoding.ASCII.GetString(base64Encoded);
                base64Decoded = System.Convert.FromBase64String(data);
                return base64Decoded;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RandomString(int len)
        {
            string AB = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                sb.Append(AB[rnd.Next(AB.Length)]);
            }
            return sb.ToString();
        }

        public static byte[] SecretKeySpec(byte[] keyValue, string text)
        {
            try
            {
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
                AesManaged tdes = new AesManaged();
                tdes.Key = keyValue;
                tdes.Mode = CipherMode.CBC;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform crypt = tdes.CreateEncryptor();
                byte[] plain = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
                byte[] cipher = crypt.TransformFinalBlock(plain, 0, plain.Length);
                string encryptedText = Convert.ToBase64String(cipher);
                return System.Text.ASCIIEncoding.ASCII.GetBytes(encryptedText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            try
            {
                byte[] ret = new byte[first.Length + second.Length];
                Buffer.BlockCopy(first, 0, ret, 0, first.Length);
                Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] extractBytes(byte[] src, int start, int end)
        {
            try
            {
                int len = end - start;
                byte[] dest = new byte[len];
                Array.Copy(src, start, dest, 0, len);
                return dest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public static string PayloadSignatureGenerator(string data, string plainSymmetricKey)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(plainSymmetricKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hashBytes);
            }
        }

    }
}
