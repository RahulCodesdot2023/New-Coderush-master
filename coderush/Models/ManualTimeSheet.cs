using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodesDotHRMS.Models
{
    [Table("ManualTimeSheet")]
    public class ManualTimeSheet
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime ManualDate { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string Description { get; set; }
        public bool? IsApprove { get; set; }
    }
}
