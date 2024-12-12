using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER.Services
{
    public interface ITokenServices
    {


        //string Generate(string cpCode, string AppCode, string UserName, string Key, string IVV, string CreatedIP);
        Task <string> Generate(string UserName, string Key, string IVV, string CreatedIP);
        Task <string> DecryptToken(string cipherText);

        //string API_ACCESS_DETAILS(string cpCode, string AppCode, string UserName, string api, string Auth_Key);
        Task <string> API_ACCESS_DETAILS(string UserName, string api, string Auth_Key);
    }
}
