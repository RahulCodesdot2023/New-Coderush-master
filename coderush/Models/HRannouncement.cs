using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodesDotHRMS.Models
{
    [Table("HRannouncement")]
    public class HRannouncement
    {
        [Key]
        public int announcementId { get; set; }
        public string Name { get; set; }
    }
}
