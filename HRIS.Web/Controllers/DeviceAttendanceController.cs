using HRIS.Model;
using HRIS.Model.Attendance;
using HRIS.Model.MasterFile;
using HRIS.Service.Attendance;
using HRIS.Service.MasterFile;
using HRIS.Web.Framework;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRIS.Web.Controllers
{
    public class DeviceAttendanceController : BaseController
    {
        private readonly IDeviceAttendanceService _deviceAttendanceService;

        public DeviceAttendanceController(IDeviceAttendanceService deviceAttendanceService)
        {
            this._deviceAttendanceService = deviceAttendanceService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DeviceAttendanceList([DataSourceRequest] DataSourceRequest request)
        {
            var data = this._deviceAttendanceService.GetQuery();
            var result = data.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeviceAttendanceCRUD([DataSourceRequest] DataSourceRequest request
            , UpdateType updateType
            , DeviceAttendanceModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                try
                {
                    switch (updateType)
                    {
                        case UpdateType.Create:
                            int deviceAttendanceId;
                            this._deviceAttendanceService.Create(model, out deviceAttendanceId);
                            model.id = deviceAttendanceId;
                            break;
                        case UpdateType.Update:
                            this._deviceAttendanceService.Update(model);
                            break;
                        case UpdateType.Destroy:
                            this._deviceAttendanceService.Delete(model.id.Value);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    this.AddModelError(ex);
                }
            }
            if (model.id.HasValue && updateType != UpdateType.Destroy)
            {
                model = this._deviceAttendanceService.GetById(model.id.Value);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }
}
