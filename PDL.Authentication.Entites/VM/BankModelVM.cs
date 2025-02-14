using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class BankModelVM
    {
        public BankData data { get; set; }
        public string status { get; set; }
        public int status_code { get; set; }
        public int response_code { get; set; }
        public string message_code { get; set; }
        public string message { get; set; }
        public long timestamp { get; set; }
        public object error { get; set; }
        public bool success { get; set; }
        public int count { get; set; }
        public string txn_id { get; set; }
    }
    public class BankData
    {
        public string client_id { get; set; }
        public bool account_exists { get; set; }
        public object upi_id { get; set; }
        public string full_name { get; set; }
        public object remarks { get; set; }
    }
}
