using System;

namespace CodesDotHRMS.Models.ViewModels
{
    public class LeaveManagementViewModel
    {
        public int LeaveManagementId { get; set; }
        public string FullName { get; set; }
        public string UserId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public decimal TotalLeaveDay { get; set; }
        public decimal LeaveBuket { get; set; }
        public string LeaveReason { get; set; }
        public int LeaveType { get; set; }
        public int LeaveDayType { get; set; }
        public decimal PaidLeaveCount { get; set; }
        public decimal UnPaidLeaveCount { get; set; }
        public int LeaveStatus { get; set; }
        public string LeaveStatusName { get; set; }
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
