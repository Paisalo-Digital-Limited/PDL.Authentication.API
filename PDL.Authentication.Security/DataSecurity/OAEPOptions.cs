using System.Security.Cryptography;

namespace PDL.Authentication.Security.DataSecurity
{
    public class OAEPOptions
    {
        public HashAlgorithmName OaepHash { get; set; }
        public HashAlgorithmName Mgf1Md { get; set; }
        public object Label { get; set; }
    }
}