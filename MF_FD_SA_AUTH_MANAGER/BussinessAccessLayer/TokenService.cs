using MF_FD_SA_AUTH_MANAGER.BussinessObject;
using MF_FD_SA_AUTH_MANAGER.DataAccessLayer;
using MF_FD_SA_AUTH_MANAGER.Services;
//using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MF_FD_SA_AUTH_MANAGER.Services;


namespace MF_FD_SA_AUTH_MANAGER.BussinessAccessLayer
{
    public class TokenService : ITokenServices
    {
        readonly string _conn = string.Empty;
        private IAuthManager _AuthManager;

        public TokenService(IAuthManager authManager, string conn)
        {
            _AuthManager = authManager;
            _conn = conn;
        }

        //public string Generate(string cpCode, string AppCode, string UserName, string Key, string IVV, string CreatedIP)
        //{
        //    try
        //    {
        //        string key = Key;
        //        if (string.IsNullOrWhiteSpace(key))
        //        {
        //            throw new System.ArgumentException(CRM_Error_Msg.ErrorMsg.TokenEncryptionKey);
        //        }

        //        string IV = IVV;
        //        if (string.IsNullOrWhiteSpace(IV))
        //        {
        //            throw new System.ArgumentException(CRM_Error_Msg.ErrorMsg.TokenEncryptionIV);
        //        }

        //        string SequenceNo = DateTime.Now.ToString("dd/MM/yyyy") + DateTime.Now.ToString("hh:mm:ss");
        //        SequenceNo = SequenceNo.Replace(":", "");
        //        SequenceNo = SequenceNo.Replace("/", "");

        //        //GUID

        //        Guid g = Guid.NewGuid();
        //        AuthManagerBO ObjBo = new AuthManagerBO
        //        {
        //            AppCode = AppCode,
        //            CP_Code = cpCode,
        //            User_Name = UserName,
        //            CreatedIP = CreatedIP,
        //            GUID = g.ToString(),
        //            Start_DateTime = SequenceNo,
        //            Auth_Key = null
        //        };
        //        string res = _AuthManager.InsertSessionMst(ObjBo);
        //        string token = "";

        //        if (!string.IsNullOrEmpty(res))
        //        {
        //            string text = cpCode + "|" + AppCode + "|" + UserName + "|" + SequenceNo + "|" + g.ToString() + "|" + res;
        //            token = AesCrypto.EncryptAES(text, key, IV);
        //            int i = _AuthManager.UpdateSessionMst(token, res);
        //            if (i > 0)
        //                return token;
        //            else
        //                throw new Exception("Token insert failed in session master.");
        //        }

        //        return token;
        //    }
        //    catch (Exception EX)
        //    {
        //        throw EX;
        //    }
        //}

        public async Task<string> Generate(string UserName, string Key, string IVV, string CreatedIP)
        {
            try
            {
                string key = Key;
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new System.ArgumentException("TokenEncryptionKey");
                }

                string IV = IVV;
                if (string.IsNullOrWhiteSpace(IV))
                {
                    throw new System.ArgumentException("TokenEncryptionIV");
                }

                string SequenceNo = DateTime.Now.ToString("dd/MM/yyyy") + DateTime.Now.ToString("hh:mm:ss");
                SequenceNo = SequenceNo.Replace(":", "");
                SequenceNo = SequenceNo.Replace("/", "");

                //GUID

                Guid g = Guid.NewGuid();
                AuthManagerBO ObjBo = new AuthManagerBO
                {
                    //AppCode = AppCode,
                    //CP_Code = cpCode,
                    username = UserName,
                    //CreatedIP = CreatedIP,
                    GUID = g.ToString(),
                    Start_DateTime = SequenceNo,
                    Auth_Key = null
                };
                string res = await _AuthManager.InsertSessionMst(ObjBo);
                string token = "";

                if (!string.IsNullOrEmpty(res))
                {
                    //string text = cpCode + "|" + AppCode + "|" + UserName + "|" + SequenceNo + "|" + g.ToString() + "|" + res;
                    string text = UserName + "|" + SequenceNo + "|" + g.ToString() + "|" + res;
                    token = await AesCrypto.EncryptAES(text, key, IV);
                    int i =await  _AuthManager.UpdateSessionMst(token, res);
                    if (i > 0)
                        return token;
                    else
                        throw new Exception("Token insert failed in session master.");
                }

                return token;
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        public async Task<string> DecryptToken(string cipherText)
        {
            DataTable dt = await _AuthManager.GetDecryptKey(cipherText);

            if (dt != null)
            {
                string key = dt.Rows[0]["f_API_Vector_Key"].ToString();
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new System.ArgumentException("TokenEncryptionKey");
                }

                string IV = dt.Rows[0]["f_API_Private_Key"].ToString();
                if (string.IsNullOrWhiteSpace(IV))
                {
                    throw new System.ArgumentException("TokenEncryptionIV");
                }

                string decryptedToken = await AesCrypto.DecryptAES(cipherText, key, IV);
                return decryptedToken;
            }

            throw new Exception("TokenEncryptionKey");
        }

        public async Task <string> API_ACCESS_DETAILS(string UserName, string api, string Auth_Key)
        {

            AuthManagerDAL objDal = new AuthManagerDAL(_conn);
            string isRights = await objDal.GET_API_ACCESS_DETAILS(UserName, api, Auth_Key);
            if (isRights != null)
            {
                return isRights;
            }
            else
                return null;

        }
    }
}
