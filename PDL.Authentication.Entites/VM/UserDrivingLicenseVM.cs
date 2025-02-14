using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class UserDrivingLicenseVM
    {
        public UserDLData data { get; set; }
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

    public class UserDLData
    {
        public string client_id { get; set; }
        public string license_number { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public string permanent_address { get; set; }
        public string permanent_zip { get; set; }
        public string temporary_address { get; set; }
        public string temporary_zip { get; set; }
        public string citizenship { get; set; }
        public string ola_name { get; set; }
        public string ola_code { get; set; }
        public string gender { get; set; }
        public string father_or_husband_name { get; set; }
        public string dob { get; set; }
        public string doe { get; set; }
        public string transport_doe { get; set; }
        public string doi { get; set; }
        public string transport_doi { get; set; }
        public string profile_image { get; set; }
        public bool has_image { get; set; }
        public string blood_group { get; set; }
        public string[] vehicle_classes { get; set; }
        public bool less_info { get; set; }
        public object[] additional_check { get; set; }
        public string initial_doi { get; set; }
        public object current_status { get; set; }
    }
}
