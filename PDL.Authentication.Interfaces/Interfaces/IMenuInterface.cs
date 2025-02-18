using PDL.Authentication.Entites.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Interfaces.Interfaces
{
    public interface IMenuInterface
    {
        dynamic InsertMenu(MenuVM menuVM, string dbname, bool islive, string activeuser);
        List<GetMenuVM> GetMainMenu(int pageNumber, int pageSize, string dbname, bool islive);
        List<AllSubMenuList> GetSubMenuList(int pageNumber, int pageSize, string dbname, bool islive);
        List<PageMenuList> GetPageMenuList(int pageNumber, int pageSize, string dbname, bool islive);
        dynamic UpdateMenuData(MenuVM menuVM, string dbname, bool islive, string activeuser);
        dynamic DeleteMenuData(MenuVM menuVM, string dbname, bool islive, string activeuser);
        List<GetRolesMasterVM> GetRoles(string dbname, bool islive);
        List<GetUSerMasterVM> GetUserDDL(int roleid, string dbname, bool islive);
        List<MenuPagePermission> AssignUserRolePage(List<MenuPagePermission> RolePErmission, string dbname, bool islive, string activeuser);
        List<GetMenuPermissionVM> GetPermissionPageList(int RoleId, string dbname, bool islive);
        List<MenuData> GetMenudata(string activeuser, string dbname, bool islive);

    }
}
