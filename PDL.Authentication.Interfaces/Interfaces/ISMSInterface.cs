using PDL.Authentication.Entites.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Interfaces.Interfaces
{
    public interface ISMSInterface
    {
        Task<int> SendSMS(SmsVM sendSmsVM, string baseUrl, string activeuser, string dbname, bool islive);
        List<OTPVerificationResponse> VerifyOtp(OtpVerifyVM verifyVM, string dbname,string activeuser, bool islive);

    }
}