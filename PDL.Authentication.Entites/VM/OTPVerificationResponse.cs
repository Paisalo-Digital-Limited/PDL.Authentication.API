using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class OTPVerificationResponse
    {
        public string SmCode { get; set; }
        public string QrCodeUrl { get; set; }       
    }
}
