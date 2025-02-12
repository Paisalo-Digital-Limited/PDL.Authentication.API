using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.Authentication.Entites.VM
{
    public class BankAccountVM
    {
        public string UserID { get; set; }
        public string AccNo { get; set; }
        public string Ifsc { get; set; }
    }
    public class VotterVM
    {
        public string UserID { get; set; }
        public string voterno { get; set; }

    }
    public class UdhyamVM
    {
        public string UserID { get; set; }
        public string udyamRegistrationNo { get; set; }
    }
    public class DLVM
    {
        public string UserID { get; set; }
        public string dlno { get; set; }
        public string dob { get; set; }
    }
    public class VehicleVM
    {
        public string UserID { get; set; }
        public string vehicleNumber { get; set; }
    }
    public class KycDocVM
    {
        public string UserID { get; set; }
        public string Type { get; set; }
        public string? AccNo { get; set; }
        public string? Ifsc { get; set; }
        public string? vehicleNumber { get; set; }
        public string? udyamRegistrationNo { get; set; }
        public string? voterno { get; set; }
        public string? dlno { get; set; }
        public string? dob { get; set; }
        public string? docType { get; set; }

    }
    public class PanVerifyRequestVM
    {
        public List<PANVerify> inputData { get; set; }
        public string signature { get; set; }
    }
    public class PANVerify
    {
        public string pan { get; set; }
        public string name { get; set; }
        public string fathername { get; set; }
        public string dob { get; set; }
    }
    public class PANVerifyResponse
    {
        public string pan { get; set; }
        public string name { get; set; }
        public string fathername { get; set; }
        public string dob { get; set; }
        public string panStatusCode { get; set; }
        public string panStatusDescription { get; set; }
        public DateTime createdAt { get; set; }
    }
}
