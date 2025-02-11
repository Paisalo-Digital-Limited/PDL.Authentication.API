using Microsoft.AspNetCore.Mvc;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.Helper;
using System.Dynamic;

namespace PDL.Authentication.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountInterface _accountInterface;
        private readonly JwtSettings _jwtSettings;

        public AccountController(IConfiguration configuration, IAccountInterface accountInterface, JwtSettings jwtSettings) : base(configuration)
        {
            _configuration = configuration;
            _accountInterface = accountInterface;
            _jwtSettings = jwtSettings;
        }

        #region --------- Generate Token By ----- Satish Maurya -------
        [HttpPost]
        public IActionResult GenerateToken(AccountLoginVM accountLogin)
        {
            try
            {
                string dbname = GetDBName();
                if (!string.IsNullOrEmpty(dbname))
                {
                    AccountTokens data = _accountInterface.LoginAccountValidate(accountLogin, dbname, GetIslive());

                    if (data != null)
                    {
                        string successMessage = resourceManager.GetString("LOGINSUCCESS");
                        return Ok(new { statuscode = 200, message = successMessage, data });
                    }
                    else
                    {
                        return Ok(new { statuscode = 205, message = resourceManager.GetString("LOGINFAIL"), data });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = resourceManager.GetString("NULLDBNAME") });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GenerateToken_Account");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }
        #endregion
    }
}