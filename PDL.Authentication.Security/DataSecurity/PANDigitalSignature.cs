using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PDL.Authentication.Entites.VM;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PDL.Authentication.Security.DataSecurity
{
    public class PANDigitalSignature
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PANDigitalSignature(IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }
        public string CreateSignature(List<PANVerify> inputData)
        {
            try
            {
                string keyPath = Path.Combine(_webHostEnvironment.WebRootPath, _configuration["PanDigitalSignature:Path"]);
                // Load certificate from .pfx file


                var certificatePath = keyPath;
                var certificatePassword = _configuration["PanDigitalSignature:Password"];
                var certificate = new X509Certificate2(certificatePath, certificatePassword,
                    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

                // Serialize input data to JSON
                string inputDataJson = JsonConvert.SerializeObject(inputData);
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputDataJson);

                // Generate signature
                byte[] signature = Sign(inputBytes, certificate);

                // Return signature as Base64 string
                return Convert.ToBase64String(signature);
            }
            catch (Exception ex)
            {
                // Log or rethrow the exception with additional context
                Console.WriteLine($"Error during signature creation: {ex.Message}");
                throw;
            }
        }

        public static byte[] Sign(byte[] data, X509Certificate2 certificate)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            // Ensure the certificate has a private key
            if (!certificate.HasPrivateKey)
                throw new InvalidOperationException("The certificate does not have a private key.");

            // Setup data to sign
            ContentInfo content = new ContentInfo(data);
            SignedCms signedCms = new SignedCms(content, false);
            CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, certificate)
            {
                IncludeOption = X509IncludeOption.EndCertOnly
            };

            // Create the signature
            signedCms.ComputeSignature(signer);

            // Return the encoded signature
            return signedCms.Encode();
        }
    }
}

