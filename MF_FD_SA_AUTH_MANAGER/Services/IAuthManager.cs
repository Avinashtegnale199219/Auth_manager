using MF_FD_SA_AUTH_MANAGER.BussinessObject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER.Services
{
    public interface IAuthManager
    {
        Task <DataTable> IsAuthorizedUser(AuthManagerBO ObjBo);

        Task <string> InsertSessionMst(AuthManagerBO ObjBo);

        Task <int> UpdateSessionMst(string Auth_Key, string SessionId);

        Task <DataTable> GetDecryptKey(string CypherKey);
    }
}
