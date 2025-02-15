using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.Helper;
using System.ComponentModel;
using System.Dynamic;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

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
                    EmailOTPInfo va = _accountInterface.CheckEmailOTP(accountLogin.EmailId, accountLogin.OTP, "Login", dbname, GetIslive());
                    var otp = va.OTP;
                    var otps = accountLogin.OTP;
                    var t = va.CreatedOn;
                    var now = DateTime.Now;
                    var fiveMinutesBeforeNow = now.AddMinutes(-5);

                    if (va.EmailId != null)
                    {
                        if (otp == otps) // Check if the OTPs match
                        {
                            if (DateTime.Parse(t) < fiveMinutesBeforeNow) // Check if the OTP onder 5 min
                            {
                                return Ok(new { statuscode = 206, message = resourceManager.GetString("EXPIREDOTP") });
                            }
                            else
                            {
                                AccountTokens data = _accountInterface.LoginAccountValidate(accountLogin, dbname, GetIslive());

                                if (data != null)
                                {
                                    if (data.Error == null)
                                    {
                                        return Ok(new { statuscode = 200, message = resourceManager.GetString("LOGINSUCCESS"), data });
                                    }
                                    else
                                    {
                                        return Ok(new { statuscode = 205, message = resourceManager.GetString("LOGINFAIL"), data });
                                    }
                                }
                                else
                                {
                                    return Ok(new { statuscode = 201, message = (resourceManager.GetString("TOKENNOTGEN")) });
                                }
                            }
                        }
                        else
                        {
                            return Ok(new { statuscode = 204, message = resourceManager.GetString("INVALIDOTP") });
                        }
                    }
                    else
                    {
                        return Ok(new { statuscode = 203, message = (resourceManager.GetString("EMAILNOT")) });
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
        [HttpPost]
        [Authorize]
        #region -----Change Password By ----------Satish Maurya-------
        public IActionResult ChangePassword(AccountFogotPassword obj)
        {
            string key = _configuration.GetValue<string>("encryptSalts:password");
            try
            {
                string dbname = GetDBName();
                if (!string.IsNullOrEmpty(dbname))
                {
                    string EmailId = User.FindFirstValue(ClaimTypes.Email);
                    string EncriptPass = Helper.Encrypt(obj.Password, key);
                    string EncriptOldPass = Helper.Encrypt(obj.OldPassword, key);
                    int res = _accountInterface.UpdateAccountPassword(EncriptPass, EncriptOldPass, EmailId, dbname, GetIslive());
                    if (res > 0)
                    {
                        return Ok(new
                        {
                            statuscode = 200,
                            message = resourceManager.GetString("UPDATESUCCESS"),
                            data = res
                        });
                    }
                    else if (res == -1)
                    {
                        return Ok(new { statuscode = 203, message = (resourceManager.GetString("NOTMATCH")), data = res });
                    }
                    else
                    {
                        return Ok(new { statuscode = 201, message = (resourceManager.GetString("UPDATEFAIL")), data = res });
                    }
                }
                else
                {
                    return Ok(new { statuscode = 405, message = (resourceManager.GetString("NULLDBNAME")) });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "ChangePassword_Account");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        #endregion
        #region CheckEmail By ---------Satish Maurya-----------
        [HttpGet]
        public IActionResult SendOTPEmail(string Email, string Type)
        {
            CommonHelper commonHelper = new CommonHelper();
            string key = _configuration.GetValue<string>("encryptSalts:password");
            try
            {
                string dbname = GetDBName();
                if (!string.IsNullOrEmpty(dbname))
                {
                    dynamic res = _accountInterface.CheckEmail(Email, dbname, GetIslive());
                    if (res != null)
                    {
                        var randampass = commonHelper.GenerateOTP();
                        SendMailVM sendMailVM = new SendMailVM();

                        sendMailVM.Type = "resetpassword";
                        sendMailVM.ToEmail = res.ToString();
                        sendMailVM.Subject = "Reset Password";
                        sendMailVM.Password = randampass;
                        bool sendPasswordOnMail = commonHelper.SendMail(sendMailVM);

                        if (sendPasswordOnMail == true)
                        {
                            int insert = _accountInterface.InsertEmailOTP(res, randampass, Type, dbname, GetIslive());

                            if (insert > 0)
                            {
                                return Ok(new
                                {
                                    statuscode = 200,
                                    message = resourceManager.GetString("SENDOTP"),
                                    data = "success"
                                });
                            }
                            else
                            {
                                return Ok(new { statuscode = 201, message = (resourceManager.GetString("INSERTFAIL")) });
                            }
                        }
                        else
                        {
                            return Ok(new { statuscode = 203, message = (resourceManager.GetString("NOTSENDOTP")) });
                        }
                    }
                    else
                    {
                        return Ok(new { statuscode = 204, message = (resourceManager.GetString("EMAILNOTFOUND")) });
                    }

                }
                else
                {
                    return Ok(new { statuscode = 405, message = (resourceManager.GetString("NULLDBNAME")) });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "ForGotPassword_Account");
                return Ok(new { statuscode = 400, message = (resourceManager.GetString("BADREQUEST")) });
            }
        }
        #endregion
    }
}