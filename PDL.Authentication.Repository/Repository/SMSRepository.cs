using Jose;
using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using System.Buffers.Text;

namespace PDL.Authentication.Repository.Repository
{
    public class SMSRepository : BaseBLL, ISMSInterface
    {
        private readonly IConfiguration _configuration;
        private readonly ISMSInterface _sMSInterface;
        public SMSRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<int> SendSMS(SmsVM sendSmsVM, string baseUrl, string activeuser, string dbname, bool islive)
        {
            using (SMSBLL sMSBLL = new SMSBLL(_configuration))
            {
                return sMSBLL.SendSMS(sendSmsVM, baseUrl, activeuser, dbname, islive);
            }
        }

        public List<OTPVerificationResponse> VerifyOtp(OtpVerifyVM verifyVM, string dbname, string activeuser, bool islive)
        {
            using (SMSBLL sMSBLL = new SMSBLL(_configuration))
            {
                return sMSBLL.VerifyOtp(verifyVM, dbname,activeuser, islive);
            }
        }
    }
}
