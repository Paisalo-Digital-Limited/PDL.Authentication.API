using PDL.Authentication.Entites.VM;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.AspNetCore.Hosting;


namespace PDL.Authentication.Logics.BLL
{
    public class HelperBLL : BaseBLL
    {
        public async Task<ApiCallResponseVM> PostRequestAsync(string TextData, Dictionary<string, string> credValues, string type, string token)
        {
            ApiCallResponseVM _apiCallResponseVM = new ApiCallResponseVM();
            try
            {
                string url = string.Empty;
                if (type.ToLower() == "bank")
                {
                    url = credValues["bankacc"].ToString();
                }
                else if (type.ToLower() == "dl")
                {
                    url = credValues["drivinglince"].ToString();
                }
                else if (type.ToLower() == "udyam")
                {
                    url = credValues["udyam"].ToString();
                }
                else if (type.ToLower() == "voter")
                {
                    url = credValues["voter"].ToString();
                }
                else if (type.ToLower() == "vehicle")
                {
                    url = credValues["vechile"].ToString();
                }
                else
                {
                    return null;
                }

                // Create a custom HttpClientHandler that bypasses SSL certificate validation
                //var handler = new HttpClientHandler
                //{
                //    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                //};

                // Create a new HttpClient instance
                //using (var _Client = new HttpClient(handler))
                using (var _Client = new HttpClient())
                {
                    UriBuilder uriBuilder = new UriBuilder(url);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    // Set default request headers
                    _Client.DefaultRequestHeaders.Add("apikey", credValues["username"].ToString());
                    _Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                    _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Prepare the request content
                    using (var encodedContent = new StringContent(TextData, Encoding.UTF8, "application/json"))
                    {
                        // Perform the HTTP POST request
                        /// HttpResponseMessage response = await _Client.PostAsync(url, encodedContent);
                        using (var response = _Client.PostAsync(uriBuilder.Uri, encodedContent).Result)
                        {    // Handle the response
                            _apiCallResponseVM.StatusCode = response.StatusCode;
                            _apiCallResponseVM.IsSuccessStatusCode = response.IsSuccessStatusCode;
                            _apiCallResponseVM.ReasonPhase = response.ReasonPhrase;
                            _apiCallResponseVM.ResponseContent = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _apiCallResponseVM;
        }


        public async Task<ApiCallResponseVM> PostRequestForPan(string TextData, string url, string userid, string version, int noofCount, string requesttime, string txnid)
        {
            ApiCallResponseVM _apiCallResponseVM = new ApiCallResponseVM();
            try
            {
                // Create a custom HttpClientHandler that bypasses SSL certificate validation
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                // Create a new HttpClient instance
                using (var _Client = new HttpClient(handler))
                {
                    // Set default request headers
                    _Client.DefaultRequestHeaders.Add("User_Id", userid);
                    _Client.DefaultRequestHeaders.Add("Version", version);
                    _Client.DefaultRequestHeaders.Add("Records_count", noofCount.ToString());
                    _Client.DefaultRequestHeaders.Add("Request_time", requesttime);
                    _Client.DefaultRequestHeaders.Add("Transaction_ID", userid + ":" + txnid);

                    _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Prepare the request content
                    using (var encodedContent = new StringContent(TextData, Encoding.UTF8, "application/json"))
                    {
                        // Perform the HTTP POST request
                        HttpResponseMessage response = await _Client.PostAsync(url, encodedContent);

                        // Handle the response
                        _apiCallResponseVM.StatusCode = response.StatusCode;
                        _apiCallResponseVM.IsSuccessStatusCode = response.IsSuccessStatusCode;
                        _apiCallResponseVM.ReasonPhase = response.ReasonPhrase;
                        _apiCallResponseVM.ResponseContent = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _apiCallResponseVM;
        }
        public static string[] GetAllDatesFormats()
        {
            return new string[]
                { // Formats with slashes and dashes
            "MM/dd/yyyy",
            "MM-dd-yyyy",
            "MM/dd/yy",
            "MM-dd-yy",
            "dd/MM/yyyy",
            "dd-MM-yyyy",
            "dd/MM/yy",
            "dd-MM-yy",
            "yyyy/MM/dd",
            "yyyy-MM-dd",
            "yyyy/MM/dd",
            "yyyy-MM-dd",
            "M/d/yyyy",
            "M-d-yyyy",
            "d/M/yyyy",
            "d-M-yyyy",
            "M/d/yy",
            "d/M/yy",
            "yyyy/M/d",
            "yyyy/M/dd",
            "yyyy/d/M",
            "yyyy/dd/M",
            // Formats with month names
            "dd-MMM-yyyy",  // e.g., 08-Aug-2024
            "d-MMM-yyyy",   // e.g., 8-Aug-2024
            "MMM-dd-yyyy",  // e.g., Aug-08-2024
            "MMM-d-yyyy",   // e.g., Aug-8-2024
            "MMM d, yyyy",  // e.g., Aug 8, 2024
            "d MMM yyyy",   // e.g., 8 Aug 2024
            "d MMMM yyyy",  // e.g., 8 August 2024
            "MMMM d, yyyy", // e.g., August 8, 2024
            // Additional common formats
            "yyyy-MM-ddTHH:mm:ss", // e.g., 2024-08-08T15:30:00
            "MM/dd/yyyy HH:mm:ss", // e.g., 08/08/2024 15:30:00
            "yyyy-MM-dd HH:mm:ss", // e.g., 2024-08-08 15:30:00
            "dd/MM/yyyy HH:mm:ss"  // e.g., 08/08/2024 15:30:00
                };
        }
        public static DateTime AttemptoCorrectDate(string dateString)
        {
            // Split the date string into components based on common delimiters
            var parts = dateString.Split(new char[] { '-', '/', '.' }, StringSplitOptions.RemoveEmptyEntries);

            // Ensure that we have exactly three parts (year, month, day)
            if (parts.Length != 3)
            {
                // Return DateTime.MinValue to indicate an invalid date
                return DateTime.MinValue;
            }

            // Parse the components
            if (int.TryParse(parts[0], out int year) &&
                int.TryParse(parts[1], out int month) &&
                int.TryParse(parts[2], out int day))
            {
                // Ensure month and day are within valid ranges
                if (month < 1) month = 1;
                if (month > 12) month = 12;

                int daysInMonth = DateTime.DaysInMonth(year, month);
                if (day < 1) day = 1;
                if (day > daysInMonth) day = daysInMonth;

                // Create and return the corrected DateTime
                return new DateTime(year, month, day);
            }

            // Return DateTime.MinValue to indicate an invalid date
            return DateTime.MinValue;
        }
        public static void LogException(IWebHostEnvironment webHostEnvironment, string source = null)
        {
            // Include logic for logging exceptions
            // Get the absolute path to the log file
            string webRootPath = webHostEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "Error");// "~/App_Data/ErrorLog.txt";
            path = path + "/Error.txt";

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(path, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);

            sw.Write("Exception Type: Without Wxception");
            sw.WriteLine("Source: " + source);
            sw.WriteLine("Stack Trace: ");
            sw.Close();
        }

    }
}
