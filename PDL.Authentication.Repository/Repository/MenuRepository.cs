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
    public class MenuRepository : BaseBLL, IMenuInterface
    {
        private IConfiguration _configuration;

        private readonly BaseBLL _basebll;

        public MenuRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _basebll = new BaseBLL();
        }
        public dynamic InsertMenu(MenuVM menuVM, string dbname, bool islive, string activeuser)
        {
            using (MenuBLL menuBll = new MenuBLL(_configuration))
            {
                return menuBll.InsertMenus(menuVM, dbname, islive, activeuser);
            }
        }
        public List<GetMenuVM> GetMainMenu(int pageNumber, int pageSize, string dbname, bool islive)
        {
            using (MenuBLL menuBll = new MenuBLL(_configuration))
            {
                List<GetMenuVM> menudata = new List<GetMenuVM>();
                return menudata = menuBll.GetMainMenu(pageNumber, pageSize, dbname, islive);
            }
        }
        public List<AllSubMenuList> GetSubMenuList(int pageNumber, int pageSize, string dbname, bool islive)
        {
            using (MenuBLL menuBll = new MenuBLL(_configuration))
            {
                List<AllSubMenuList> submenudata = new List<AllSubMenuList>();
                return submenudata = menuBll.GetSubMenuList(pageNumber, pageSize, dbname, islive);
            }
        }
        public List<PageMenuList> GetPageMenuList(int pageNumber, int pageSize, string dbname, bool islive)
        {
            using (MenuBLL menuBll = new MenuBLL(_configuration))
            {
                List<PageMenuList> pagemenudata = new List<PageMenuList>();
                return pagemenudata = menuBll.GetPageMenuList(pageNumber, pageSize, dbname, islive);
            }
        }
        public dynamic UpdateMenuData(MenuVM menuVM, string dbname, bool islive, string activeuser)
        {
            using (MenuBLL menuBll = new MenuBLL(_configuration))
            {
                return menuBll.UpdateMenuData(menuVM, dbname, islive, activeuser);
            }
        }
        public dynamic DeleteMenuData(MenuVM menuVM, string dbname, bool islive, string activeuser)
        {
            using (MenuBLL menuBll = new MenuBLL(_configuration))
            {
                return menuBll.DeleteMenuData(menuVM, dbname, islive, activeuser);
            }
        }
        public List<GetRolesMasterVM> GetRoles(string dbname, bool islive)
        {
            using (MenuBLL menuBll = new MenuBLL(_configuration))
            {
                return menuBll.GetRoles(dbname, islive);
            }
        }
        public List<GetUSerMasterVM> GetUserDDL(int roleid, string dbname, bool islive)
        {
            using (MenuBLL menuBll = new MenuBLL(_configuration))
            {
                return menuBll.GetUserDDL(roleid, dbname, islive);
            }
        }
        public List<MenuPagePermission> AssignUserRolePage(List<MenuPagePermission> RolePErmission, string dbname, bool islive, string activeuser)
        {
            using (MenuBLL menuBll = new MenuBLL(_configuration))
            {
                return menuBll.AssignInsertMenus(RolePErmission, dbname, islive, activeuser);
            }
        }
    }
}
