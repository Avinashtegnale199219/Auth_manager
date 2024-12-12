using MF_FD_SA_AUTH_MANAGER.BussinessObject;
using MF_FD_SA_AUTH_MANAGER.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER.Filters
{
    public class ExtendedAthorizeFilter : IAsyncAuthorizationFilter
    {
        private ITokenServices _iTokenServices;
        private readonly IExceptionServices _exceptionServices;

        public ExtendedAthorizeFilter(ITokenServices iTokenServices, IExceptionServices exceptionServices)
        {
            _iTokenServices = iTokenServices;
            _exceptionServices = exceptionServices;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            List<Error> errors = new List<Error>();
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            string authorizationHeaderKey = context.HttpContext.Request.Headers["Authorization"];
            await Task.Run(async () =>
            {
                if (string.IsNullOrWhiteSpace(authorizationHeaderKey))
                {
                    Error er = new Error();
                    er.message = ErrorMsg.ErrorMsgs["Unauthorized"].Value;
                    er.code = ErrorMsg.ErrorMsgs["Unauthorized"].Key;
                    errors.Add(er);

                    JsonResult jerror = new JsonResult(new { status = "error", code= 401, message =  "", errors = errors, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "" });
                    jerror.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    context.Result = jerror;
                    return;
                }
                else
                {
                    try
                    {

                        string decryptedRequestKey = await _iTokenServices.DecryptToken(authorizationHeaderKey);

                        string[] values = decryptedRequestKey.Split('|');

                        if (!string.IsNullOrEmpty(values[0].ToString()))
                        {
                            var displayUrl = UriHelper.GetDisplayUrl(context.HttpContext.Request);
                            var urlBuilder =
                                new UriBuilder(displayUrl)
                                {
                                    Query = null,
                                    Fragment = null
                                };
                            string url = urlBuilder.ToString();

                            GlobalBO.CreatedIP = context.HttpContext.Connection.RemoteIpAddress.ToString();
                           // string strAccRights = await _iTokenServices.API_ACCESS_DETAILS(values[0], url, authorizationHeaderKey);
                            //if (!string.IsNullOrEmpty(strAccRights))
                            //{
                            //    if (strAccRights != "OK")
                            //    {
                            //        if (ErrorMsg.ErrorMsgs.ContainsKey(strAccRights))
                            //        {
                            //            context.Result = new  JsonResult(new { status = "error", code = ErrorMsg.ErrorMsgs["Unauthorized"].Key, message = ErrorMsg.ErrorMsgs["Unauthorized"].Value, errors = "" });

                            //            //JsonResult(new { status = "error", error_code = ErrorMsg.ErrorMsgs[strAccRights].Key, error_msg = ErrorMsg.ErrorMsgs[strAccRights].Value, result = "" });
                            //        }
                            //        else
                            //        {
                            //            //context.Result = new JsonResult(new { status = "Failure", error_code = strAccRights, error_msg = strAccRights, result = "" });
                            //            throw new Exception("strAccRights");
                            //        }
                            //        return;
                            //    }
                            //    else
                            //    {
                                    GlobalBO.CreatedBy = GlobalBO.CPCode = values[0];
                                    GlobalBO.CreatedByUName = values[0];
                                    GlobalBO.SessionID = Convert.ToInt64(values[3]);
                                    GlobalBO.CalledURL = url;
                                    GlobalBO.Version = "V1";
                            //    }
                            //}
                            //else
                            //{
                            //    context.Result = new JsonResult(new { status = "error", error_code = ErrorMsg.ErrorMsgs["Unauthorized"].Key, error_msg = ErrorMsg.ErrorMsgs["Unauthorized"].Value, result = "" });
                            //    return;
                            //}
                        }
                        else
                        {
                            Error er2 = new Error();
                            er2.message = ErrorMsg.ErrorMsgs["InvalidToken"].Value;
                            er2.code = ErrorMsg.ErrorMsgs["InvalidToken"].Key;
                            errors.Add(er2);

                            JsonResult jerror = new JsonResult(new { status = "error", code = 401, message = "", errors = errors, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "" });
                            jerror.StatusCode = (int)System.Net.HttpStatusCode.OK;
                            context.Result = jerror;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            });
        }

    }
}
