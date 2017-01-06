using HRIS.Model.Sys;

namespace HRIS.Service.Sys
{
    public interface ILoggerService
    {
        void Log(LoggedType loggedType, string info, string message, string otherMessage = "", int? createdBy = default(int?));
    }
}