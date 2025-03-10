using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class SendSMSVM
    {
        public Int64 ContentId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Unicode { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Text { get; set; }
    }
}
