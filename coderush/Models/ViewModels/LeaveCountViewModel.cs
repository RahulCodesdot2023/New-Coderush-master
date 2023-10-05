using coderush.DataEnum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    public class LeaveCountViewModel
    {
        public LeaveCountViewModel()
        {
            List = new List<LeaveCountViewModel>();//Harshal working on-->
        }
        public List<LeaveCountViewModel> List { get; set; }
        public int Id { get; set; }
        public string Userid { get; set; }
        public string FullName { get; set; }
        [Required]
        public DateTime? Fromdate { get; set; }
        public string FromdateView { get; set; }
        [Required]
        public DateTime? Todate { get; set; }
        public string TodateView { get; set; }
        public IFormFile FileUpload { get; set; }
        public string Filename { get; set; }
        [Required]
        public string Count { get; set; }
        [Required]
        public string EmployeeDescription { get; set; }
        public string HrDescription { get; set; }

        public bool IsReject { get; set; }
        public bool Isapprove { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string Approveby { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string UserRole { get; set; }
        public bool AdminRole { get; set; }
        public bool adminrolesa { get; set; }
        public string UserName { get; set; }
        public bool isedit { get; set; }
        public string colouris { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? totalLeavesCount { get; set; }
        public int? LeaveReason { get; set; }
        public string LeaveVal { get; set; }
        public string LeaveOtherReason { get; set; }
    }
}
