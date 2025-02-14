using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PDL.Authentication.Entites.VM;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Logics.Helper
{
    public class IdentityVerficationHelper
    {
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public IdentityVerficationHelper(IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }
        #region Api calling Bank Pan VoterId Details ---Satish Maurya---
        public async Task<ApiCallResponseVM> GetDocVerifyResponseAsync(string requestData, string url, string apikey)
        {
            ApiCallResponseVM _apiCallResponseVM = new ApiCallResponseVM();
            try
            {
                HttpClient _Client = new HttpClient();
                _Client.DefaultRequestHeaders.Clear();
                Uri uri = new Uri(url);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                if (_Client.DefaultRequestHeaders.Authorization == null)
                {
                    _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _Client.DefaultRequestHeaders.Add("api-key", apikey);
                }
                using (var encodedContent = new StringContent(requestData, Encoding.UTF8, "application/json"))
                using (var response = _Client.PostAsync(uri, encodedContent))
                {
                    _apiCallResponseVM.StatusCode = response.Result.StatusCode;
                    _apiCallResponseVM.IsSuccessStatusCode = response.Result.IsSuccessStatusCode;
                    _apiCallResponseVM.ReasonPhase = response.Result.ReasonPhrase;
                    _apiCallResponseVM.ResponseContent = response.Result.Content.ReadAsStringAsync().Result;
                    return _apiCallResponseVM;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        public string GetPancardBaseurl()
        {
            string val = null;
            val = _configuration.GetValue<string>("pancardBaseurl");
            return val;
        }
        public string GetvoteridBaseurl()
        {
            string val = null;
            val = _configuration.GetValue<string>("voteridBaseurl");
            return val;
        }
        public string GetRCchecknoBaseurl()
        {
            string val = null;
            val = _configuration.GetValue<string>("rcchecknoBaseurl");
            return val;
        }
        public string GetDrivingLicenseBaseurl()
        {
            string val = null;
            val = _configuration.GetValue<string>("drivinglicenseBaseurl");
            return val;
        }
        public string GetBankAccountBaseurl()
        {
            string val = null;
            val = _configuration.GetValue<string>("bankaccountBaseurl");
            return val;
        }



    }
}
