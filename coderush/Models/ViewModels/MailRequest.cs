using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CodesDotHRMS.Models.ViewModels
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
