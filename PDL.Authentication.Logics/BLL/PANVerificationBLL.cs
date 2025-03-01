﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.Credentials;
using System.Globalization;
using Microsoft.Data.SqlClient;
using PDL.Authentication.Security.DataSecurity;
using System.Data;

namespace PDL.Authentication.Logics.BLL
{
    public class PANVerificationBLL:BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly CredManager _credManager;
        public PANVerificationBLL(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _credManager = new CredManager(configuration);
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }
        #region    -----PANVerification -------- Satsih Maurya ---------
        public List<PANVerifyResponse> ProcessVerifyPanData(List<PANVerify> panVerify, string dbname, bool isCredlive, bool islive)
        {
            List<PANVerifyResponse> responseList = null;
            try
            {
                HelperBLL.LogException(_webHostEnvironment, "ENTER IN REPO");
                using (HelperBLL helperbll = new HelperBLL())
                {
                    ApiCallResponseVM apiCallResponseVM = new ApiCallResponseVM();
                    PANDigitalSignature panDigitalSignature = new PANDigitalSignature(_configuration, _webHostEnvironment);
                    var panRequest = new PanVerifyRequestVM
                    {
                        inputData = panVerify,
                        signature = panDigitalSignature.CreateSignature(panVerify) // Create signature using the provided data
                    };
                    if (!string.IsNullOrEmpty(panRequest.signature))
                    {
                        responseList = new List<PANVerifyResponse>();
                        HelperBLL.LogException(_webHostEnvironment, JsonConvert.SerializeObject(panRequest));
                        string url = string.Empty;
                        string userId = string.Empty;
                        if (isCredlive)
                        {
                            url = _configuration["livePanUrl"];
                            userId = _configuration["PanSettings:liveUserId"];
                        }
                        else
                        {
                            url = _configuration["uatPanUrl"];
                            userId = _configuration["PanSettings:User_Id"];
                        }
                        var version = _configuration["PanSettings:Version"];

                        int recordsCount = panVerify.Count;
                        DateTime requestTime = DateTime.Now;
                        string formattedRequestTime = requestTime.ToString("yyyy-MM-ddTHH:mm:ss");
                        string transactionId = formattedRequestTime.Replace("-", "");

                        var t = Task.Run(() => helperbll.PostRequestForPan(JsonConvert.SerializeObject(panRequest), url, userId, version, recordsCount, formattedRequestTime, transactionId));
                        t.Wait();
                        apiCallResponseVM = t.Result;
                        HelperBLL.LogException(_webHostEnvironment, JsonConvert.SerializeObject(t.Result));
                        // Save Response in db
                        string panNumber = panVerify.FirstOrDefault().pan;
                        string requestContent = JsonConvert.SerializeObject(panRequest);
                        string responseContent = JsonConvert.SerializeObject(apiCallResponseVM);
                        var result = SaveRequestResponseToDb(panNumber, transactionId, requestContent, responseContent, requestTime,dbname,islive);

                        HelperBLL.LogException(_webHostEnvironment, "SaveRequestResponseToDb____" + result);

                        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        string innerResponseContent = responseObject.ResponseContent.ToString();
                        var innerContentObject = JObject.Parse(innerResponseContent);
                        string responseCode = innerContentObject["response_Code"].ToString();
                        string panStatus = innerContentObject["outputData"][0]["pan_status"].ToString();
                        string matchedDescription = FetchPanStatusData(panStatus, dbname, islive);

                        if (responseCode == "1")
                        {
                            responseList.Add(new PANVerifyResponse
                            {
                                pan = panNumber,
                                name = panVerify.FirstOrDefault().name,
                                fathername = panVerify.FirstOrDefault().fathername,
                                dob = panVerify.FirstOrDefault().dob,
                                panStatusCode = panStatus,
                                panStatusDescription = matchedDescription,
                                createdAt = requestTime
                            });
                            var res = SaveSuccessResponseToDb(responseList, dbname, islive);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseList;
        }
        public bool SaveRequestResponseToDb(string panNumber, string transactionId, string requestContent, string responseContent, DateTime createdAt, string dbname,bool islive)
        {

            using (SqlConnection conn = _credManager.getConnections(dbname, islive))
            {
                
                using (SqlCommand cmd = new SqlCommand("Usp_ProteanKycLogs", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "SaveRequestResponseToDb");
                    cmd.Parameters.AddWithValue("@panNumber", panNumber);
                    cmd.Parameters.AddWithValue("@transactionId", transactionId);
                    cmd.Parameters.AddWithValue("@requestContent", requestContent);
                    cmd.Parameters.AddWithValue("@responseContent", responseContent);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public bool SaveSuccessResponseToDb(List<PANVerifyResponse> responses, string dbname, bool islive)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = _credManager.getConnections(dbname, islive))
            {
                conn.Open();

                foreach (var response in responses)
                {
                    DateTime correctedDate = DateTime.MinValue;
                    if (DateTime.TryParseExact(response.dob, HelperBLL.GetAllDatesFormats(), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        correctedDate = parsedDate;
                    }
                    if (correctedDate != DateTime.MinValue)
                    {
                        using (SqlCommand cmd = new SqlCommand("Usp_ProteanKycLogs", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Mode", "SaveSuccessResponseToDb");
                            cmd.Parameters.AddWithValue("@panNumber", response.pan);
                            cmd.Parameters.AddWithValue("@Name", response.name);
                            cmd.Parameters.AddWithValue("@FatherName", response.fathername);
                            cmd.Parameters.AddWithValue("@DOB", correctedDate.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@PanStatusCode", response.panStatusCode);
                            cmd.Parameters.AddWithValue("@PanStatusDescription", response.panStatusDescription);
                            rowsAffected = cmd.ExecuteNonQuery();
                        }
                    }
                }
                conn.Close();
            }

            return rowsAffected > 0 ? true : false;
        }
        public string FetchPanStatusData(string panStatus, string dbname, bool islive)
        {
            using (SqlConnection conn = _credManager.getConnections(dbname, islive))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_ProteanKycLogs", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "FetchPanStatusData");
                    cmd.Parameters.AddWithValue("@panStatusCode", panStatus);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<PANVerifyResponse> results = new List<PANVerifyResponse>();

                        while (reader.Read())
                        {
                            return reader["PANStatusDescription"].ToString();
                        }
                        return null;
                    }
                }
            }
        }
        #endregion

    }
}
