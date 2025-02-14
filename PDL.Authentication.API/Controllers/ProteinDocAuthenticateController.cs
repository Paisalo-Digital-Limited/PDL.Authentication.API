using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.Helper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PDL.Authentication.API.Controllers
{
    public class ProteinDocAuthenticateController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        private readonly IProteinAuthenticate _proteinAuthenticate;
        private readonly IDocVerify _iDocVerify;
        private readonly IPANVerify _panVerify;
        private readonly IWebHostEnvironment _environment;

        public ProteinDocAuthenticateController(IConfiguration configuration, IProteinAuthenticate proteinAuthenticate, IDocVerify docVerify, IPANVerify panVerify) : base(configuration)
        {
            _configuration = configuration;
            _proteinAuthenticate = proteinAuthenticate;
        }

        #region --------- Doc verification By ----- Satish Maurya -------
        [HttpPost]
        public IActionResult GetVoterDetails(VotterVM objVm)
        {
            try
            {
                var taskToken = Task.Run(() => _proteinAuthenticate.GetAccessTokenAsync(GetDBName(), GetIsCredlive(), GetIslive()));
                taskToken.Wait();

                if (!string.IsNullOrEmpty(taskToken.Result))
                {
                    KycDocVM docVM = new KycDocVM();
                    docVM.UserID = objVm.UserID;
                    docVM.voterno = objVm.voterno;
                    docVM.Type = "voter";
                    var data = _iDocVerify.GetVerifyDetails(docVM, taskToken.Result, GetDBName(), GetIsCredlive(), GetIslive());
                    return Ok(new { statuscode = 200, message = "Get Record Successfully!!", data });
                }
                else
                {
                    return Ok(new { statuscode = 201, message = "Token was empty!!!" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetVoterDetails_ProteinDocAuthenticate");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }

        [HttpPost]
        public IActionResult GetDLDetails(DLVM objVm)
        {
            try
            {
                var taskToken = Task.Run(() => _proteinAuthenticate.GetAccessTokenAsync(GetDBName(), GetIsCredlive(), GetIslive()));
                taskToken.Wait();

                if (!string.IsNullOrEmpty(taskToken.Result))
                {
                    KycDocVM docVM = new KycDocVM();
                    docVM.UserID = objVm.UserID;
                    docVM.dlno = objVm.dlno;
                    docVM.dob = objVm.dob;
                    docVM.Type = "dl";
                    var data = _iDocVerify.GetVerifyDetails(docVM, taskToken.Result, GetDBName(), GetIsCredlive(), GetIslive());

                    return Ok(new { statuscode = 200, message = "Get Record Successfully!!", data });
                }
                else
                {
                    return Ok(new { statuscode = 201, message = "Token was empty!!!" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetDLDetails_ProteinDocAuthenticate");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }

        [HttpPost]
        public IActionResult GetUdyamVerify(UdhyamVM objVm)
        {
            try
            {
                var taskToken = Task.Run(() => _proteinAuthenticate.GetAccessTokenAsync(GetDBName(), GetIsCredlive(), GetIslive()));
                taskToken.Wait();

                if (!string.IsNullOrEmpty(taskToken.Result))
                {
                    KycDocVM docVM = new KycDocVM();
                    docVM.udyamRegistrationNo = objVm.udyamRegistrationNo;
                    docVM.UserID = objVm.UserID;
                    docVM.Type = "udyam";
                    var data = _iDocVerify.GetVerifyDetails(docVM, taskToken.Result, GetDBName(), GetIsCredlive(), GetIslive());
                    return Ok(new { statuscode = 200, message = "Get Record Successfully!!", data });
                }
                else
                {
                    return Ok(new { statuscode = 201, message = "Token was empty!!!" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetUdyamVerify_ProteinDocAuthenticate");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }
        [HttpPost]
        public IActionResult GetVehicleVerify(VehicleVM objVm)
        {
            try
            {
                var taskToken = Task.Run(() => _proteinAuthenticate.GetAccessTokenAsync(GetDBName(), GetIsCredlive(), GetIslive()));
                taskToken.Wait();

                if (!string.IsNullOrEmpty(taskToken.Result))
                {
                    KycDocVM docVM = new KycDocVM();
                    docVM.vehicleNumber = objVm.vehicleNumber;
                    docVM.UserID = objVm.UserID;
                    docVM.Type = "vehicle";
                    var data = _iDocVerify.GetVerifyDetails(docVM, taskToken.Result, GetDBName(), GetIsCredlive(), GetIslive());
                    return Ok(new { statuscode = 200, message = "Get Record Successfully!!", data });
                }
                else
                {
                    return Ok(new { statuscode = 201, message = "Token was empty!!!" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetVehicleVerify_ProteinDocAuthenticate");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }
        [HttpPost]
        public IActionResult GetBankVerify(BankAccountVM objVm)
        {
            try
            {
                var taskToken = Task.Run(() => _proteinAuthenticate.GetAccessTokenAsync(GetDBName(), GetIsCredlive(), GetIslive()));
                taskToken.Wait();

                if (!string.IsNullOrEmpty(taskToken.Result))
                {
                    KycDocVM docVM = new KycDocVM();
                    docVM.AccNo = objVm.AccNo;
                    docVM.Ifsc = objVm.Ifsc;
                    docVM.UserID = objVm.UserID;
                    docVM.Type = "bank";
                    var data = _iDocVerify.GetVerifyDetails(docVM, taskToken.Result, GetDBName(), GetIsCredlive(), GetIslive());
                    return Ok(new { statuscode = 200, message = "Get Record Successfully!!", data });
                }
                else
                {
                    return Ok(new { statuscode = 201, message = "Token was empty!!!" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetBankVerify_ProteinDocAuthenticate");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }

        [HttpPost]
        public IActionResult GetVerifyDetails(KycDocVM docVM)
        {
            try
            {
                var taskToken = Task.Run(() => _proteinAuthenticate.GetAccessTokenAsync(GetDBName(), GetIsCredlive(), GetIslive()));
                taskToken.Wait();

                if (!string.IsNullOrEmpty(taskToken.Result))
                {
                    var data = _iDocVerify.GetVerifyDetails(docVM, taskToken.Result, GetDBName(), GetIsCredlive(), GetIslive());
                    return Ok(new { statuscode = 200, message = "Get Record Successfully!!", data });
                }
                else
                {
                    return Ok(new { statuscode = 201, message = "Token was empty!!!" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "GetVerifyDetails_ProteinDocAuthenticate");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }

        [HttpPost]
        public IActionResult PANVerification(List<PANVerify> panVerify)
        {
            try
            {
                var result = _panVerify.ProcessVerifyPanData(panVerify, GetDBName(), GetIsCredlive(), GetIslive());

                if (result != null && result.Count > 0)
                {
                    return Ok(new { statuscode = 200, message = "Get Record Successfully!!", result });
                }
                else
                {
                    return Ok(new { statuscode = 201, message = "No record found!!" , result });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, GetDBName(), GetIslive(), "PANVerification_ProteinDocAuthenticate");
                return Ok(new { statuscode = 400, message = resourceManager.GetString("BADREQUEST") });
            }
        }
        #endregion

    }
}
