namespace CodesDotHRMS.Models
{
    public class MonthlyTotalHours
    {
        public int Id { get; set; }
        public int EmployeeCode { get; set; }
        public string Month { get; set; }
        public decimal TotalHours { get; set; }
    }
}
