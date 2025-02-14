using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class SendMailVM
    {
        public string Type { get; set; }
        public string ToEmail { get; set; }
        public string? ccEmail { get; set; }
        public string? Subject { get; set; }
        public string? Password { get; set; }
        public string? AssignTo { get; set; }
        public string? decription { get; set; }
        public IFormFile? attachment { get; set; }
        public string? OTP { get; set; }

    }
}
