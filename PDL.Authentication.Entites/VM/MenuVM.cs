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
        public string Icon { get; set; }
        public int IsActive { get; set; }
        public int IsDeleted { get; set; }


    }
    public class GetMenuVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public int? IsActive { get; set; }
        public int? IsDeleted { get; set; }
    }
    public class AllSubMenuList
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string? PageUrl { get; set; }
        public int? IsActive { get; set; }
        public int? IsDeleted { get; set; }
    }
    public class PageMenuList
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
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
}