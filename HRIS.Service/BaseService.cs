using HRIS.Model.Sys;
using HRIS.Service.Sys;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace HRIS.Service
{
    public class BaseService
    {
        public void Log(string info, Exception ex, LoggedType loggedType = LoggedType.Warning)
        {
            try
            {
                //string source = "HRIS";
                //if (!EventLog.SourceExists(source))
                //    EventLog.CreateEventSource(source, "Application");

                //string errorMessage = string.Format("Message: {0}", ex.Message);
                //if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                //{
                //    errorMessage += string.Format("\nInnerException: {0}", ex.InnerException.Message);
                //}

                //EventLog.WriteEntry(source, errorMessage, loggedType);

               var loggerService =  DependencyResolver.Current.GetService<ILoggerService>();
                loggerService.Log(loggedType, info, ex.Message, ex.InnerException == null ? "" : ex.InnerException.Message);
            }
            catch { }
        }

        public void Log(string info, string msg, LoggedType loggedType = LoggedType.Warning)
        {
            this.Log(info, new Exception(msg), loggedType);
        }

        public void Log(string info, string msg, LoggedType loggedType = LoggedType.Warning, params object[] args)
        {
            this.Log(info, new Exception(string.Format(msg, args)), loggedType);
        }

        public int GetCurrentCompanyId()
        {
            int sessionId = int.Parse(System.Web.HttpContext.Current.User.Identity.Name.Split('-')[0]);
            int companyId = DependencyResolver.Current.GetService<IUserService>().GetCompanyIdBySessionId(sessionId);
            return companyId;
        }

        public void ExecuteSql(string sql)
        {
            DependencyResolver.Current.GetService<Data.HRISContext>().Database.ExecuteSqlCommand(sql);
        }

        public int GetCurrentSessionId()
        {
            int sessionId = int.Parse(System.Web.HttpContext.Current.User.Identity.Name.Split('-')[0]);
            return sessionId;
        }

        public int GetCurrentUserId()
        {
            string username = System.Web.HttpContext.Current.User.Identity.Name.Split('-')[1];
            return DependencyResolver.Current.GetService<IUserService>().GetUserIdByUsername(username);
        }

        public string GetSettingValue(string settingName)
        {
            int companyId = GetCurrentCompanyId();
            return DependencyResolver.Current.GetService<ISettingService>().GetValue(companyId, settingName);
        }

        public string GetSettingValue(int companyId, string settingName)
        {
            return DependencyResolver.Current.GetService<ISettingService>().GetValue(companyId, settingName);
        }
    }
}