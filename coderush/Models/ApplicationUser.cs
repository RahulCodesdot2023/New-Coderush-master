using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        //override identity user, add new column
        public int EmpCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name = "JoiningDate:")]
        public DateTime? JoiningDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DOB { get; set; }
        public int Salary { get; set; }
        [Display(Name = "ProfilePicture:")]
        public string ProfilePicture { get; set; }
        public bool isSuperAdmin { get; set; } = false;

        public decimal PaidLeave { get; set; }
        public decimal UnpaidLeave { get; set; }
        public string Pl { get; set; }
        // public int TotalLeave { get; set; }
        public int EmployeeType { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? EndOfDate { get; set; }

        public byte[] AadharFront { get; set; }
        [Display(Name = "Aadhar Card Backside Photo")]
        public byte[] AadharBack { get; set; }
        [Display(Name = "PAN Card Photo")]
        public byte[] PanCard { get; set; }
        [Display(Name = "Profile Picture ")]
        public byte[] ProfilePic { get; set; }

        public string PanCardNo { get; set; }
        public string AadharCardNo { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCode { get; set; }
        public bool IsWFH { get; set; }
    }
}
