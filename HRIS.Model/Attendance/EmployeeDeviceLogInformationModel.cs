using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Model.Attendance
{
    public class EmployeeDeviceLogInformationModel
    {
        public string badgeNo { get; set; }
        public DateTime date { get; set; }
        public TIME_LOG_TYPE InOutMode { get; set; }
    }
}
