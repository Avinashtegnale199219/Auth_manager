using DBHelper;
using MF_FD_SA_AUTH_MANAGER.BussinessObject;
using MF_FD_SA_AUTH_MANAGER.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER.DataAccessLayer
{
    public class AuthManagerDAL : IAuthManager
    {
        private readonly string _conn = string.Empty;
        public AuthManagerDAL(string ConnStr)
        {
            _conn = ConnStr;
        }
        public async Task<DataTable> IsAuthorizedUser(AuthManagerBO objBo)
        {
            try
            {
                DataTable dt;
                SqlParameter[] Params =
                {
                new SqlParameter("@Username",objBo.username),
                new SqlParameter("@Password",objBo.password)
                };
                dt = SqlHelper.ExecuteDataTable(_conn, CommandType.StoredProcedure, "Usp_MF_FD_SA_Get_User_Mst", Params);

                SqlParameter[] Params1 =
               {
                new SqlParameter("@Username",objBo.username),
                new SqlParameter("@API",objBo.API_Name),
                new SqlParameter("@CreatedBy",objBo.username),
                new SqlParameter("@CreatedByUName",objBo.username),
                new SqlParameter("@CreatedIP",GlobalBO.CreatedIP),
                new SqlParameter("@SessionId",DBNull.Value),
                new SqlParameter("@FormCode",DBNull.Value)
                };
                SqlHelper.ExecuteNonQuery(_conn, CommandType.StoredProcedure, "Usp_MF_FD_SA_Insert_Access_Log", Params1);


                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> InsertSessionMst(AuthManagerBO objBo)
        {
            int i = 0;
            try
            {
                SqlParameter[] Params1 =
              {
                new SqlParameter("@Username",objBo.username),
                new SqlParameter("@Start_DateTime",objBo.Start_DateTime),
                new SqlParameter("@GUID",objBo.GUID),
                new SqlParameter("@Auth_Key",DBNull.Value),
                new SqlParameter("@CreatedBy",objBo.username),
                new SqlParameter("@CreatedByUName",objBo.username),
                new SqlParameter("@CreatedIP",GlobalBO.CreatedIP),
                new SqlParameter("@SessionID",DBNull.Value)
                };
                Params1[7].Size = 100;
                Params1[7].Direction = ParameterDirection.Output;
                i = SqlHelper.ExecuteNonQuery(_conn, CommandType.StoredProcedure, "Usp_MF_FD_SA_Insert_Session_Mst", Params1);
                if (string.IsNullOrEmpty(Convert.ToString(Params1[7].Value)))
                {
                    return null;
                }
                else
                    return Params1[7].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateSessionMst(string Auth_Key, string SessionId)
        {
            int i = 0;
            try
            {
                SqlParameter[] Params1 =
              {

                new SqlParameter("@Auth_Key",Auth_Key),
                new SqlParameter("@SessionID",SessionId)
                };
                i = SqlHelper.ExecuteNonQuery(_conn, CommandType.StoredProcedure, "Usp_MF_FD_SA_Update_Session_Mst", Params1);
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetDecryptKey(string CypherKey)
        {
            try
            {

                DataTable dt;
                SqlParameter[] Params =
                {
                    new SqlParameter("@AUTH_KEY",CypherKey)
                };
                dt = SqlHelper.ExecuteDataTable(_conn, CommandType.StoredProcedure, "Usp_MF_FD_SA_Get_Token_Key", Params);

                return dt;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GET_API_ACCESS_DETAILS(string UserName, string api, string Auth_Key)
        {
            try
            {

                SqlParameter[] Params1 =
                {
                    new SqlParameter("@Username",UserName),
                    new SqlParameter("@API",api),
                    new SqlParameter("@CreatedBy",UserName),
                    new SqlParameter("@CreatedByUName",UserName),
                    new SqlParameter("@CreatedIP",GlobalBO.CreatedIP),
                    new SqlParameter("@SessionId",DBNull.Value),
                    new SqlParameter("@FormCode",DBNull.Value)
                };
                SqlHelper.ExecuteNonQuery(_conn, CommandType.StoredProcedure, "Usp_MF_FD_SA_Insert_Access_Log", Params1);

                DataSet ds;
                SqlParameter[] Params =
                {
                new SqlParameter("@Username",UserName),
                new SqlParameter("@API",api),
                new SqlParameter("@Auth_Key",Auth_Key)
                };
                ds = SqlHelper.ExecuteDataSet(_conn, CommandType.StoredProcedure, "Usp_MF_FD_SA_Get_API_Access_Dtls", Params);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                    return null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

    }
}
