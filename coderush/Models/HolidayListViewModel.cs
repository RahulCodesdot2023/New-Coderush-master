using coderush.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodesDotHRMS.Models
{
    public class HolidayListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Day { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public List<ProjectMasterViewModel> projectMaster { get; set; }

    }
    public class DashBoardViewModel
    {
        public List<HolidayListViewModel> HolidayList { get; set; }
        public List<ProjectMasterViewModel> projectMaster { get; set; }
        public List<LeaveCountViewModel> ApprovedData { get; set; }
        public List<LeaveCountViewModel> RejectedData { get; set; }

    }

    public class ProjectMasterViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string projectname { get; set; }

        [Required]
        [Display(Name = "Technologies:")]
        public int technologies { get; set; }

        [Required]
        [Display(Name = "Description:")]
        public string description { get; set; }
        [Display(Name = "Manager Name:")]
        [Required]
        public string managername { get; set; }
        [Display(Name = "Developer Name:")]
        [Required]
        public string developername { get; set; }
        public string developerId { get; set; }
        [Display(Name = "Start Date:")]
        public DateTime? StartedDate { get; set; }
        [Display(Name = "Status:")]
        public projectstatus ProjectStatus { get; set; }
        [Display(Name = "isactive:")]
        public bool isactive { get; set; }
        public bool isdeleted { get; set; }
        public string createdby { get; set; }
        public DateTime? createddate { get; set; }
        public string updatedby { get; set; }
        public DateTime? updateddate { get; set; }
        [Required]
        [Display(Name = "payment type:")]
        public string paymenttype { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "project amount must be numeric")]
        [Display(Name = "Amount:")]
        public string projectamount { get; set; }
        [Display(Name = "currency:")]
        public int currency { get; set; }
        public string TechnologyName { get; set; }
        //public string Currency{ get; set; }
        public string CurrencyName { get; set; }
        public int BankAccountId { get; set; }
        public string BankAccontname { get; set; }
        public string platform { get; set; }
        public string clientname { get; set; }
        public string emailid { get; set; }
        public string Handingpersonname { get; set; }

        public int PenaltyCount { get; set; }
        public int AchievementCount { get; set; }
    }

    public enum projectstatus
    {
        Inprogress = 1,
        Started = 2,
        OnHold = 3,
        Close = 4
    }
}
