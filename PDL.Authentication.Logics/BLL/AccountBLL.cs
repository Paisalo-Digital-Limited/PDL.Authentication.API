using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Dynamic;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.Credentials;
using Jose;
using PDL.Authentication.Logics.Helper;
using JwtSettings = PDL.Authentication.Entites.VM.JwtSettings;
namespace PDL.Authentication.Logics.BLL
{
    public class AccountBLL : BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;
        private readonly JwtSettings _jwtSettings;

        public AccountBLL(IConfiguration configuration, JwtSettings jwtSettings)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
            _jwtSettings = jwtSettings;
        }

        #region --------- Generate Token By ----- Satish Maurya -------
        public AccountTokens GetLoginAccountValidate(AccountLoginVM accountLogin, string dbname, bool islive)
        {
            AccountTokens logindata = new AccountTokens();
            try
            {
                string key = _configuration.GetValue<string>("encryptSalts:password");
                string encryptedPassword = Helper.Helper.Encrypt(accountLogin.Password, key);

                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    con.Open();
                    string query = "Usp_UserDataTokenOrPassword";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "GetToken");
                        cmd.Parameters.AddWithValue("@Email", accountLogin.EmailId);
                        cmd.Parameters.AddWithValue("@Password", encryptedPassword);

                        using (SqlDataReader rdrUser = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (rdrUser.HasRows)
                            {
                                while (rdrUser.Read())
                                {
                                    logindata.Id = rdrUser["Id"] != DBNull.Value ? Convert.ToInt64(rdrUser["Id"]) : 0; 
                                    logindata.Name = rdrUser["Name"] != DBNull.Value ? rdrUser["Name"].ToString() : string.Empty;
                                    logindata.Email = rdrUser["Email"] != DBNull.Value ? rdrUser["Email"].ToString() : string.Empty;
                                    logindata.Creator = rdrUser["Creator"] != DBNull.Value ? rdrUser["Creator"].ToString() : string.Empty;
                                    logindata.EmpCode = rdrUser["EmpCode"] != DBNull.Value ? rdrUser["EmpCode"].ToString() : string.Empty;
                                    logindata.RoleName = rdrUser["RoleName"] != DBNull.Value ? rdrUser["RoleName"].ToString() : string.Empty;
                                }
                                return JwtHelpers.GenTokenkey(logindata, _jwtSettings);
                            }
                            else
                            {
                                if (logindata.Error == null)
                                {
                                    logindata.Error = new ErrorMessageVM();
                                }
                                logindata.Error.errormsg = "No matching user found.";
                                logindata.Error.isValidate = false;
                                return logindata;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}