namespace PDL.Authentication.Entites.VM
{
    public class BankAccVM
    {
        public BankAccVM()
        {
            consent = "Y";
        }
        public string consent { get; set; }
        public string ifsc { get; set; }
        public string accountNumber { get; set; }
        // public Clientdata clientData { get; set; }
    }

    public class Clientdata
    {
        public Clientdata()
        {
            caseId = Guid.NewGuid().ToString();
        }
        public string caseId { get; set; }
    }
    public class UdyamVM
    {
        public UdyamVM()
        {
            consent = "Y";
            isPDFRequired = "Y";
            getEnterpriseDetails="Y";
            clientData=new Clientdata();
        }
        public string consent { get; set; }
        public string udyamRegistrationNo { get; set; }
        public string isPDFRequired { get; set; }
        public string getEnterpriseDetails { get; set; }
        public Clientdata clientData { get; set; }
    }

    public class VoterVM
    {
        public VoterVM()
        {
           // UserId = "vpit1@paisalo.in";
            consent = "Y";
            clientData = new Clientdata();
        }
      //  public string UserId { get; set; }
        public string epicNo { get; set; }
        public string consent { get; set; }
        public Clientdata clientData { get; set; }
    }

    public class DrivingLicenseVM
    {
        public DrivingLicenseVM()
        {
            consent = "Y";
        }
        public string dlNo { get; set; }
        public string dob { get; set; }
        public string consent { get; set; }
        // public bool additionalDetails { get; set; }

        //public Clientdata clientData { get; set; }
    }

    public class VehicleRcVM
    {
        public VehicleRcVM()
        {
            consent = "Y";
            version = 3.1f;
        }
        public string consent { get; set; }
        public string registrationNumber { get; set; }
        public float version { get; set; }
    }

}
