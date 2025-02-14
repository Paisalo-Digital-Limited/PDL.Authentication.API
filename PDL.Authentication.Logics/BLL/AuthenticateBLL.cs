using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PDL.Authentication.Logics.Credentials;
using PDL.Authentication.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Logics.BLL
{
    public class AuthenticateBLL:BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CredManager _credManager;
        public AuthenticateBLL(IConfiguration configuration, IWebHostEnvironment webHostEnvironment,IMemoryCache memoryCache,IHttpClientFactory httpClientFactory)
        {
            _credManager = new CredManager(configuration);
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;

        }
        #region --------- Doc verification By ----- Satish Maurya -------
        public async Task<string> GetAccessTokenAsync(string dbname, bool isCredlive, bool islive)
        {
            if (_memoryCache.TryGetValue<string>("AccessToken", out var accessToken))
            {
                return accessToken;
            }
            accessToken = await RequestNewAccessToken(_credManager.KycCredential(islive));

            _memoryCache.Set("AccessToken", accessToken, TimeSpan.FromMinutes(60));

            return accessToken;
        }

        private async Task<string> RequestNewAccessToken(Dictionary<string, string> cred)
        {
            string credentials = $"{cred["username"].ToString()}:{cred["password"].ToString()}";
            byte[] credentialsBytes = Encoding.UTF8.GetBytes(credentials);
            string base64Credentials = Convert.ToBase64String(credentialsBytes);

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            var client = _httpClientFactory.CreateClient();
            client = new HttpClient(handler);

            var request = new HttpRequestMessage(HttpMethod.Post, cred["tokenUrl"].ToString());
            request.Headers.Add("Authorization", $"Basic {base64Credentials}");
            request.Content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", cred["grant_type"].ToString())
            });

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var newAccessToken = await response.Content.ReadAsStringAsync();
                TokenResponseVM tokenResponseVM = JsonConvert.DeserializeObject<TokenResponseVM>(newAccessToken);
                return tokenResponseVM.access_token;
            }
            else
            {
                return null;
            }
        }

        //private async Task InitializeTokenAsync()
        //{
        //    string islive = _configExtension.GetMyKey("isCredlive");
        //    // Initial token retrieval and caching
        //    var initialToken = await RequestNewAccessToken(_credManager.KycCredential(Convert.ToBoolean(islive)));
        //    _memoryCache.Set("AccessToken", initialToken, TimeSpan.FromMinutes(60));

        //    // Set up a timer to refresh the token before expiration
        //    var timer = new Timer(async _ => await RefreshToken(), null, TimeSpan.Zero, TimeSpan.FromMinutes(55));
        //}

        //private async Task RefreshToken()
        //{
        //    string islive = _configExtension.GetMyKey("isCredlive");
        //    // Refresh the token and update the cache
        //    var refreshedToken = await RequestNewAccessToken(_credManager.KycCredential(Convert.ToBoolean(islive)));
        //    _memoryCache.Set("AccessToken", refreshedToken, TimeSpan.FromMinutes(60));
        //}

        #endregion

    }
}
