using System;
using System.ComponentModel.DataAnnotations;

namespace CodesDotHRMS.Models
{
    public class FinanceMaster
    {
        public int Id { get; set; }
        [Display(Name = "Bank Name:")]
        [Required]
        public string Bankname { get; set; }
        [Display(Name = "Account Owner Name:")]
        [Required]
        public string Accountownername { get; set; }
        [Display(Name = "A/C Number:")]
        [Required]
        public string Acnumber { get; set; }
        [Display(Name = "Branch Name:")]
        [Required]
        public string Branchname { get; set; }
        [Display(Name = "IFSC Code:")]
        [Required]
        public string Ifsccode { get; set; }
        [Display(Name = "Cheque Photo:")]
        [Required]
        public string Chequephoto { get; set; }
        [Display(Name = "ATM Photo:")]
        [Required]
        public string Atmphoto { get; set; }
        [Display(Name = "Project Name:")]
        [Required]
        public string Projectname { get; set; }
        [Display(Name = "Account Manager:")]
        public string Handelby { get; set; }
        [Display(Name = "Active:")]
        public bool Isactive { get; set; }
        public string Createdby { get; set; }
        [Display(Name = "Created Date:")]
        public DateTime? Createddate { get; set; }
        [Display(Name = "Updated By:")]
        public string Updatedby { get; set; }
        [Display(Name = "Updated Date:")]
        public DateTime? Updateddate { get; set; }
    }
}
