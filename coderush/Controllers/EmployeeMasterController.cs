using coderush.Data;
using coderush.DataEnum;
using coderush.Models;
using coderush.Models.ViewModels;
using coderush.Services.Security;
using coderush.ViewModels;
using CodesDotHRMS.Data;
using CodesDotHRMS.Helper;
using CodesDotHRMS.Models;
using CodesDotHRMS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Controllers
{
    [Authorize(Roles = "SuperAdmin,HR")]
    public class EmployeeMasterController : Controller
    {
        private readonly Services.Security.ICommon _security;
        private readonly IdentityDefaultOptions _identityDefaultOptions;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        //dependency injection through constructor, to directly access services  // Harshal Working on
        public EmployeeMasterController(
            Services.Security.ICommon security,
            IOptions<IdentityDefaultOptions> identityDefaultOptions,
            IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions,
            ApplicationDbContext context,
             IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
            )
        {
            _security = security;
            _identityDefaultOptions = identityDefaultOptions.Value;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmployeeInfo(bool? isWFH = false)
        {
            var adminUser = _configuration.GetSection("AdminUser").Get<AdminUser>();
            List<ApplicationUser> users = new List<ApplicationUser>();
            List<ApplicationUser> finaluser = new List<ApplicationUser>();
            ApplicationUser usersm = new ApplicationUser();
            users = _security.GetAllMembers().Where(x => x.Email != adminUser.Email).ToList();
            var lastMonthAsString = System.DateTime.Now.ToString("Y");

            foreach (var ldata in users)
            {
                usersm = _context.ApplicationUser.Where(x => x.Id == ldata.Id).FirstOrDefault();
                usersm.DOB = ldata.DOB.HasValue ? ldata.DOB : null;
                var month = ldata.JoiningDate.HasValue ? ldata.JoiningDate.Value.Month : 0;
                var finmalmoth = 12 - month;
                var pl = 0;
                var regularlevs = 0;

                if (ldata.JoiningDate.HasValue)
                {
                    var totalleves = new List<LeaveCount>();
                    try
                    {
                        totalleves = _context.LeaveCount
                            .Where(x => x.Userid == ldata.Id && x.Fromdate.Value.Month >= ldata.JoiningDate.Value.Month && x.Fromdate.Value.Year == DateTime.Now.Year)
                            .ToList();

                        foreach (var data1 in totalleves)
                        {
                            if (data1.Count == "1")
                            {
                                pl++;
                            }
                            else if (Convert.ToInt32(data1.Count) > 1)
                            {
                                pl++;
                                regularlevs = regularlevs + Convert.ToInt32(data1.Count) - 1;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
                usersm.IsWFH = usersm.IsWFH != null ? usersm.IsWFH : false;
                usersm.Pl = "" + ldata.PaidLeave + "/" + finmalmoth + "";
                usersm.UnpaidLeave = ldata.UnpaidLeave != null ? ldata.UnpaidLeave : 0;
                usersm.PhoneNumber = ldata.PhoneNumber != null ? ldata.PhoneNumber : "";
                usersm.ProfilePicture = ldata.ProfilePicture != null ? ldata.ProfilePicture : "";
                finaluser.Add(usersm);
            }

            return View("~/Views/EmployeeMaster/RegularEmployee.cshtml", finaluser.Where(x => x.IsWFH == isWFH).ToList());
        }

        //display change profile screen if member founded, otherwise 404
        [HttpGet]
        public IActionResult ChangeProfile(string id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var appUser = _security.GetMemberByApplicationId(id);

            if (appUser == null)
            {
                return NotFound();
            }

            ViewBag.TeamLeadDDL = (from emp in _userManager.GetUsersInRoleAsync("TeamAdmin").Result
                                   select new EmployeeTypeViewModel { empId = emp.Id, TextVal = emp.FirstName + " " + emp.LastName }).ToList();

            ViewBag.EmployeeTypeDDL = (from emp in _context.Datamaster
                                       where emp.Isdeleted != true && emp.Type == DataSelection.EmployeeType
                                       select new EmployeeTypeViewModel { Id = emp.Id, TextVal = emp.Text }).ToList();

            var teamleaddata = _context.TeamLeader.Where(x => x.EmployeeId == appUser.Id).FirstOrDefault();
            if (teamleaddata != null)
                ViewBag.TeamLeaderValue = _context.TeamLeader.Where(x => x.EmployeeId == appUser.Id).FirstOrDefault().TeamLeaderId;
            else
                ViewBag.TeamLeaderValue = "0";

            var data = new ApplicationViewModel();

            data = (from a in _context.ApplicationUser
                    where a.Id == id
                    select new ApplicationViewModel()
                    {
                        UserName = a.UserName,
                        EmpCode = a.EmpCode,
                        Email = a.Email,
                        PhoneNumber = a.PhoneNumber,
                        ProfilePictureString = a.ProfilePicture,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        EmployeeType = a.EmployeeType,
                        Salary = a.Salary,
                        EmailConfirmed = a.EmailConfirmed,
                        EndOfDate = a.EndOfDate,
                        AadharCardNo = a.AadharCardNo,
                        PanCardNo = a.PanCardNo,
                        BankAccNo = a.AccountNumber,
                        IFSCode = a.IFSCode,
                        BankBranchName = a.BankBranchName,
                        BankName = a.BankName,
                        isWFH = a.IsWFH,
                    }).FirstOrDefault();
            return View(data);
        }

        //post submited change profile request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitChangeProfile([Bind("Id,EmailConfirmed,Email,FirstName,LastName,PhoneNumber,isWFH,ProfilePicture,Salary,EmployeeType,TeamLeaderId,EmpCode,PanCardNo,AadharCardNo,BankAccNo,IFSCode,BankName,BankBranchName")] ApplicationViewModel applicationUser)
        {
            try
            {

                if (applicationUser == null)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ChangeProfile), new { id = applicationUser.Id });
                }

                string wwwPath = this._webHostEnvironment.WebRootPath;
                string contentPath = this._webHostEnvironment.ContentRootPath;

                var updatedUser = _security.GetMemberByApplicationId(applicationUser.Id);
                if (updatedUser == null)
                {
                    TempData[StaticString.StatusMessage] = "Error: Can not found the member.";
                    return RedirectToAction(nameof(ChangeProfile), new { id = applicationUser.Id });
                }

                //if (_identityDefaultOptions.IsDemo && _superAdminDefaultOptions.Email.Equals(applicationUser.Email))
                //{
                //    TempData[StaticString.StatusMessage] = "Error: Demo mode can not change super@admin.com data.";
                //    return RedirectToAction(nameof(ChangeProfile), new { id = applicationUser.Id });
                //}                

                updatedUser.Email = applicationUser.Email;
                updatedUser.EmpCode = applicationUser.EmpCode;
                updatedUser.FirstName = applicationUser.FirstName;
                updatedUser.LastName = applicationUser.LastName;
                updatedUser.PhoneNumber = applicationUser.PhoneNumber;
                updatedUser.EmailConfirmed = applicationUser.EmailConfirmed;
                updatedUser.EndOfDate = applicationUser.EndOfDate;
                updatedUser.BankName = applicationUser.BankName;
                updatedUser.BankBranchName = applicationUser.BankBranchName;
                updatedUser.AccountNumber = applicationUser.BankAccNo;
                updatedUser.IFSCode = applicationUser.IFSCode;
                updatedUser.AadharCardNo = applicationUser.AadharCardNo;
                updatedUser.PanCardNo = applicationUser.PanCardNo;
                updatedUser.IsWFH = applicationUser.isWFH;

                //updatedUser.JoiningDate = applicationUser.JoiningDate;
                if (applicationUser.ProfilePicture != null)
                {
                    var filename = applicationUser.ProfilePicture.FileName.ToString();
                    string path = Path.Combine(this._webHostEnvironment.WebRootPath, "document/Userimage");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    List<string> uploadedFiles = new List<string>();

                    string fileName = Path.GetFileName(applicationUser.ProfilePicture.FileName);
                    string filetext = Path.GetExtension(fileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, applicationUser.Id + filetext), FileMode.Create))
                    {
                        applicationUser.ProfilePicture.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                    }
                    updatedUser.ProfilePicture = applicationUser.Id + filetext;
                }
                updatedUser.Salary = applicationUser.Salary.Value;
                updatedUser.EmployeeType = applicationUser.EmployeeType;

                _context.Update(updatedUser);

                // update in team leader table
                var teamleader = _context.TeamLeader.Where(x => x.EmployeeId == updatedUser.Id).FirstOrDefault();
                var tldata = _context.ApplicationUser.Where(x => x.Id == applicationUser.TeamLeaderId).FirstOrDefault();
                if (teamleader == null)
                {
                    _context.TeamLeader.Add(new TeamLeader()
                    {
                        TeamLeaderId = tldata.Id,
                        TeamLeaderEmail = tldata.Email,
                        EmployeeId = applicationUser.Id,
                        EmployeeEmail = applicationUser.Email
                    });
                }
                else
                {
                    teamleader.TeamLeaderId = tldata.Id;
                    teamleader.TeamLeaderEmail = tldata.Email;
                    _context.Update(teamleader);
                }
                await _context.SaveChangesAsync();
                TempData[StaticString.StatusMessage] = "Update success";
                return RedirectToAction(nameof(ChangeProfile), new { id = updatedUser.Id });
            }
            catch (Exception ex)
            {
                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ChangeProfile), new { id = applicationUser.Id });
            }

        }

        //display change password screen if user founded, otherwise 404
        [HttpGet]
        public IActionResult ChangePassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = _security.GetMemberByApplicationId(id);
            if (member == null)
            {
                return NotFound();
            }

            ResetPassword cp = new ResetPassword();
            cp.Id = id;
            cp.UserName = member.UserName;

            return View(cp);
        }

        //post submitted change password request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitChangePassword([Bind("Id,OldPassword,NewPassword,ConfirmPassword")] ResetPassword changePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
                }

                var member = _security.GetMemberByApplicationId(changePassword.Id);

                if (member == null)
                {
                    TempData[StaticString.StatusMessage] = "Error: Can not found the member.";
                    return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
                }

                if (_identityDefaultOptions.IsDemo && member.Email == "super@admin.com")
                {
                    TempData[StaticString.StatusMessage] = "Error: Demo mode can not change super@admin.com data.";
                    return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
                }
                var tokenResetPassword = await _userManager.GeneratePasswordResetTokenAsync(member);
                var changePasswordResult = await _userManager.ResetPasswordAsync(member, tokenResetPassword, changePassword.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    TempData[StaticString.StatusMessage] = "Error: ";
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        TempData[StaticString.StatusMessage] = TempData[StaticString.StatusMessage] + " " + error.Description;
                    }
                    return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
                }

                TempData[StaticString.StatusMessage] = "Reset password success";
                return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
            }
            catch (Exception ex)
            {
                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ChangePassword), new { id = changePassword.Id });
            }

        }

        //display change role screen if user founded, otherwise 404
        [HttpGet]
        public async Task<IActionResult> ChangeRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = _security.GetMemberByApplicationId(id);
            if (member == null)
            {
                return NotFound();
            }

            var registeredRoles = await _userManager.GetRolesAsync(member);

            ChangeRoles changeRole = new ChangeRoles();
            changeRole.Id = id;
            changeRole.UserName = member.UserName;
            changeRole.HR = registeredRoles.Contains("HR") ? true : false;
            changeRole.SuperAdmin = registeredRoles.Contains("SuperAdmin") ? true : false;
            changeRole.TeamAdmin = registeredRoles.Contains("TeamAdmin") ? true : false;
            changeRole.Employee = registeredRoles.Contains("Employee") ? true : false;
            changeRole.Sales = registeredRoles.Contains("Sales") ? true : false;

            return View(changeRole);
        }

        //post submitted change role request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitChangeRole([Bind("Id", "HR", "TeamAdmin", "SuperAdmin", "Employee", "Sales")] ChangeRoles changeRoles)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(ChangeRole), new { id = changeRoles.Id });
                }

                var member = _security.GetMemberByApplicationId(changeRoles.Id);
                if (member == null)
                {
                    TempData[StaticString.StatusMessage] = "Error: Can not found the member.";
                    return RedirectToAction(nameof(ChangeRole), new { id = changeRoles.Id });
                }

                //if (_identityDefaultOptions.IsDemo && _superAdminDefaultOptions.Email.Equals(member.Email))
                //{
                //    TempData[StaticString.StatusMessage] = "Error: Demo mode can not change super@admin.com data.";
                //    return RedirectToAction(nameof(ChangeRole), new { id = changeRoles.Id });
                //}

                //todo role
                //if (changeRoles.IsTodoRegistered)
                //{
                //    await _userManager.AddToRoleAsync(member, "Todo");
                //}
                //else
                //{
                //    await _userManager.RemoveFromRoleAsync(member, "Todo");
                //}

                ////membership role
                //if (changeRoles.IsMembershipRegistered)
                //{
                //    await _userManager.AddToRoleAsync(member, "Membership");
                //}
                //else
                //{
                //    await _userManager.RemoveFromRoleAsync(member, "Membership");
                //}

                ////role role
                //if (changeRoles.IsRoleRegistered)
                //{
                //    await _userManager.AddToRoleAsync(member, "Role");
                //}
                //else
                //{
                //    await _userManager.RemoveFromRoleAsync(member, "Role");
                //}
                //if (changeRoles.IsCandidateMasterRegistered)
                //{
                //    await _userManager.AddToRoleAsync(member, "CandidateMaster");
                //}
                //else
                //{
                //    await _userManager.RemoveFromRoleAsync(member, "CandidateMaster");
                //}
                //if (changeRoles.IsDataMasterRegistered)
                //{
                //    await _userManager.AddToRoleAsync(member, "DataMaster");
                //}
                //else
                //{
                //    await _userManager.RemoveFromRoleAsync(member, "DataMaster");
                //}
                //if (changeRoles.IsExpenseMasterRegistered)
                //{
                //    await _userManager.AddToRoleAsync(member, "ExpenseMaster");
                //}
                //else
                //{
                //    await _userManager.RemoveFromRoleAsync(member, "ExpenseMaster");
                //}
                //if (changeRoles.IsInvoiceMasterRegistered)
                //{
                //    await _userManager.AddToRoleAsync(member, "InvoiceMaster");
                //}
                //else
                //{
                //    await _userManager.RemoveFromRoleAsync(member, "InvoiceMaster");
                //}
                //if (changeRoles.IsLeadMasterRegistered)
                //{
                //    await _userManager.AddToRoleAsync(member, "LeadMaster");
                //}
                //else
                //{
                //    await _userManager.RemoveFromRoleAsync(member, "LeadMaster");
                //}
                //if (changeRoles.IsLeavecountRegistered)
                //{
                //    await _userManager.AddToRoleAsync(member, "Leavecount");
                //}
                //else
                //{
                //    await _userManager.RemoveFromRoleAsync(member, "Leavecount");
                //}
                //if (changeRoles.IsProjectMasterRegistered)
                //{
                //    await _userManager.AddToRoleAsync(member, "ProjectMaster");
                //}
                //else
                //{
                //    await _userManager.RemoveFromRoleAsync(member, "ProjectMaster");
                //}
                if (changeRoles.HR)
                {
                    await _userManager.AddToRoleAsync(member, "HR");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "HR");
                }
                if (changeRoles.SuperAdmin)
                {
                    await _userManager.AddToRoleAsync(member, "SuperAdmin");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "SuperAdmin");
                }
                if (changeRoles.TeamAdmin)
                {
                    await _userManager.AddToRoleAsync(member, "TeamAdmin");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "TeamAdmin");
                }
                if (changeRoles.Employee)
                {
                    await _userManager.AddToRoleAsync(member, "Employee");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Employee");
                }
                if (changeRoles.Sales)
                {
                    await _userManager.AddToRoleAsync(member, "Sales");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(member, "Sales");
                }

                TempData[StaticString.StatusMessage] = "Update success";
                return RedirectToAction(nameof(ChangeRole), new { id = changeRoles.Id });
            }
            catch (Exception ex)
            {
                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(ChangeRole), new { id = changeRoles.Id });
            }

        }

        //display member registration screen
        [HttpGet]
        public IActionResult Register()
        {
            var lastcode = _context.ApplicationUser.ToList();
            var test = lastcode.LastOrDefault().EmpCode;
            int empcode;
            if (test == 0)
                empcode = 1;
            else
                empcode = test + 1;
            ViewBag.Code = empcode;
            ViewBag.TeamLeadRole = new SelectList((_userManager.GetUsersInRoleAsync("TeamAdmin")).Result.OrderBy(l => l.Id));
            ViewBag.EmployeeType = (from emp in _context.Datamaster
                                    where emp.Isdeleted != true && emp.Type == DataSelection.EmployeeType
                                    select new EmployeeTypeViewModel { Id = emp.Id, TextVal = emp.Text }).ToList();
            return View();
        }

        //post submitted registration request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitRegister([Bind("EmailConfirmed,Email,FirstName,LastName,PhoneNumber,JoiningDate,isWFH,Password,ConfirmPassword,DOB,Salary,EmpCode,TeamLeader,EmployeeType,EmpCode,PanCardNo,AadharCardNo,BankAccNo,IFSCode,BankName,BankBranchName")] Register register)
        {
            var respmodel = new object();
            try
            {
                var varr = _context.ApplicationUser.Where(s => s.EmpCode == register.EmpCode).FirstOrDefault();
                if (varr != null)
                {
                    TempData[StaticString.StatusMessage] = "Error: employee code already exist ";
                    return RedirectToAction(nameof(Register));
                }

                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Register));
                }

                //string wwwPath = this._webHostEnvironment.WebRootPath;
                //string contentPath = this._webHostEnvironment.ContentRootPath;
                //var foldername = DateTime.Now.ToString("MMYYYY");

                //string path = Path.Combine(this._webHostEnvironment.WebRootPath, "document/Attendance/" + foldername);
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}

                //List<string> uploadedFiles = new List<string>();
                //foreach (IFormFile postedFile in postedFiles)
                //{
                //    string filename = Path.GetFileName(postedFile.FileName);
                //    using (FileStream stream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                //    {
                //        postedFile.CopyTo(stream);
                //        uploadedFiles.Add(filename);



                //        //        //ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                //    }
                //}

                //if (applicationUser.EmpCode != null)
                //{
                //    string wwwPath = this._webHostEnvironment.WebRootPath;
                //    string contentPath = this._webHostEnvironment.ContentRootPath;
                //    var fileexten = Path.GetExtension(applicationUser.EmpCode.ToString());

                //    var filename = string.Format("{0}{1}", Guid.NewGuid(), fileexten);
                //    var foldername = DateTime.Now.ToString("MMYYYY");

                //    string path = Path.Combine(this._webHostEnvironment.WebRootPath, "document/Attendance/" + foldername);
                //    if (!Directory.Exists(path))
                //    {
                //        Directory.CreateDirectory(path);
                //    }

                //    List<string> uploadedFiles = new List<string>();                  
                //    string fileName = Path.GetFileName(applicationUser.EmpCode.ToString());
                //    using (FileStream stream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                //    {
                //        //applicationUser.EmpCode.CopyTo(stream);
                //        uploadedFiles.Add(fileName);
                //    }
                //}

                //string wwwPath = this._webHostEnvironment.WebRootPath;
                //string contentPath = this._webHostEnvironment.ContentRootPath;
                //var filename = applicationUser.ProfilePicture.FileName.ToString();
                ////var getmonth = DateTime.Now.Month;
                ////var getyear = DateTime.Now.Year;
                ////var foldername = getmonth + getyear;

                //string path = Path.Combine(this._webHostEnvironment.WebRootPath, "document/Attendance/" + 012022 + "");
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}

                //List<string> uploadedFiles = new List<string>();

                //string fileName = Path.GetFileName(applicationUser.ProfilePicture.FileName);
                //string filetext = Path.GetExtension(fileName);
                //using (FileStream stream = new FileStream(Path.Combine(path, applicationUser.Id + filetext), FileMode.Create))
                //{
                //    applicationUser.ProfilePicture.CopyTo(stream);
                //    uploadedFiles.Add(fileName);
                //}

                //var updatedUser = _security.GetMemberByApplicationId(applicationUser.Id);
                //if (updatedUser == null)
                //{
                //    TempData[StaticString.StatusMessage] = "Error: Can not found the member.";
                //    return RedirectToAction(nameof(ChangeProfile), new { id = applicationUser.Id });
                //}

                ApplicationUser newMember = new ApplicationUser();
                newMember.Email = register.Email;
                newMember.EmpCode = register.EmpCode;
                newMember.FirstName = register.FirstName;
                newMember.JoiningDate = register.JoiningDate;
                newMember.LastName = register.LastName;
                newMember.UserName = register.Email;
                newMember.PhoneNumber = register.PhoneNumber;
                newMember.EmailConfirmed = register.EmailConfirmed;
                newMember.DOB = register.DOB;
                newMember.Salary = register.Salary;
                newMember.isSuperAdmin = false;
                newMember.EmployeeType = register.EmployeeType;
                newMember.BankName = register.BankName;
                newMember.IFSCode = register.IFSCode;
                newMember.BankBranchName = register.BankBranchName;
                newMember.AccountNumber = register.BankAccNo;
                newMember.AadharCardNo = register.AadharCardNo;
                newMember.PanCardNo = register.PanCardNo;
                newMember.IsWFH = register.isWFH != null ? register.isWFH : false;


                var result = await _userManager.CreateAsync(newMember, register.Password);
                var tlid = _context.ApplicationUser.Where(x => x.Email == "jenit@codesdot.com").FirstOrDefault();
                await _userManager.AddToRoleAsync(newMember, "Employee");

                var leaderIdList = _context.ApplicationUser.Where(x => x.Email == register.TeamLeader).FirstOrDefault();
                //var employeeIdList = _context.ApplicationUser.Where(x => x.Email == register.Email);

                TeamLeader teamLeader = new TeamLeader();
                if (leaderIdList == null)
                    teamLeader.TeamLeaderId = tlid.Id;
                else
                    teamLeader.TeamLeaderId = leaderIdList.Id;
                teamLeader.TeamLeaderEmail = register.TeamLeader;
                teamLeader.EmployeeId = newMember.Id;
                teamLeader.EmployeeEmail = register.Email;
                _context.TeamLeader.Add(teamLeader);
                _context.SaveChanges();

                if (result.Succeeded)
                {
                    TempData[StaticString.StatusMessage] = "Register new member success";
                    return RedirectToAction(nameof(ChangeProfile), new { id = newMember.Id });
                }
                else
                {
                    TempData[StaticString.StatusMessage] = "Error: Register new member not success";
                    return RedirectToAction(nameof(Register));
                }

            }
            catch (Exception ex)
            {
                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Register));
            }
        }

        //  Save Membership indes Deatails
        [HttpPost]
        public ActionResult SaveMemberIndex(List<IFormFile> FormFile)
        {
            string path = Path.Combine(this._webHostEnvironment.WebRootPath, "document/RecordFile");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var formFile in FormFile)
            {
                if (formFile.Length > 0)
                {
                    string fileName = Path.GetFileName(formFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        formFile.CopyToAsync(stream);
                    }
                }
            }
            return RedirectToAction("Index", "Membership");
        }
        [HttpGet]
        public JsonResult SalaryEmployee()
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            users = _security.GetAllMembers();

            return Json(users);
        }

        [HttpPost]
        public ActionResult SaveSalary(List<SaveEmployeeHistoryModel> request)
        {
            try
            {

                if (request != null && request.Count > 0)
                {
                    List<EmployeeHistory> employeeHistories = new List<EmployeeHistory>();

                    employeeHistories = (from a in request
                                         select new EmployeeHistory()
                                         {
                                             UserId = a.userid,
                                             Salary = a.salary,
                                             Date = DateTime.Now
                                         }).ToList();

                    _context.EmployeeHistory.AddRange(employeeHistories);
                    _context.SaveChanges();
                }

                var result = new { Success = "true", Message = "Data save successfully." };
                return Json(result);
            }
            catch (Exception ex)
            {
                var result = new { Success = "False", Message = ex.Message };
                return Json(result);
            }

        }

        [HttpPost]
        public async Task<IActionResult> ImportMonthlyReport(IFormFile FileUpload)
        {
            try
            {
                if (FileUpload != null && FileUpload.Length > 0)
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    string wwwPath = this._webHostEnvironment.WebRootPath;
                    string contentPath = this._webHostEnvironment.ContentRootPath;
                    var filename = FileUpload.FileName;
                    List<int> numbers = new List<int>();
                    List<int> NotFoundEmpCode = new List<int>();

                    string path = Path.Combine(this._webHostEnvironment.WebRootPath, "document\\MonthlyReport");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    List<string> uploadedFiles = new List<string>();

                    string fileName = Path.GetFileName(FileUpload.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        FileUpload.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                    }
                    var whfTimeline = new List<CodesDotHRMS.Models.TimeSheet>();
                    var manualTimeline = new List<ManualTimeSheet>();
                    var filepath = path + "\\" + filename;

                    using (var package = new ExcelPackage(new FileInfo(filepath)))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        if (worksheet != null)
                        {
                            int rowCount = worksheet.Dimension.Rows;
                            var dataModel = new MonthlyReport();
                            var monthlyRepList = new List<MonthlyReport>();
                            var totalHourModel = new MonthlyTotalHours();
                            int lastDayOfMonth = 0;
                            decimal dtValue = 0;
                            decimal difftime = 0;
                            int code = 0;
                            string month = "";
                            bool isedit = false;
                            bool chkFirstLine = true;
                            var attType = "";
                            string cellValue = "";
                            bool isSaveData = false;
                            decimal Arrsum = 0, ArrVal = 0, Out1 = 0, In2 = 0, Out2 = 0, In3 = 0, Out3 = 0, In4 = 0, Out4 = 0, In5 = 0, Out5 = 0, In6 = 0, Out6 = 0, In7 = 0, Out7 = 0, In8 = 0, Out8 = 0, In9 = 0, DepSum = 0, DeptVal = 0;
                            decimal OutVal1 = 0, InVal2 = 0, OutVal2 = 0, InVal3 = 0, OutVal3 = 0, InVal4 = 0, OutVal4 = 0, InVal5 = 0, OutVal5 = 0, InVal6 = 0, OutVal6 = 0, InVal7 = 0, OutVal7 = 0, InVal8 = 0, OutVal8 = 0, InVal9 = 0;
                            decimal WorkHrs = 9;
                            var currentMonth = string.Empty;
                            bool IsWFH = false;
                            bool IsManual = false;
                            int MM = 0;
                            int noCountWHF = 0;
                            int rowIndex = 1;

                            decimal monthworkhrs = 0;


                            for (int row = 3; row <= rowCount; row++) // Assuming the first row is the header
                            {
                                rowIndex++;
                                var text = "";
                                if (chkFirstLine == true)
                                {
                                    text = GetCellValue<string>(worksheet.Cells[row, 1]);
                                    chkFirstLine = false;
                                }
                                else
                                {
                                    text = GetCellValue<string>(worksheet.Cells[row, 1]);
                                    if (text == "")
                                    {
                                        text = GetCellValue<string>(worksheet.Cells[row, 2]);
                                        if (text == "")
                                        {
                                            text = GetCellValue<string>(worksheet.Cells[row, 12]);
                                            if (text != "")
                                            {
                                                string[] parts = text.Split("To");

                                                if (parts.Length > 1)
                                                {
                                                    string substring = parts[0].Trim();
                                                    string[] splitDataMonth = text.Split(":");
                                                    DateTime dtSpliteDate = Convert.ToDateTime(splitDataMonth[2].ToString());
                                                    //string monthName = substring.Split(' ')[0].Trim();
                                                    //string monthNumber = DateTime.ParseExact(dtSpliteDate.Month, "MMM", CultureInfo.CurrentCulture).Month.ToString();
                                                    string monthNumber = dtSpliteDate.Month.ToString();
                                                    int year = int.Parse(dtSpliteDate.Year.ToString());
                                                    lastDayOfMonth = DateTime.DaysInMonth(year, Convert.ToInt32(monthNumber));
                                                    MM = Convert.ToInt32(monthNumber);
                                                    dataModel.Month = monthNumber + "/" + year;
                                                    currentMonth = dataModel.Month;
                                                    totalHourModel.Month = monthNumber + "/" + year;
                                                    month = totalHourModel.Month;
                                                    worksheet.DeleteRow(row);
                                                    row--;
                                                    rowCount--;
                                                    // break;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (text == "" || text == "Monthly All In-Out Report\n" || text == "codesdot solutions LLP" || text == "Emp.code" || text == "Branch : DEMO"
                                    || text == "Department : STAFF" || text == "Total For Company :" || text == "Print Date :")
                                {
                                    worksheet.DeleteRow(row);
                                    row--;
                                    rowCount--;
                                }
                                else if (text.Contains("0000"))
                                {
                                    dataModel.EmployeeCode = GetCellValue<int>(worksheet.Cells[row, 2]);
                                    totalHourModel.EmployeeCode = GetCellValue<int>(worksheet.Cells[row, 2]);
                                    code = totalHourModel.EmployeeCode;
                                    var IsValid = IsValidEmployee(code);
                                    if (IsValid == false)
                                    {
                                        NotFoundEmpCode.Add(code);
                                        //break;
                                        continue;
                                    }

                                    IsWFH = false;
                                    manualTimeline = GetManualTimeSheetData(code, MM);
                                    if (manualTimeline.Count > 0)
                                    {
                                        IsManual = true;
                                        foreach (var wftData in manualTimeline)
                                        {
                                            numbers.Add(wftData.ManualDate.Day);
                                        }
                                    }

                                    whfTimeline = GetWorkFromHome(code, MM);
                                    if (whfTimeline.Count > 0)
                                    {
                                        IsWFH = true;
                                        foreach (var wftData in whfTimeline)
                                        {
                                            for (DateTime date = wftData.FromDate; date.Date <= wftData.ToDate; date = date.AddDays(1))
                                            {
                                                if ((date.DayOfWeek != DayOfWeek.Saturday) && (date.DayOfWeek != DayOfWeek.Sunday))
                                                {
                                                    numbers.Add(date.Day);
                                                }
                                            }
                                        }
                                    }
                                    worksheet.DeleteRow(row);
                                    row--;
                                    rowCount--;
                                    monthlyRepList = new List<MonthlyReport>();
                                    isSaveData = false;
                                    Arrsum = Out1 = Out2 = Out3 = Out4 = Out5 = Out6 = In2 = In3 = In4 = In5 = In6 = In7 = DepSum = 0;
                                    OutVal1 = InVal2 = OutVal2 = InVal3 = OutVal3 = InVal4 = OutVal4 = InVal5 = OutVal5 = InVal6 = OutVal6 = InVal7 = 0;
                                    difftime = 0;
                                    DeptVal = 0;
                                    ArrVal = 0;
                                }
                                else if (text == "Date")
                                {
                                    int outCount = 1;
                                    int inCount = 1;

                                    monthlyRepList = _context.MonthlyReport.Where(x => x.EmployeeCode == code && x.Month == currentMonth).ToList();

                                    if (monthlyRepList.Count == 0)
                                    {
                                        for (int i = 1; i <= 20; i++)
                                        {

                                            if (i == 19)
                                            {
                                                attType = "WorkingHrs";
                                            }
                                            else if (i == 20)
                                            {
                                                attType = "ReportType";
                                            }
                                            //else if (i == 1 && IsWFH == false)
                                            //{
                                            //    attType = "ArrTim";
                                            //}
                                            //else if (i == 14 && IsWFH == false)
                                            //{
                                            //    attType = "DepTim";
                                            //}
                                            else if (i % 2 == 1 && IsWFH == false)
                                            {
                                                attType = "Out" + outCount;
                                                outCount++;
                                            }
                                            else if (i % 2 == 0 && IsWFH == false)
                                            {
                                                attType = "In" + inCount;
                                                inCount++;
                                            }

                                            monthlyRepList.Add(new MonthlyReport
                                            {
                                                DataType = attType,
                                                Month = dataModel.Month,
                                                EmployeeCode = dataModel.EmployeeCode,
                                            });
                                        }
                                        _context.MonthlyReport.AddRange(monthlyRepList);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        isedit = true;
                                    }
                                }
                                else
                                {
                                    int day;
                                    if (text.Contains(":"))
                                    {
                                        var splitDate = text.Split(":");
                                        var splitDay = splitDate[1].Split("/");
                                        day = Convert.ToInt32(splitDay[0].ToString());

                                    }
                                    else
                                    {
                                        var splitDate = text;
                                        var splitDay = splitDate.Split("/");
                                        day = Convert.ToInt32(splitDay[0].ToString());
                                    }
                                    //var splitDate = text.Split(":");
                                    //var splitDay = splitDate[1].Split("/");
                                    //int day = Convert.ToInt32(splitDate[2].ToString());
                                    //int day = Convert.ToInt32(splitDay[0].ToString());
                                    foreach (var mrData in monthlyRepList)
                                    {
                                        var resultDT = _context.MonthlyReport.FirstOrDefault(x => x.Id == mrData.Id);
                                        if (resultDT != null)
                                        {
                                            if (IsWFH == true || IsManual == true)
                                                noCountWHF = numbers.Where(x => x == day).FirstOrDefault();

                                            if (noCountWHF > 0) break;

                                            if (resultDT.DataType == "In1")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 2]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                if (dtValue == 0)
                                                {
                                                    cellValue = GetCellValue<string>(worksheet.Cells[row, 19]);
                                                    cellValue = cellValue.Replace(":", ".");
                                                    DeptVal = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                    if (DeptVal > 0)
                                                        dtValue = DeptVal - WorkHrs;
                                                }
                                                ArrVal = dtValue;
                                                Arrsum += dtValue;
                                            }
                                            else if (resultDT.DataType == "Out1")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 3]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                Out1 += dtValue;
                                                OutVal1 = dtValue;
                                            }
                                            else if (resultDT.DataType == "In2")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 4]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                In2 += dtValue;
                                                InVal2 = dtValue;
                                            }
                                            else if (resultDT.DataType == "Out2")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 5]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                Out2 += dtValue;
                                                OutVal2 = dtValue;
                                            }
                                            else if (resultDT.DataType == "In3")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 6]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                In3 += dtValue;
                                                InVal3 = dtValue;
                                            }
                                            else if (resultDT.DataType == "Out3")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 7]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                Out3 += dtValue;
                                                OutVal3 = dtValue;
                                            }
                                            else if (resultDT.DataType == "In4")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 8]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                In4 += dtValue;
                                                InVal4 = dtValue;
                                            }
                                            else if (resultDT.DataType == "Out4")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 9]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                Out4 += dtValue;
                                                OutVal4 = dtValue;
                                            }
                                            else if (resultDT.DataType == "In5")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 10]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                In5 += dtValue;
                                                InVal5 = dtValue;
                                            }
                                            else if (resultDT.DataType == "Out5")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 11]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                Out5 += dtValue;
                                                OutVal5 = dtValue;
                                            }
                                            else if (resultDT.DataType == "In6")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 12]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                In6 += dtValue;
                                                InVal6 = dtValue;
                                            }
                                            else if (resultDT.DataType == "Out6")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 13]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                Out6 += dtValue;
                                                OutVal6 = dtValue;
                                            }
                                            else if (resultDT.DataType == "In7")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 14]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                In7 += dtValue;
                                                InVal7 = dtValue;
                                            }
                                            else if (resultDT.DataType == "Out7")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 15]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                Out7 += dtValue;
                                                OutVal7 = dtValue;
                                            }
                                            else if (resultDT.DataType == "In8")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 16]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                In8 += dtValue;
                                                InVal8 = dtValue;
                                            }
                                            else if (resultDT.DataType == "Out8")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 17]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                Out8 += dtValue;
                                                InVal8 = dtValue;
                                            }
                                            else if (resultDT.DataType == "In9")
                                            {
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 18]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                In9 += dtValue;
                                                InVal9 = dtValue;
                                            }
                                            else if (resultDT.DataType == "Out9")
                                            {
                                                DeptVal = 0;
                                                cellValue = GetCellValue<string>(worksheet.Cells[row, 19]);
                                                cellValue = cellValue.Replace(":", ".");
                                                dtValue = IsValid(cellValue) == true ? 0 : Math.Round(Convert.ToDecimal(cellValue), 2);
                                                //if (dtValue == 0)
                                                //{
                                                //    if (ArrVal > 0)
                                                //    {
                                                //        var testdtValue = ArrVal + WorkHrs;
                                                //        DepSum += testdtValue;
                                                //        DeptVal = dtValue;
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    DepSum += dtValue;
                                                //    DeptVal = dtValue;
                                                //}
                                                DepSum += dtValue;
                                                DeptVal = dtValue;
                                            }
                                            else if (resultDT.DataType == "WorkingHrs")
                                            {
                                                dtValue = (OutVal1 - ArrVal) + (OutVal2 - InVal2) + (OutVal3 - InVal3) + (OutVal4 - InVal4) + (OutVal5 - InVal5) + (OutVal6 - InVal6) + (OutVal7 - InVal7) + (OutVal8 - InVal8) + (DeptVal - InVal9);
                                                monthworkhrs += dtValue;

                                                if (lastDayOfMonth == day)
                                                    isSaveData = true;
                                            }
                                            else if (resultDT.DataType == "ReportType")
                                            {
                                                dtValue = 1;
                                            }

                                            // add value for update
                                            GetDaywiseVal(day, dtValue, resultDT);
                                            _context.MonthlyReport.Update(resultDT);
                                            _context.SaveChanges();

                                            if (isSaveData == true)
                                            {
                                                //difftime = (Out1 - Arrsum) + (Out2 - In2) + (Out3 - In3) + (Out4 - In4) + (Out5 - In5) + (Out6 - In6) + (Out7 - In7) + (Out8 - In8) + (DepSum - In9);
                                                difftime = monthworkhrs;
                                                totalHourModel.TotalHours = difftime;
                                                totalHourModel.Month = month;
                                                totalHourModel.EmployeeCode = code;
                                                if (isedit == true)
                                                {
                                                    var existingTotalHours = _context.MonthlyTotalHours.Where(x => x.Month == currentMonth && x.EmployeeCode == totalHourModel.EmployeeCode).FirstOrDefault();
                                                    if (existingTotalHours != null)
                                                    {
                                                        existingTotalHours.TotalHours = totalHourModel.TotalHours;

                                                        // Update other properties as needed
                                                        _context.MonthlyTotalHours.Update(existingTotalHours);
                                                        _context.SaveChanges();
                                                        isSaveData = false;
                                                        isedit = false;
                                                    }
                                                }
                                                else
                                                {
                                                    totalHourModel.Id = 0;
                                                    _context.MonthlyTotalHours.Add(totalHourModel);
                                                    _context.SaveChanges();
                                                    isSaveData = false;
                                                    totalHourModel = new MonthlyTotalHours();
                                                }
                                                monthworkhrs = 0;
                                            }

                                        }
                                    }
                                }
                            }
                            var Msg = "Monthly Report Imprted successfully.";
                            if (NotFoundEmpCode.Count > 0)
                            {
                                for (int i = 0; i < NotFoundEmpCode.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        Msg += $"& NOT FOUND EMPLOYEE CODE LIST EmpCodes: {NotFoundEmpCode[i]}";
                                    }
                                    else
                                    {
                                        Msg += $", {NotFoundEmpCode[i]}";
                                    }
                                }
                            }

                            var result = new { Success = "true", Message = Msg };
                            return Json(result);
                        }
                    }
                }
                return Json(new { Success = "False", Message = "Please select file." });
            }
            catch (Exception ex)
            {
                var result = new { Success = "False", Message = ex.Message };
                return Json(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile FileUpload)
        {
            try
            {
                var adminUser = _configuration.GetSection("AdminUser").Get<AdminUser>();

                if (FileUpload != null && FileUpload.Length > 0)
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    string wwwPath = this._webHostEnvironment.WebRootPath;
                    string contentPath = this._webHostEnvironment.ContentRootPath;
                    var filename = FileUpload.FileName;

                    string path = Path.Combine(this._webHostEnvironment.WebRootPath, "document\\EmployeeImp");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    List<string> uploadedFiles = new List<string>();

                    string fileName = Path.GetFileName(FileUpload.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        FileUpload.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                    }

                    var filepath = path + "\\" + filename;
                    var newUser = new ApplicationUser();
                    var empUserName = "";
                    var teamLeader = "";
                    var employeeType = "";
                    TeamLeader teamLeaderDT = new TeamLeader();
                    var manageTeamLeader = new ApplicationUser();
                    var manageEmployeeType = new Models.DataMaster();

                    using (var package = new ExcelPackage(new FileInfo(filepath)))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet
                        if (worksheet != null)
                        {
                            int rowCount = worksheet.Dimension.Rows;
                            for (int row = 2; row <= rowCount; row++) // Assuming the first row is the header
                            {
                                empUserName = GetCellValue<string>(worksheet.Cells[row, 4]);
                                newUser = _context.ApplicationUser.Where(x => x.UserName == empUserName).FirstOrDefault();
                                teamLeader = GetCellValue<string>(worksheet.Cells[row, 15]);
                                employeeType = GetCellValue<string>(worksheet.Cells[row, 16]);

                                manageTeamLeader = _context.ApplicationUser.Where(x => x.FirstName == teamLeader).FirstOrDefault();

                                if (manageTeamLeader == null)
                                {
                                    manageTeamLeader = _context.ApplicationUser.Where(x => x.FirstName == adminUser.FirstName).FirstOrDefault();
                                }

                                manageEmployeeType = _context.Datamaster.Where(x => x.Text == employeeType && x.Isdeleted != true && x.Isactive == true).FirstOrDefault();
                                var isTrue = GetCellValue<string>(worksheet.Cells[row, 17]) == "1" ? true : false;
                                if (newUser != null)
                                {
                                    newUser.EmpCode = GetCellValue<int>(worksheet.Cells[row, 1]);
                                    newUser.FirstName = GetCellValue<string>(worksheet.Cells[row, 2]);
                                    newUser.LastName = GetCellValue<string>(worksheet.Cells[row, 3]);
                                    newUser.UserName = empUserName;
                                    newUser.PhoneNumber = GetCellValue<string>(worksheet.Cells[row, 5]);
                                    newUser.Email = GetCellValue<string>(worksheet.Cells[row, 6]);
                                    newUser.DOB = Convert.ToDateTime(GetCellValue<string>(worksheet.Cells[row, 7]));
                                    newUser.PanCardNo = GetCellValue<string>(worksheet.Cells[row, 8]);
                                    newUser.AadharCardNo = GetCellValue<string>(worksheet.Cells[row, 9]);
                                    newUser.BankName = GetCellValue<string>(worksheet.Cells[row, 10]);
                                    newUser.BankBranchName = GetCellValue<string>(worksheet.Cells[row, 11]);
                                    newUser.AccountNumber = GetCellValue<string>(worksheet.Cells[row, 12]);
                                    newUser.IFSCode = GetCellValue<string>(worksheet.Cells[row, 13]);
                                    newUser.Salary = GetCellValue<int>(worksheet.Cells[row, 14]);
                                    newUser.EmployeeType = manageEmployeeType.Id;
                                    newUser.EmailConfirmed = isTrue;

                                    _context.ApplicationUser.Update(newUser);
                                    _context.SaveChanges();

                                    //TeamLeader teamDT = new TeamLeader();
                                    var teamDT = _context.TeamLeader.Where(x => x.TeamLeaderId == manageTeamLeader.Id).FirstOrDefault();
                                    if (teamDT != null)
                                    {
                                        teamDT.TeamLeaderId = manageTeamLeader.Id;
                                        teamDT.TeamLeaderEmail = manageTeamLeader.Email;
                                        teamDT.EmployeeId = newUser.Id;
                                        teamDT.EmployeeEmail = newUser.Email;
                                        _context.TeamLeader.Update(teamDT);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        teamDT = new TeamLeader();

                                        teamDT.TeamLeaderId = manageTeamLeader.Id;
                                        teamDT.TeamLeaderEmail = manageTeamLeader.Email;
                                        teamDT.EmployeeId = newUser.Id;
                                        teamDT.EmployeeEmail = newUser.Email;
                                        _context.TeamLeader.Add(teamDT);
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    newUser = new ApplicationUser()
                                    {
                                        EmpCode = GetCellValue<int>(worksheet.Cells[row, 1]),
                                        FirstName = GetCellValue<string>(worksheet.Cells[row, 2]),
                                        LastName = GetCellValue<string>(worksheet.Cells[row, 3]),
                                        UserName = empUserName,
                                        PhoneNumber = GetCellValue<string>(worksheet.Cells[row, 5]),
                                        Email = GetCellValue<string>(worksheet.Cells[row, 6]),
                                        DOB = Convert.ToDateTime(GetCellValue<string>(worksheet.Cells[row, 7])),
                                        //EmployeeType= GetCellValue<string>(worksheet.Cells[row, 8]),
                                        PanCardNo = GetCellValue<string>(worksheet.Cells[row, 8]),
                                        AadharCardNo = GetCellValue<string>(worksheet.Cells[row, 9]),
                                        BankName = GetCellValue<string>(worksheet.Cells[row, 10]),
                                        BankBranchName = GetCellValue<string>(worksheet.Cells[row, 11]),
                                        AccountNumber = GetCellValue<string>(worksheet.Cells[row, 12]),
                                        IFSCode = GetCellValue<string>(worksheet.Cells[row, 13]),
                                        Salary = GetCellValue<int>(worksheet.Cells[row, 14]),
                                        EmployeeType = manageEmployeeType.Id,
                                        EmailConfirmed = isTrue
                                    };

                                    var response = await _userManager.CreateAsync(newUser, ConstantData.DefaultPassword);
                                    await _userManager.AddToRoleAsync(newUser, ConstantData.Employee);

                                    TeamLeader teamDT = new TeamLeader();

                                    teamDT.TeamLeaderId = manageTeamLeader.Id;
                                    teamDT.TeamLeaderEmail = manageTeamLeader.Email;
                                    teamDT.EmployeeId = newUser.Id;
                                    teamDT.EmployeeEmail = newUser.Email;
                                    _context.TeamLeader.Add(teamDT);
                                    _context.SaveChanges();

                                }
                            }
                            var result = new { Success = "true", Message = "Excel Import successfully" };
                            return Json(result);
                        }
                    }

                }
                var resultDT = new { Success = "false", Message = "Invalid excel." };
                return Json(resultDT);
            }
            catch (Exception ex)
            {
                var result = new { Success = "false", Message = ex.Message };
                return Json(result);
            }
        }

        public T GetCellValue<T>(ExcelRangeBase cell)
        {
            object? value = cell?.Text;
            if (value != null && value != DBNull.Value)
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            return default(T);
        }

        [HttpGet]
        public async Task<IActionResult> ManualSeed()
        {
            try
            {
                await SeedData.SeedEmployeeManagementAsync(true);

                var result = new { Success = "true", Message = "Manual Seed Done successfully." };
                return Json(result);
            }
            catch (Exception ex)
            {
                var result = new { Success = "False", Message = ex.Message };
                return Json(result);
            }

        }

        private bool IsValid(string cellValue)
        {
            bool result = string.IsNullOrEmpty(cellValue);
            if (result)
            {
                return result;
            }
            return false;
        }

        private void GetDaywiseVal(int day, decimal dtValue, MonthlyReport resultDT)
        {
            if (day == 1) resultDT._1 = dtValue;
            else if (day == 2) resultDT._2 = dtValue;
            else if (day == 3) resultDT._3 = dtValue;
            else if (day == 4) resultDT._4 = dtValue;
            else if (day == 5) resultDT._5 = dtValue;
            else if (day == 6) resultDT._6 = dtValue;
            else if (day == 7) resultDT._7 = dtValue;
            else if (day == 8) resultDT._8 = dtValue;
            else if (day == 9) resultDT._9 = dtValue;
            else if (day == 10) resultDT._10 = dtValue;
            else if (day == 11) resultDT._11 = dtValue;
            else if (day == 12) resultDT._12 = dtValue;
            else if (day == 13) resultDT._13 = dtValue;
            else if (day == 14) resultDT._14 = dtValue;
            else if (day == 15) resultDT._15 = dtValue;
            else if (day == 16) resultDT._16 = dtValue;
            else if (day == 17) resultDT._17 = dtValue;
            else if (day == 18) resultDT._18 = dtValue;
            else if (day == 19) resultDT._19 = dtValue;
            else if (day == 20) resultDT._20 = dtValue;
            else if (day == 21) resultDT._21 = dtValue;
            else if (day == 22) resultDT._22 = dtValue;
            else if (day == 23) resultDT._23 = dtValue;
            else if (day == 24) resultDT._24 = dtValue;
            else if (day == 25) resultDT._25 = dtValue;
            else if (day == 26) resultDT._26 = dtValue;
            else if (day == 27) resultDT._27 = dtValue;
            else if (day == 28) resultDT._28 = dtValue;
            else if (day == 29) resultDT._29 = dtValue;
            else if (day == 30) resultDT._30 = dtValue;
            else if (day == 31) resultDT._31 = dtValue;
        }

        private List<CodesDotHRMS.Models.TimeSheet> GetWorkFromHome(int employeeCode, int month)
        {
            var empId = _context.ApplicationUser.Where(x => x.EmpCode == employeeCode).FirstOrDefault().Id;
            var ckdTimeSheet = _context.TimeSheet.Where(x => x.ToDate.Month == month && x.EmployeeId == empId).ToList();

            return ckdTimeSheet;
        }

        private List<ManualTimeSheet> GetManualTimeSheetData(int employeeCode, int month)
        {
            var empId = _context.ApplicationUser.Where(x => x.EmpCode == employeeCode).FirstOrDefault().Id;
            var ckdManualTimeSheet = _context.ManualTimeSheet.Where(x => x.ManualDate.Month == month && x.EmployeeId == empId).ToList();

            return ckdManualTimeSheet;
        }

        private bool IsValidEmployee(int employeeCode)
        {
            var result = _context.ApplicationUser.Where(x => x.EmpCode == employeeCode).FirstOrDefault();

            if (result != null)
            {
                return true;
            }

            return false;
        }
    }
}


