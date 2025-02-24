using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class MenuVM
    {
        public string? Title { get; set; }
        public string? PageUrl { get; set; }
        public string? Icon { get; set; }
        public int? ParentId { get; set; }
        public bool? IsPage { get; set; }
        public string? type { get; set; }
        public int mainid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class MenuData
    {
        public int mainid { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string? titlename { get; set; }
        public string? PageUrl { get; set; }
        public string? PageName { get; set; }
        public string Icon { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }
        public int TotalCount { get; set; }


    }
    public class GetMenuVM
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PageMasterId { get; set; }
        public int ParentId { get; set; }
        public int userid { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string PageUrl { get; set; }
      
    }
    public class AllSubMenuList
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string? PageUrl { get; set; }
        public string? ParentName { get; set; }
        public int? IsActive { get; set; }
        public int? IsDeleted { get; set; }
    }
    public class PageMenuList
    {
        public int? MenuId { get; set; }
        public string MenuName { get; set; }
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string Icon { get; set; }
        public string? PageUrl { get; set; }
        public bool? IsPage { get; set; }
        public int? IsActive { get; set; }
        public int? IsDeleted { get; set; }
    }
    public class GetRolesMasterVM
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }


    }
    public class GetUSerMasterVM
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }

    }
    public class MenuPagePermission
    {
        public int RoleId { get; set; }
        public int PageMasterId { get; set; }
        public int userid { get; set; }
    }
    public class GetMenuPermissionVM
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PageMasterId { get; set; }
        public string AssignStatus { get; set; }
        public string Title { get; set; }
    }
}