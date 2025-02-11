using Microsoft.AspNetCore.Mvc;
using PDL.Authentication.Logics.Helper;
using System.Resources;

namespace PDL.Authentication.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public BaseApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected static ResourceManager resourceManager = new ResourceManager("PDL.Authentication.API.Extensions.ReturnMessages", typeof(BaseApiController).Assembly);
        protected string GetDBName()
        {
            string val = _configuration.GetValue<string>("encryptSalts:dbval");
            string salt = _configuration.GetValue<string>("encryptSalts:dbName");
            val = Helper.Decrypt(val, salt);
            return val;
        }
        protected bool GetIslive()
        {
            bool val = false;
            val = _configuration.GetValue<bool>("isliveDb");
            return val;

        }
    }
}
