using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using System.Text;

namespace PDL.Authentication.Security.DataSecurity
{
    public class Asymmentric
    {
        public static string Encrypt(string text)
        {
            try
            {
                byte[] sskeyBytes = Encoding.UTF8.GetBytes(text);
                Org.BouncyCastle.OpenSsl.PemReader pemReader = new Org.BouncyCastle.OpenSsl.PemReader(new StringReader(Utils.GetProteanPublicKey()));
                AsymmetricKeyParameter publicKey = (AsymmetricKeyParameter)pemReader.ReadObject();

                IAsymmetricBlockCipher cipher = new OaepEncoding(new RsaEngine(), new Sha256Digest());
                cipher.Init(true, publicKey);
                byte[] encryptedBytes = cipher.ProcessBlock(sskeyBytes, 0, sskeyBytes.Length);

                string encryptedHex = BitConverter.ToString(encryptedBytes).Replace("-", string.Empty);
                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Decrypt(string encryptedData)
        {
            try
            {
                var rsaParams = LoadPrivateKeyFromPem(Utils.GetPdlPrivateKey());

                var rsaEngine = new RsaEngine();
                var oaepEncoding = new OaepEncoding(rsaEngine, new Sha256Digest());

                oaepEncoding.Init(false, rsaParams);

                var encryptedBytes = Convert.FromBase64String(encryptedData);
                var decryptedBytes = oaepEncoding.ProcessBlock(encryptedBytes, 0, encryptedBytes.Length);

                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                Console.WriteLine("Error occurred during decryption: " + ex.Message);
                return null;
            }
        }

        private static AsymmetricKeyParameter LoadPrivateKeyFromPem(string privateKeyPem)
        {
            using (var stringReader = new StringReader(privateKeyPem))
            {
                var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(stringReader);

                return (AsymmetricKeyParameter)pemReader.ReadObject();
            }
        }
    }
}
