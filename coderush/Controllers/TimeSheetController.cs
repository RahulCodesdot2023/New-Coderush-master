using coderush.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using coderush.Controllers;
using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using CodesDotHRMS.Models;
using CodesDotHRMS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using coderush.Services.Database;
using MimeKit;
using System.Net;

namespace CodesDotHRMS.Controllers
{
    public class TimeSheetController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //private readonly IHostEnvironment _hostingEnvironment;
        public TimeSheetController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context,
           IWebHostEnvironment webHostEnvironment)
        //IHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            //_hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var userList = (from userData in _context.ApplicationUser
                            join ur in _context.UserRoles on userData.Id equals ur.UserId
                            join role in _context.Roles on ur.RoleId equals role.Id
                            where role.Name == "Employee"
                            where userData.IsWFH == true
                            select new UserDropdownListViewModel
                            {
                                Userid = userData.Id,
                                FullName = userData.FirstName + " " + userData.LastName
                            }).ToList();
            ViewBag.UserList = new SelectList(userList, "Userid", "FullName");

            var alluserList = (from userData in _context.ApplicationUser
                               join ur in _context.UserRoles on userData.Id equals ur.UserId
                               join role in _context.Roles on ur.RoleId equals role.Id
                               where role.Name != "SuperAdmin"
                               select new UserDropdownListViewModel
                               {
                                   Userid = userData.Id,
                                   FullName = userData.FirstName + " " + userData.LastName
                               }).ToList();

            ViewBag.AllUserList = new SelectList(alluserList, "Userid", "FullName");

            var user = _userManager.GetUserAsync(User).Result;
            var adminrole = await _userManager.IsInRoleAsync(user, "HR");
            var suadminrole = await _userManager.IsInRoleAsync(user, "SuperAdmin");

            ViewBag.IsWFHResult = user.IsWFH;

            ViewBag.EmployeeName = user.FirstName + " " + user.LastName;
            ViewBag.EmployeeId = user.Id;
            if (suadminrole || adminrole)
            {
                ViewBag.IsSUperAdmin = true;
            }
            else
            {
                ViewBag.IsSUperAdmin = false;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BindGridData(string id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var adminrole = await _userManager.IsInRoleAsync(user, "HR");
            var suadminrole = await _userManager.IsInRoleAsync(user, "SuperAdmin");
            var teamAdminrole = await _userManager.IsInRoleAsync(user, "TeamAdmin");

            var TimeSheetCount = new List<TimeSheet>();
            var ManualSheetCount = new List<ManualTimeSheet>();
            var mainList = new List<TimeSheet>();
            var todayDate = DateTime.Now;

            if (!adminrole && !suadminrole && !teamAdminrole)
            {
                try
                {

                    TimeSheetCount = _context.TimeSheet
                                        .Where(w => w.EmployeeId == user.Id)
                                       .Select(s => new TimeSheet()

                                       {
                                           Id = s.Id,
                                           EmployeeId = _userManager.Users.Where(x => x.Id == s.EmployeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),//s.Userid, 
                                           FromDate = s.FromDate,
                                           ToDate = s.ToDate,
                                           Description = s.Description != null ? s.Description : "-",
                                       }).ToList();

                    ManualSheetCount = _context.ManualTimeSheet.Where(x => x.EmployeeId == user.Id).Select(s => new ManualTimeSheet()
                    {
                        Id = s.Id,
                        EmployeeId = _userManager.Users.Where(x => x.Id == s.EmployeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),//s.Userid, 
                        ManualDate = s.ManualDate,
                        FromTime = s.FromTime,
                        ToTime = s.ToTime,
                        Description = s.Description != null ? s.Description : "-",
                        IsApprove = s.IsApprove
                    }).ToList();

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else if (adminrole || suadminrole)
            {
                try
                {
                    TimeSheetCount = _context.TimeSheet
                                      .Select(s => new TimeSheet()
                                      {
                                          Id = s.Id,
                                          EmployeeId = _userManager.Users.Where(x => x.Id == s.EmployeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),//s.Userid, 
                                          FromDate = s.FromDate,
                                          ToDate = s.ToDate,
                                          Description = s.Description != null ? s.Description : "-",
                                      }).ToList();

                    ManualSheetCount = _context.ManualTimeSheet.Select(s => new ManualTimeSheet()
                    {
                        Id = s.Id,
                        EmployeeId = _userManager.Users.Where(x => x.Id == s.EmployeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),//s.Userid, 
                        ManualDate = s.ManualDate,
                        FromTime = s.FromTime,
                        ToTime = s.ToTime,
                        Description = s.Description != null ? s.Description : "-",
                        IsApprove = s.IsApprove
                    }).ToList();
                }
                catch (Exception ew)
                {
                    throw ew;
                }
            }
            else if (teamAdminrole)
            {
                var teamUsers = _context.TeamLeader.Where(x => x.TeamLeaderId == user.Id).Select(x => x.EmployeeId).ToList();
                teamUsers.Add(user.Id);

                TimeSheetCount = _context.TimeSheet
                                      .Where(x => teamUsers.Contains(x.EmployeeId))
                                      .Select(s => new TimeSheet()
                                      {
                                          Id = s.Id,
                                          EmployeeId = _userManager.Users.Where(x => x.Id == s.EmployeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),//s.Userid, 
                                          FromDate = s.FromDate,
                                          ToDate = s.ToDate,
                                          Description = s.Description != null ? s.Description : "-",
                                      }).ToList();

                ManualSheetCount = _context.ManualTimeSheet
                    .Where(x => teamUsers.Contains(x.EmployeeId))
                    .Select(s => new ManualTimeSheet()
                    {
                        Id = s.Id,
                        EmployeeId = _userManager.Users.Where(x => x.Id == s.EmployeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),//s.Userid, 
                        ManualDate = s.ManualDate,
                        FromTime = s.FromTime,
                        ToTime = s.ToTime,
                        Description = s.Description != null ? s.Description : "-",
                        IsApprove = s.IsApprove
                    }).ToList();
            }
            
            TimeSheetVM timeSheetVM = new TimeSheetVM();
            timeSheetVM.TimeSheets = TimeSheetCount.OrderByDescending(x => x.Id).ToList();
            timeSheetVM.ManualTimeSheets = ManualSheetCount.OrderByDescending(x => x.Id).ToList();
            return Json(timeSheetVM);
        }

        [HttpPost]
        public IActionResult AddTimeSheet(TimeSheet timesheet)
        {
            try
            {
                TimeSheet newTimeSheet = new TimeSheet
                {
                    Id = 0,
                    EmployeeId = timesheet.EmployeeId,
                    FromDate = timesheet.FromDate,
                    ToDate = timesheet.ToDate,
                    Description = timesheet.Description

                };
                _context.TimeSheet.Add(newTimeSheet);
                _context.SaveChanges();

                MonyhlyReportCalculation(newTimeSheet);
                return Json(new { issuccess = true, message = "TimeSheet Record added successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = "Something went wrong." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddManualTimeSheet(ManualTimeSheet timesheet, bool Isapprove)
        {
            var email = new MailRequest();
            ManualTimeSheet newTimeSheet = new ManualTimeSheet();
            try
            {
                if (timesheet != null && timesheet.Id > 0)
                {
                    newTimeSheet = _context.ManualTimeSheet.Where(x => x.Id == timesheet.Id).FirstOrDefault();
                    newTimeSheet.ToTime = timesheet.ToTime;
                    newTimeSheet.FromTime = timesheet.FromTime;
                    _context.ManualTimeSheet.Update(newTimeSheet);
                }
                else
                {
                    newTimeSheet = new ManualTimeSheet
                    {
                        Id = 0,
                        EmployeeId = timesheet.EmployeeId,
                        ManualDate = timesheet.ManualDate,
                        FromTime = timesheet.FromTime,
                        ToTime = timesheet.ToTime,
                        Description = timesheet.Description

                    };
                    _context.ManualTimeSheet.Add(newTimeSheet);
                }
                _context.SaveChanges();

                // Mail Code
                var tlID = _context.TeamLeader.Where(x => x.EmployeeId == newTimeSheet.EmployeeId).FirstOrDefault();

                var teamLeaderDetails = _context.ApplicationUser.Where(x => x.Id == tlID.TeamLeaderId).FirstOrDefault();
                var employeeDetails = _context.ApplicationUser.Where(x => x.Id == tlID.EmployeeId).FirstOrDefault();

                var tlName = teamLeaderDetails.FirstName + " " + teamLeaderDetails.LastName;
                var EmpName = employeeDetails.FirstName + " " + employeeDetails.LastName;
                var hr = _userManager.GetUsersInRoleAsync("HR").Result.FirstOrDefault();
                var hrname = hr.FirstName + " " + hr.LastName;
                var user = _userManager.GetUserAsync(User).Result;
                var adminrole = await _userManager.IsInRoleAsync(user, "HR");
                var emprole = await _userManager.IsInRoleAsync(user, "EmpName");
                var suadminrole = await _userManager.IsInRoleAsync(user, "SuperAdmin");
                var teamAdmin = await _userManager.IsInRoleAsync(user, "TeamAdmin");

                if (suadminrole == true || adminrole == true || teamAdmin == true || emprole == true)
                {
                    Isapprove = true;
                }

                if (Isapprove)
                {
                    var result = ManualTimesheetCalculation(newTimeSheet);
                    newTimeSheet.IsApprove = true;
                    _context.ManualTimeSheet.Update(newTimeSheet);
                    _context.SaveChanges();
                    email.ToEmail = employeeDetails.Email;
                    email.Subject = "Manual Attendance completed: " + EmpName;
                    email.Body = "Dear " + EmpName + "\r\n\r\nI am writing this to inform you that your requst for add Manual attendance Is Accepted\r\n\r\nThank You\r\n\r\n" + tlName;
                    SendMail(email);
                }
                else
                {
                    email.ToEmail = employeeDetails.Email;
                    email.Subject = "Manual Attendance added.";
                    email.Body = "Dear " + EmpName + "\r\n\r\nI am writing this to inform you that your requst for add Manual attendance is added\r\n\r\nPlease Feel Free to Contact us regarding any issues\r\n\r\nThank You\r\n\r\n" + tlName;
                    SendMail(email);
                }
                return Json(new { issuccess = true, message = "Manual timesheet save successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = "Something went wrong." });
            }
        }

        public IActionResult AcceptOrRejectManualTime(bool status, int[] ids)
        {
            var email = new MailRequest();
            var updateStatus = new ManualTimeSheet();
            var Message = string.Empty;
            foreach (var id in ids)
            {
                updateStatus = _context.ManualTimeSheet.Where(x => x.Id == id).FirstOrDefault();
                if (status == true)
                {
                    var result = ManualTimesheetCalculation(updateStatus);
                    if (result == true)
                    {
                        Message = "Approved";
                    }
                    else
                    {
                        Message = "Invalid.";
                    }
                    updateStatus.IsApprove = status;
                    _context.ManualTimeSheet.Update(updateStatus);
                }
                else
                {
                    Message = "Rejected";
                    updateStatus.IsApprove = status;
                    _context.ManualTimeSheet.Update(updateStatus);
                }
                _context.SaveChanges();

                // Mail Code
                var users = _context.ApplicationUser.Where(x => x.Id == updateStatus.EmployeeId).FirstOrDefault();
                var userName = users.FirstName + " " + users.LastName;
                var hr = _userManager.GetUsersInRoleAsync("HR").Result.FirstOrDefault();
                var hrname = hr.FirstName + " " + hr.LastName;
                var user = _userManager.GetUserAsync(User).Result;


                if (status==true)
                {
                    email.ToEmail = users.Email;
                    email.Subject = "Manual Attendance Request Accepted by";
                    email.Body = "Dear " + userName + "\r\n\r\nI am writing this to inform you that your requst for add Manual attendance request is Accepted\r\n\r\nThank You\r\n\r\n" + user.FirstName+" "+user.LastName;
                    SendMail(email);
                }
                else
                {
                    email.ToEmail = users.Email;
                    email.Subject = "Manual Attendance Request Rejected";
                    email.Body = "Dear " + userName + "\r\n\r\nI am writing this to inform you that your requst for add Manual attendance request is not Accepted.\r\n\r\nPlease Feel Free to Contact us regarding any issues\r\n\r\nThank You\r\n\r\n" + hrname;
                    SendMail(email);
                }
            }
            return Json($"Your request {Message}.");
        }

        [HttpGet]
        public IActionResult GetTimeSheet(int id)
        {
            try
            {
                var data = (from ts in _context.TimeSheet
                            join usr in _context.ApplicationUser on ts.EmployeeId equals usr.Id
                            where (ts.Id == id)
                            select new
                            {
                                ts.Id,
                                ts.FromDate,
                                ts.ToDate,
                                ts.EmployeeId,
                                ts.Description,
                                usr.FirstName,
                                usr.LastName
                            }).FirstOrDefault();

                return Json(new { issuccess = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = "Something went wrong." });
            }
        }

        [HttpPost]
        public IActionResult UpdateTimeSheet(TimeSheet timesheet)
        {
            try
            {
                var data = _context.TimeSheet.Where(x => x.Id == timesheet.Id).FirstOrDefault();

                data.EmployeeId = timesheet.EmployeeId;
                data.FromDate = timesheet.FromDate;
                data.ToDate = timesheet.ToDate;
                data.Description = timesheet.Description;
                _context.TimeSheet.Update(data);
                _context.SaveChanges();

                MonyhlyReportCalculation(data);

                return Json(new { issuccess = true, message = "Timesheet Record updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = "Something went wrong." });
            }
        }

        public object MonyhlyReportCalculation(TimeSheet timesheet)
        {
            DateTime startdate = timesheet.FromDate;
            DateTime enddate = timesheet.ToDate;

            var empcode = _context.ApplicationUser.Where(x => x.Id == timesheet.EmployeeId).FirstOrDefault().EmpCode;
            var month = timesheet.FromDate.Month;
            var year = timesheet.FromDate.Year;
            var monthandyear = month + "/" + year;

            var holidayLists = _context.HolidayList.Where(x => x.Date >= startdate && x.Date <= enddate).Select(x => x.Date).ToList();

            List<int> numbers = new List<int>();
            for (DateTime date = startdate; date.Date <= enddate; date = date.AddDays(1))
            {
                if ((date.DayOfWeek != DayOfWeek.Saturday) && (date.DayOfWeek != DayOfWeek.Sunday))
                {
                    numbers.Add(date.Day);
                }
            }

            if (holidayLists.Count > 0)
            {
                foreach (var holiday in holidayLists)
                {
                    numbers.Remove(holiday.Value.Day);
                }
            }

            decimal intime = 10.00M;
            decimal outtime = 18.00M;
            var ExistMonthlyReport = new MonthlyReport();
            ExistMonthlyReport.EmployeeCode = empcode;
            ExistMonthlyReport.Month = monthandyear;

            foreach (int number in numbers)
            {
                var adTimeList = _context.MonthlyReport
                    .Where(x => x.EmployeeCode == empcode && x.Month == monthandyear && (x.DataType == "ArrTim" || x.DataType == "DepTim")).ToList();

                if (adTimeList.Count == 0)
                {
                    ExistMonthlyReport = new MonthlyReport()
                    {
                        Id = 0,
                        DataType = "ArrTim",
                        EmployeeCode = empcode,
                        Month = monthandyear
                    };
                    _context.MonthlyReport.Add(ManageMonthlyReportData(number, intime, ExistMonthlyReport));
                    _context.SaveChanges();

                    ExistMonthlyReport = new MonthlyReport()
                    {
                        Id = 0,
                        DataType = "DepTim",
                        EmployeeCode = empcode,
                        Month = monthandyear
                    };
                    _context.MonthlyReport.Add(ManageMonthlyReportData(number, outtime, ExistMonthlyReport));
                    _context.SaveChanges();
                }
                else
                {
                    foreach (var ADValue in adTimeList)
                    {
                        ExistMonthlyReport = _context.MonthlyReport.Where(x => x.Id == ADValue.Id).FirstOrDefault();
                        if (ADValue.DataType == "ArrTim")
                        {
                            _context.MonthlyReport.Update(ManageMonthlyReportData(number, intime, ExistMonthlyReport));
                        }
                        else if (ADValue.DataType == "DepTim")
                        {
                            _context.MonthlyReport.Update(ManageMonthlyReportData(number, outtime, ExistMonthlyReport));
                        }
                        _context.SaveChanges();
                    }
                }
            }

            var arrival = _context.MonthlyReport.Where(x => x.EmployeeCode == empcode && x.Month == monthandyear && x.DataType == "ArrTim").FirstOrDefault();
            var departure = _context.MonthlyReport.Where(x => x.EmployeeCode == empcode && x.Month == monthandyear && x.DataType == "DepTim").FirstOrDefault();
            var hours = Convert.ToDecimal(CalculateTotalHours(arrival, departure));

            var totalhours = new MonthlyTotalHours();
            var existhours = _context.MonthlyTotalHours.Where(x => x.EmployeeCode == empcode && x.Month == monthandyear).FirstOrDefault();
            if (existhours != null)
            {
                existhours.TotalHours = hours;
                _context.MonthlyTotalHours.Update(existhours);
            }
            else
            {
                totalhours.TotalHours = hours;
                totalhours.Month = monthandyear;
                totalhours.Id = 0;
                totalhours.EmployeeCode = empcode;
                _context.MonthlyTotalHours.Add(totalhours);
            }
            _context.SaveChanges();
            return Json("");
        }

        public object CalculateTotalHours(MonthlyReport arrival, MonthlyReport departure)
        {
            decimal arrivaltotal = arrival._1 + arrival._2 + arrival._3 + arrival._4 + arrival._5 + arrival._6 + arrival._7 + arrival._8 + arrival._9 + arrival._10 + arrival._11 + arrival._12 + arrival._13
                + arrival._14 + arrival._15 + arrival._16 + arrival._17 + arrival._18 + arrival._19 + arrival._20 + arrival._21 + arrival._22 + arrival._23 + arrival._24 + arrival._25 + arrival._26
                + arrival._27 + arrival._28 + arrival._29 + arrival._30 + arrival._31;

            decimal departuretotal = departure._1 + departure._2 + departure._3 + departure._4 + departure._5 + departure._6 + departure._7 + departure._8 + departure._9 + departure._10 + departure._11 + departure._12 + departure._13
                + departure._14 + departure._15 + departure._16 + departure._17 + departure._18 + departure._19 + departure._20 + departure._21 + departure._22 + departure._23 + departure._24 + departure._25 + departure._26
                + departure._27 + departure._28 + departure._29 + departure._30 + departure._31;

            decimal difference = departuretotal - arrivaltotal;
            return difference;
        }

        private MonthlyReport ManageMonthlyReportData(int number, decimal onTime, MonthlyReport monthlyReport)
        {
            switch (number)
            {
                case 1:
                    monthlyReport._1 = onTime;
                    break;
                case 2:
                    monthlyReport._2 = onTime;
                    break;
                case 3:
                    monthlyReport._3 = onTime;
                    break;
                case 4:
                    monthlyReport._4 = onTime;
                    break;
                case 5:
                    monthlyReport._5 = onTime;
                    break;
                case 6:
                    monthlyReport._6 = onTime;
                    break;
                case 7:
                    monthlyReport._7 = onTime;
                    break;
                case 8:
                    monthlyReport._8 = onTime;
                    break;
                case 9:
                    monthlyReport._9 = onTime;
                    break;
                case 10:
                    monthlyReport._10 = onTime;
                    break;
                case 11:
                    monthlyReport._11 = onTime;
                    break;
                case 12:
                    monthlyReport._12 = onTime;
                    break;
                case 13:
                    monthlyReport._13 = onTime;
                    break;
                case 14:
                    monthlyReport._14 = onTime;
                    break;
                case 15:
                    monthlyReport._15 = onTime;
                    break;
                case 16:
                    monthlyReport._16 = onTime;
                    break;
                case 17:
                    monthlyReport._17 = onTime;
                    break;
                case 18:
                    monthlyReport._18 = onTime;
                    break;
                case 19:
                    monthlyReport._19 = onTime;
                    break;
                case 20:
                    monthlyReport._20 = onTime;
                    break;
                case 21:
                    monthlyReport._21 = onTime;
                    break;
                case 22:
                    monthlyReport._22 = onTime;
                    break;
                case 23:
                    monthlyReport._23 = onTime;
                    break;
                case 24:
                    monthlyReport._24 = onTime;
                    break;
                case 25:
                    monthlyReport._25 = onTime;
                    break;
                case 26:
                    monthlyReport._26 = onTime;
                    break;
                case 27:
                    monthlyReport._27 = onTime;
                    break;
                case 28:
                    monthlyReport._28 = onTime;
                    break;
                case 29:
                    monthlyReport._29 = onTime;
                    break;
                case 30:
                    monthlyReport._30 = onTime;
                    break;
                case 31:
                    monthlyReport._31 = onTime;
                    break;
            }
            return monthlyReport;
        }

        private bool ManualTimesheetCalculation(ManualTimeSheet timesheet)
        {
            try
            {
                var employeeData = _context.ApplicationUser.Where(x => x.Id == timesheet.EmployeeId).FirstOrDefault();
                var empcode = employeeData.EmpCode;
                var day = timesheet.ManualDate.Day;
                var month = timesheet.ManualDate.Month;
                var year = timesheet.ManualDate.Year;
                var monthandyear = month + "/" + year;

                decimal intime = getTime(timesheet.FromTime);
                decimal outtime = getTime(timesheet.ToTime);
                var ExistMonthlyReport = new MonthlyReport();
                ExistMonthlyReport.EmployeeCode = empcode;
                ExistMonthlyReport.Month = monthandyear;


                var adTimeList = _context.MonthlyReport
                    .Where(x => x.EmployeeCode == empcode && x.Month == monthandyear && (x.DataType == "ArrTim" || x.DataType == "DepTim" || x.DataType == "WorkingHrs" || x.DataType == "ReportType")).ToList();

                if (adTimeList.Count == 0)
                {
                    ExistMonthlyReport = new MonthlyReport()
                    {
                        Id = 0,
                        DataType = "ArrTim",
                        EmployeeCode = empcode,
                        Month = monthandyear
                    };
                    _context.MonthlyReport.Add(ManageMonthlyReportData(day, intime, ExistMonthlyReport));
                    _context.SaveChanges();

                    ExistMonthlyReport = new MonthlyReport()
                    {
                        Id = 0,
                        DataType = "DepTim",
                        EmployeeCode = empcode,
                        Month = monthandyear
                    };
                    _context.MonthlyReport.Add(ManageMonthlyReportData(day, outtime, ExistMonthlyReport));
                    _context.SaveChanges();

                    ExistMonthlyReport = new MonthlyReport()
                    {
                        Id = 0,
                        DataType = "WorkingHrs",
                        EmployeeCode = empcode,
                        Month = monthandyear
                    };

                    var wrHrs = outtime - intime;
                    _context.MonthlyReport.Add(ManageMonthlyReportData(day, wrHrs, ExistMonthlyReport));
                    _context.SaveChanges();

                    ExistMonthlyReport = new MonthlyReport()
                    {
                        Id = 0,
                        DataType = "ReportType",
                        EmployeeCode = empcode,
                        Month = monthandyear
                    };

                    var manual = 0;
                    _context.MonthlyReport.Add(ManageMonthlyReportData(day, manual, ExistMonthlyReport));
                    _context.SaveChanges();
                }
                else
                {
                    foreach (var ADValue in adTimeList)
                    {
                        ExistMonthlyReport = _context.MonthlyReport.Where(x => x.Id == ADValue.Id).FirstOrDefault();
                        if (ADValue.DataType == "ArrTim")
                        {
                            _context.MonthlyReport.Update(ManageMonthlyReportData(day, intime, ExistMonthlyReport));
                        }
                        else if (ADValue.DataType == "DepTim")
                        {
                            _context.MonthlyReport.Update(ManageMonthlyReportData(day, outtime, ExistMonthlyReport));
                        }
                        else if (ADValue.DataType == "WorkingHrs")
                        {

                            _context.MonthlyReport.Update(ManageMonthlyReportData(day, outtime, ExistMonthlyReport));
                        }
                        else if (ADValue.DataType == "ReportType")
                        {
                            outtime = 0;
                            _context.MonthlyReport.Update(ManageMonthlyReportData(day, outtime, ExistMonthlyReport));
                        }
                        _context.SaveChanges();
                    }
                }


                var arrival = _context.MonthlyReport.Where(x => x.EmployeeCode == empcode && x.Month == monthandyear && x.DataType == "ArrTim").FirstOrDefault();
                var departure = _context.MonthlyReport.Where(x => x.EmployeeCode == empcode && x.Month == monthandyear && x.DataType == "DepTim").FirstOrDefault();
                var hours = Convert.ToDecimal(CalculateTotalHours(arrival, departure));

                var totalhours = new MonthlyTotalHours();
                var existhours = _context.MonthlyTotalHours.Where(x => x.EmployeeCode == empcode && x.Month == monthandyear).FirstOrDefault();
                if (existhours != null)
                {
                    existhours.TotalHours = hours;
                    _context.MonthlyTotalHours.Update(existhours);
                }
                else
                {
                    totalhours.TotalHours = hours;
                    totalhours.Month = monthandyear;
                    totalhours.Id = 0;
                    totalhours.EmployeeCode = empcode;
                    _context.MonthlyTotalHours.Add(totalhours);
                }
                _context.SaveChanges();

                var email = new MailRequest();
                email.ToEmail = employeeData.Email;

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private decimal getTime(DateTime dateTime)
        {
            var hrs = dateTime.Hour.ToString();
            var mins = dateTime.Minute.ToString();
            var time = hrs + "." + mins;
            return Convert.ToDecimal(time);
        }

        private MimeMessage SendMail(MailRequest message)
        {
            var _configuration = getCredential();
            var emailMessage = new MimeMessage();
            MailboxAddress emailFrom = new MailboxAddress(_configuration.Key, _configuration.Value);
            emailMessage.From.Add(emailFrom);
            MailboxAddress emailTo = new MailboxAddress("", message.ToEmail);
            emailMessage.To.Add(emailTo);
            if (message.Cc != null)
            {
                if (message.Cc != "")
                {
                    MailboxAddress ccto = new MailboxAddress("", message.Cc);
                    emailMessage.Cc.Add(ccto);
                }
            }
            if (message.Bcc != null)
            {
                if (message.Bcc != "")
                {
                    MailboxAddress bccto = new MailboxAddress("", message.Bcc);
                    emailMessage.Bcc.Add(bccto);
                }
            }
            emailMessage.Subject = message.Subject;
            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.TextBody = message.Body;
            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.Auto);
                //var creds = new NetworkCredential("hamza.codedot@gmail.com", "Hamza#@@78");
                var creds = new NetworkCredential(_configuration.Key, _configuration.Value);
                smtpClient.Authenticate(creds);
                smtpClient.Send(emailMessage);
                smtpClient.Disconnect(true);
            }

            return emailMessage;
        }
        private Configuration getCredential()
        {
            return _context.Configuration.Where(x => x.IsActive == true).FirstOrDefault();
        }
    }
}

