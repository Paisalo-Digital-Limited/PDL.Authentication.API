using Microsoft.Extensions.Configuration;
using PDL.Authentication.Logics.BLL;
using Microsoft.Data.SqlClient;

namespace PDL.Authentication.Logics.Credentials
{
    public class CredManager:BaseBLL
    {
        private IConfiguration _configuration;
        public CredManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public SqlConnection getConnections(string db, bool islive)
        {
            string conStr = string.Empty;
           
            SqlConnection newConn = new SqlConnection();
            try
            {
           
                if (!islive)
                    conStr = $"Data Source=192.168.10.2;Initial Catalog={db};User ID={""};Password={""};Connection Timeout=120;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";
                else
                    conStr = $"Data Source=192.168.10.2;Initial Catalog={db};User ID={""};Password={""};Connection Timeout=120;Trusted_Connection=False;MultipleActiveResultSets=True;Encrypt=false";
                    newConn = new SqlConnection(conStr);
                return newConn;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL error: " + ex.Message);
                throw;
            }
        }
        public Dictionary<string, string> KycCredential(bool islive)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (!islive)
            {
                keyValuePairs.Add("username", "S0zc1PcE83bEq7s2bMOApHlg6tgArdu7deIN1MDgzq8JBp2i");
                keyValuePairs.Add("password", "PzwNFOnmD06Al5PiIW4tZPxZC9FSjA63OfAmDUNGVBQDogjPK8ncUjmPa8BNKhkG");
                keyValuePairs.Add("grant_type", "client_credentials");
                keyValuePairs.Add("tokenUrl", "https://uat.risewithprotean.io/v1/oauth/token");
                keyValuePairs.Add("voter", "https://uat.risewithprotean.io/api/v1/retail/v3/voter");
                keyValuePairs.Add("bankacc", "https://uat.risewithprotean.io/api/v1/bank_auth/bankacc");
                keyValuePairs.Add("drivinglince", "https://uat.risewithprotean.io/api/v1/retail/dl");
                keyValuePairs.Add("udyam", "https://uat.risewithprotean.io/api/v1/commercial/udyam/auth");
                keyValuePairs.Add("vechile", "https://uat.risewithprotean.io/api/v1/assetvehicle/rc-advanced");
            }
            else
            {
                keyValuePairs.Add("username", "s7AvPWYKWh1QW06xxSTvAoz6dSI2zahMyNhzwqMm0xbrysNZ");
                keyValuePairs.Add("password", "TPqAUNLggN5DLe1WsFEj6R99hClGpYgGPj1ZtNLgYtWwlzcuIaIsC16zBGzJooYh");
                keyValuePairs.Add("grant_type", "client_credentials");
                keyValuePairs.Add("tokenUrl", "https://api.risewithprotean.io/v1/oauth/token");
                keyValuePairs.Add("voter", "https://api.risewithprotean.io/api/v1/retail/v3/voter");
                keyValuePairs.Add("bankacc", "https://api.risewithprotean.io/api/v1/bank_auth/bankacc");
                keyValuePairs.Add("drivinglince", "https://api.risewithprotean.io/api/v1/retail/dl");
                keyValuePairs.Add("udyam", "https://api.risewithprotean.io/api/v1/commercial/udyam/auth");
                keyValuePairs.Add("vechile", "https://api.risewithprotean.io/api/v1/assetvehicle/rc-advanced");
            }
            return keyValuePairs;
        }
    }
}
