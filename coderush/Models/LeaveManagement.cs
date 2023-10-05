using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodesDotHRMS.Models
{
    public class LeaveManagement
    {
        [Key]
        public int LeaveManagementId { get; set; }
        public Guid UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalLeaveDay { get; set; }
        public decimal LeaveBuket { get; set; }
        public string LeaveReason { get; set; }
        public int LeaveType { get; set; }
        public int LeaveDayType { get; set; }
        public decimal PaidLeaveCount { get; set; }
        public decimal UnPaidLeaveCount { get; set; }
        public int LeaveStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedReasone { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool IsDeleted { get; set; }
        public string UploadedFile { get; set; }
        public bool IsMonthlyLeaveLeft { get; set; }
    }
     
}
