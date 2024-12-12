using MF_FD_SA_AUTH_MANAGER.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MF_FD_SA_AUTH_MANAGER.BussinessObject;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MF_FD_SA_AUTH_MANAGER.Filters
{
    public class BadRequestFilter : IResultFilter, IFilterMetadata
    {
        private IExceptionServices _exceptionServices;

        public BadRequestFilter(IExceptionServices exceptionServices)
        {
            this._exceptionServices = exceptionServices;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            List<Error> errorList = new List<Error>();
            Error error1 = new Error();
          
            if ((context).ModelState.IsValid)
                return;
            foreach (KeyValuePair<string, ModelStateEntry> keyValuePair in (context).ModelState)
            {
                foreach (ModelError error2 in (keyValuePair.Value.Errors))
                {
                    Error error3 = new Error();
                    if (!string.IsNullOrEmpty(error2.ErrorMessage))
                    {
                        if (ErrorMsg.ErrorMsgs.ContainsKey(error2.ErrorMessage))
                        {
                            error3.field = keyValuePair.Key;
                            error3.message = ErrorMsg.ErrorMsgs[error2.ErrorMessage].Value;
                            error3.code = ErrorMsg.ErrorMsgs[error2.ErrorMessage].Key;
                        }
                        else
                        {
                            error3.field = keyValuePair.Key;
                            error3.message = error2.ErrorMessage;
                            error3.code = "";
                        }
                    }
                    else
                    {
                        Error error4 = error3;
                        KeyValuePair<string, string> errorMsg = ErrorMsg.ErrorMsgs["InvalidInput"];
                        string key = errorMsg.Key;
                        error4.field = key;
                        Error error5 = error3;
                        errorMsg = ErrorMsg.ErrorMsgs["InvalidInput"];
                        string str = errorMsg.Value;
                        error5.message = str;
                    }
                    errorList.Add(error3);
                }
            }
            context.Result = new OkObjectResult(new
            {
                status = "error",
                code = 400,
                message = "Validation failed for the submitted data.",
                errors = errorList,
                timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                path = ""
            });
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}