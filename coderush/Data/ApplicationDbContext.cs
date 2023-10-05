using coderush.Models;
using CodesDotHRMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace coderush.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //custom entity, override identity user with new column
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        //custom entity, for simple todo app
        public DbSet<Todo> Todo { get; set; }
        public DbSet<DataMaster> Datamaster { get; set; }
        public DbSet<CandidateMaster> CandidateMaster { get; set; }
        public DbSet<ExpenseMaster> ExpenseMaster { get; set; }
        public DbSet<InvoiceMaster> InvoiceMaster { get; set; }
        public DbSet<ProjectMaster> ProjectMaster { get; set; }
        public DbSet<LeadMaster> LeadMaster { get; set; }
        public DbSet<LeaveCount> LeaveCount { get; set; }
        public DbSet<LeaveManagement> LeaveManagement { get; set; }
        public DbSet<RoleDetails> RoleDetails { get; set; }
        public DbSet<LeaveHistory> LeaveHistory { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Credit> Credit { get; set; }
        //public DbSet<Creadit> Creadit { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Hire> Hire { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistory { get; set; }
        public DbSet<HolidayList> HolidayList { get; set; }
        public DbSet<Thoughts> thoughts { get; set; }
        public DbSet<HRannouncement> HRannouncements { get; set; }
        public DbSet<Weekdays> weekdays { get; set; }
        
        //public DbSet<LeaveManagement> LeaveManagement { get; set; }
        public DbSet<LeaveBucket> LeaveBucket { get; set; }
        
        public DbSet<NotificationMaster> NotificationMaster { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<TeamLeader> TeamLeader { get; set; }
        public DbSet<MonthlyReport> MonthlyReport { get; set; }
        public DbSet<MonthlyTotalHours> MonthlyTotalHours { get; set; }
        public DbSet<TimeSheet> TimeSheet { get; set; }
        public DbSet<ProjectActivity> ProjectActivity { get; set; }
        public DbSet<FinanceHistory> FinanceHistory { get; set; }
        public DbSet<FinanceMaster> FinanceMaster { get; set; }
        public DbSet<ManualTimeSheet> ManualTimeSheet { get; set; }

    }
}
