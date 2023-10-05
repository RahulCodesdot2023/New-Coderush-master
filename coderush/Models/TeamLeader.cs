namespace CodesDotHRMS.Models
{
    public class TeamLeader
    {
        public int Id { get; set; }
        public string TeamLeaderId { get; set; }
        public string TeamLeaderEmail { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeEmail { get; set; }
    }
}
