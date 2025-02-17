using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Repository.Repository
{
    public class MasterRepository :BaseBLL, IMasterService
    {
        private IConfiguration _configuration;
        public MasterRepository(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public int CreatePageApiPermission(APIModule obj, string activeuser, string dbname, bool islive)
        {
            using (MasterBLL masterBLL = new MasterBLL(_configuration))
            {
                return masterBLL.CreatePageApiPermission(obj, activeuser, dbname, islive);
            }
        }
        public int UpdatePageApiPermission(APIData obj, string activeuser, string dbname, bool islive)
        {
            using (MasterBLL masterBLL = new MasterBLL(_configuration))
            {
                return masterBLL.UpdatePageApiPermission(obj, activeuser, dbname, islive);
            }
        }
        public int DeletePageApiPermission(int Id, string activeuser, string dbname, bool islive)
        {
            using (MasterBLL masterBLL = new MasterBLL(_configuration))
            {
                return masterBLL.DeletePageApiPermission(Id, activeuser, dbname, islive);
            }
        }
        public List<APIModule> GetPermission(PagepermissionVM pagePermission, string dbname, bool islive)
        {
            using (MasterBLL masterBLL = new MasterBLL(_configuration))
            {
                return masterBLL.GetapiPermission(pagePermission, dbname, islive);
            }
        }
        public List<APIModule> GetRolePermission(string dbname, bool islive)
        {
            using (MasterBLL masterBLL = new MasterBLL(_configuration))
            {
                return masterBLL.GetRolePermission(dbname, islive);
            }
        }
    }
}
