using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class SmsVM
    {
        public string ContentId { get; set; }
        public string Language { get; set; }
        public string MobileNo { get; set; }
        public List<string>? data { get; set; }
    }

}
