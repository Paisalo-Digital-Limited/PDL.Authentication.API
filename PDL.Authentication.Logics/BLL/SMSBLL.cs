using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.Credentials;
using PDL.Authentication.Logics.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PDL.Authentication.Logics.BLL
{
    public class SMSBLL : BaseBLL
    {
        private IConfiguration _configuration;
        private readonly CredManager _credManager;
        private readonly HttpClient _httpClient;
        public SMSBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }

        #region SEND SMS  -----------------Kartik------------------
        public async Task<int> SendSMS(SmsVM smsVM, string baseUrl, string activeuser, string dbName, bool isLive)
        {
            CommonHelper commonHelper = new CommonHelper();
            int affected = 0;
            try
            {
                SMSTemplateVM sMSTemplate = GetSmsTemplate(smsVM.ContentId, dbName, isLive);

                if (sMSTemplate != null)
                {
                    string otpCode = string.Empty;
                    SendSMSVM sendSMSVM = new SendSMSVM();
                    if (sMSTemplate.Type.ToLower().Contains("otp"))
                    {
                        sendSMSVM.Username = "paisalo.trans";
                        sendSMSVM.Password = "oDqLM";
                        sendSMSVM.From = "PAISAL";
                        otpCode = commonHelper.GenerateOTP(6);
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{OTP}", otpCode);
                    }
                    if (sMSTemplate.Type.ToLower().Contains("karnatka bank"))
                    {
                        sendSMSVM.Username = "paisalo.trans";
                        sendSMSVM.Password = "oDqLM";
                        sendSMSVM.From = "PAISAL";
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{#var#}", smsVM.data[0]);
                    }


                    //------------Add New Cases------------//
                    else if (sMSTemplate.Type.ToLower() == "crif1")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{DATE}", DateTime.Now.ToString("dd-MMMM-yyyy"));
                    }
                    else if (sMSTemplate.Type.ToLower() == "crif2")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{DATE}", DateTime.Now.ToString("dd-MMMM-yyyy"));
                    }
                    else if (sMSTemplate.Type.ToLower() == "crifmsg1")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{accno}", smsVM.data[0]).Replace("{conid}", smsVM.data[1]);
                    }
                    else if (sMSTemplate.Type.ToLower() == "crifmsg2")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{accno}", smsVM.data[0]).Replace("{conid}", smsVM.data[1]);
                    }
                    else if (sMSTemplate.Type.ToLower() == "crifmsg3")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{link}", smsVM.data[0]);
                    }
                    else if (sMSTemplate.Type.ToLower() == "crif2financialinfo")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{accno}", smsVM.data[0]).Replace("{status}", smsVM.data[1]);
                    }
                    else if (sMSTemplate.Type.ToLower() == "crif3purgefi")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{accno}", smsVM.data[0]).Replace("{DATE}", smsVM.data[1]);
                    }
                    else if (sMSTemplate.Type.ToLower() == "crifdatarequest")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{link}", smsVM.data[0]);
                    }
                    else if (sMSTemplate.Type.ToLower() == "installment")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{var}", smsVM.data[0]).Replace("{link}", smsVM.data[1]);
                    }
                    else if (sMSTemplate.Type.ToLower() == "emiinfo")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{TEXT}", smsVM.data[0]);
                    }
                    else if (sMSTemplate.Type.ToLower() == "foreclosure")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "late emi")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "emi overdue")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "ivr npa msg")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "emi due today")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "emi due tomorrow")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "npa account message")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "disbursementData")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{loanAppNo}", smsVM.data[0]).Replace("{link}", smsVM.data[1]);

                    }
                    else if (sMSTemplate.Type.ToLower() == "emi paid")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }

                    else if (sMSTemplate.Type.ToLower() == "receipt")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{Receipt}", smsVM.data[0]);


                    }
                    else if (sMSTemplate.Type.ToLower() == "loan repayment reminder")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{Date}", smsVM.data[0]).Replace("{DATE}", smsVM.data[1]);


                    }
                    else if (sMSTemplate.Type.ToLower() == "emiagentfinal")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "e-sign 2")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{ApplicationNo}", smsVM.data[0]).Replace("{Days}", smsVM.data[1]);


                    }
                    else if (sMSTemplate.Type.ToLower() == "loan application")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{Link}", smsVM.data[0]);


                    }
                    else if (sMSTemplate.Type.ToLower() == "loan application logged in")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "emiagent hindi")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "emi due - reminder")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "what to do with a business loan")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "disbursement")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{var}", smsVM.data[0]).Replace("{var}", smsVM.data[1]);


                    }
                    else if (sMSTemplate.Type.ToLower() == "emireminder")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage.Replace("{emidateddmmyyy}", smsVM.data[0]).Replace("{emi}", smsVM.data[1]);


                    }
                    else if (sMSTemplate.Type.ToLower() == "benefits of timely emi")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "emiunpaid")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }
                    else if (sMSTemplate.Type.ToLower() == "qrcode")
                    {
                        sMSTemplate.TextMessage = sMSTemplate.TextMessage;

                    }

                    sendSMSVM.ContentId = Convert.ToInt64(sMSTemplate.ContentId);
                    sendSMSVM.To = smsVM.MobileNo;
                    sendSMSVM.Text = sMSTemplate.TextMessage;
                    if (sMSTemplate.SMSLanguage.ToLower() == "hindi")
                    {
                        sendSMSVM.Unicode = true;
                    }

                    string fullApiUrl = $"{baseUrl}?username={sendSMSVM.Username}&password={sendSMSVM.Password}&unicode={sendSMSVM.Unicode}&from={sendSMSVM.From}&to={sendSMSVM.To}&text={sendSMSVM.Text}&dltContentId={smsVM.ContentId}";
                    var _httpClient = new HttpClient();
                    HttpResponseMessage response = await _httpClient.GetAsync(fullApiUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Error: " + errorContent); // Log the error content
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response: " + responseContent); // Log the successful response content
                    }

                    int statusCode = (int)response.StatusCode;

                    if (response.IsSuccessStatusCode)
                    {
                        affected = 1;
                    }
                    else
                    {
                        affected = 0;
                    }
                    string res = response.Content.ReadAsStringAsync().Result;
                    string query = "Usp_InsertSendSMS";

                    using (SqlConnection con = _credManager.getConnections(dbName, isLive))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Mode", "InsertData");
                            cmd.Parameters.AddWithValue("@ContentId", sendSMSVM.ContentId);
                            cmd.Parameters.AddWithValue("@Username", sendSMSVM?.Username ?? string.Empty);
                            cmd.Parameters.AddWithValue("@Password", sendSMSVM?.Password ?? string.Empty);
                            cmd.Parameters.AddWithValue("@Unicode", sendSMSVM?.Unicode);
                            cmd.Parameters.AddWithValue("@SendFrom", sendSMSVM?.From ?? string.Empty);
                            cmd.Parameters.AddWithValue("@SendTo", sendSMSVM?.To ?? string.Empty);
                            cmd.Parameters.AddWithValue("@Text", sendSMSVM?.Text ?? string.Empty);
                            cmd.Parameters.AddWithValue("@OTP", otpCode ?? string.Empty);
                            cmd.Parameters.AddWithValue("@Request", fullApiUrl ?? string.Empty);
                            cmd.Parameters.AddWithValue("@Response", res ?? string.Empty);

                            cmd.Connection = con;
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            affected = cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Dispose();
                        }
                    }
                }
                return affected;
            }
            catch (Exception ex)
            {
                ExceptionLog.InsertLogException(ex, _configuration, dbName, isLive, "SendSMS");
                throw new Exception("Error: " + ex.Message);
            }
        }

        public List<OTPVerificationResponse> VerifyOtp(OtpVerifyVM verifyVM, string dbname, string activeuser,  bool islive)
        {
            var results = new List<OTPVerificationResponse>();

                using (SqlConnection con = _credManager.getConnections(dbname, islive))
                {
                    string query = "Usp_VerifyOtp"; // Make sure this is the correct name
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 10).Value = verifyVM.mobile;
                        cmd.Parameters.Add("@Otp", SqlDbType.VarChar, 10).Value = verifyVM.otp;
                        cmd.CommandTimeout = 30;

                        con.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var response = new OTPVerificationResponse
                                {
                                    SmCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                                    QrCodeUrl = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                                };
                                results.Add(response);
                            }
                        }
                    }
                }
         

            return results;
        }

        public SMSTemplateVM GetSmsTemplate(string contentId, string dbName, bool isLive)
        {
            SMSTemplateVM smsTemplateDetails = new SMSTemplateVM();

            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                string query = "Usp_InsertSendSMS";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetData");
                    cmd.Parameters.AddWithValue("@ContentId", contentId);

                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.Read())
                        {
                            smsTemplateDetails.Type = !sdr.IsDBNull(sdr.GetOrdinal("Type")) ? sdr.GetString(sdr.GetOrdinal("Type")) : null;
                            smsTemplateDetails.TextMessage = !sdr.IsDBNull(sdr.GetOrdinal("TextMessage")) ? sdr.GetString(sdr.GetOrdinal("TextMessage")) : null;
                            smsTemplateDetails.SMSLanguage = !sdr.IsDBNull(sdr.GetOrdinal("SMSLanguage")) ? sdr.GetString(sdr.GetOrdinal("SMSLanguage")) : null;
                            smsTemplateDetails.ContentId = !sdr.IsDBNull(sdr.GetOrdinal("ContentId")) ? sdr.GetString(sdr.GetOrdinal("ContentId")) : null;
                        }
                    }
                }
            }

            return smsTemplateDetails;
        }
        #endregion
    }
}
