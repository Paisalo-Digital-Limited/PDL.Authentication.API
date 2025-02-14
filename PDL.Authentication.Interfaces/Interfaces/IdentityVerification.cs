using PDL.Authentication.Entites.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Interfaces.Interfaces
{
    public interface IdentityVerification
    {
        dynamic Get(IdentityVerficationVM objVM, string docVerifyApiKey,string activeuser, string dbname, bool islive);
    }
}
