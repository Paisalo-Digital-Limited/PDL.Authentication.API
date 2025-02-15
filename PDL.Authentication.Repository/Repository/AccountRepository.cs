using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.BLL;

namespace PDL.Authentication.Repository.Repository
{
    public class AccountRepository : BaseBLL, IAccountInterface
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;

        public AccountRepository(IConfiguration configuration, JwtSettings jwtSettings)
        {
            _configuration = configuration;
            _jwtSettings = jwtSettings;
        }

        public AccountTokens LoginAccountValidate(AccountLoginVM accountLogin, string dbname, bool islive)
        {
            using (AccountBLL accountBLL = new AccountBLL(_configuration, _jwtSettings))
            {
                return accountBLL.GetLoginAccountValidate(accountLogin, dbname, islive);
            }
        }
        public dynamic UpdateAccountPassword(string encryptPass, string? EncriptOldPass, string Email, string dbname, bool islive)
        {
            using (AccountBLL accountBLL = new AccountBLL(_configuration, _jwtSettings))
            {
                return accountBLL.UpdateAccountPassword(encryptPass, EncriptOldPass, Email, dbname, islive);
            }
        }
        public dynamic ForGotPassword(string Email, string dbname, bool islive)
        {
            using (AccountBLL accountBLL = new AccountBLL(_configuration, _jwtSettings))
            {
                return accountBLL.ForGotPassword(Email, dbname, islive);
            }
        }

    }
}