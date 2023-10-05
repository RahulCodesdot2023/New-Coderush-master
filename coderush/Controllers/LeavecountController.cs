using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using coderush.DataEnum;
using Microsoft.AspNetCore.Authorization;
using DemoCreate.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using coderush.ViewModels;
using coderush.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CodesDotHRMS.Models;
using System.Net;
using MimeKit;
using CodesDotHRMS.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using static coderush.Services.App.Pages;
using coderush.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace coderush.Controllers
{
    [Authorize(Roles = "HR,SuperAdmin,Employee")] //Harshal working on
    public class LeavecountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHubContext<BirthdayNotificationHub> _DobNotificationHubContext;
        //private readonly IHostEnvironment _hostingEnvironment;
        public static string userid;
        public static string username;
        public LeavecountController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context,
           IHubContext<BirthdayNotificationHub> ACNotificationHubContext,
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
        public IActionResult LeaveIndex(string userId)
        {
            List<LeaveCountViewModel> model = new List<LeaveCountViewModel>();
            ViewBag.leavcountId = userId;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> BindGridData(string id, string UserName)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var adminrole = await _userManager.IsInRoleAsync(user, "HR");
            var suadminrole = await _userManager.IsInRoleAsync(user, "SuperAdmin");
            var teamAdminrole = await _userManager.IsInRoleAsync(user, "TeamAdmin");

            LeaveCountViewModel levcunt = new LeaveCountViewModel();
            var leavecount = new List<LeaveCountViewModel>();
            var todayDate = DateTime.Now;
            if (!adminrole && !suadminrole)
            {
                UserName = user.UserName;
                id = user.Id;
            }
            if (!adminrole && !teamAdminrole)
            {
                try
                {
                    leavecount = (from s in _context.LeaveCount
                                  join dm in _context.Datamaster on s.LeaveReason equals Convert.ToInt32(dm.Id)
                                  where s.Userid == id
                                  select new LeaveCountViewModel
                                  {
                                      Id = s.Id,
                                      Userid = _userManager.Users.Where(x => x.Id == s.Userid).Select(x => x.Id).FirstOrDefault(),//s.Userid, 
                                      FullName = _userManager.Users.Where(x => x.Id == id).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),
                                      Fromdate = s.Fromdate,
                                      Todate = s.Todate,
                                      Filename = s.FileUpload != null ? s.FileUpload : "-",
                                      Count = s.Count != null ? s.Count : "0",
                                      EmployeeDescription = s.EmployeeDescription != null ? s.EmployeeDescription : "-",
                                      LeaveReason = s.LeaveReason,
                                      HrDescription = s.HrDescription != null ? s.HrDescription : "-",
                                      Isapprove = s.Isapprove != true ? false : true,
                                      IsReject = s.IsReject != true ? false : true,
                                      ApproveDate = s.ApproveDate,
                                      Approveby = s.Approveby != null ? s.Approveby : "-",
                                      AdminRole = adminrole,
                                      isedit = (s.Todate >= todayDate) ? true : false,
                                      colouris = s.Todate > todayDate ? "#ffe0bb" : "",
                                      CreatedDate = s.CreatedDate,
                                      LeaveVal = dm.Text

                                  }).ToList();
                }
                catch (Exception ex)
                {

                    throw;
                }
                var leavecount1 = leavecount.Where(x => x.colouris == "").ToList();
                leavecount = leavecount.OrderByDescending(x => x.Todate).Where(x => x.Todate > todayDate).OrderBy(x => x.Todate).ToList();
                foreach (var data in leavecount1)
                {
                    LeaveCountViewModel ab = new LeaveCountViewModel();
                    ab.Id = data.Id;
                    ab.Userid = user.Id;
                    ab.UserName = user.UserName;
                    ab.FullName = user.FirstName + " " + user.LastName;
                    ab.CreatedDate = data.CreatedDate;
                    ab.Fromdate = data.Fromdate;
                    ab.Todate = data.Todate;
                    ab.Filename = data.Filename;
                    ab.Count = data.Count;
                    ab.EmployeeDescription = data.EmployeeDescription != null ? data.EmployeeDescription : string.Empty;
                    ab.HrDescription = data.HrDescription != null ? data.HrDescription : string.Empty;
                    ab.Isapprove = data.Isapprove;
                    ab.IsReject = data.IsReject;
                    ab.ApproveDate = data.ApproveDate;
                    ab.Approveby = data.Approveby;
                    ab.AdminRole = adminrole;
                    ab.isedit = (data.Todate <= todayDate) ? true : false;
                    ab.LeaveReason = data.LeaveReason;
                    leavecount.Add(ab);
                }
            }
            else if ((adminrole || suadminrole) && teamAdminrole)
            {
                try
                {
                    leavecount = _context.LeaveCount
                         .Where(w => w.Userid == id)
                                      .Select(s => new LeaveCountViewModel()
                                      {
                                          Id = s.Id,
                                          Userid = _userManager.Users.Where(x => x.Id == s.Userid).Select(x => x.Id).FirstOrDefault(),//s.Userid, 
                                          FullName = _userManager.Users.Where(x => x.Id == id).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),
                                          Fromdate = s.Fromdate,
                                          Todate = s.Todate,
                                          Filename = s.FileUpload != null ? s.FileUpload : "-",
                                          Count = s.Count,
                                          EmployeeDescription = s.EmployeeDescription != null ? s.EmployeeDescription : "-",
                                          HrDescription = s.HrDescription != null ? s.HrDescription : "-",
                                          Isapprove = s.Isapprove != null ? s.Isapprove : false,
                                          IsReject = s.IsReject != null ? s.IsReject : false,
                                          ApproveDate = s.ApproveDate != null ? s.ApproveDate : null,
                                          Approveby = s.Approveby != null ? s.Approveby : "-",
                                          AdminRole = adminrole,
                                          isedit = (s.Todate <= todayDate) ? false : true,
                                          colouris = s.Todate > todayDate ? "#ffe0bb" : "",
                                          CreatedDate = s.CreatedDate,
                                          LeaveReason = s.LeaveReason,
                                      }).ToList();
                    //leavecount = leavecount.OrderByDescending(x => x.Todate).ToList();
                    //leavecount = leavecount.OrderByDescending(x => x.Todate).Where(x => x.Todate > todayDate).OrderBy(x=>x.Todate).ToList();
                    var leavecount1 = leavecount.Where(x => x.colouris == "").ToList();
                    leavecount = leavecount.OrderByDescending(x => x.Todate).Where(x => x.Todate > todayDate).OrderBy(x => x.Todate).ToList();
                    foreach (var data in leavecount1)
                    {
                        LeaveCountViewModel ab = new LeaveCountViewModel();
                        ab.Id = data.Id;
                        ab.Userid = user.Id;
                        ab.UserName = user.UserName;
                        ab.FullName = user.FirstName + " " + user.LastName;
                        ab.CreatedDate = data.CreatedDate;
                        ab.Fromdate = data.Fromdate;
                        ab.Todate = data.Todate;
                        ab.Filename = data.Filename;
                        ab.Count = data.Count;
                        ab.EmployeeDescription = data.EmployeeDescription != null ? data.EmployeeDescription : string.Empty;
                        ab.HrDescription = data.HrDescription != null ? data.HrDescription : string.Empty;
                        ab.Isapprove = data.Isapprove;
                        ab.IsReject = data.IsReject;
                        ab.ApproveDate = data.ApproveDate;
                        ab.Approveby = data.Approveby;
                        ab.AdminRole = adminrole;
                        ab.isedit = (data.Todate >= todayDate) ? true : false;
                        ab.LeaveReason = data.LeaveReason;
                        leavecount.Add(ab);
                    }


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

                leavecount = _context.LeaveCount
                         .Where(w => teamUsers.Contains(w.Userid))
                                      .Select(s => new LeaveCountViewModel()
                                      {
                                          Id = s.Id,
                                          Userid = _userManager.Users.Where(x => x.Id == s.Userid).Select(x => x.Id).FirstOrDefault(),//s.Userid, 
                                          FullName = _userManager.Users.Where(x => x.Id == s.Userid).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault(),
                                          Fromdate = s.Fromdate,
                                          Todate = s.Todate,
                                          Filename = s.FileUpload != null ? s.FileUpload : "-",
                                          Count = s.Count,
                                          EmployeeDescription = s.EmployeeDescription != null ? s.EmployeeDescription : "-",
                                          HrDescription = s.HrDescription != null ? s.HrDescription : "-",
                                          Isapprove = s.Isapprove != null ? s.Isapprove : false,
                                          IsReject = s.IsReject != null ? s.IsReject : false,
                                          ApproveDate = s.ApproveDate != null ? s.ApproveDate : null,
                                          Approveby = s.Approveby != null ? s.Approveby : "-",
                                          AdminRole = adminrole,
                                          isedit = (s.Todate <= todayDate) ? false : true,
                                          colouris = s.Todate > todayDate ? "#ffe0bb" : "",
                                          CreatedDate = s.CreatedDate,
                                          LeaveReason = s.LeaveReason,
                                      }).ToList();

                var leavecount1 = leavecount.Where(x => x.colouris == "").ToList();
                leavecount = leavecount.OrderByDescending(x => x.Todate).Where(x => x.Todate > todayDate).OrderBy(x => x.Todate).ToList();
                foreach (var data in leavecount1)
                {
                    LeaveCountViewModel ab = new LeaveCountViewModel();
                    ab.Id = data.Id;
                    ab.Userid = user.Id;
                    ab.UserName = user.UserName;
                    ab.FullName = user.FirstName + " " + user.LastName;
                    ab.CreatedDate = data.CreatedDate;
                    ab.Fromdate = data.Fromdate;
                    ab.Todate = data.Todate;
                    ab.Filename = data.Filename;
                    ab.Count = data.Count;
                    ab.EmployeeDescription = data.EmployeeDescription != null ? data.EmployeeDescription : string.Empty;
                    ab.HrDescription = data.HrDescription != null ? data.HrDescription : string.Empty;
                    ab.Isapprove = data.Isapprove;
                    ab.IsReject = data.IsReject;
                    ab.ApproveDate = data.ApproveDate;
                    ab.Approveby = data.Approveby;
                    ab.AdminRole = adminrole;
                    ab.isedit = (data.Todate >= todayDate) ? true : false;
                    ab.LeaveReason = data.LeaveReason;
                    leavecount.Add(ab);
                }
            }
            levcunt.List = leavecount.OrderByDescending(x => x.Id).ToList();
            return Json(levcunt);
        }

        public async Task<IActionResult> BinddrpdwnData()
        {
            var user = _userManager.GetUserAsync(User).Result;
            var adminrole = await _userManager.IsInRoleAsync(user, "HR");
            var suadminrole = await _userManager.IsInRoleAsync(user, "SuperAdmin");
            var teamAdminrole = await _userManager.IsInRoleAsync(user, "TeamAdmin");

            if (adminrole || suadminrole || teamAdminrole)
            {
                var leavecount = _userManager.Users
                                    .Where(w => w.UserName != null)
                                    .OrderByDescending(w => w.Id)
                                   .Select(s => new SelectListItem()
                                   {
                                       Text = String.Format("{0} {1} {2}", s.UserName, s.FirstName != null ? "|| " + s.FirstName : "", s.LastName != null ? "|| " + s.LastName : "").ToString(),
                                       Value = s.Id.ToString()
                                   }).ToList();
                return Json(leavecount);
            }
            else
            {
                var leavecount = _userManager.Users
                                      .Where(w => w.UserName != null && w.Email == user.ToString())
                                     .Select(s => new SelectListItem()
                                     {
                                         Text = String.Format("{0} {1} {2}", s.UserName, s.FirstName != null ? "|| " + s.FirstName : "", s.LastName != null ? "|| " + s.LastName : "").ToString(),
                                         Value = s.Id.ToString()
                                     }).ToList();
                return Json(leavecount);
            }
        }
        //post submitted leavecount data. if todo.TodoId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitForm(/*[Bind("Id", "Userid", "Fromdate", "Todate", "Count", "LeaveReason", "EmployeeDescription", "HrDescription", "ApproveDate",  "Isapprove")]*/ LeaveCountViewModel leaveCounts)
        {
            var user = _userManager.GetUserAsync(User).Result;

            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = leaveCounts.Id > 0 ? leaveCounts.Id : 0 });
                }
                var result = SaveLeaveCount(leaveCounts);
                TempData[StaticString.StatusMessage] = result.Result.LeaveMessage.ToString();
                return RedirectToAction(nameof(Form), new { id = result.Result.Id > 0 ? result.Result.Id : 0, userid = result.Result.UserId.ToString() });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = leaveCounts.Id > 0 ? leaveCounts.Id : 0 });
            }
        }

        //display leavecount create edit form
        [HttpGet]
        public async Task<IActionResult> Form(int id, string userid)
        {
            LeaveCountViewModel model = new LeaveCountViewModel();
            var userRole = _userManager.GetUserAsync(User).Result;
            var roleHR = await _userManager.IsInRoleAsync(userRole, "HR");
            var roleSuperAdmin = await _userManager.IsInRoleAsync(userRole, "SuperAdmin");

            var LeaveReasonList = (from emp in _context.Datamaster
                                   where emp.Isdeleted != true && emp.Type == DataSelection.LeaveReasons
                                   select new LeaveCountViewModel { Id = emp.Id, LeaveVal = emp.Text }).ToList();

            ViewBag.LeaveReason = new SelectList(LeaveReasonList, "Id", "LeaveVal");

            var userList = (from userData in _context.ApplicationUser
                            join ur in _context.UserRoles on userData.Id equals ur.UserId
                            join role in _context.Roles on ur.RoleId equals role.Id
                            where role.Name == "Employee"
                            select new UserDropdownListViewModel
                            {
                                Userid = userData.Id,
                                FullName = userData.FirstName + " " + userData.LastName
                            }).ToList();

            var userDetail = _userManager.GetUserAsync(User).Result;

            if (!roleHR && !roleSuperAdmin)
                userid = userDetail.Id;

            if (id == 0)
            {
                if (userid != null)
                {
                    var udser = _userManager.Users.Where(w => w.Id == userid).FirstOrDefault();
                    model.UserName = udser.UserName;
                    model.Userid = udser.Id;
                }


                ViewBag.UserList = new SelectList(userList, "Userid", "FullName");
                return View(model);
            }
            else
            {
                var editnewleavecount = (from data in _context.LeaveCount
                                         join user in _userManager.Users on data.Userid equals user.Id
                                         where data.Id == id
                                         select new LeaveCountViewModel
                                         {
                                             Id = data.Id,
                                             Userid = user.Id,
                                             UserName = user.UserName,
                                             //UserName = _userManager.Users.Where(x=>x.Id== userid.ToString()).Select(s=>s.FirstName +" "+s.LastName).FirstOrDefault(),
                                             Fromdate = data.Fromdate,
                                             Todate = data.Todate,
                                             Filename = data.FileUpload == null ? "-" : data.FileUpload,
                                             Count = data.Count,
                                             EmployeeDescription = data.EmployeeDescription,
                                             HrDescription = data.HrDescription,
                                             Isapprove = data.Isapprove,
                                             ApproveDate = data.ApproveDate,
                                             Approveby = data.Approveby,
                                             //LeaveReason = data.LeaveReason
                                         }).FirstOrDefault();

                ViewBag.UserList = new SelectList(userList, "Userid", "FullName", editnewleavecount.Userid);

                return View(editnewleavecount);
            }
        }
        //display leavecount item for deletion
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var leavecount = _context.LeaveCount.Where(x => x.Id.Equals(id)).FirstOrDefault();
            return View(leavecount);
        }
        //delete submitted leave count item if found, otherwise 404
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("Id")] LeaveCount leave)
        {
            try
            {
                var deleteleavecount = _context.LeaveCount.Where(x => x.Id.Equals(leave.Id)).FirstOrDefault();
                if (deleteleavecount == null)
                {
                    return NotFound();
                }
                deleteleavecount.Isapprove = true;
                deleteleavecount.IsDelete = false;
                _context.LeaveCount.Update(deleteleavecount);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete leave count item success.";
                return RedirectToAction(nameof(LeaveIndex));
            }
            catch (Exception ex)
            {
                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = leave.Id > 0 ? leave.Id : 0, userid = leave.Userid });
            }
        }
        [HttpGet]
        public IActionResult EditData(int id)
        {
            var Data = _context.LeaveCount.Where(x => x.Id == id).FirstOrDefault();
            return Json(Data);
        }
        [HttpPost]
        public async Task<ActionResult> leavepopuop(int Id, string HrDescription, bool Isapprove)
        {
            try
            {
                var email = new MailRequest();
                var models = (from lc in _context.LeaveCount
                              where lc.Id == Id
                              select new LeaveCount
                              {
                                  Id = lc.Id,
                                  Userid = lc.Userid,
                                  Approveby = lc.Approveby != null ? lc.Approveby : "-",
                                  ApproveDate = lc.ApproveDate != null ? lc.ApproveDate : null,
                                  Count = lc.Count != null ? lc.Count : "0",
                                  EmployeeDescription = lc.EmployeeDescription != null ? lc.EmployeeDescription : "-",
                                  CreatedBy = lc.CreatedBy != null ? lc.CreatedBy : "-",
                                  CreatedDate = lc.CreatedDate != null ? lc.CreatedDate : DateTime.Now,
                                  Fromdate = lc.Fromdate,
                                  Todate = lc.Todate,
                                  HrDescription = lc.HrDescription != null ? lc.HrDescription : "-",
                                  Isapprove = lc.Isapprove != true ? lc.Isapprove : false,
                                  IsReject = lc.IsReject != true ? lc.IsReject : false,
                                  LeaveReason = lc.LeaveReason > 0 ? lc.LeaveReason : 0,
                                  FileUpload = lc.FileUpload != null ? lc.FileUpload : "-",
                              }).FirstOrDefault();

                var users = _context.ApplicationUser.Where(x => x.Id == models.Userid).FirstOrDefault();
                var userName = users.FirstName + " " + users.LastName;
                var hr = _userManager.GetUsersInRoleAsync("HR").Result.FirstOrDefault();
                var hrname = hr.FirstName + " " + hr.LastName;

                var user = _userManager.GetUserAsync(User).Result;

                models.HrDescription = HrDescription;
                if (Isapprove)
                {
                    models.Isapprove = Isapprove;
                    models.ApproveDate = DateTime.Now;
                    models.IsReject = false;
                    models.Approveby = user.Id;
                    models.UpdatedDate = DateTime.Now;
                    //if (users.PaidLeave > 0)
                    //{
                    //    users.PaidLeave = users.PaidLeave - Convert.ToDecimal(models.Count);
                    //    _context.Users.Update(users);
                    //    _context.SaveChanges();
                    //}
                    //else
                    //{
                    //    users.UnpaidLeave = users.UnpaidLeave + Convert.ToDecimal(models.Count);
                    //    _context.Users.Update(users);
                    //    _context.SaveChanges();
                    //}

                    email.ToEmail = users.Email;
                    email.Subject = "Leave Application Accepted by: " + hrname;
                    email.Body = "Dear " + userName + "\r\n\r\nI am writing this to inform you that your leave Application Is Accepted\r\n\r\nThank You\r\n\r\n" + hrname;
                    SendMail(email);
                    await PaidLeaveMangement(models);
                }
                else
                {
                    models.UpdatedDate = DateTime.Now;
                    models.IsReject = true;
                    models.Isapprove = Isapprove;

                    email.ToEmail = users.Email;
                    email.Subject = "Leave Application Rejected by: " + hrname;
                    email.Body = "Dear " + userName + "\r\n\r\nI am writing this to inform you that your leave Application Is Not Accepted\r\n\r\nPlease Feel Free to Contact us regarding any issues\r\n\r\nThank You\r\n\r\n" + hrname;
                    SendMail(email);
                }
                _context.LeaveCount.Update(models);
                _context.SaveChanges();

                var result = new { Success = "true", Message = "Data save successfully." };
                return Json(result);
            }
            catch (Exception ex)
            {
                var result = new { Success = "False", Message = ex.Message };
                return Json(result);
            }
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
        private async Task<LeaveRequest> SaveLeaveCount(LeaveCountViewModel leaveCountsData)
        {
            LeaveRequest request = new LeaveRequest();
            LeaveCount leaveCount = new LeaveCount();
            LeaveHistory leaveHistory = new LeaveHistory();
            var teamLeaders = new List<TeamLeader>();

            var userDT = _userManager.GetUserAsync(User).Result;
            var user = _userManager.FindByIdAsync(leaveCountsData.Userid).Result;

            var suadminrole = await _userManager.IsInRoleAsync(userDT, "SuperAdmin");
            var hrrole = await _userManager.IsInRoleAsync(userDT, "HR");
            var teamAdmin = await _userManager.IsInRoleAsync(userDT, "TeamAdmin");

            List<string> uploadedFiles = new List<string>();

            try
            {
                if (leaveCountsData.FileUpload != null)
                {
                    string wwwPath = this._webHostEnvironment.WebRootPath;
                    string contentPath = this._webHostEnvironment.ContentRootPath;

                    var filename = leaveCountsData.FileUpload.FileName;
                    string path = Path.Combine(this._webHostEnvironment.WebRootPath, "document/Leave");

                    string fileName = Path.GetFileName(leaveCountsData.FileUpload.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        leaveCountsData.FileUpload.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                    }
                }

                //create new
                if (leaveCountsData.Id == 0)
                {
                    leaveCount.Userid = leaveCountsData.Userid;
                    leaveCount.Fromdate = leaveCountsData.Fromdate;
                    leaveCount.Todate = leaveCountsData.Todate;
                    leaveCount.Count = leaveCountsData.Count;
                    leaveCount.EmployeeDescription = leaveCountsData.EmployeeDescription;
                    leaveCount.HrDescription = leaveCountsData.HrDescription;
                    if (leaveCountsData.FileUpload != null)
                    {
                        leaveCount.FileUpload = leaveCountsData.FileUpload.FileName.ToString();
                    }
                    else
                    {
                        leaveCount.FileUpload = string.Empty;
                    }
                    leaveCount.CreatedBy = userDT.Id;
                    leaveCount.CreatedDate = DateTime.Now;
                    leaveCount.LeaveReason = Convert.ToInt32(leaveCountsData.LeaveReason);
                    leaveCount.LeaveOtherReason = leaveCountsData.LeaveOtherReason;

                    leaveHistory.Userid = leaveCountsData.Userid;
                    leaveHistory.Fromdate = leaveCountsData.Fromdate;
                    leaveHistory.Todate = leaveCountsData.Todate;
                    leaveHistory.Count = leaveCountsData.Count;
                    leaveHistory.EmployeeDescription = leaveCountsData.EmployeeDescription;
                    leaveHistory.HRDescription = leaveCountsData.HrDescription;
                    if (leaveCountsData.FileUpload != null)
                    {
                        leaveHistory.FileUpload = leaveCountsData.FileUpload.FileName.ToString();
                    }
                    else
                    {
                        leaveHistory.FileUpload = string.Empty;
                    }
                    leaveHistory.CreatedBy = userDT.Id;
                    leaveHistory.CreatedDate = DateTime.Now;
                    leaveHistory.LeaveReason = Convert.ToInt32(leaveCountsData.LeaveReason);
                    leaveHistory.LeaveOtherReason = leaveCountsData.LeaveOtherReason;


                    var notification = new NotificationMaster();
                    notification.Title = $"Leave-Info for {user.FirstName + " " + user.LastName}.";
                    notification.UserId = leaveCountsData.Userid.ToString();
                    notification.NotifyAction = "LeaveIndex";
                    notification.NotifyController = "Leavecount";
                    notification.CreatedDate = DateTime.Now;
                    _context.NotificationMaster.Add(notification);
                    request.LeaveMessage = "Create new leave count item success.";
                }
                else
                {
                    leaveCount = (from lc in _context.LeaveCount
                                  where lc.Id == leaveCountsData.Id
                                  select new LeaveCount
                                  {
                                      Fromdate = lc.Fromdate,
                                      Todate = lc.Todate,
                                      Count = lc.Count,
                                      EmployeeDescription = lc.EmployeeDescription != null ? "-" : lc.EmployeeDescription,
                                      HrDescription = lc.HrDescription != null ? "-" : lc.HrDescription,
                                      LeaveReason = lc.LeaveReason > 0 ? lc.LeaveReason : 0,
                                      LeaveOtherReason = lc.LeaveOtherReason != null ? lc.LeaveOtherReason : "-",
                                      Approveby = lc.Approveby != null ? lc.Approveby : "-",
                                      ApproveDate = lc.ApproveDate != null ? lc.ApproveDate : DateTime.Now,
                                      FileUpload = lc.FileUpload != null ? lc.FileUpload : "-",
                                      Isapprove = lc.Isapprove != null ? lc.Isapprove : false,
                                      IsReject = lc.IsReject != null ? lc.IsReject : false,
                                      UpdatedBy = lc.UpdatedBy != null ? lc.UpdatedBy : "-",
                                      UpdatedDate = lc.UpdatedDate != null ? lc.UpdatedDate : DateTime.Now,
                                  }).FirstOrDefault();

                    leaveCount.Userid = leaveCountsData.Userid;
                    leaveCount.Fromdate = leaveCountsData.Fromdate;
                    leaveCount.Todate = leaveCountsData.Todate;
                    leaveCount.Count = leaveCountsData.Count;
                    leaveCount.CreatedDate = leaveCountsData.CreatedDate;
                    leaveCount.EmployeeDescription = leaveCountsData.EmployeeDescription != null ? leaveCountsData.EmployeeDescription : "-";
                    leaveCount.HrDescription = leaveCountsData.HrDescription != null ? leaveCountsData.HrDescription : "-";
                    if (leaveCount.FileUpload != null && leaveCount.FileUpload != "")
                    {
                        leaveCount.FileUpload = leaveCountsData.FileUpload.FileName.ToString() == null ? leaveCountsData.FileUpload.FileName.ToString() : leaveCountsData.FileUpload.FileName.ToString();
                    }
                    leaveCount.UpdatedBy = userDT.Id;
                    leaveCount.UpdatedDate = DateTime.Now;
                    leaveCount.LeaveReason = Convert.ToInt32(leaveCountsData.LeaveReason) > 0 ? Convert.ToInt32(leaveCountsData.LeaveReason) : 0;
                    leaveCount.LeaveOtherReason = leaveCountsData.LeaveOtherReason;

                    leaveHistory.Userid = leaveCountsData.Userid;
                    leaveHistory.Fromdate = leaveCountsData.Fromdate;
                    leaveHistory.Todate = leaveCountsData.Todate;
                    leaveHistory.Count = leaveCountsData.Count;
                    leaveHistory.CreatedDate = leaveCountsData.CreatedDate;
                    leaveHistory.EmployeeDescription = leaveCountsData.EmployeeDescription != null ? leaveCountsData.EmployeeDescription : "-";
                    leaveHistory.HRDescription = leaveCountsData.HrDescription != null ? leaveCountsData.HrDescription : "-";
                    if (leaveCount.FileUpload != null && leaveCount.FileUpload != "")
                    {
                        leaveHistory.FileUpload = leaveCount.FileUpload.ToString();
                    }
                    leaveHistory.UpdatedBy = userDT.Id;
                    leaveHistory.UpdatedDate = DateTime.Now;
                    leaveHistory.LeaveReason = Convert.ToInt32(leaveCountsData.LeaveReason);
                    leaveHistory.LeaveOtherReason = leaveCountsData.LeaveOtherReason;


                    request.LeaveMessage = "Edit existing leave count item success.";
                }

                if (suadminrole == true
                    || (hrrole == true && leaveCount.Userid != user.Id)
                    || (teamAdmin == true && leaveCount.Userid != user.Id))
                {
                    leaveCount.Isapprove = leaveCountsData.Isapprove != null ? leaveCountsData.Isapprove : false;
                    leaveCount.Approveby = userDT.Id;
                    leaveCount.ApproveDate = DateTime.Now;

                    leaveHistory.Isapprove = leaveCountsData.Isapprove != null ? leaveCountsData.Isapprove : false;
                    leaveHistory.Approveby = userDT.Id;
                    leaveHistory.ApproveDate = DateTime.Now;

                    if (user.PaidLeave > 0)
                    {
                        user.PaidLeave = user.PaidLeave - Convert.ToDecimal(leaveCountsData.Count);
                        _context.Users.Update(user);
                    }
                    else
                    {
                        user.UnpaidLeave = user.UnpaidLeave + Convert.ToDecimal(leaveCountsData.Count);
                        _context.Users.Update(user);
                    }

                    // Send the Leave Email to HR, TeamLead and SuperAdmin
                    var employee = _context.ApplicationUser.Where(x => x.Id == leaveCountsData.Userid).First();
                    var userName = employee.FirstName + " " + employee.LastName;
                    var adminMail = _userManager.GetUsersInRoleAsync("SuperAdmin").Result.FirstOrDefault().Email;
                    var hr = _userManager.GetUsersInRoleAsync("HR").Result.FirstOrDefault();
                    var hrMail = hr.Email;
                    var hrName = hr.FirstName + " " + hr.LastName;


                    var email = new MailRequest();
                    email.ToEmail = _userManager.FindByIdAsync(leaveCount.Userid).Result.Email;
                    email.Cc = adminMail;
                    email.Bcc = hrMail;
                    email.Subject = $"Leave Application for {userName}";
                    email.Body = $"Dear " + userName + "\r\n\r\nI am writing this to inform you that your leave Application Is Accepted\r\n\r\nThank You\r\n\r\n" + userDT.FirstName + " " + userDT.LastName + "\r\n\r\n" + userDT.PhoneNumber;
                    SendMail(email);

                    teamLeaders = _context.TeamLeader.Where(x => x.EmployeeId == leaveCountsData.Userid).ToList();
                    var teamLeaderEmail = "";
                    if (teamLeaders.Count > 0)
                    {
                        var teamleademail = new MailRequest();
                        var lead = _userManager.GetUsersInRoleAsync("TeamAdmin").Result.Where(x => x.Email == teamLeaders.First().TeamLeaderEmail).FirstOrDefault();
                        teamLeaderEmail = lead.Email;
                        var teamLeadername = lead.FirstName + " "+ lead.LastName;
                        teamleademail.ToEmail = teamLeaderEmail;
                        teamleademail.Subject = $"Leave Application Approval for {userName}";
                        teamleademail.Body = $"Dear " + teamLeadername + "\r\n\r\nI am writing this to inform you that your team Member " + userName + " has applied on a leave from " + leaveCountsData.Fromdate.Value.ToString("d/M/yyyy") + " to " + leaveCountsData.Todate.Value.ToString("d/M/yyyy") + " for a total of " + leaveCountsData.Count + " days. It has already been Approved from my end. So, please keep a note of that.\r\n\r\nThank You\r\n\r\n" + hrName;
                        SendMail(teamleademail);
                    }

                    request.LeaveMessage = "Create new leave count item success.";
                }
                else
                {
                    teamLeaders = _context.TeamLeader.Where(x => x.EmployeeId == leaveCountsData.Userid).ToList();
                    var employee = _context.ApplicationUser.Where(x => x.Id == leaveCountsData.Userid).First();
                    // Send the Leave Email to HR, TeamLead and SuperAdmin
                    var userName = employee.FirstName + " " + employee.LastName;
                    var adminMail = _userManager.GetUsersInRoleAsync("SuperAdmin").Result.FirstOrDefault().Email;
                    var hrMail = _userManager.GetUsersInRoleAsync("HR").Result.FirstOrDefault().Email;
                    var teamLeaderEmail = "";
                    if (teamLeaders.Count > 0)
                    {
                        teamLeaderEmail = _userManager.GetUsersInRoleAsync("TeamAdmin").Result.Where(x => x.Email == teamLeaders.First().TeamLeaderEmail).FirstOrDefault().Email;
                    }

                    var email = new MailRequest();
                    email.ToEmail = adminMail;
                    email.Cc = hrMail;
                    email.Bcc = teamLeaderEmail;
                    email.Subject = "Leave Application By: " + userName;
                    email.Body = "Dear Sir/Maam\r\n\r\nI am writing this to inform you that I need to take a leave from " + leaveCountsData.Fromdate.Value.ToString("d/M/yyyy") + " to " + leaveCountsData.Todate.Value.ToString("d/M/yyyy") + " for a total of " + leaveCountsData.Count + " days as " + leaveCountsData.EmployeeDescription + " .\r\n\r\n\r\nSo, Please Consider My Leave Application. Please feel free to contact me at my email id (" + user.Email + ") and phone number (" + user.PhoneNumber + "). \r\n\r\nThank You\r\n\r\nYours Sincerely\r\n\r\n" + userName;
                    SendMail(email);
                }

                if (leaveCountsData.Id == 0)
                {
                    _context.LeaveCount.Add(leaveCount);
                    _context.SaveChanges();

                    _context.LeaveHistory.Add(leaveHistory);
                    _context.SaveChanges();

                    if (suadminrole == true
                    || (hrrole == true && leaveCount.Userid != user.Id)
                    || (teamAdmin == true && leaveCount.Userid != user.Id))
                    {
                        leaveCount.Isapprove = true;
                        leaveCount.Approveby = userDT.Id;
                        leaveCount.ApproveDate = DateTime.Now;
                        await PaidLeaveMangement(leaveCount);
                    }
                }
                else
                {
                    _context.Update(leaveCount);
                    _context.Add(leaveHistory);

                    _context.SaveChanges();
                }
                request.Id = leaveCount.Id;
                request.UserId = leaveCount.Userid;

                return request;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<IActionResult> PaidLeaveMangement(LeaveCount leaveCount)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var userLeaveManager = _context.LeaveManagement.Where(x => x.UserId.ToString() == leaveCount.Userid).OrderByDescending(x => x.FromDate).FirstOrDefault();
                var users = _context.ApplicationUser.Where(x => x.Id == leaveCount.Userid).FirstOrDefault();

                var totalleave = leaveCount.Count;
                userLeaveManager.TotalLeaveDay = userLeaveManager.TotalLeaveDay + Convert.ToDecimal(totalleave);
                _context.LeaveManagement.Update(userLeaveManager);

                decimal remaining = Convert.ToDecimal(totalleave);
                if (userLeaveManager.LeaveBuket > 0)
                {
                    if (userLeaveManager.LeaveBuket > Convert.ToDecimal(totalleave))
                    {
                        userLeaveManager.PaidLeaveCount = userLeaveManager.PaidLeaveCount + Convert.ToDecimal(totalleave);
                        remaining = userLeaveManager.LeaveBuket - remaining;
                        userLeaveManager.LeaveBuket = remaining;
                        _context.LeaveManagement.Update(userLeaveManager);
                        users.PaidLeave = users.PaidLeave - Convert.ToDecimal(totalleave);
                        _context.Users.Update(users);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        remaining = remaining - userLeaveManager.LeaveBuket;
                        userLeaveManager.PaidLeaveCount = userLeaveManager.LeaveBuket;
                        userLeaveManager.LeaveBuket = 0;
                        _context.LeaveManagement.Update(userLeaveManager);
                        users.PaidLeave = user.PaidLeave - userLeaveManager.LeaveBuket;
                        _context.Users.Update(users);
                        await _context.SaveChangesAsync();

                        if (remaining > 0)
                        {
                            remaining = remaining - 1;
                            userLeaveManager.PaidLeaveCount++;
                            userLeaveManager.IsMonthlyLeaveLeft = false;
                            _context.LeaveManagement.Update(userLeaveManager);
                            users.PaidLeave--;
                            _context.Users.Update(users);
                            await _context.SaveChangesAsync();

                            if (remaining > 0)
                            {
                                userLeaveManager.UnPaidLeaveCount = userLeaveManager.UnPaidLeaveCount + remaining;
                                _context.LeaveManagement.Update(userLeaveManager);
                                users.UnpaidLeave = userLeaveManager.UnPaidLeaveCount;
                                _context.Users.Update(users);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
                else
                {
                    if (userLeaveManager.IsMonthlyLeaveLeft == true)
                    {
                        remaining = remaining - 1;
                        userLeaveManager.PaidLeaveCount++;
                        userLeaveManager.IsMonthlyLeaveLeft = false;
                        _context.LeaveManagement.Update(userLeaveManager);
                        users.PaidLeave--;
                        _context.Users.Update(users);
                        await _context.SaveChangesAsync();

                        if (remaining > 0)
                        {
                            userLeaveManager.UnPaidLeaveCount = userLeaveManager.UnPaidLeaveCount + remaining;
                            _context.LeaveManagement.Update(userLeaveManager);
                            users.UnpaidLeave = userLeaveManager.UnPaidLeaveCount;
                            _context.Users.Update(users);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        userLeaveManager.UnPaidLeaveCount = userLeaveManager.UnPaidLeaveCount + remaining;
                        _context.LeaveManagement.Update(userLeaveManager);
                        users.UnpaidLeave = userLeaveManager.UnPaidLeaveCount;
                        _context.Users.Update(users);
                        await _context.SaveChangesAsync();
                    }
                }
                var result = new { Success = "true", Message = "Leave Managesd Successfully." };
                return Json(result);
            }
            catch (Exception ex)
            {
                var result = new { Success = "False", ex.Message };
                return Json(result);
            }
        }
    }
}


