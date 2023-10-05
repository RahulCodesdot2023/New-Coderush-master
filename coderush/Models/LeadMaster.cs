using coderush.DataEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class LeadMaster
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Name:")]
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "* A valid Name is required.")]
        public string Name { get; set; }
        [Display(Name = "Email:")]
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Display(Name = "Phone:")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }
        [Display(Name = "Technologies:")]
        [Required]
        public int Technologies { get; set; }
        [Display(Name = "Lead Type:")]
        public int Leadtype { get; set; }
        [Display(Name = "CVUpload:")]
        public string FileUpload { get; set; }
        [Display(Name = "Lead Date:")]
        [Required]
        public DateTime? Leaddate { get; set; }
        [Display(Name = "Source:")]
        [Required]
        public int Source { get; set; }
        [Display(Name = "Active:")]
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [Display(Name = "Url:")]
        public string Url { get; set; }
        [Display(Name = "Description:")]
        public string Description { get; set; }
        [Display(Name = "Other Connectivity Platform:")]
        [Required]
        public int OtherConnectivityPlatform { get; set; }
        [Display(Name = "Profile Name:")]
        [Required]
        public int ProfileName { get; set; }
        public string LeadCode { get; set; }

        public bool IsLead { get; set; }
        public bool IsClose { get; set; }
        public bool IsProject { get; set; }
        [Display(Name = "Lead Status:")]
        public int LeadStatus { get; set; }

        [Display(Name = "Primary Connectivity Platform:")]
        [Required]
        public int PrimaryConnectivityPlatform { get; set; }
        public string CloseReason { get; set; }
        public int? CustomerId { get; set; }
    }

}