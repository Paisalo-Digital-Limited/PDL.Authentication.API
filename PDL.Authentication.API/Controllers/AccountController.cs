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
        private JwtSettings _jwtSettings;

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
                    dynamic data = _accountInterface.LoginAccountValidate(accountLogin, dbname, GetIslive());
                    dynamic validCredData = new ExpandoObject();

                    var Token = new AccountTokens();
                    if (data != null)
                    {
                        validCredData.accountTokens = data;
                        if (data.isValidate)
                        {
                            Token = JwtHelpers.GenTokenkey(new AccountTokens()
                            {
                                Id = data.Id,
                                Name = data.Name,
                                Email = data.Email,
                                RoleId = data.RoleId,
                                EmpCode = data.EmpCode,
                                Creator = data.Creator,
                                RoleName = data.RoleName,

                            }, _jwtSettings);
                            validCredData.accountTokens = data.accountTokens;
                            validCredData.tokenDetails = Token;
                            string successMessage = resourceManager.GetString("LOGINSUCCESS");
                            return Ok(new { statuscode = 200, message = successMessage, data = validCredData });
                        }
                        else
                        {
                            return Ok(new { statuscode = 205, message = (resourceManager.GetString("LOGINFAIL")), data = validCredData });
                        }
                    }
                    else
                    {
                        return Ok(new { statuscode = 201, message = (resourceManager.GetString("NOUSERFOUND")), data = new List<object>() });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = (resourceManager.GetString("NULLDBNAME")), data = new List<object>() });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GenerateToken_Account");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")), data = new List<object>() });
            }
        }
        #endregion
    }
}
