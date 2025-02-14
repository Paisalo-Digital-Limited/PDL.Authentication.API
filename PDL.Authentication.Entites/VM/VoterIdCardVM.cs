using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class VoterIdCardVM
    {
        public Data data { get; set; }
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

    public class Data
    {
        public string client_id { get; set; }
        public string epic_no { get; set; }
        public string gender { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public string relation_name { get; set; }
        public string relation_type { get; set; }
        public object house_no { get; set; }
        public object dob { get; set; }
        public string age { get; set; }
        public string area { get; set; }
        public string district { get; set; }
        public object[] additional_check { get; set; }
        public bool multiple { get; set; }
        public string last_update { get; set; }
        public string assembly_constituency { get; set; }
        public string assembly_constituency_number { get; set; }
        public string polling_station { get; set; }
        public string part_number { get; set; }
        public string part_name { get; set; }
        public string slno_inpart { get; set; }
        public string ps_lat_long { get; set; }
        public string rln_name_v1 { get; set; }
        public string rln_name_v2 { get; set; }
        public string rln_name_v3 { get; set; }
        public string section_no { get; set; }
        public string name_v1 { get; set; }
        public string name_v2 { get; set; }
        public string name_v3 { get; set; }
        public string st_code { get; set; }
        public string parliamentary_constituency { get; set; }
        public string id { get; set; }
    }
}
