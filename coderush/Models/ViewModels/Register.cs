using coderush.DataEnum;
using CodesDotHRMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    //view model for register screen
    public class Register
    {

        public int Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public int EmpCode { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DOB { get; set; } 
        public int Salary { get; set; }

        public bool EmailConfirmed { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
       
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public DateTime? JoiningDate { get; set; }

        [Display(Name = "Team Leader Name")]
        public string TeamLeader { get; set; }

        [Required]
        [Display(Name = "Employee Type")]
        public int EmployeeType { get; set; }

        public string PanCardNo { get; set; }

        public string AadharCardNo { get; set; }

        public string BankAccNo { get; set; }

        [Display(Name = "IFS Code")]
        public string IFSCode { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Bank Branch Name")]
        public string BankBranchName { get; set; }
        public bool isWFH { get; set; }

    }
}
