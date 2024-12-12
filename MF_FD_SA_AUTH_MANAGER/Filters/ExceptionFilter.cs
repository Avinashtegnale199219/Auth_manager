using MF_FD_SA_AUTH_MANAGER.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MF_FD_SA_AUTH_MANAGER.BussinessObject;

namespace MF_FD_SA_AUTH_MANAGER.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private IExceptionServices _exceptionServices;

        public ExceptionFilter(IExceptionServices exceptionServices)
        {
            _exceptionServices = exceptionServices;
        }

        public override void OnException(ExceptionContext context)
        {
            List<Error> errorList = new List<Error>();
            Error error1 = new Error();

            if (!context.ExceptionHandled)
            {
                _exceptionServices.LogExceptionAsync(context, context.ActionDescriptor as ControllerActionDescriptor);
                var message = context.Exception.Message ?? context.Exception.InnerException.Message;
                if (ErrorMsg.ErrorMsgs.ContainsKey(message))
                {
                    error1.message = ErrorMsg.ErrorMsgs[message].Value;
                    error1.code = ErrorMsg.ErrorMsgs[message].Key;
                    errorList.Add(error1);
                    JsonResult jerror = new JsonResult(new { status = "error", code = "400", message = "Validation failed for the submitted data.", errors = errorList, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "" });
                    context.ExceptionHandled = true;
                    jerror.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    context.Result = jerror;
                }
                else if(!string.IsNullOrEmpty(message))
                {
                    error1.message= message;
                    errorList.Add(error1);
                    JsonResult jerror = new JsonResult(new { status = "error", code = "400", message = "Validation failed for the submitted data.", errors = errorList, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "" });
                    context.ExceptionHandled = true;
                    jerror.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    context.Result = jerror;

                }
                //else if (context.Exception.Message.Contains("MFFDCB"))
                //{
                //    JsonResult jerror = new JsonResult(new { status = "error", code = 500, message = "Something went Wrong", errors = "", timestamp = DateTime.Today.ToString(), path = "" });
                //    context.ExceptionHandled = true;
                //    jerror.StatusCode = (int)System.Net.HttpStatusCode.OK;
                //    context.Result = jerror;
                //}
                else
                {
                    JsonResult jerror = new JsonResult(new { status = "error", code = "500", message = "Something went Wrong", errors = "", timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "" });
                    context.ExceptionHandled = true;
                    jerror.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    context.Result = jerror;
                }
            }

            base.OnException(context);
        }

    }
}
