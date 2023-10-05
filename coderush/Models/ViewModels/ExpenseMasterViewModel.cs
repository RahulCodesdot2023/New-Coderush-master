using coderush.DataEnum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models.ViewModels
{
    public class ExpenseMasterViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "* A valid Name is required.")]
        [Display(Name = "Name")]
        public string ExpName { get; set; }
        [Required]
        [Display(Name = "Type")]
        public int Exptype { get; set; }
        public string exptype { get; set; }
        [Display(Name = "Amount:")]
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Amount must be numeric")]
        public long? Amount { get; set; }

        [Required]
        [Display(Name = "Expense Date:")]
        public DateTime? ExpenseDate { get; set; }

        [Display(Name = "Description:")]
        [Required]
        public string Description { get; set; }
        [Display(Name = "Receipt Upload:")]
        public IFormFile FileUpload { get; set; }
        public string filename { get; set; }
        [Display(Name = "isactive:")]
        public bool isactive { get; set; }
        public bool Isdelete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<ExpenseMasterViewModel> List { get; set; }
        public List<CreditMasterViewModel> CreditList { get; set; }
        public long? TotalAmount { get; set; }
    }

    public class CreditMasterViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Amount:")]
        [Required]
        public int? Amount { get; set; }
        public string Managername { get; set; }

        [Display(Name = "Date:")]
        public DateTime? Creditdate { get; set; }
    }
}
