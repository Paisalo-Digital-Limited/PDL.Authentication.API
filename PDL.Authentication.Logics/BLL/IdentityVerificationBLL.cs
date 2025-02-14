using Azure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.Credentials;
using PDL.Authentication.Logics.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlDataAdapter = Microsoft.Data.SqlClient.SqlDataAdapter;

namespace PDL.Authentication.Logics.BLL
{
    public class IdentityVerificationBLL : BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IdentityVerficationHelper _helper;

        public IdentityVerificationBLL(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
            _webHostEnvironment = webHostEnvironment;
            _helper = new IdentityVerficationHelper(_configuration, _webHostEnvironment);
        }
        #region    -----IdentityVerification -------- Satsih Maurya ---------
        public async Task<dynamic> Get(IdentityVerficationVM objVM, string docVerifyApiKey, string activeuser, string dbname, bool islive)
        {
            object result = null;
            if (objVM.Key.Length < 1)
            {
                return null;
            }
            else
            {
                if (objVM.type.ToLower().Trim() == "pancard")
                {
                    dynamic obj = new ExpandoObject();
                    obj.panNumber = objVM.txtnumber;

                    ApiCallResponseVM response = await _helper.GetDocVerifyResponseAsync(JsonConvert.SerializeObject(obj), _configuration.GetValue<string>("pancardBaseurl"), docVerifyApiKey);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<PanCardModelVM>(response.ResponseContent);
                        string names = ((PanCardModelVM)result).data.name;

                        if (((PanCardModelVM)result).success == true)
                        {
                            dynamic values = IDCentralApiLogs(names, objVM.type, JsonConvert.SerializeObject(obj), response.ResponseContent, activeuser, dbname, islive);
                        }
                        return result;
                    }
                    else
                    {
                        response.ExceptionMessage = "data not fetched";
                    }

                }
                else if (objVM.type.ToLower().Trim() == "voterid")
                {
                    dynamic obj = new ExpandoObject();
                    obj.voter_id = objVM.txtnumber;


                    string baseurl = _configuration.GetValue<string>("voteridBaseurl") + objVM.txtnumber;

                    ApiCallResponseVM response = await _helper.GetDocVerifyResponseAsync(JsonConvert.SerializeObject(obj), baseurl, docVerifyApiKey);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<VoterIdCardVM>(response.ResponseContent);
                        string names = ((VoterIdCardVM)result).data.name;
                        
                        if (((VoterIdCardVM)result).success == true)
                        {
                            dynamic values = IDCentralApiLogs(names, objVM.type, JsonConvert.SerializeObject(obj), response.ResponseContent, activeuser, dbname, islive);
                        }
                        return result;
                    }
                    else
                    {
                        response.ExceptionMessage = "data not fetched";
                    }
                }
                if (objVM.type.ToLower() == "rccheckno")
                {
                    dynamic obj = new ExpandoObject();
                    obj.rc_number = objVM.txtnumber;

                    ApiCallResponseVM response = await _helper.GetDocVerifyResponseAsync(JsonConvert.SerializeObject(obj), _configuration.GetValue<string>("rcchecknoBaseurl"), docVerifyApiKey);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<RcChecknumberVM>(response.ResponseContent);
                        string names = ((RcChecknumberVM)result).data.owner_name;
                        if (((RcChecknumberVM)result).success == true)
                        {
                            dynamic values = IDCentralApiLogs(names, objVM.type, JsonConvert.SerializeObject(obj), response.ResponseContent, activeuser, dbname, islive);
                        }
                        return result;
                    }
                    else
                    {
                        response.ExceptionMessage = "data not fetched";
                    }
                }

                else if (objVM.type.ToLower().Trim() == "bankaccount")
                {
                    dynamic obj = new ExpandoObject();
                    obj.id_number = objVM.txtnumber;
                    obj.ifsc = objVM.Ifsc;

                    ApiCallResponseVM response = await _helper.GetDocVerifyResponseAsync(JsonConvert.SerializeObject(obj), _configuration.GetValue<string>("bankaccountBaseurl"), docVerifyApiKey);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<BankModelVM>(response.ResponseContent);
                        string names = ((BankModelVM)result).data.full_name;
                        if (((BankModelVM)result).success == true)
                        {
                            dynamic values = IDCentralApiLogs(names, objVM.type, JsonConvert.SerializeObject(obj), response.ResponseContent, activeuser, dbname, islive);
                        }
                        return result;
                    }
                    else
                    {
                        response.ExceptionMessage = "data not fetched";
                    }
                }
                else if (objVM.type.ToLower().Trim() == "drivinglicense")
                {
                    dynamic obj = new ExpandoObject();
                    obj.id_number = objVM.txtnumber;
                    obj.dob = objVM.userdob;

                    ApiCallResponseVM response = await _helper.GetDocVerifyResponseAsync(JsonConvert.SerializeObject(obj), _configuration.GetValue<string>("drivinglicenseBaseurl"), docVerifyApiKey);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<UserDrivingLicenseVM>(response.ResponseContent);
                        string names = ((UserDrivingLicenseVM)result).data.name;
                        if (((UserDrivingLicenseVM)result).success == true)
                        {
                            dynamic values = IDCentralApiLogs(names, objVM.type, JsonConvert.SerializeObject(obj), response.ResponseContent, activeuser, dbname, islive);
                        }
                        return result;
                    }
                    else
                    {
                        response.ExceptionMessage = "data not fetched";
                    }
                }
                return result;
            }
        }
        public dynamic IDCentralApiLogs(string names, string DocName, string Request, string Response, string activeuser, string dbname, bool islive)
        {
            string affected = null;
            using (SqlConnection con = _credManager.getConnections(dbname, islive))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_IDCentralApiLogs", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", names.ToString());
                    cmd.Parameters.AddWithValue("@DocName", DocName.ToString());
                    cmd.Parameters.AddWithValue("@Request", Request.ToString());
                    cmd.Parameters.AddWithValue("@Response", Response.ToString());
                    cmd.Parameters.AddWithValue("@CreatedBy", activeuser);

                    try
                    {
                        con.Open();
                        affected = Convert.ToString(cmd.ExecuteNonQuery());
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            return affected;
        }
        #endregion
    }
}
