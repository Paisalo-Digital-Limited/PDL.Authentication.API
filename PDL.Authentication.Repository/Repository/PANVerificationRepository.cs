using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.BLL;

namespace PDL.Authentication.Repository.Repository
{
    public class PANVerificationRepository:BaseBLL, IPANVerify
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PANVerificationRepository(IConfiguration configuration,IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<PANVerifyResponse> ProcessVerifyPanData(List<PANVerify> panVerify, string dbname, bool isCredlive, bool islive)
        {
            using (PANVerificationBLL panVerificationBLL = new PANVerificationBLL(_configuration,_webHostEnvironment))
            {
                return panVerificationBLL.ProcessVerifyPanData(panVerify, dbname, isCredlive, islive);
            }
        }
    }
}
