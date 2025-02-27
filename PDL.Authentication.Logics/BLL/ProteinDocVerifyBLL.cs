using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.Credentials;
using PDL.Authentication.Security.DataSecurity;
using System.Data;
using System.Net;
using System.Text;

namespace PDL.Authentication.Logics.BLL
{
    public class ProteinDocVerifyBLL : BaseBLL
    {
        string strKey = ""; byte[] bKey = null;
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;
        private IWebHostEnvironment _webHostEnvironment;

        public ProteinDocVerifyBLL(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _credManager = new CredManager(configuration);
            strKey = Utils.RandomString(16);
            bKey = Encoding.UTF8.GetBytes(strKey);
            _webHostEnvironment = webHostEnvironment;

        }
        #region    -----ProteinDocVerify -------- Satsih Maurya ---------
        public dynamic GetVerifyDetails(KycDocVM docVM, string token,string dbname, bool iscredlive, bool isdblive)
        {
            dynamic result = null;
            Dictionary<string, string> keyValuePairs = _credManager.KycCredential(iscredlive);
            if (docVM.Type.ToLower() == "bank")
            {
                result = GetBankAccountVerification(docVM, keyValuePairs, token, dbname, isdblive);
            }
            else if (docVM.Type.ToLower() == "dl")
            {
                result = GetDrivingVerification(docVM, keyValuePairs, token, dbname, isdblive);
            }
            else if (docVM.Type.ToLower() == "udyam")
            {
                result = GetUdyamVerification(docVM, keyValuePairs, token, dbname, isdblive);
            }
            else if (docVM.Type.ToLower() == "voter")
            {
                result = GetVoterVerification(docVM, keyValuePairs, token, dbname, isdblive);
            }
            else if (docVM.Type.ToLower() == "vehicle")
            {
                result = GetVehicleVerification(docVM, keyValuePairs, token, dbname, isdblive);
            }
            //else
            //{
            //    return null;
            //}
            return result;
        }
        private dynamic GetUdyamVerification(KycDocVM docVM, Dictionary<string, string> keyValuePairs, string token, string dbname, bool isdblive)
        {
            using (HelperBLL helperbll = new HelperBLL())
            {
                ApiCallResponseVM apiCallResponseVM = new ApiCallResponseVM();
                string encodedEncryptedKey = string.Empty;
                string encryptedContent = string.Empty;
                string content = string.Empty;
                ApiRequestVM apiRequestVM = new ApiRequestVM();

                UdyamVM objVM = new UdyamVM();
                objVM.udyamRegistrationNo = docVM.udyamRegistrationNo;

                content = JsonConvert.SerializeObject(objVM);

                encodedEncryptedKey = Asymmentric.Encrypt(strKey, _webHostEnvironment);
                encryptedContent = AesGcmCrypto.Encrypt(content, strKey);

                apiRequestVM.requestId = Guid.NewGuid().ToString();
                apiRequestVM.version = "1.0";
                apiRequestVM.timestamp = TimesStampCreation();
                apiRequestVM.symmetricKey = encodedEncryptedKey;
                apiRequestVM.data = encryptedContent;
                apiRequestVM.hash = Utils.PayloadSignatureGenerator(content, strKey);
                var t = Task.Run(() => helperbll.PostRequestAsync(JsonConvert.SerializeObject(apiRequestVM), keyValuePairs, docVM.Type, token));
                t.Wait();
                apiCallResponseVM = t.Result;
                string docType = "UD";
                if (apiCallResponseVM.StatusCode == HttpStatusCode.OK)
                {
                    ApiRequestVM response = JsonConvert.DeserializeObject<ApiRequestVM>(apiCallResponseVM.ResponseContent);

                    string decryptedKey = Asymmentric.Decrypt(response.symmetricKey, _webHostEnvironment);
                    string encryptedData = response.data;
                    string data = AesGcmCrypto.Decrypt(encryptedData, decryptedKey);
                    InsertProteanKycLogs(docVM.UserID, content, data, docType,dbname, isdblive);
                    return data;
                }
                else
                {
                    ApiRequestVM response = JsonConvert.DeserializeObject<ApiRequestVM>(apiCallResponseVM.ResponseContent);

                    string decryptedKey = Asymmentric.Decrypt(response.symmetricKey,_webHostEnvironment);
                    string encryptedData = response.data;
                    string data = AesGcmCrypto.Decrypt(encryptedData, decryptedKey);
                    InsertProteanKycLogs(docVM.UserID, content, data, docType,dbname, isdblive);
                }
            }
            return null;
        }
        private dynamic GetDrivingVerification(KycDocVM docVM, Dictionary<string, string> keyValuePairs, string token, string dbname, bool isdblive)
        {
            using (HelperBLL helperbll = new HelperBLL())
            {
                ApiCallResponseVM apiCallResponseVM = new ApiCallResponseVM();
                string encodedEncryptedKey = string.Empty;
                string encryptedContent = string.Empty;
                string content = string.Empty;
                ApiRequestVM apiRequestVM = new ApiRequestVM();

                DrivingLicenseVM objVM = new DrivingLicenseVM();
                objVM.dob = docVM.dob;
                objVM.dlNo = docVM.dlno;

                content = JsonConvert.SerializeObject(objVM);

                encodedEncryptedKey = Asymmentric.Encrypt(strKey, _webHostEnvironment);
                encryptedContent = AesGcmCrypto.Encrypt(content, strKey);

                apiRequestVM.requestId = Guid.NewGuid().ToString();
                apiRequestVM.version = "1.0";
                apiRequestVM.timestamp = TimesStampCreation();
                apiRequestVM.symmetricKey = encodedEncryptedKey;
                apiRequestVM.data = encryptedContent;
                apiRequestVM.hash = Utils.PayloadSignatureGenerator(content, strKey);
                var t = Task.Run(() => helperbll.PostRequestAsync(JsonConvert.SerializeObject(apiRequestVM), keyValuePairs, docVM.Type, token));
                t.Wait();
                apiCallResponseVM = t.Result;
                string docType = "D";

                if (apiCallResponseVM.StatusCode == HttpStatusCode.OK)
                {
                    ApiRequestVM response = JsonConvert.DeserializeObject<ApiRequestVM>(apiCallResponseVM.ResponseContent);

                    string decryptedKey = Asymmentric.Decrypt(response.symmetricKey,_webHostEnvironment);;
                    string encryptedData = response.data;
                    string data = AesGcmCrypto.Decrypt(encryptedData, decryptedKey);
                    InsertProteanKycLogs(docVM.UserID, content, data, docType,dbname, isdblive);
                    return data;
                }
                else
                {
                    ApiRequestVM response = JsonConvert.DeserializeObject<ApiRequestVM>(apiCallResponseVM.ResponseContent);

                    string decryptedKey = Asymmentric.Decrypt(response.symmetricKey,_webHostEnvironment);;
                    string encryptedData = response.data;
                    string data = AesGcmCrypto.Decrypt(encryptedData, decryptedKey);
                    InsertProteanKycLogs(docVM.UserID, content, data, docType,dbname, isdblive);
                }
            }
            return null;
        }
        private dynamic GetVoterVerification(KycDocVM docVM, Dictionary<string, string> keyValuePairs, string token, string dbname, bool isdblive)
        {
            using (HelperBLL helperbll = new HelperBLL())
            {
                ApiCallResponseVM apiCallResponseVM = new ApiCallResponseVM();
                string encodedEncryptedKey = string.Empty;
                string encryptedContent = string.Empty;
                string content = string.Empty;
                ApiRequestVM apiRequestVM = new ApiRequestVM();

                VoterVM objVM = new VoterVM();
                objVM.epicNo = docVM.voterno;


                content = JsonConvert.SerializeObject(objVM);

                encodedEncryptedKey = Asymmentric.Encrypt(strKey, _webHostEnvironment);
                encryptedContent = AesGcmCrypto.Encrypt(content, strKey);

                apiRequestVM.requestId = Guid.NewGuid().ToString();
                apiRequestVM.version = "1.0";
                apiRequestVM.timestamp = TimesStampCreation();
                apiRequestVM.symmetricKey = encodedEncryptedKey;
                apiRequestVM.data = encryptedContent;
                apiRequestVM.hash = Utils.PayloadSignatureGenerator(content, strKey);
                var t = Task.Run(() => helperbll.PostRequestAsync(JsonConvert.SerializeObject(apiRequestVM), keyValuePairs, docVM.Type, token));
                t.Wait();
                apiCallResponseVM = t.Result;
                string docType = "V";
                if (apiCallResponseVM.StatusCode == HttpStatusCode.OK)
                {
                    ApiRequestVM response = JsonConvert.DeserializeObject<ApiRequestVM>(apiCallResponseVM.ResponseContent);

                    string decryptedKey = Asymmentric.Decrypt(response.symmetricKey,_webHostEnvironment);;
                    string encryptedData = response.data;
                    string data = AesGcmCrypto.Decrypt(encryptedData, decryptedKey);
                    InsertProteanKycLogs(docVM.UserID, content, data, docType,dbname, isdblive);
                    return data;
                }
                else
                {
                    ApiRequestVM response = JsonConvert.DeserializeObject<ApiRequestVM>(apiCallResponseVM.ResponseContent);

                    string decryptedKey = Asymmentric.Decrypt(response.symmetricKey,_webHostEnvironment);;
                    string encryptedData = response.data;
                    string data = AesGcmCrypto.Decrypt(encryptedData, decryptedKey);
                    InsertProteanKycLogs(docVM.UserID, content, data, docType,dbname, isdblive);
                }
            }
            return null;
        }
        private dynamic GetVehicleVerification(KycDocVM docVM, Dictionary<string, string> keyValuePairs, string token, string dbname, bool isdblive)
        {
            using (HelperBLL helperbll = new HelperBLL())
            {
                ApiCallResponseVM apiCallResponseVM = new ApiCallResponseVM();
                string encodedEncryptedKey = string.Empty;
                string encryptedContent = string.Empty;
                string content = string.Empty;
                ApiRequestVM apiRequestVM = new ApiRequestVM();

                VehicleRcVM objVM = new VehicleRcVM();
                objVM.registrationNumber = docVM.vehicleNumber;

                content = JsonConvert.SerializeObject(objVM);

                encodedEncryptedKey = Asymmentric.Encrypt(strKey, _webHostEnvironment);
                encryptedContent = AesGcmCrypto.Encrypt(content, strKey);

                apiRequestVM.requestId = Guid.NewGuid().ToString();
                apiRequestVM.version = "1.0";
                apiRequestVM.timestamp = TimesStampCreation();
                apiRequestVM.symmetricKey = encodedEncryptedKey;
                apiRequestVM.data = encryptedContent;
                apiRequestVM.hash = Utils.PayloadSignatureGenerator(content, strKey);
                var t = Task.Run(() => helperbll.PostRequestAsync(JsonConvert.SerializeObject(apiRequestVM), keyValuePairs, docVM.Type, token));
                t.Wait();
                apiCallResponseVM = t.Result;
                string docType = "VH";
                if (apiCallResponseVM.StatusCode == HttpStatusCode.OK)
                {
                    ApiRequestVM response = JsonConvert.DeserializeObject<ApiRequestVM>(apiCallResponseVM.ResponseContent);

                    string decryptedKey = Asymmentric.Decrypt(response.symmetricKey,_webHostEnvironment);;
                    string encryptedData = response.data;
                    string data = AesGcmCrypto.Decrypt(encryptedData, decryptedKey);
                    InsertProteanKycLogs(docVM.UserID, content, data, docType,dbname, isdblive);
                    return data;
                }
                else
                {
                    ApiRequestVM response = JsonConvert.DeserializeObject<ApiRequestVM>(apiCallResponseVM.ResponseContent);

                    string decryptedKey = Asymmentric.Decrypt(response.symmetricKey,_webHostEnvironment);;
                    string encryptedData = response.data;
                    string data = AesGcmCrypto.Decrypt(encryptedData, decryptedKey);
                    InsertProteanKycLogs(docVM.UserID, content, data, docType,dbname, isdblive);
                }
            }
            return null;
        }
        private dynamic GetBankAccountVerification(KycDocVM docVM, Dictionary<string, string> keyValuePairs, string token,string dbname, bool isdblive)
        {
            using (HelperBLL helperbll = new HelperBLL())
            {
                ApiCallResponseVM apiCallResponseVM = new ApiCallResponseVM();
                string encodedEncryptedKey = string.Empty;
                string encryptedContent = string.Empty;
                string content = string.Empty;
                ApiRequestVM apiRequestVM = new ApiRequestVM();
                BankAccVM objVM = new BankAccVM();
                objVM.accountNumber = docVM.AccNo;
                objVM.ifsc = docVM.Ifsc;
                content = JsonConvert.SerializeObject(objVM);

                encodedEncryptedKey = Asymmentric.Encrypt(strKey, _webHostEnvironment);
                encryptedContent = AesGcmCrypto.Encrypt(content, strKey);

                apiRequestVM.requestId = Guid.NewGuid().ToString();
                apiRequestVM.version = "1.0";
                apiRequestVM.timestamp = TimesStampCreation();
                apiRequestVM.symmetricKey = encodedEncryptedKey;
                apiRequestVM.data = encryptedContent;
                apiRequestVM.hash = Utils.PayloadSignatureGenerator(content, strKey);
                var t = Task.Run(() => helperbll.PostRequestAsync(JsonConvert.SerializeObject(apiRequestVM), keyValuePairs, docVM.Type, token));
                t.Wait();
                apiCallResponseVM = t.Result;
                string docType = "B";
                if (apiCallResponseVM.StatusCode == HttpStatusCode.OK)
                {
                    ApiRequestVM response = JsonConvert.DeserializeObject<ApiRequestVM>(apiCallResponseVM.ResponseContent);

                    string decryptedKey = Asymmentric.Decrypt(response.symmetricKey,_webHostEnvironment);;
                    string encryptedData = response.data;
                    string data = AesGcmCrypto.Decrypt(encryptedData, decryptedKey);
                    InsertProteanKycLogs(docVM.UserID, content, data, docType,dbname, isdblive);
                    return data;
                }
                else
                {
                    ApiRequestVM response = JsonConvert.DeserializeObject<ApiRequestVM>(apiCallResponseVM.ResponseContent);

                    string decryptedKey = Asymmentric.Decrypt(response.symmetricKey,_webHostEnvironment);;
                    string encryptedData = response.data;
                    string data = AesGcmCrypto.Decrypt(encryptedData, decryptedKey);
                    InsertProteanKycLogs(docVM.UserID, content, data, docType,dbname, isdblive);
                }
            }
            return null;
        }
        private void InsertProteanKycLogs(string userid, string request, string response, string docType, string dbname, bool isdblive)
        {
            try
            {
                JObject jsonResponse = JObject.Parse(response);

                string name = jsonResponse["result"]?["name"]?.ToString();
                string epicNo = jsonResponse["result"]?["epicNo"]?.ToString();

                string accountName = jsonResponse["result"]?["accountName"]?.ToString();
                string accountNumber = jsonResponse["result"]?["accountNumber"]?.ToString();
                string ifsc = jsonResponse["result"]?["ifsc"]?.ToString();

                //For Bank Details
                string dlNumber = jsonResponse["result"]?["dlNumber"]?.ToString();

                //Set value for insert
                string finalName = accountName ?? name; 
                string finalIdNumber = accountNumber ?? epicNo ?? dlNumber; 
                string finalIFSC = ifsc;

                using (SqlConnection con = _credManager.getConnections(dbname, isdblive))
                {
                    using (SqlCommand cmd = new SqlCommand("Usp_ProteanKycLogs",con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "InsertProteanKycLogs");

                        cmd.Parameters.AddWithValue("@UserId", userid);
                        cmd.Parameters.AddWithValue("@JsonRequest", request);
                        cmd.Parameters.AddWithValue("@JsonResponse", response);
                        cmd.Parameters.AddWithValue("@DocType",  docType);
                        cmd.Parameters.AddWithValue("@Name",  finalName); 
                        cmd.Parameters.AddWithValue("@IdNumber",  finalIdNumber);
                        cmd.Parameters.AddWithValue("@IFSC", finalIFSC ); 

                        cmd.Connection = con;
                        con.Open();
                        SqlTransaction trans = con.BeginTransaction();
                        try
                        {
                            cmd.Transaction = trans;
                            cmd.ExecuteNonQuery();  
                            trans.Commit();        
                        }
                        catch (SqlException sqlEx)
                        {
                            trans.Rollback();
                            throw new Exception(sqlEx.Message + " : " + sqlEx.Procedure + " : " + sqlEx.LineNumber);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        private string TimesStampCreation()
        {
            DateTime currentDate = DateTime.Now;
            DateTime currentDatePlusOne = currentDate.AddSeconds(1); // Adding one second to the current date
            string formattedTimestamp = currentDatePlusOne.ToString("yyyy-MM-dd 'T'HH:mm:ss.fff");
            return formattedTimestamp;
        }
        #endregion
    }
}
