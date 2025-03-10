using PDL.Authentication.Interfaces.Interfaces;
using PDL.Authentication.Logics.Credentials;
using PDL.Authentication.Repository.Repository;

namespace PDL.Authentication.API.Extensions
{
    public static class ServiceExtensions
    {
        #region Dependency Inject
        public static void RegisterRepository(this IServiceCollection collection)
        {
            collection.AddMemoryCache();
            collection.AddHttpClient();
            collection.AddScoped<CredManager>();
            collection.AddScoped<IAccountInterface, AccountRepository>();
            collection.AddScoped<IProteinAuthenticate, AuthenticateRepository>();
            collection.AddScoped<IdentityVerification, IdentityVerificationRepository>();
            collection.AddScoped<IPANVerify, PANVerificationRepository>();
            collection.AddScoped<IDocVerify, ProteinDocVerifyRepository>();
            collection.AddScoped<IMenuInterface, MenuRepository>();
            collection.AddScoped<IMasterService, MasterRepository>();
            collection.AddScoped<ISMSInterface, SMSRepository>();
        }
        #endregion
    }
}
