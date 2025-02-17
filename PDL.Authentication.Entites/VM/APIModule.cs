using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class APIModule
    {
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionsName { get; set; }
        public string? ServiceName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public int ApiModuleId { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public int? UrlId { get; set; }
        public string? Title { get; set; }
    }
    public class APIData
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ControllerName { get; set; }
        public string ActionsName { get; set; }
    }
    public class PagepermissionVM
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }

    }
}
