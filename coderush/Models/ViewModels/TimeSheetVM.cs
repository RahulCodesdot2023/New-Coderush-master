using System.Collections.Generic;

namespace CodesDotHRMS.Models.ViewModels
{
    public class TimeSheetVM
    {
        public List<TimeSheet> TimeSheets { get; set; }
        public List<ManualTimeSheet> ManualTimeSheets { get; set; }
    }
}
