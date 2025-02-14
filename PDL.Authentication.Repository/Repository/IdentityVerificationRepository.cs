using Microsoft.AspNetCore.Hosting;
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
    public class IdentityVerificationRepository:BaseBLL, IdentityVerification
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IdentityVerificationRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;

        }
        public dynamic Get(IdentityVerficationVM objVM, string docVerifyApiKey,string activeuser, string dbname, bool islive)
        {
            using (IdentityVerificationBLL identityVerificationBLL = new IdentityVerificationBLL(_configuration,_webHostEnvironment))
            {
                return identityVerificationBLL.Get(objVM, docVerifyApiKey, activeuser, dbname, islive);
            }
        }
    }
}
