using System.Net;

namespace PDL.Authentication.Entites.VM
{
    public class ApiCallResponseVM
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhase { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public bool isByteArrayContent { get; set; }
        public string ResponseContent { get; set; }
        public string ExceptionMessage { get; set; }

    }
    public class ApiRequestVM
    {
        public string requestId { get; set; }
        public string version { get; set; }
        public string timestamp { get; set; }
        public string symmetricKey { get; set; }
        public string data { get; set; }
        public string hash { get; set; }
    }
}
