using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.BLL;

namespace PDL.Authentication.Repository.Repository
{
    public class AccountRepository : BaseBLL, IAccountInterface
    {
        private readonly IConfiguration _configuration;
        //private readonly CredManager _credManager;
        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            //_credManager = new CredManager(configuration);
        }
        public dynamic LoginAccountValidate(AccountLoginVM accountLogin, string dbname, bool islive)
        {
            using (AccountBLL accountBLL = new AccountBLL(_configuration))
            {
                return accountBLL.GetLoginAccountValidate(accountLogin, dbname, islive);
            }
        }
    }
}
