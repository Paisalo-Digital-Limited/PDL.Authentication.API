using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Interfaces.Interfaces
{
    public interface IProteinAuthenticate
    {
        Task<string> GetAccessTokenAsync(string dbname, bool isCredlive, bool islive);
    }
}
