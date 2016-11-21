using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Model.Attendance
{
    public class DeviceAttendanceModel : ModelAuditInfo
    {
        public int? id { get; set; }

        [Required]
        [DisplayName("Description")]
        public string description { get; set; }

        [Required]
        [DisplayName("Machine Number")]
        public string machineNumber { get; set; }

        [DisplayName("IP Address")]
        public string ipAddress { get; set; }

        [DisplayName("Port")]
        public int? port { get; set; }

        [DisplayName("Set Device In Use")]
        public bool inProgressImporting { get; set; }
    }
}