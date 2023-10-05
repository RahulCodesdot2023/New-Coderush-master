using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodesDotHRMS.Models.ViewModels
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string NotifyAction { get; set; }
        public string NotifyController { get; set; }
        public bool? IsRead { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
