using HRIS.Data;
using HRIS.Model.Attendance;
using HRIS.Service.Sys;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Service.Attendance
{
    public class DeviceAttendanceService : BaseService, IDeviceAttendanceService
    {
        private readonly IEnumReferenceService _enumReferenceService;
        private readonly IRepository<mf_DeviceAttendance> _repoDeviceAttendance;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceAttendanceService(IUnitOfWork unitOfWork
            , IEnumReferenceService enumReferenceService
            , IRepository<mf_DeviceAttendance> repoDeviceAttendance)
        {
            this._unitOfWork = unitOfWork;
            this._enumReferenceService = enumReferenceService;
            this._repoDeviceAttendance = repoDeviceAttendance;
        }

        public void Create(DeviceAttendanceModel model, out int deviceAttendanceId)
        {
            int currentUserId = this.GetCurrentUserId();
            var ins = this._repoDeviceAttendance.Insert(new mf_DeviceAttendance()
            {
                description = model.description,
                machineNumber = model.machineNumber,
                ipAddress = model.ipAddress,
                port = model.port,
                inProgressImporting = model.inProgressImporting,
                updatedBy = currentUserId,
            });
            this._unitOfWork.Save();
            deviceAttendanceId = ins.id;
        }

        public void Delete(int deviceAttendanceId)
        {
            var data = this._repoDeviceAttendance.Find(deviceAttendanceId);
            data.deleted = true;
            data.updatedBy = this.GetCurrentUserId();
            data.updatedDate = DateTime.Now;
            this._repoDeviceAttendance.Update(data);
            this._unitOfWork.Save();
        }

        public DeviceAttendanceModel GetById(int deviceAttendanceId)
        {
            return this.GetQuery().First(x => x.id == deviceAttendanceId);
        }

        public IQueryable<DeviceAttendanceModel> GetQuery()
        {
            var data = this._repoDeviceAttendance
                .Query()
                .Get()
                .Select(x => new DeviceAttendanceModel()
                {
                    id = x.id,
                    ipAddress = x.ipAddress,
                    description = x.description,
                    machineNumber = x.machineNumber,
                    port = x.port,
                    inProgressImporting = x.inProgressImporting ?? false,
                    updatedBy = x.sys_User.username,
                    updatedDate = x.updatedDate,
                });
            return data;
        }

        public bool IsDeviceInUse(int deviceId)
        {
            return this._repoDeviceAttendance.Query().Filter(x => x.id == deviceId && (x.inProgressImporting ?? false)).Get().Any();
        }

        public void Update(DeviceAttendanceModel model)
        {
            var upt = this._repoDeviceAttendance.Find(model.id);

            upt.description = model.description;
            upt.machineNumber = model.machineNumber;
            upt.ipAddress = model.ipAddress;
            upt.port = model.port;
            upt.inProgressImporting = model.inProgressImporting;
            upt.updatedBy = this.GetCurrentUserId();
            upt.updatedDate = DateTime.Now;
            this._repoDeviceAttendance.Update(upt);
            this._unitOfWork.Save();
        }
    }
}