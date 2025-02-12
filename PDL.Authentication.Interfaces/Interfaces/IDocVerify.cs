
using PDL.Authentication.Entites.VM;
namespace PDL.Authentication.Interfaces.Interfaces
{
    public interface IDocVerify
    {
        dynamic GetVerifyDetails(KycDocVM docVM,string token,string dbname,bool isCredlive, bool islive);
    }
}
