using PDL.Authentication.Entites.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Interfaces.Interfaces
{
    public interface IAccountInterface
    {
        AccountTokens LoginAccountValidate(AccountLoginVM accountLogin, string dbname, bool islive);
        dynamic UpdateAccountPassword(string encryptPass, string? EncriptOldPass, string Email, string dbname, bool islive);

    }
}
