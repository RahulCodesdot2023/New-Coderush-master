using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodesDotHRMS.Models
{
    [Table("Thoughts")]
    public class Thoughts
    {
        [Key]
        public int ThoughtsID { get; set; }
        public string thoughts { get; set; }
        public string Day { get; set; }
        public bool Isactive { get; set; }
        public DateTime createdate { get; set; }
        public DateTime Updatedate { get; set; }

    }
}
