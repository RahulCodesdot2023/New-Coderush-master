using System;

namespace CodesDotHRMS.Models
{
    public class TimeSheet
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Description { get; set; }
    }
}
