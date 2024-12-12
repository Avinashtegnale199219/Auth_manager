using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER.Services
{
    public interface IExceptionServices
    {
        void LogExceptionAsync(Exception exception, Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controller = null);
        void LogExceptionAsync(ExceptionContext context, Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controller = null);

    }
}
