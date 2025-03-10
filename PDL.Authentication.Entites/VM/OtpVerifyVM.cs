using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class OtpVerifyVM
    {
        public string mobile { get; set; }
        public string otp { get; set; }
    }
}
