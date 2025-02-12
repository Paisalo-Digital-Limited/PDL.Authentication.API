
using PDL.Authentication.Entites.VM;

namespace PDL.Authentication.Interfaces.Interfaces
{
    public interface IPANVerify
    {
        List<PANVerifyResponse> ProcessVerifyPanData(List<PANVerify> panVerify, string dbname, bool isCredlive, bool islive);
    }
}
