using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Dynamic;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.Credentials;

namespace PDL.Authentication.Logics.BLL
{
    public class AccountBLL : BaseBLL
    {
        private IConfiguration _configuration;
        private readonly CredManager _credManager;
        public AccountBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }
        #region --------- Generate Token By ----- Satish Maurya -------
        public dynamic GetLoginAccountValidate(AccountLoginVM accountLogin, string dbname, bool islive)
        {
            bool isValidate = false;
            dynamic logindata = new ExpandoObject();
            string key = _configuration.GetValue<string>("encryptSalts:password");
            string EncriptPass = Helper.Helper.Encrypt(accountLogin.Password, key);
            try
            {
                string query = "Usp_UserDataTokenOrPassword";
                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "GetToken");
                        cmd.Parameters.AddWithValue("@Email", accountLogin.EmailId);
                        cmd.Parameters.AddWithValue("@Password", EncriptPass);
                        con.Open();
                        using (SqlDataReader rdrUser = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (rdrUser.HasRows)
                            {
                                while (rdrUser.Read())
                                {
                                    isValidate = true;
                                    logindata.Id = rdrUser["Id"];
                                    logindata.Name = rdrUser["Name"].ToString();
                                    logindata.Email = rdrUser["Email"].ToString();
                                    logindata.RoleId = rdrUser["RoleId"];
                                    logindata.Creator = rdrUser["Creator"].ToString();
                                    logindata.EmpCode = rdrUser["EmpCode"].ToString();
                                    logindata.Password = rdrUser["Password"].ToString();
                                    logindata.RoleName = rdrUser["RoleName"].ToString();
                                    logindata.userCred = accountLogin;
                                    logindata.accountTokens = GetAccountData(accountLogin.EmailId, dbname, islive);
                                    logindata.isValidate = isValidate;
                                    logindata.errormsg = "";
                                    return logindata;
                                }
                            }
                            else
                            {
                                logindata.errormsg = "No matching user found.";
                                logindata.isValidate = false;
                                return logindata;
                            }
                        }
                    }
                }
                return logindata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<AccountTokens> GetAccountData(string EmailId, string dbname, bool isDbLive)
        {
            List<AccountTokens> accountList = new List<AccountTokens>();
            string query = "Usp_LoginFatchLOSData";
            using (SqlConnection con = _credManager.getConnections(dbname, isDbLive))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetUserData");
                    cmd.Parameters.AddWithValue("@EmailId", EmailId);

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            AccountTokens accountTokens = new AccountTokens
                            {
                                Id = rdr.IsDBNull(rdr.GetOrdinal("Id")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("Id")),
                                Name = rdr.IsDBNull(rdr.GetOrdinal("Name")) ? null : rdr.GetString(rdr.GetOrdinal("Name")),
                                Email = rdr.IsDBNull(rdr.GetOrdinal("Email")) ? null : rdr.GetString(rdr.GetOrdinal("Email")),
                                RoleId = rdr.IsDBNull(rdr.GetOrdinal("RoleId")) ? 0 : rdr.GetInt32(rdr.GetOrdinal("RoleId")),
                                Creator = rdr.IsDBNull(rdr.GetOrdinal("Creator")) ? null : rdr.GetString(rdr.GetOrdinal("Creator")),
                                EmpCode = rdr.IsDBNull(rdr.GetOrdinal("EmpCode")) ? null : rdr.GetString(rdr.GetOrdinal("EmpCode")),
                                RoleName = rdr.IsDBNull(rdr.GetOrdinal("RoleName")) ? null : rdr.GetString(rdr.GetOrdinal("RoleName"))
                            };

                            accountList.Add(accountTokens);
                        }
                    }
                }
            }
            return accountList;
        }
        #endregion
    }
}
