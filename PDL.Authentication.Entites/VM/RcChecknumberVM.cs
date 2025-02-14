using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class RcChecknumberVM
    {
        public DataRC data { get; set; }
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

    public class DataRC
    {
        public object blacklist_status { get; set; }
        public string body_type { get; set; }
        public object challan_details { get; set; }
        public string color { get; set; }
        public string cubic_capacity { get; set; }
        public string father_name { get; set; }
        public bool financed { get; set; }
        public string financer { get; set; }
        public string fit_up_to { get; set; }
        public string fuel_type { get; set; }
        public string insurance_company { get; set; }
        public string insurance_policy_number { get; set; }
        public string insurance_upto { get; set; }
        public string latest_by { get; set; }
        public bool less_info { get; set; }
        public string maker_description { get; set; }
        public string maker_model { get; set; }
        public string manufacturing_date { get; set; }
        public bool masked_name { get; set; }
        public string mobile_number { get; set; }
        public string national_permit_issued_by { get; set; }
        public object national_permit_number { get; set; }
        public object national_permit_upto { get; set; }
        public string no_cylinders { get; set; }
        public string noc_details { get; set; }
        public string non_use_from { get; set; }
        public string non_use_status { get; set; }
        public string non_use_to { get; set; }
        public string norms_type { get; set; }
        public string owner_name { get; set; }
        public string owner_number { get; set; }
        public string permanent_address { get; set; }
        public string permit_issue_date { get; set; }
        public object permit_number { get; set; }
        public string permit_type { get; set; }
        public string permit_valid_from { get; set; }
        public object permit_valid_upto { get; set; }
        public string present_address { get; set; }
        public string pucc_number { get; set; }
        public string pucc_upto { get; set; }
        public string rc_number { get; set; }
        public string rc_status { get; set; }
        public string registered_at { get; set; }
        public string registration_date { get; set; }
        public string seat_capacity { get; set; }
        public string sleeper_capacity { get; set; }
        public string standing_capacity { get; set; }
        public string tax_paid_upto { get; set; }
        public object tax_upto { get; set; }
        public string unladen_weight { get; set; }
        public object variant { get; set; }
        public string vehicle_category { get; set; }
        public string vehicle_category_description { get; set; }
        public string vehicle_chasi_number { get; set; }
        public string vehicle_engine_number { get; set; }
        public string vehicle_gross_weight { get; set; }
        public string wheelbase { get; set; }
    }
}
