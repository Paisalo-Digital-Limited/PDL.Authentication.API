using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.Helper;
using System.Security.Claims;

namespace PDL.Authentication.API.Controllers
{
    public class SMSController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly ISMSInterface _sMSInterface;
        public SMSController(IConfiguration configuration, ISMSInterface sMSInterface) : base(configuration)
        {
            _configuration = configuration;
            _sMSInterface = sMSInterface;
        }

        #region SEND SMS  -----------------Kartik------------------
        [HttpPost]
        public async Task<ActionResult> SendSMS(SmsVM sendSmsVM)
        {
            try
            {
                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(dbname))
                {
                    string baseUrl = string.Empty;
                    baseUrl = _configuration.GetValue<string>("SendSms");

                    int affected = await _sMSInterface.SendSMS(sendSmsVM, baseUrl, activeuser, dbname, GetIslive());

                    if (affected > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = resourceManager.GetString("SENDSMS"),
                            data = affected
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = resourceManager.GetString("SMSFAILED")
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 405,
                        message = resourceManager.GetString("NULLDBNAME")

                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "SendSMS_SMSController");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }
        [HttpPost]
        public IActionResult VerifyOTP(OtpVerifyVM verifyVM)
        {
            try
            {
                string dbname = GetDBName();
                string activeuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(dbname))
                {
                    List<OTPVerificationResponse> credentials = _sMSInterface.VerifyOtp(verifyVM, dbname, activeuser, GetIslive());


                    if (credentials.Count > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = resourceManager.GetString("VERIFYOTP"),
                            data = credentials
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            statuscode = 201,
                            message = resourceManager.GetString("VERIFYFAILEDOTP")
                        });
                    }
                }
                else
                {
                    return Ok(new
                    {
                        statuscode = 405,
                        message = resourceManager.GetString("NULLDBNAME")

                    });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "VerifyOTP_SMSController");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }
        #endregion
    }
}