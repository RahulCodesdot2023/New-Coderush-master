using System;
using System.ComponentModel.DataAnnotations;

namespace CodesDotHRMS.Models
{
    public class ProjectActivity
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string UsersId { get; set; }
        public string Comment { get; set; }
        public DateTime? PenaltyDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsPenalty { get; set; }
        public bool IsAchievement { get; set; }
        public int ConfigrationId { get; set; }
    }
}
