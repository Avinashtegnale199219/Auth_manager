using DBHelper;
using MF_FD_SA_AUTH_MANAGER.BussinessObject;
using MF_FD_SA_AUTH_MANAGER.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF_FD_SA_AUTH_MANAGER.BussinessAccessLayer
{
    public class ExceptionUtility : IExceptionServices
    {
        private static string _conn = string.Empty;
        private static string _errFilePath = string.Empty;
        public ExceptionUtility(string conn, string errFilePath)
        {
            _conn = conn;
            _errFilePath = errFilePath;
        }
        #region Properties
        string AppName { get; set; }

        string ControllerName { get; set; }

        string ActionName { get; set; }

        string PageUrl { get; set; }

        string ExceptionType { get; set; }

        string Exception { get; set; }

        string ExceptionSource { get; set; }

        int LineNo { get; set; }

        string ExceptionStackTrace { get; set; }


        #endregion

        public async void LogExceptionAsync(Exception exception, Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controller = null)
        {
            try
            {
                string ControllerName = string.Empty, ActionName = string.Empty, DisplayName = string.Empty;


                ExceptionUtility ext = SetValue(exception);

                if (controller != null)
                {
                    ext.ControllerName = controller.ControllerName;
                    ext.ActionName = controller.ActionName;
                    ext.AppName = controller.ControllerTypeInfo.ToString();

                }


                await Task.Run(() =>
                {
                    DBErrorLog(ext);

                    //SendErrorMailAsync(ext);

                });

                FileErrorLogAsync(ext);
            }
            catch (Exception)
            {
            }
        }


        public async void LogExceptionAsync(ExceptionContext context, Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controller = null)
        {
            try
            {
                string ControllerName = string.Empty, ActionName = string.Empty, DisplayName = string.Empty;


                ExceptionUtility ext = SetValue(context);

                if (controller != null)
                {
                    ext.ControllerName = controller.ControllerName;
                    ext.ActionName = controller.ActionName;
                    ext.AppName = controller.ControllerTypeInfo.Assembly.FullName.ToString();

                }


                await Task.Run(() =>
                {
                    DBErrorLog(ext);

                    //SendErrorMailAsync(ext);

                });

                FileErrorLogAsync(ext);
            }
            catch (Exception)
            {
            }
        }

        public static void DBErrorLog(ExceptionUtility _eul)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(_conn))
                {
                    SqlParameter[] sqlparam = new SqlParameter[13];
                    sqlparam[0] = new SqlParameter("@Err_Type", _eul.ControllerName);
                    sqlparam[1] = new SqlParameter("@Err_ExecpMsg", _eul.Exception);
                    sqlparam[2] = new SqlParameter("@Err_Source", _eul.ExceptionSource);
                    sqlparam[3] = new SqlParameter("@LineNo", _eul.LineNo);
                    sqlparam[4] = new SqlParameter("@MethodName", _eul.ActionName);
                    sqlparam[5] = new SqlParameter("@RequestUrl", _eul.PageUrl);
                    sqlparam[6] = new SqlParameter("@CREATEDIP", GlobalBO.CreatedIP);
                    sqlparam[7] = new SqlParameter("@CREATEDBYUNAME", GlobalBO.CreatedByUName);
                    sqlparam[8] = new SqlParameter("@SESSIONID", GlobalBO.SessionID);
                    sqlparam[9] = new SqlParameter("@FileName", string.Empty);
                    sqlparam[10] = new SqlParameter("@FormCode", GlobalBO.FormCode);
                    sqlparam[11] = new SqlParameter("@CreatedBy", GlobalBO.CreatedBy);
                    sqlparam[12] = new SqlParameter("@Source", GlobalBO.Source);


                    SqlHelper.ExecuteNonQuery(_conn, CommandType.StoredProcedure, "USP_FD_CB_Error_Log", sqlparam);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void FileErrorLogAsync(ExceptionUtility _eul)
        {
            string ErrorLogFilePath = _errFilePath;

            if (string.IsNullOrEmpty(ErrorLogFilePath))
            {
                return;
            }

            if (!Directory.Exists(ErrorLogFilePath.Trim()))
            {
                return;
            }

            FileStream file = null;
            StreamWriter sw = null;
            StringBuilder sbLogMessage = new StringBuilder();
            try
            {
                #region Log Error in Text File                

                string logFile = ErrorLogFilePath + @"\" + System.DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                sbLogMessage.AppendLine();
                sbLogMessage.AppendLine("****************Exception****************");
                sbLogMessage.AppendLine("\nException Date: " + DateTime.Now.ToString());
                sbLogMessage.AppendLine("\nApplication Name: " + _eul.AppName);
                sbLogMessage.AppendLine("\nModule Name: " + _eul.ControllerName);
                sbLogMessage.AppendLine("\nSub Module Name: " + _eul.ActionName);
                sbLogMessage.AppendLine("Request Url: " + _eul.PageUrl);
                sbLogMessage.AppendLine("Created By: " + GlobalBO.CreatedBy);
                sbLogMessage.AppendLine("Created ByUName: " + GlobalBO.CreatedByUName);
                sbLogMessage.AppendLine("Created IP: " + GlobalBO.CreatedIP);
                sbLogMessage.AppendLine("Session ID: " + GlobalBO.SessionID);
                sbLogMessage.AppendLine("FormCode: " + GlobalBO.FormCode);
                sbLogMessage.AppendLine("Exception Type: " + _eul.ExceptionType);
                sbLogMessage.AppendLine("Exception: " + _eul.Exception);
                sbLogMessage.AppendLine("Source: " + _eul.ExceptionSource);
                sbLogMessage.AppendLine("Stack Trace: " + _eul.ExceptionStackTrace);

                using (file = new System.IO.FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    using (sw = new StreamWriter(file, Encoding.Unicode))
                    {
                        sw.Write(sbLogMessage.ToString());
                    }
                }
                #endregion
            }
            catch (IOException)
            {
                if (sw != null)
                {
                    sw.Close(); sw.Dispose();
                }
                if (file != null)
                {
                    file.Close(); file.Dispose();
                }
                if (sbLogMessage != null)
                {
                    sbLogMessage.Clear(); sbLogMessage = null;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close(); sw.Dispose();
                }
                if (file != null)
                {
                    file.Close(); file.Dispose();
                }
                if (sbLogMessage != null)
                {
                    sbLogMessage.Clear();
                }
            }
        }

        public void ClientSideErrorLog(string ErrorMessage)
        {
            try
            {
                string ErrorLogFilePath = Convert.ToString(_errFilePath);

                if (string.IsNullOrEmpty(ErrorLogFilePath))
                {
                    return;
                }

                if (!Directory.Exists(ErrorLogFilePath.Trim()))
                {
                    return;
                }

                string logFile = ErrorLogFilePath + @"\" + System.DateTime.Now.ToString("dd_MM_yyyy") + ".txt";

                using (FileStream file = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    using (StreamWriter sw = new StreamWriter(file, Encoding.Unicode))
                    {
                        sw.Write(ErrorMessage);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        static ExceptionUtility SetValue(Exception ex)
        {
            ExceptionUtility extut = new ExceptionUtility(_conn, _errFilePath);
            Clear(extut);
            try
            {
                if (ex != null)
                {
                    try
                    {
                        extut.Exception = (string.IsNullOrEmpty(ex.Message) ? ex.InnerException.Message : ex.Message);
                        extut.ExceptionSource = (string.IsNullOrEmpty(ex.Source) ? ex.InnerException.Source : ex.Source);
                        extut.ExceptionStackTrace = (string.IsNullOrEmpty(ex.StackTrace) ? (ex.InnerException.StackTrace) : ex.StackTrace);
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        StackTrace stk = new StackTrace(ex, true);
                        if (stk != null && stk.FrameCount > 0)
                        {
                            //extut.Exception += " Line Number: " + stk.GetFrame(0).GetFileLineNumber().ToString();
                            extut.ExceptionSource += " Method Name: " + stk.GetFrame(0).GetMethod().ToString();
                            extut.ExceptionStackTrace = (string.IsNullOrEmpty(ex.StackTrace) ? (ex.InnerException.StackTrace) : ex.StackTrace);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }


                HttpContextAccessor ctx = new HttpContextAccessor();

                if (ctx != null && ctx.HttpContext != null)
                {

                    HttpRequest Request = ctx.HttpContext.Request;

                    //UserApplicationDtls.UserApplicationDtls _app = new UserApplicationDtls.UserApplicationDtls(ctx.HttpContext);
                   // extut.PageUrl = _app.RequestUrl;
                }

            }
            catch (Exception)
            {
            }

            return extut;
        }

        static ExceptionUtility SetValue(ExceptionContext context)
        {
            ExceptionUtility extut = new ExceptionUtility(_conn, _errFilePath);
            Exception ex = context.Exception;

            Clear(extut);
            try
            {
                if (ex != null)
                {
                    try
                    {
                        extut.Exception = (string.IsNullOrEmpty(ex.Message) ? ex.InnerException.Message : ex.Message);
                        extut.ExceptionSource = (string.IsNullOrEmpty(ex.Source) ? ex.InnerException.Source : ex.Source);
                        extut.ExceptionStackTrace = (string.IsNullOrEmpty(ex.StackTrace) ? (ex.InnerException.StackTrace) : ex.StackTrace);
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        StackTrace stk = new StackTrace(ex, true);
                        if (stk != null && stk.FrameCount > 0)
                        {
                            //extut.Exception += " Line Number: " + stk.GetFrame(0).GetFileLineNumber().ToString();
                            extut.ExceptionSource += " Method Name: " + stk.GetFrame(0).GetMethod().ToString();
                            extut.LineNo = Convert.ToInt32(stk.GetFrame(0).GetFileLineNumber());
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                extut.PageUrl = context.HttpContext.Request.Path;

            }
            catch (Exception)
            {
            }

            return extut;
        }

        static void Clear(ExceptionUtility _eul)
        {
            _eul.AppName = _eul.Exception = _eul.ExceptionSource = _eul.ExceptionType = _eul.PageUrl = _eul.ExceptionStackTrace = string.Empty;
        }
    }
}
