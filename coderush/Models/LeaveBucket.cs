using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodesDotHRMS.Models
{
    public class LeaveBucket
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Bucket { get; set; }
    }
}
