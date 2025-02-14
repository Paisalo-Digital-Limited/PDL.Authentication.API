using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class AccountLoginVM
    {
        [Required]
        public string EmailId { get; set; }
        [Required]
        public string Password { get; set; }
        public string? errormsg { get; set; }
        public bool isValidate { get; set; }
        public string? Name { get; set; }
        public int? RoleId { get; set; }
        public string? Creator { get; set; }
        public string? EmpCode { get; set; }
        public string? RoleName { get; set; }
    }
    public class AccountFogotPassword
    {
        public string Password { get; set; }
        public string? OldPassword { get; set; }
    }
}
