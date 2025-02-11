using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class AccountTokens
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string? Creator { get; set; }
        public string? RoleName { get; set; }
        public string? EmpCode { get; set; }
        public string? Token { get; set; }
        public TimeSpan? Validity { get; set; }
    }
}
