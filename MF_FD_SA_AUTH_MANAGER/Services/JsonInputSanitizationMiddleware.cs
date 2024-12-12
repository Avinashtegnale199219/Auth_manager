using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MF_FD_SA_AUTH_MANAGER.BussinessObject;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace MF_FD_SA_AUTH_MANAGER.Services
{
    public class JsonInputSanitizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public JsonInputSanitizationMiddleware(RequestDelegate next,IConfiguration configuration)
        {
            _next = next;
            _config = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            List<Error> errorList = new List<Error>();
            Error error1 = new Error();

            // Buffer the request body so we can read it multiple times
            context.Request.EnableBuffering();

            // Read the request body
            string requestBody;
            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset the position of the request body stream
            }

            // Sanitize the JSON request body
           // string sanitizedBody = SanitizeJsonInput(requestBody);

            // Replace the original request body with the sanitized version
            //context.Request.Body = GenerateStreamFromString(sanitizedBody);

            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                if (SanitizeJsonInput1(requestBody))
                {
                    Error er = new Error();
                    er.message = ErrorMsg.ErrorMsgs["InvalidRequest"].Value;
                    er.code = ErrorMsg.ErrorMsgs["InvalidRequest"].Key;
                    errorList.Add(er);

                    context.Response.StatusCode = StatusCodes.Status400BadRequest;

                    string res = JsonConvert.SerializeObject(new { status = "error", code = 400, message = "", errors = errorList, timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), path = "" });

                    await context.Response.WriteAsync(res);
                    return;
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }

        // Function to sanitize JSON input by removing all HTML tags
        private string SanitizeJsonInput(string input)
        {
            // Use a regular expression to remove all HTML tags from the input string
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
        private bool SanitizeJsonInput1(string input)
        {
           if(input != null)
            {
                string specialChar = _config["AppSettings:specialChar"];
            //    string specialChar = "<.*?>\\|!#$%&/()?»«@£§€";//@"\|!#$%&/()?»«@£§€{}.;'<>_,";
                //string specialChar = "<>»«";//@"\|!#$%&/()?»«@£§€{}.;'<>_,";
                foreach (var item in specialChar)
                {
                    if (input.Contains(item)) return true;
                }

                //if (input.Length > 0 && input.Contains(input,"<.*?>"))
                //{
                //    return true;
                //}
            }
           return false;
        }

        // Helper method to generate a stream from a string
        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
