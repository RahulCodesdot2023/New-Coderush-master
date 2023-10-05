using System;

namespace CodesDotHRMS.Models
{
    public class NotificationMaster
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string NotifyAction { get; set; }

        public string NotifyController { get; set; }

        public bool? IsRead { get; set; }
    }
}
