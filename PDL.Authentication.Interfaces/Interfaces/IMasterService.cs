using PDL.Authentication.Entites.VM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Interfaces.Interfaces
{
    public interface IMasterService
    {
        int CreatePageApiPermission(APIModule obj, string activeuser, string dbname, bool islive);
        int UpdatePageApiPermission(APIData obj, string activeuser, string dbname, bool islive);
        int DeletePageApiPermission(int Id, string activeuser, string dbname, bool islive);
        List<APIModule> GetPermission(PagepermissionVM pagePermission, string dbname, bool islive);
        List<APIModule> GetRolePermission(string dbname, bool islive);
        List<APIModule> AssignRolePermission(List<APIModule> obj, string activeuser, string dbname, bool islive);

    }
}
