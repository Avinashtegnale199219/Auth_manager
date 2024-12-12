using MF_FD_SA_AUTH_MANAGER.BussinessObject;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace MF_FD_SA_AUTH_MANAGER.Services
{
    public class DuplicateParametersDetectionMiddleware
    {
        private RequestDelegate _next;

        public DuplicateParametersDetectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
       
        public async Task InvokeAsync(HttpContext context)
        {
            List<Error> errorList = new List<Error>();
         
            if (context.Request.ContentType != null && context.Request.ContentType.Contains("application/json"))
            {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(body))
                {
                    var jsonBodyParam = GetDuplicateJsonKeys(body);

                    if (jsonBodyParam.Count != 0)
                    {
                        Error er = new Error();
                        er.message = ErrorMsg.ErrorMsgs["DuplicateInput"].Value;
                        er.code = ErrorMsg.ErrorMsgs["DuplicateInput"].Key;
                        errorList.Add(er);
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;

                        string res = JsonConvert.SerializeObject(new { status = "error", code = 400, message = "", errors = errorList, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "" });

                        await context.Response.WriteAsync(res);

                        //context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        //await context.Response.WriteAsync("Request contains duplicate keys.");
                        return;
                    }
                }

                context.Request.Body.Position = 0;
            }

            await _next(context);
        }

        //private static List<string> FindDup(List<string> keys)
        //{
        //    return keys.GroupBy(k => k)
        //        .Where(g => g.Count() > 1)
        //        .Select(g => g.Key)
        //        .ToList();
        //}

        private List<string> GetDuplicateJsonKeys(string jsonString)
        {
            var duplicatekeys = new List<string>();

            try
            {
                using (var jsonDoc = JsonDocument.Parse(jsonString))
                {
                    var element = jsonDoc.RootElement;
                    ExtractKeys(element, duplicatekeys);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return duplicatekeys;
        }

        private static void ExtractKeys(JsonElement element, List<string> keys)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                var keySet = new HashSet<string>();
                foreach (var prop in element.EnumerateObject())
                {
                    if (!keySet.Add(prop.Name.ToLower()))
                    {
                        keys.Add(prop.Name.ToLower());
                    }


                    ExtractKeys(prop.Value, keys);
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var prop in element.EnumerateArray())
                {
                    ExtractKeys(prop, keys);
                }
            }

        }


    }
    //public class DuplicateParametersDetectionMiddleware
    //{
    //    private RequestDelegate _next;

    //    public DuplicateParametersDetectionMiddleware(RequestDelegate next)
    //    {
    //        _next = next;
    //    }
    //    public async Task InvokeAsync(HttpContext httpContext)
    //    {
    //        List<Error> errorList = new List<Error>();
    //        Error error1 = new Error();

    //        httpContext.Request.EnableBuffering();

    //        using StreamReader reader = new(httpContext.Request.Body, Encoding.UTF8, leaveOpen: true);
    //        string body = await reader.ReadToEndAsync();
    //        httpContext.Request.Body.Position = 0;

    //        if (!string.IsNullOrWhiteSpace(body))
    //        {
    //            if (HasDuplicateKeys(body))
    //            {
    //                Error er = new Error();
    //                er.message = ErrorMsg.ErrorMsgs["DeuplicateInput"].Value;
    //                er.code = ErrorMsg.ErrorMsgs["DeuplicateInput"].Key;
    //                errorList.Add(er);

    //                httpContext.Response.StatusCode = StatusCodes.Status200OK;

    //                string res = JsonConvert.SerializeObject(new { status = "error", code = 400, message = "", errors = errorList, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "" });

    //                await httpContext.Response.WriteAsync(res);
    //                return;
    //            }
    //        }


    //        await _next(httpContext);
    //    }


    //    private bool HasDuplicateKeys(string jsonString)
    //    {
    //        try
    //        {
    //            using (var stringreader = new StringReader(jsonString))
    //            {
    //                using (var Jsonreader = new JsonTextReader(stringreader))
    //                {
    //                    var keys = new HashSet<string>();
    //                    string currentkey = null;
    //                    while (Jsonreader.Read())
    //                    {
    //                        if (Jsonreader.TokenType == JsonToken.PropertyName)
    //                        {
    //                            currentkey = Jsonreader.Value.ToString().ToLower();
    //                            if (!keys.Add(currentkey.ToLower()))
    //                            {
    //                                return true;
    //                            }

    //                        }
    //                    }
    //                }
    //            }
    //            return false;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }
    //    }
    //}
}
