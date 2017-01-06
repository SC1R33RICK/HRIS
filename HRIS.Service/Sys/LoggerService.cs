using HRIS.Data;
using HRIS.Model.Sys;
using Repository;

namespace HRIS.Service.Sys
{
    public class LoggerService : ILoggerService
    {
        private readonly IRepository<sys_Log> _repoLog;
        private readonly IUnitOfWork _unitOfWork;

        public LoggerService()
        {
            var context = new HRISContext();
            _repoLog = new Repository<sys_Log>(context);
            _unitOfWork = new UnitOfWork(context);
        }

        public void Log(LoggedType loggedType, string info, string message, string otherMessage = "", int? createdBy = null)
        {
            _repoLog.Insert(new sys_Log()
            {
                loggedType = (int)loggedType,
                info = info,
                message = message,
                otherMessage = otherMessage,
                createdBy = createdBy,
            });
        }
    }
}