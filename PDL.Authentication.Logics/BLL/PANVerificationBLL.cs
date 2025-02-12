using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Logics.Credentials;
using System.Globalization;
using Microsoft.Data.SqlClient;
using PDL.Authentication.Security.DataSecurity;

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
                        if (islive)
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
                string query = "INSERT INTO PanVerificationLog (panNumber, transactionId, requestContent, responseContent, createdAt) " +
                               "VALUES (@panNumber, @transactionId, @requestContent, @responseContent, @createdAt)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@panNumber", panNumber);
                    cmd.Parameters.AddWithValue("@transactionId", transactionId);
                    cmd.Parameters.AddWithValue("@requestContent", requestContent);
                    cmd.Parameters.AddWithValue("@responseContent", responseContent);
                    cmd.Parameters.AddWithValue("@createdAt", createdAt);

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

                        string query = "INSERT INTO PanVerification (panNumber, name, fathername, dob, panStatusCode, panStatusDescription, createdAt) " +
                                       "VALUES (@panNumber, @name, @fathername, @dob, @panStatusCode, @panStatusDescription, @createdAt)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@panNumber", response.pan);
                            cmd.Parameters.AddWithValue("@name", response.name);
                            cmd.Parameters.AddWithValue("@fathername", response.fathername);
                            cmd.Parameters.AddWithValue("@dob", correctedDate.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@panStatusCode", response.panStatusCode);
                            cmd.Parameters.AddWithValue("@panStatusDescription", response.panStatusDescription);
                            cmd.Parameters.AddWithValue("@createdAt", response.createdAt);
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
                string query = "SELECT StatusCode, PANStatusDescription FROM [PanStatus] WHERE StatusCode = @panStatusCode";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
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
    }
}
