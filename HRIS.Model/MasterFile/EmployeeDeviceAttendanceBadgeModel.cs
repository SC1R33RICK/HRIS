using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Model.MasterFile
{
    public class EmployeeDeviceAttendanceBadgeModel : ModelAuditInfo
    {
        public int deviceAttendanceId { get; set; }

        [DisplayName("Description")]
        public string description { get; set; }

        [DisplayName("Badge Number")]
        public string badgeNumber { get; set; }
        
    }
}