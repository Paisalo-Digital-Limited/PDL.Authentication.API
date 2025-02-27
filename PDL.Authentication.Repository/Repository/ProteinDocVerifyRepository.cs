using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.BLL;
using PDL.Authentication.Logics.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Repository.Repository
{
    public class ProteinDocVerifyRepository:BaseBLL,IDocVerify
    {
        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _webHostEnvironment;
        public ProteinDocVerifyRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
           _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }
        public dynamic GetVerifyDetails(KycDocVM docVM, string token,string dbname, bool isCredlive, bool islive)
        {
            using (ProteinDocVerifyBLL docVerifyBLL = new ProteinDocVerifyBLL(_configuration,_webHostEnvironment))
            {
                return docVerifyBLL.GetVerifyDetails(docVM, token, dbname, isCredlive, islive);
            }
        }
    }
}
