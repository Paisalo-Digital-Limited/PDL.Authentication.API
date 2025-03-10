using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class SMSTemplateVM
    {
        
        public int Id { get; set; }
        public string Type { get; set; }
        public string TextMessage { get; set; }
        public string SMSLanguage { get; set; }
        public string ContentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
