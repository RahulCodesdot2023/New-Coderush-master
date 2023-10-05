using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models.ViewModels
{
    public class ApplicationViewModel 
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public int EmpCode { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime? JoiningDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required]
        public DateTime? DOB { get; set; }

        [Required]
        public int? Salary { get; set; }
        [Required]
        public IFormFile ProfilePicture { get; set; }
        [Required]
        public string ProfilePictureString { get; set; }
        [Required]
        public bool isSuperAdmin { get; set; } = false;

        [Required]
        public string TeamLeaderId { get; set; }

        [Required]
        public int EmployeeType { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "phone format is not valid.")]
        public string PhoneNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? EndOfDate { get; set; }

        [Required]
        public string PanCardNo { get; set; }

        [Required]
        public string AadharCardNo { get; set; }

        [Required]
        public string BankAccNo { get; set; }

        [Required]
        [Display(Name = "IFS Code")]
        public string IFSCode { get; set; }

        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "Bank Branch Name")]
        public string BankBranchName { get; set; }
        public bool isWFH { get; set; }


    }
    public class ThoughtViewModel
    {

        public int ThoughttId { get; set; }
        public string ThoughtName { get; set; }
        public string Day { get; set; }
        public string DayName { get; set; }
        public bool Isactive { get; set; }
        

    }
}
