using MF_FD_SA_AUTH_MANAGER;
using MF_FD_SA_AUTH_MANAGER.BussinessObject;
using MF_FD_SA_AUTH_MANAGER.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WI_MF_FD_SA_AUTH_MANAGER.Controllers
{

    [Route("api/authmanager")]
    [ApiController]
    public class AuthManagerController : ControllerBase
    {
        private readonly IAuthManager _IAuthManager;
        private readonly ITokenServices _tokenService;
        public AuthManagerController(IAuthManager IAuthManager, ITokenServices tokenService)
        {
            _IAuthManager = IAuthManager;
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<IActionResult> POST([FromBody] AuthManagerBO objBo)
        {
            List<Error> errors = new List<Error>();
            try
            {
                GlobalBO.CreatedIP = Convert.ToString(HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress);

                string displayUrl = UriHelper.GetDisplayUrl(Request);
                UriBuilder urlBuilder =
                    new(displayUrl)
                    {
                        Query = null,
                        Fragment = null
                    };
                string url = urlBuilder.ToString();

                objBo.API_Name = url;

                DataTable dt = await _IAuthManager.IsAuthorizedUser(objBo);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string key = dt.Rows[0]["f_API_Vector_Key"].ToString();
                    string IV = dt.Rows[0]["f_API_Private_Key"].ToString();
                    string token = await _tokenService.Generate(objBo.username, key, IV, GlobalBO.CreatedIP);
                    return Ok(new { status = "success", code  = 200, message = "success", data = token, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "api/authmanager" });
                }
                else
                {
                    Error er = new Error();
                    er.field = "";
                    er.message = ErrorMsg.ErrorMsgs["Unauthorized"].Value;
                    er.code = ErrorMsg.ErrorMsgs["Unauthorized"].Key;
                    errors.Add(er);
                    return Ok(new { status = "error", code = 401, message = "fail", errors = errors, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "api/authmanager" });
                }

            }
            catch (Exception ex)
            {
                Error er = new Error();
                er.message = ex.Message;
                errors.Add(er);
                return Ok(new { status = "error", code = 500, message = "Validation failed for the submitted data.", errors = errors, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "api/authmanager" });

            }


        }
    }






}
