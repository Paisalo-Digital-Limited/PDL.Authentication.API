using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class IdentityVerficationVM
    {
        public string? type { get; set; }
        public string? txtnumber { get; set; }
        public string? Ifsc { get; set; }
        public string? userdob { get; set; }
        public string Key { get; set; }
    }
}
