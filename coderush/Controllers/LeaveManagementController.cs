using coderush;
using coderush.Data;
using coderush.DataEnum;
using coderush.Models;
using CodesDotHRMS.Models;
using CodesDotHRMS.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;



namespace CodesDotHRMS.Controllers
{
    public class LeaveManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        public LeaveManagementController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            //_hostingEnvironment = hostingEnvironment;
        }

        public IActionResult LeaveManagIndex()
        {
            try
            {
                var data = (from lm in _context.LeaveManagement
                            join user in _userManager.Users on lm.UserId.ToString() equals user.Id
                            where lm.IsDeleted == false
                            orderby lm.FromDate descending
                            select new CodesDotHRMS.Models.ViewModels.LeaveManagementViewModel
                            {
                                FullName = user.FirstName + " " + user.LastName,
                                FromDate = lm.FromDate.ToString(),
                                ToDate = lm.ToDate.ToString(),
                                LeaveType = lm.LeaveType,
                                PaidLeaveCount = Convert.ToInt16(lm.PaidLeaveCount),
                                UnPaidLeaveCount = lm.UnPaidLeaveCount,
                                LeaveStatus = lm.LeaveStatus,
                                LeaveReason = lm.LeaveReason != null ? lm.LeaveReason : "-",
                                LeaveManagementId = lm.LeaveManagementId,
                                IsMonthlyLeaveLeft = lm.IsMonthlyLeaveLeft,
                                LeaveBuket=lm.LeaveBuket
                            }).ToList();

                foreach (var item in data)
                {
                    LeaveStatus enumname = (LeaveStatus)item.LeaveStatus;
                    item.LeaveStatusName = enumname.GetType().GetMember(enumname.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.GetName();
                }

                return View(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult AddLeaveIndex(LeaveBucket leavebckt)
        {

            var user = _userManager.GetUserAsync(User).Result;
            var Bckt_data = _context.LeaveBucket.Where(x => x.UserId.ToString() == user.Id).Select(s => s.Bucket).FirstOrDefault();

            ViewBag.leavetypelist = Bckt_data;
            return View();
        }

        public IActionResult SaveLeave([Bind("LeaveManagementId", "FromDate", "ToDate", "TotalLeaveDay", "LeaveType", "PaidLeaveCount", "UnPaidLeaveCount")] LeaveManagementViewModel leavemanage)
        {
            var respon = new object();
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var Bckt_data = _context.LeaveBucket.Where(x => x.UserId.ToString() == user.Id).Select(s => s.Bucket).FirstOrDefault();
                ViewBag.leavetypelist = Bckt_data;

                if (leavemanage.LeaveManagementId == 0)
                {
                    LeaveManagement levmng = new LeaveManagement();
                    levmng.CreatedBy = user.Id;
                    levmng.UserId = Guid.Parse(user.Id);
                    levmng.FromDate = Convert.ToDateTime(leavemanage.FromDate);
                    levmng.ToDate = Convert.ToDateTime(leavemanage.ToDate);
                    levmng.TotalLeaveDay = Convert.ToInt32(leavemanage.LeaveType) == 1 ? Convert.ToDecimal(0.5) : leavemanage.TotalLeaveDay;
                    levmng.LeaveBuket = Convert.ToInt16(Bckt_data);
                    //levmng.LeaveStatus = (int)LeaveStatus.Inprogress; 
                    levmng.LeaveStatus = leavemanage.LeaveStatus;
                    levmng.LeaveType = Convert.ToInt32(leavemanage.LeaveType);
                    levmng.PaidLeaveCount = leavemanage.PaidLeaveCount == 1 ? Convert.ToDecimal(0.5) : leavemanage.PaidLeaveCount;
                    //levmng.PaidLeaveCount = leavemanage.PaidLeaveCount;
                    levmng.UnPaidLeaveCount = leavemanage.UnPaidLeaveCount;
                    levmng.CreatedDate = DateTime.Now;
                    levmng.UpdatedDate = DateTime.Now;
                    _context.LeaveManagement.Add(levmng);
                    _context.SaveChanges();

                    respon = new { success = true, msg = "Leave Request Successfully." };
                    //return RedirectToAction(nameof(Form), new { id = leavemanage.LeaveManagementId > 0 ? leavemanage.LeaveManagementId : 0 });

                }
                //else
                //{

                //   LeaveManagement levmng = new LeaveManagement();
                //  levmng.LeaveManagementId = leavemanage.LeaveManagementId;
                //  levmng.CreatedBy = user.Id;
                //  levmng.UserId = leavemanage.UserId;
                //  levmng.FromDate = leavemanage.FromDate;
                //  levmng.ToDate = leavemanage.ToDate;
                //  levmng.TotalLeaveDay = leavemanage.TotalLeaveDay;
                //  levmng.LeaveBuket = ViewBag.leavetypelist.Bckt_data.ToString();

                //  levmng.LeaveType = leavemanage.LeaveType;
                //  levmng.PaidLeaveCount = ViewBag.leavetypelist.Bckt_data.ToString();
                //  levmng.UnPaidLeaveCount = leavemanage.UnPaidLeaveCount;
                //  _context.LeaveManagement.Update(levmng);
                //  _context.SaveChanges();
                //}

            }
            catch (Exception ex)
            {

                respon = new { success = false, msg = "something went wrong." };
            }
            return Json(respon);
        }
        public ActionResult LeavegetInList(int id)
        {
            if (id == 0)
            {
                return null;
            }

            LeaveManagement model = new LeaveManagement();
            // ViewData["user"] = _userManager.GetUserAsync(User).Result;
            var dataget = _context.LeaveManagement.Where(x => x.LeaveManagementId.Equals(id)).ToList();

            return Json(dataget);
        }

        public ActionResult SaveLeaveAccept(int Id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;

                var data = _context.LeaveManagement.Where(x => x.LeaveManagementId == Id).FirstOrDefault();
                data.LeaveStatus = (int)LeaveStatus.Accepted;
                data.UpdatedDate = DateTime.Now;
                data.UpdatedBy = user.ToString();

                _context.LeaveManagement.Update(data);
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

        public async Task<IActionResult> BindGridData(string id, string UserName)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var adminrole = await _userManager.IsInRoleAsync(user, "HR");
            var suadminrole = await _userManager.IsInRoleAsync(user, "SuperAdmin");
            LeaveManagementViewModel levcunt = new LeaveManagementViewModel();
            var leavecount = new List<LeaveManagementViewModel>();

            leavecount = (from leave in _context.LeaveManagement
                          where !leave.IsDeleted && ((adminrole && suadminrole) || (leave.UserId.ToString() == user.Id))
                          select new LeaveManagementViewModel
                          {
                              LeaveManagementId = leave.LeaveManagementId,
                              UserId = leave.UserId.ToString(),
                              FromDate = leave.FromDate.ToString(),
                              ToDate = leave.ToDate.ToString(),
                              TotalLeaveDay = leave.TotalLeaveDay,
                              LeaveBuket = leave.LeaveBuket,
                              PaidLeaveCount = leave.PaidLeaveCount,
                              UnPaidLeaveCount = leave.UnPaidLeaveCount,
                              LeaveReason = leave.LeaveReason,
                              IsMonthlyLeaveLeft = leave.IsMonthlyLeaveLeft
                          }).ToList();

            return View(leavecount);

        }

        public ActionResult SaveLeaveReason(int Id, string Leavereason)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;

                var data = _context.LeaveManagement.Where(x => x.LeaveManagementId == Id).FirstOrDefault();
                data.LeaveReason = Leavereason;
                data.IsRejected = true;
                data.LeaveStatus = (int)LeaveStatus.Rejected;
                data.UpdatedDate = DateTime.Now;
                data.UpdatedBy = user.ToString();

                _context.LeaveManagement.Update(data);
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

    }

}

