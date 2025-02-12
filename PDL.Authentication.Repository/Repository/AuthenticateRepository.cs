using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Repository.Repository
{
    public class AuthenticateRepository:BaseBLL, IProteinAuthenticate
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthenticateRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public Task<string> GetAccessTokenAsync(string dbname, bool isCredlive, bool islive)
        {
            using (AuthenticateBLL authenticateBLL = new AuthenticateBLL(_configuration, _webHostEnvironment,_memoryCache,_httpClientFactory))
            {
                return authenticateBLL.GetAccessTokenAsync(dbname, isCredlive, islive);
            }
        }
    }
}
