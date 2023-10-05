using System;

namespace CodesDotHRMS.Models
{
    public class FinanceHistory
    {
        public int Id { get; set; }
        public int FinanceID { get; set; }
        public string Bankname { get; set; }
        public string Accountownername { get; set; }
        public string Acnumber { get; set; }
        public string Branchname { get; set; }
        public string Ifsccode { get; set; }
        public string Chequephoto { get; set; }
        public string Atmphoto { get; set; }
        public string Projectname { get; set; }
        public string Handelby { get; set; }
        public bool Isactive { get; set; }
        public string Createdby { get; set; }
        public DateTime? Createddate { get; set; }
        public string Updatedby { get; set; }
        public DateTime? Updateddate { get; set; }
    }
}
