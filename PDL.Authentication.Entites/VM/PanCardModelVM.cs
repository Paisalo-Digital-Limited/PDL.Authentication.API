using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class PanCardModelVM
    {
        public PanData data { get; set; }
        public string status { get; set; }
        public int status_code { get; set; }
        public int response_code { get; set; }
        public string message_code { get; set; }
        public string message { get; set; }
        public long timestamp { get; set; }
        public object error { get; set; }
        public bool success { get; set; }
        public string txn_id { get; set; }
    }
    public class PanData
    {
        public string name { get; set; }
        public string number { get; set; }
        public string type_of_holder { get; set; }
        public string type_of_holder_code { get; set; }
        public string is_individual { get; set; }
        public string is_valid { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string title { get; set; }
        public string pan_status { get; set; }
        public string pan_status_code { get; set; }
        public string aadhaar_seeding_status { get; set; }
        public string aadhaar_seeding_status_code { get; set; }
        public string last_updated_on { get; set; }
    }
}
