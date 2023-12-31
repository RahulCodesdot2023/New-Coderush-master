﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class LeaveHistory
    {
        [Key]
        public int Id { get; set; }
        public string Userid { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public string Count { get; set; }
        public string EmployeeDescription { get; set; }
        public string HRDescription { get; set; }
        
    
        public string FileUpload { get; set; }
        public bool Isapprove { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string Approveby { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int LeaveReason { get; set; }

        public string LeaveOtherReason { get; set; }
    }
}
