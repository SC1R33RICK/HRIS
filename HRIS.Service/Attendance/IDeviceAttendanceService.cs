using System.Linq;
using HRIS.Model.Attendance;

namespace HRIS.Service.Attendance
{
    public interface IDeviceAttendanceService
    {
        void Create(DeviceAttendanceModel model, out int deviceAttendanceId);
        void Delete(int deviceAttendanceId);
        DeviceAttendanceModel GetById(int deviceAttendanceId);
        IQueryable<DeviceAttendanceModel> GetQuery();
        void Update(DeviceAttendanceModel model);
        bool IsDeviceInUse(int deviceId);
    }
}