using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.Helper;
using System.Security.Claims;

namespace PDL.Authentication.API.Controllers
{
    public class IdentityVerificationController : BaseApiController
    {
        private IWebHostEnvironment _webHostEnvironment;
        private IConfiguration _configuration;
        private IdentityVerification _identityVerification;
        public IdentityVerificationController(IConfiguration iConfig, IWebHostEnvironment webHostEnvironment, IdentityVerification identityVerification) :base(iConfig) 
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = iConfig;
            _identityVerification = identityVerification;
        }

        #region    -----IdentityVerification -------- Satsih Maurya ---------
        [HttpPost]
        [Authorize]
        public IActionResult Get(IdentityVerficationVM objVM)
        {
            try
            {

                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(dbname))
                {
                    dynamic data = _identityVerification.Get(objVM, GetDocLiveVerifyApiKey(), activeuser, dbname, GetIslive());

                    if (data != null)
                    {
                        return Ok(new { statuscode = 200,
                            message = resourceManager.GetString("GETSUCCESS"), data.Result });
                    }
                    else
                    {
                        return Ok(new { statuscode = 201, message = resourceManager.GetString("GETFAIL"), data });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = resourceManager.GetString("NULLDBNAME") });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "Get_IdentityVerification");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }
        #endregion
    }
}
