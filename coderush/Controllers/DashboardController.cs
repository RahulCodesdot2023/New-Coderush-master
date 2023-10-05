using coderush.Data;
using coderush.DataEnum;
using coderush.Hubs;
using coderush.Models;
using coderush.Models.ViewModels;
using coderush.ViewModels;
using CodesDotHRMS.Models;
using CodesDotHRMS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
//using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Controllers
{
    [Authorize(Roles = "HR,SuperAdmin,Employee")]
    public class DashboardController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHubContext<BirthdayNotificationHub> _DobNotificationHubContext;
        public static string userid;
        public static string username;
        private readonly IConfiguration _config;

        public DashboardController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IHubContext<BirthdayNotificationHub> ACNotificationHubContext, IConfiguration config)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _DobNotificationHubContext = ACNotificationHubContext;
            _config = config;
            //_hostingEnvironment = hostingEnvironment;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        public IActionResult DashboardIndex()
        {

            var TODAYDATE = DateTime.Now;
            var after15day = DateTime.Now.AddDays(+15);

            var data = new List<HolidayListViewModel>();

            data = (from h in _context.HolidayList.OrderByDescending(x => x.Id)
                    where !h.Isdelete && h.Date <= after15day && h.Date >= TODAYDATE

                    select new HolidayListViewModel
                    {
                        Id = h.Id,
                        Name = h.Name,
                        Day = h.Day,
                        Date = h.Date,

                    }).ToList();

            var user = _userManager.GetUserAsync(User).Result;

            ViewBag.PaidLeave = user.PaidLeave;
            ViewBag.UnPaidLeave = user.UnpaidLeave;
            var isadmin = User.IsInRole("SuperAdmin");
            var projeid = _context.ProjectMaster.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();

            var connectionstring = _config.GetValue<string>("ConnectionStrings:DefaultConnection").ToString();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@isadmin", isadmin);
            parameters.Add("@userId", user.Id);

            var MasterListDataSet = DbAccess.ExecuteStoredProc("usp_GetProjectList", parameters, connectionstring);

            var _List = new List<ProjectMasterViewModel>();
            if (MasterListDataSet != null)
            {
                _List = DbAccess.ConvertDataTableToList<ProjectMasterViewModel>(MasterListDataSet.Tables[0]);
            }

            DashBoardViewModel modeldata = new DashBoardViewModel();
            var todayDate = new DateTime();
            todayDate = DateTime.Now.Date;

            if (data != null)
                modeldata.HolidayList = data;

            modeldata.projectMaster = _List;
            modeldata.ApprovedData = _context.LeaveCount
                                       .Where(w => w.Isapprove == true && w.Userid == user.Id/* && w.Todate > todayDate*/)
                                       .OrderByDescending(x => x.Id)
                                       .Select(s => new LeaveCountViewModel()
                                       {
                                           Id = s.Id,
                                           FullName = _userManager.Users.Where(x => x.Id == s.Userid).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),//s.Userid, 
                                                                                                                                                                   //Userid = user.UserName,
                                           Fromdate = s.Fromdate.HasValue ? s.Fromdate : null,
                                           Todate = s.Todate.HasValue ? s.Todate : null,
                                           Count = s.Count != null ? s.Count : string.Empty,
                                           EmployeeDescription = s.EmployeeDescription != null ? s.EmployeeDescription : string.Empty,
                                           HrDescription = s.HrDescription != null ? s.HrDescription : string.Empty,
                                       }).ToList();

            modeldata.RejectedData = _context.LeaveCount
                                       .Where(w => w.IsReject == true && w.Userid == user.Id /*&& w.Todate > todayDate*/)
                                       .OrderByDescending(x => x.Id)
                                       .Select(s => new LeaveCountViewModel()
                                       {
                                           Id = s.Id,
                                           FullName = _userManager.Users.Where(x => x.Id == s.Userid).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),//s.Userid, 
                                                                                                                                                                   //Userid = user.UserName,
                                           Fromdate = s.Fromdate.HasValue ? s.Fromdate : null,
                                           Todate = s.Todate.HasValue ? s.Todate : null,
                                           Count = s.Count != null ? s.Count : string.Empty,
                                           EmployeeDescription = s.EmployeeDescription != null ? s.EmployeeDescription : string.Empty,
                                           HrDescription = s.HrDescription != null ? s.HrDescription : string.Empty,
                                       }).ToList();


            return View(modeldata);
        }

        [HttpGet]
        public async Task<IActionResult> DashboardBindGridData(string id, string UserName)
        {
            userid = id;
            username = UserName;

            var user = _userManager.GetUserAsync(User).Result;
            var adminrole = await _userManager.IsInRoleAsync(user, "HR");
            var suadminrole = await _userManager.IsInRoleAsync(user, "SuperAdmin");

            LeaveCountViewModel levcunt = new LeaveCountViewModel();
            var leavecount = new List<LeaveCountViewModel>();
            var todayDate = DateTime.Now;
            if (!adminrole)
            {
                leavecount = _context.LeaveCount
                                   //.Where(w => w.Userid == user.Id)
                                   .Select(s => new LeaveCountViewModel()
                                   {
                                       Id = s.Id,
                                       Userid = _userManager.Users.Where(x => x.Id == s.Userid).Select(x => x.UserName).FirstOrDefault(),//s.Userid, 
                                       //Userid = user.UserName,
                                       Fromdate = s.Fromdate.HasValue ? s.Fromdate : null,
                                       Todate = s.Todate.HasValue ? s.Todate : null,
                                       Filename = s.FileUpload ?? "-",
                                       Count = s.Count != null ? s.Count : "0",
                                       EmployeeDescription = s.EmployeeDescription != null ? s.EmployeeDescription : "-",
                                       HrDescription = s.HrDescription != null ? s.HrDescription : "-",
                                       Isapprove = s.Isapprove,
                                       ApproveDate = s.ApproveDate,
                                       Approveby = s.Approveby,
                                       AdminRole = adminrole,
                                       isedit = s.Todate != null ? (s.Todate <= todayDate) ? true : false : false,
                                       colouris = s.Todate != null ? s.Todate > todayDate ? "#ffe0bb" : "" : "",
                                       LeaveReason = s.LeaveReason
                                   }).ToList();
                var leavecount1 = leavecount.Where(x => x.colouris == "").ToList();
                leavecount = leavecount.OrderByDescending(x => x.Todate).Where(x => x.Todate > todayDate).OrderBy(x => x.Todate).ToList();
                foreach (var data in leavecount1)
                {
                    LeaveCountViewModel ab = new LeaveCountViewModel();
                    ab.Id = data.Id;
                    //ab.Userid = _userManager.Users.Where(x => x.Id == data.Userid).Select(x => x.UserName).FirstOrDefault();//s.Userid, 
                    ab.Userid = data.Userid;
                    ab.Fromdate = data.Fromdate;
                    ab.Todate = data.Todate;
                    ab.Filename = data.Filename;
                    ab.Count = data.Count;
                    ab.EmployeeDescription = data.EmployeeDescription != null ? data.EmployeeDescription : "-";
                    ab.HrDescription = data.HrDescription != null ? data.HrDescription : "-";
                    ab.Isapprove = data.Isapprove;
                    ab.ApproveDate = data.ApproveDate;
                    ab.Approveby = data.Approveby;
                    ab.AdminRole = adminrole;
                    ab.isedit = data.Todate != null ? (data.Todate <= todayDate) ? true : false : false;
                    ab.LeaveReason = data.LeaveReason;
                    leavecount.Add(ab);
                }

            }
            else if (adminrole || suadminrole)
            {
                try
                {
                    leavecount = _context.LeaveCount
                                      //.Where(w => w.Userid == id)
                                      .Select(s => new LeaveCountViewModel()
                                      {
                                          Id = s.Id,
                                          /*Userid = _userManager.Users.Where(x => x.Id == UserName).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),//s.Userid, */
                                          Userid = _userManager.Users.Where(x => x.Id == s.Userid).Select(x => x.UserName).FirstOrDefault(),//s.Userid, 
                                          //Userid = UserName,
                                          Fromdate = s.Fromdate.HasValue ? s.Fromdate : null,
                                          Todate = s.Todate.HasValue ? s.Todate : null,
                                          Filename = s.FileUpload != null ? s.FileUpload : "-",
                                          Count = s.Count != null ? s.Count : "0",
                                          EmployeeDescription = s.EmployeeDescription != null ? s.EmployeeDescription : "-",
                                          HrDescription = s.HrDescription != null ? s.HrDescription : "-",
                                          Isapprove = s.Isapprove,
                                          ApproveDate = s.ApproveDate,
                                          Approveby = s.Approveby,
                                          AdminRole = adminrole,
                                          isedit = s.Todate != null ? (s.Todate <= todayDate) ? true : false : false,
                                          colouris = s.Todate != null ? s.Todate > todayDate ? "#ffe0bb" : "" : "",
                                          LeaveReason = s.LeaveReason
                                      }).ToList();
                    //leavecount = leavecount.OrderByDescending(x => x.Todate).ToList();
                    //leavecount = leavecount.OrderByDescending(x => x.Todate).Where(x => x.Todate > todayDate).OrderBy(x=>x.Todate).ToList();
                    var leavecount1 = leavecount.Where(x => x.colouris == "").ToList();
                    leavecount = leavecount.OrderByDescending(x => x.Todate).Where(x => x.Todate > todayDate).OrderBy(x => x.Todate).ToList();
                    foreach (var data in leavecount1)
                    {
                        LeaveCountViewModel ab = new LeaveCountViewModel();
                        ab.Id = data.Id;
                        //ab.Userid = _userManager.Users.Where(x => x.Id == data.Userid).Select(x => x.UserName).FirstOrDefault();//s.Userid, 
                        ab.Userid = data.Userid;
                        ab.Fromdate = data.Fromdate;
                        ab.Todate = data.Todate;
                        ab.Filename = data.Filename;
                        ab.Count = data.Count ?? "0";
                        ab.EmployeeDescription = data.EmployeeDescription != null ? data.EmployeeDescription : "-";
                        ab.HrDescription = data.HrDescription != null ? data.HrDescription : "-";
                        ab.Isapprove = data.Isapprove;
                        ab.ApproveDate = data.ApproveDate;
                        ab.Approveby = data.Approveby;
                        ab.AdminRole = adminrole;
                        ab.isedit = data.Todate != null ? (data.Todate <= todayDate) ? true : false : false;
                        ab.LeaveReason = data.LeaveReason;
                        leavecount.Add(ab);

                    }


                }
                catch (Exception ew)
                {
                    throw ew;
                }
            }
            levcunt.List = leavecount;
            //return RedirectToAction(nameof(LeaveIndex), new { id = leaveCounts.Id > 0 ? leaveCounts.Id : 0 });
            return Json(levcunt);
        }


        public async Task<IActionResult> GetReminderDobNotiy(DateTime datetime)
        {
            try
            {
                var loginUser = await GetCurrentUserAsync();
                var user = _userManager.GetUserAsync(User).Result;
                var adminrole = await _userManager.IsInRoleAsync(user, "HR");
                var suadminrole = await _userManager.IsInRoleAsync(user, "SuperAdmin");

                if (adminrole || suadminrole)
                {
                    var result = _context.ApplicationUser.Where(x => x.DOB.Value.Date == datetime.Date || x.DOB.Value.Date == datetime.AddDays(1).Date).ToList();
                    if (result != null && result.Count > 0)
                    {
                        await _DobNotificationHubContext.Clients.All.SendAsync("ReceivedDOBNotification", loginUser.Id, result);
                    }
                    return Json(result);
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Your Role is not hr and superadmin"
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        public async Task<IActionResult> Home()
        {
            var data = new List<ApplicationViewModel>();

            data = (from h in _context.ApplicationUser
                    where h.DOB.Value.Day == DateTime.Now.Day && h.DOB.Value.Month == DateTime.Now.Month
                    select new ApplicationViewModel
                    {
                        Id = h.Id,
                        FirstName = h.FirstName,
                        DOB = h.DOB,
                        UserName = h.UserName,
                    }).ToList();
            return View(data);
        }

        public ActionResult GetthougthByday()
        {
            try
            {
                var DayName = DateTime.Now.ToString("ddd");
                var thoughts = _context.weekdays.Where(x => x.isactive && x.dayvalue.Contains(DayName)).FirstOrDefault();
                var data = new thoughtsviewmodel();
                if (thoughts != null)
                {
                    var thoughtdata = _context.thoughts.Where(x => x.Day == thoughts.dayvalue && x.Isactive).FirstOrDefault();
                    if (thoughtdata != null)
                    {
                        data.thoughts = thoughtdata.thoughts;
                        data.dayvalue = thoughts.dayname;
                    }
                }


                return Json(new { data = data });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult GetBirthdayListByMonth()
        {
            try
            {
                var currentMonth = DateTime.Now.Month;
                var birthdaysInMonth = _context.ApplicationUser
                    .Where(x => x.DOB.HasValue && x.DOB.Value.Month == currentMonth)
                    .Select(x => new BirthdayViewModel
                    {
                        Name = x.FirstName + " " + x.LastName, // Replace with the actual property name for the user's name
                        DOB = x.DOB.Value // Assuming DOB is of type DateTime
                    }).ToList();

                return Json(new { data = birthdaysInMonth });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // You can also handle it differently based on your requirements
                throw ex;
            }
        }

        public async Task<IActionResult> leave()
        {
            try
            {
                var TODAYDATE = DateTime.Now;

                //var tod = Convert.ToDateTime(todate);

                var data = new List<LeaveCountViewModel>();
                //var data = _context.LeaveCount.Where(x => x.Fromdate == x.Todate).OrderByDescending(o => o.Id).ToList();
                data = (from h in _context.LeaveCount
                        where h.Fromdate <= TODAYDATE && h.Todate >= TODAYDATE
                        let username = _context.ApplicationUser.Where(x => x.Id == h.Userid).Select(x => x.UserName).FirstOrDefault()
                        select new LeaveCountViewModel
                        {
                            UserName = username != null || username != "" ? username : "",
                            Fromdate = h.Fromdate,
                            Todate = h.Todate,
                        }).ToList();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IActionResult> Hrannocuncement()
        {
            try
            {
                var data = _context.Datamaster.Where(x => !x.Isdeleted && x.Type == DataSelection.HRAnnocuncement && x.Isactive).OrderByDescending(x => x.CreatedDate).ToList();

                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IActionResult> HolidayList(HolidayListViewModel holiday)
        {
            try
            {
                var TODAYDATE = DateTime.Now;
                var exitdata = _context.HolidayList.Where(x => x.Date == holiday.Date).FirstOrDefault();

                if (exitdata != null)
                {
                    var After15day = DateTime.Now.AddDays(+5);
                    var data = new List<HolidayListViewModel>();

                    data = (from h in _context.HolidayList.OrderByDescending(x => x.Id)
                            where h.Date <= After15day && h.Date >= TODAYDATE

                            select new HolidayListViewModel
                            {
                                //Userid =h.Userid,
                                Name = h.Name,
                                Day = h.Day,
                                Date = h.Date,


                            }).ToList();
                    return Json(new { data = data });
                }
                else
                {
                    var data = new List<HolidayListViewModel>();
                    data = (from h in _context.HolidayList.OrderByDescending(x => x.Id)
                            where !h.Isdelete
                            select new HolidayListViewModel
                            {

                                Name = h.Name,
                                Day = h.Day,
                                Date = h.Date,


                            }).ToList();

                    return Json(new { data = data });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IActionResult Managethoughts()
        {
            try
            {
                var mangthoughts = _context.thoughts.OrderByDescending(x => x.Isactive).ToList();

                return View(mangthoughts);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public IActionResult createnewthoughts()
        {
            try
            {
                var data = new Thoughts();
                return View(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public IActionResult addathoughts(Thoughts tho, string Day)
        {

            try
            {
                var getthoughtsbyday = _context.thoughts.Where(z => z.Day == Day).ToList();
                foreach (var item in getthoughtsbyday)
                {
                    item.Isactive = false;

                }
                tho.Isactive = true;

                _context.thoughts.Add(tho);
                _context.SaveChanges();


                return Json(tho);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IActionResult Getweekday()
        {
            try
            {
                var weeklist = _context.weekdays.OrderByDescending(a => a.isactive).ToList();  //where
                return Json(weeklist);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpGet]
        public IActionResult GetThoughtList(string MyInput, string ddlWeekday)
        {

            try
            {
                if (ddlWeekday == "null")
                    ddlWeekday = null;
                var ThoughtList = (from t in _context.thoughts
                                   join d in _context.weekdays on t.Day equals d.dayvalue
                                   where (MyInput == null || (MyInput != null && t.thoughts.Contains(MyInput)))
                                   && (ddlWeekday == null || (ddlWeekday != null && t.Day == ddlWeekday))
                                   orderby t.Isactive descending
                                   select new ThoughtViewModel
                                   {
                                       Day = t.Day,
                                       ThoughtName = t.thoughts,
                                       DayName = d.dayname,
                                       Isactive = t.Isactive,
                                       ThoughttId = t.ThoughtsID,


                                   }).ToList();
                return Json(new { data = ThoughtList });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public IActionResult checkboxcheck(int ThoughtsID, string Day)
        {
            try
            {
                var thoughts = _context.thoughts.Where(x => x.ThoughtsID == ThoughtsID).FirstOrDefault();
                var getthoughtsbyday = _context.thoughts.Where(z => z.Day == thoughts.Day && z.Isactive).ToList();

                foreach (var item in getthoughtsbyday)
                {
                    item.Isactive = false;
                }
                thoughts.Isactive = true;



                //_context.thoughts.Update(thoughts);
                _context.SaveChanges();


                return Json(thoughts);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IActionResult getNotification()
        {
            var result = _context.NotificationMaster.Where(x => x.IsRead == null).OrderByDescending(x => x.Id).ToList();
            return View("~/Views/Shared/Notification.cshtml", result);
        }

        public IActionResult ReadNotification(int id)
        {
            var getNotifyDT = _context.NotificationMaster.Where(x => x.Id == id).FirstOrDefault();
            getNotifyDT.IsRead = true;
            _context.NotificationMaster.Update(getNotifyDT);
            _context.SaveChanges();
            return RedirectToAction(getNotifyDT.NotifyAction, getNotifyDT.NotifyController, new { userId = getNotifyDT.UserId });
        }

        public IActionResult getNotificationList()
        {
            var response = from n in _context.NotificationMaster
                           join u in _context.ApplicationUser on n.UserId equals u.Id
                           orderby n.Id descending
                           select new NotificationViewModel
                           {
                               Id = n.Id,
                               UserId = n.UserId,
                               FullName = u.FirstName + " " + u.LastName,
                               Title = n.Title,
                               CreatedDate = n.CreatedDate,
                               NotifyAction = n.NotifyAction,
                               NotifyController = n.NotifyController,
                               IsRead = n.IsRead
                           };
            return View(response.ToList());
        }

        [HttpGet]
        public IActionResult DeleteNotification(int id)
        {
            try
            {
                var deletenofication = _context.NotificationMaster.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (deletenofication == null)
                {
                    return NotFound();
                }
                _context.NotificationMaster.Remove(deletenofication);
                _context.SaveChanges();
                return Json(new { issuccess = true, message = "Notification deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = "Something went wrong." });
            }
        }

    }
}


