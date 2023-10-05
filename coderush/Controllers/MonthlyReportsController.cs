using coderush.Data;
using coderush.Models;
using coderush.Services.Security;
using CodesDotHRMS.Helper;
using CodesDotHRMS.Models;
using CodesDotHRMS.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace CodesDotHRMS.Controllers
{
    public class MonthlyReportsController : Controller
    {
        private readonly IdentityDefaultOptions _identityDefaultOptions;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //dependency injection through constructor, to directly access services  // Harshal Working on
        public MonthlyReportsController(
            IOptions<IdentityDefaultOptions> identityDefaultOptions,
            IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions,
            ApplicationDbContext context,
             IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager
            )
        {
            _identityDefaultOptions = identityDefaultOptions.Value;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment; ;
        }
        //[WebMethod]
        public IActionResult Index()
        {
            var userList = (from userData in _context.ApplicationUser
                            join ur in _context.UserRoles on userData.Id equals ur.UserId
                            join role in _context.Roles on ur.RoleId equals role.Id
                            where role.Name == "Employee"
                            select new UserDropdownListViewModel
                            {
                                Userid = userData.Id,
                                FullName = userData.FirstName + " " + userData.LastName,
                            }).ToList();

            ViewBag.UserList = new SelectList(userList, "Userid", "FullName");

            return View();
        }


        public async Task<IActionResult> BindBarChart(string id, string month, string year)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var adminrole = await _userManager.IsInRoleAsync(user, "HR");
            var suadminrole = await _userManager.IsInRoleAsync(user, "SuperAdmin");
            var empcode = _context.ApplicationUser.Where(x => x.Id == id).FirstOrDefault().EmpCode;

            string finalmonth = month + "/" + year;

            var empdata = _context.MonthlyReport.Where(x => x.EmployeeCode == empcode).ToList();
            if (empdata.Count != 0)
            {
                var arrival = empdata.Where(x => x.DataType == "In1" && x.Month == finalmonth).FirstOrDefault();
                if (arrival != null)
                {
                    var arrivalList = new List<decimal>();
                    arrivalList.Add(arrival._1);
                    arrivalList.Add(arrival._2);
                    arrivalList.Add(arrival._3);
                    arrivalList.Add(arrival._4);
                    arrivalList.Add(arrival._5);
                    arrivalList.Add(arrival._6);
                    arrivalList.Add(arrival._7);
                    arrivalList.Add(arrival._8);
                    arrivalList.Add(arrival._9);
                    arrivalList.Add(arrival._10);
                    arrivalList.Add(arrival._11);
                    arrivalList.Add(arrival._12);
                    arrivalList.Add(arrival._13);
                    arrivalList.Add(arrival._14);
                    arrivalList.Add(arrival._15);
                    arrivalList.Add(arrival._16);
                    arrivalList.Add(arrival._17);
                    arrivalList.Add(arrival._18);
                    arrivalList.Add(arrival._19);
                    arrivalList.Add(arrival._20);
                    arrivalList.Add(arrival._21);
                    arrivalList.Add(arrival._22);
                    arrivalList.Add(arrival._23);
                    arrivalList.Add(arrival._24);
                    arrivalList.Add(arrival._25);
                    arrivalList.Add(arrival._26);
                    arrivalList.Add(arrival._27);
                    arrivalList.Add(arrival._28);
                    arrivalList.Add(arrival._29);
                    arrivalList.Add(arrival._30);
                    arrivalList.Add(arrival._31);

                    var dep1 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._1;
                    if (dep1 == null || dep1 == 0)
                    {
                        dep1 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._1;
                        if (dep1 == null || dep1 == 0)
                        {
                            dep1 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._1;
                            if (dep1 == null || dep1 == 0)
                            {
                                dep1 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._1;
                                if (dep1 == null || dep1 == 0)
                                {
                                    dep1 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._1;
                                    if (dep1 == null || dep1 == 0)
                                    {
                                        dep1 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._1;
                                        if (dep1 == null || dep1 == 0)
                                        {
                                            dep1 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._1;
                                            if (dep1 == null || dep1 == 0)
                                            {
                                                dep1 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._1;
                                                if (dep1 == null || dep1 == 0)
                                                {
                                                    dep1 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep2 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._2;
                    if (dep2 == null || dep2 == 0)
                    {
                        dep2 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._2;
                        if (dep2 == null || dep2 == 0)
                        {
                            dep2 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._2;
                            if (dep2 == null || dep2 == 0)
                            {
                                dep2 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._2;
                                if (dep2 == null || dep2 == 0)
                                {
                                    dep2 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._2;
                                    if (dep2 == null || dep2 == 0)
                                    {
                                        dep2 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._2;
                                        if (dep2 == null || dep2 == 0)
                                        {
                                            dep2 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._2;
                                            if (dep2 == null || dep2 == 0)
                                            {
                                                dep2 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._2;
                                                if (dep2 == null || dep2 == 0)
                                                {
                                                    dep2 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._2;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep3 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._3;
                    if (dep3 == null || dep3 == 0)
                    {
                        dep3 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._3;
                        if (dep3 == null || dep3 == 0)
                        {
                            dep3 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._3;
                            if (dep3 == null || dep3 == 0)
                            {
                                dep3 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._3;
                                if (dep3 == null || dep3 == 0)
                                {
                                    dep3 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._3;
                                    if (dep3 == null || dep3 == 0)
                                    {
                                        dep3 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._3;
                                        if (dep3 == null || dep3 == 0)
                                        {
                                            dep3 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._3;
                                            if (dep3 == null || dep3 == 0)
                                            {
                                                dep3 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._3;
                                                if (dep3 == null || dep3 == 0)
                                                {
                                                    dep3 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._3;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep4 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._4;
                    if (dep4 == null || dep4 == 0)
                    {
                        dep4 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._4;
                        if (dep4 == null || dep4 == 0)
                        {
                            dep4 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._4;
                            if (dep4 == null || dep4 == 0)
                            {
                                dep4 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._4;
                                if (dep4 == null || dep4 == 0)
                                {
                                    dep4 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._4;
                                    if (dep4 == null || dep4 == 0)
                                    {
                                        dep4 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._4;
                                        if (dep4 == null || dep4 == 0)
                                        {
                                            dep4 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._4;
                                            if (dep4 == null || dep4 == 0)
                                            {
                                                dep4 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._4;
                                                if (dep4 == null || dep4 == 0)
                                                {
                                                    dep4 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._4;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep5 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._5;
                    if (dep5 == null || dep5 == 0)
                    {
                        dep5 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._5;
                        if (dep5 == null || dep5 == 0)
                        {
                            dep5 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._5;
                            if (dep5 == null || dep5 == 0)
                            {
                                dep5 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._5;
                                if (dep5 == null || dep5 == 0)
                                {
                                    dep5 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._5;
                                    if (dep5 == null || dep5 == 0)
                                    {
                                        dep5 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._5;
                                        if (dep5 == null || dep5 == 0)
                                        {
                                            dep5 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._5;
                                            if (dep5 == null || dep5 == 0)
                                            {
                                                dep5 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._5;
                                                if (dep5 == null || dep5 == 0)
                                                {
                                                    dep5 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._5;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep6 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._6;
                    if (dep6 == null || dep6 == 0)
                    {
                        dep6 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._6;
                        if (dep6 == null || dep6 == 0)
                        {
                            dep6 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._6;
                            if (dep6 == null || dep6 == 0)
                            {
                                dep6 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._6;
                                if (dep6 == null || dep6 == 0)
                                {
                                    dep6 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._6;
                                    if (dep6 == null || dep6 == 0)
                                    {
                                        dep6 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._6;
                                        if (dep6 == null || dep6 == 0)
                                        {
                                            dep6 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._6;
                                            if (dep6 == null || dep6 == 0)
                                            {
                                                dep6 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._6;
                                                if (dep6 == null || dep6 == 0)
                                                {
                                                    dep6 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._6;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep7 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._7;
                    if (dep7 == null || dep7 == 0)
                    {
                        dep7 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._7;
                        if (dep7 == null || dep7 == 0)
                        {
                            dep7 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._7;
                            if (dep7 == null || dep7 == 0)
                            {
                                dep7 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._7;
                                if (dep7 == null || dep7 == 0)
                                {
                                    dep7 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._7;
                                    if (dep7 == null || dep7 == 0)
                                    {
                                        dep7 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._7;
                                        if (dep7 == null || dep7 == 0)
                                        {
                                            dep7 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._7;
                                            if (dep7 == null || dep7 == 0)
                                            {
                                                dep7 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._7;
                                                if (dep7 == null || dep7 == 0)
                                                {
                                                    dep7 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._7;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep8 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._8;
                    if (dep8 == null || dep8 == 0)
                    {
                        dep8 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._8;
                        if (dep8 == null || dep8 == 0)
                        {
                            dep8 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._8;
                            if (dep8 == null || dep8 == 0)
                            {
                                dep8 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._8;
                                if (dep8 == null || dep8 == 0)
                                {
                                    dep8 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._8;
                                    if (dep8 == null || dep8 == 0)
                                    {
                                        dep8 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._8;
                                        if (dep8 == null || dep8 == 0)
                                        {
                                            dep8 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._8;
                                            if (dep8 == null || dep8 == 0)
                                            {
                                                dep8 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._8;
                                                if (dep8 == null || dep8 == 0)
                                                {
                                                    dep8 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._8;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep9 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._9;
                    if (dep9 == null || dep9 == 0)
                    {
                        dep9 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._9;
                        if (dep9 == null || dep9 == 0)
                        {
                            dep9 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._9;
                            if (dep9 == null || dep9 == 0)
                            {
                                dep9 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._9;
                                if (dep9 == null || dep9 == 0)
                                {
                                    dep9 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._9;
                                    if (dep9 == null || dep9 == 0)
                                    {
                                        dep9 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._9;
                                        if (dep9 == null || dep9 == 0)
                                        {
                                            dep9 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._9;
                                            if (dep9 == null || dep9 == 0)
                                            {
                                                dep9 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._9;
                                                if (dep9 == null || dep9 == 0)
                                                {
                                                    dep9 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._9;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep10 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._10;
                    if (dep10 == null || dep10 == 0)
                    {
                        dep10 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._10;
                        if (dep10 == null || dep10 == 0)
                        {
                            dep10 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._10;
                            if (dep10 == null || dep10 == 0)
                            {
                                dep10 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._10;
                                if (dep10 == null || dep10 == 0)
                                {
                                    dep10 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._10;
                                    if (dep10 == null || dep10 == 0)
                                    {
                                        dep10 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._10;
                                        if (dep10 == null || dep10 == 0)
                                        {
                                            dep10 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._10;
                                            if (dep10 == null || dep10 == 0)
                                            {
                                                dep10 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._10;
                                                if (dep10 == null || dep10 == 0)
                                                {
                                                    dep10 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._10;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep11 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._11;
                    if (dep11 == null || dep11 == 0)
                    {
                        dep11 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._11;
                        if (dep11 == null || dep11 == 0)
                        {
                            dep11 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._11;
                            if (dep11 == null || dep11 == 0)
                            {
                                dep11 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._11;
                                if (dep11 == null || dep11 == 0)
                                {
                                    dep11 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._11;
                                    if (dep11 == null || dep11 == 0)
                                    {
                                        dep11 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._11;
                                        if (dep11 == null || dep11 == 0)
                                        {
                                            dep11 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._11;
                                            if (dep11 == null || dep11 == 0)
                                            {
                                                dep11 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._11;
                                                if (dep11 == null || dep11 == 0)
                                                {
                                                    dep11 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._11;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep12 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._12;
                    if (dep12 == null || dep12 == 0)
                    {
                        dep12 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._12;
                        if (dep12 == null || dep12 == 0)
                        {
                            dep12 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._12;
                            if (dep12 == null || dep12 == 0)
                            {
                                dep12 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._12;
                                if (dep12 == null || dep12 == 0)
                                {
                                    dep12 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._12;
                                    if (dep12 == null || dep12 == 0)
                                    {
                                        dep12 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._12;
                                        if (dep12 == null || dep12 == 0)
                                        {
                                            dep12 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._12;
                                            if (dep12 == null || dep12 == 0)
                                            {
                                                dep12 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._12;
                                                if (dep12 == null || dep12 == 0)
                                                {
                                                    dep12 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._12;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep13 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._13;
                    if (dep13 == null || dep13 == 0)
                    {
                        dep13 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._13;
                        if (dep13 == null || dep13 == 0)
                        {
                            dep13 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._13;
                            if (dep13 == null || dep13 == 0)
                            {
                                dep13 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._13;
                                if (dep13 == null || dep13 == 0)
                                {
                                    dep13 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._13;
                                    if (dep13 == null || dep13 == 0)
                                    {
                                        dep13 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._13;
                                        if (dep13 == null || dep13 == 0)
                                        {
                                            dep13 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._13;
                                            if (dep13 == null || dep13 == 0)
                                            {
                                                dep13 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._13;
                                                if (dep13 == null || dep13 == 0)
                                                {
                                                    dep13 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._13;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep14 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._14;
                    if (dep14 == null || dep14 == 0)
                    {
                        dep14 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._14;
                        if (dep14 == null || dep14 == 0)
                        {
                            dep14 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._14;
                            if (dep14 == null || dep14 == 0)
                            {
                                dep14 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._14;
                                if (dep14 == null || dep14 == 0)
                                {
                                    dep14 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._14;
                                    if (dep14 == null || dep14 == 0)
                                    {
                                        dep14 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._14;
                                        if (dep14 == null || dep14 == 0)
                                        {
                                            dep14 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._14;
                                            if (dep14 == null || dep14 == 0)
                                            {
                                                dep14 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._14;
                                                if (dep14 == null || dep14 == 0)
                                                {
                                                    dep14 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._14;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep15 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._15;
                    if (dep15 == null || dep15 == 0)
                    {
                        dep15 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._15;
                        if (dep15 == null || dep15 == 0)
                        {
                            dep15 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._15;
                            if (dep15 == null || dep15 == 0)
                            {
                                dep15 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._15;
                                if (dep15 == null || dep15 == 0)
                                {
                                    dep15 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._15;
                                    if (dep15 == null || dep15 == 0)
                                    {
                                        dep15 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._15;
                                        if (dep15 == null || dep15 == 0)
                                        {
                                            dep15 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._15;
                                            if (dep15 == null || dep15 == 0)
                                            {
                                                dep15 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._15;
                                                if (dep15 == null || dep15 == 0)
                                                {
                                                    dep15 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._15;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep16 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._16;
                    if (dep16 == null || dep16 == 0)
                    {
                        dep16 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._16;
                        if (dep16 == null || dep16 == 0)
                        {
                            dep16 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._16;
                            if (dep16 == null || dep16 == 0)
                            {
                                dep16 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._16;
                                if (dep16 == null || dep16 == 0)
                                {
                                    dep16 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._16;
                                    if (dep16 == null || dep16 == 0)
                                    {
                                        dep16 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._16;
                                        if (dep16 == null || dep16 == 0)
                                        {
                                            dep16 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._16;
                                            if (dep16 == null || dep16 == 0)
                                            {
                                                dep16 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._16;
                                                if (dep16 == null || dep16 == 0)
                                                {
                                                    dep16 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._16;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep17 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._17;
                    if (dep17 == null || dep17 == 0)
                    {
                        dep17 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._17;
                        if (dep17 == null || dep17 == 0)
                        {
                            dep17 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._17;
                            if (dep17 == null || dep17 == 0)
                            {
                                dep17 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._17;
                                if (dep17 == null || dep17 == 0)
                                {
                                    dep17 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._17;
                                    if (dep17 == null || dep17 == 0)
                                    {
                                        dep17 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._17;
                                        if (dep17 == null || dep17 == 0)
                                        {
                                            dep17 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._17;
                                            if (dep17 == null || dep17 == 0)
                                            {
                                                dep17 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._17;
                                                if (dep17 == null || dep17 == 0)
                                                {
                                                    dep17 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._17;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep18 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._18;
                    if (dep18 == null || dep18 == 0)
                    {
                        dep18 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._18;
                        if (dep18 == null || dep18 == 0)
                        {
                            dep18 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._18;
                            if (dep18 == null || dep18 == 0)
                            {
                                dep18 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._18;
                                if (dep18 == null || dep18 == 0)
                                {
                                    dep18 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._18;
                                    if (dep18 == null || dep18 == 0)
                                    {
                                        dep18 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._18;
                                        if (dep18 == null || dep18 == 0)
                                        {
                                            dep18 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._18;
                                            if (dep18 == null || dep18 == 0)
                                            {
                                                dep18 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._18;
                                                if (dep18 == null || dep18 == 0)
                                                {
                                                    dep18 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._18;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep19 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._19;
                    if (dep19 == null || dep19 == 0)
                    {
                        dep19 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._19;
                        if (dep19 == null || dep19 == 0)
                        {
                            dep19 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._19;
                            if (dep19 == null || dep19 == 0)
                            {
                                dep19 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._19;
                                if (dep19 == null || dep19 == 0)
                                {
                                    dep19 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._19;
                                    if (dep19 == null || dep19 == 0)
                                    {
                                        dep19 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._19;
                                        if (dep19 == null || dep19 == 0)
                                        {
                                            dep19 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._19;
                                            if (dep19 == null || dep19 == 0)
                                            {
                                                dep19 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._19;
                                                if (dep19 == null || dep19 == 0)
                                                {
                                                    dep19 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._19;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep20 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._20;
                    if (dep20 == null || dep20 == 0)
                    {
                        dep20 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._20;
                        if (dep20 == null || dep20 == 0)
                        {
                            dep20 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._20;
                            if (dep20 == null || dep20 == 0)
                            {
                                dep20 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._20;
                                if (dep20 == null || dep20 == 0)
                                {
                                    dep20 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._20;
                                    if (dep20 == null || dep20 == 0)
                                    {
                                        dep20 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._20;
                                        if (dep20 == null || dep20 == 0)
                                        {
                                            dep20 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._20;
                                            if (dep20 == null || dep20 == 0)
                                            {
                                                dep20 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._20;
                                                if (dep20 == null || dep20 == 0)
                                                {
                                                    dep20 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._20;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep21 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._21;
                    if (dep21 == null || dep21 == 0)
                    {
                        dep21 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._21;
                        if (dep21 == null || dep21 == 0)
                        {
                            dep21 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._21;
                            if (dep21 == null || dep21 == 0)
                            {
                                dep21 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._21;
                                if (dep21 == null || dep21 == 0)
                                {
                                    dep21 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._21;
                                    if (dep21 == null || dep21 == 0)
                                    {
                                        dep21 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._21;
                                        if (dep21 == null || dep21 == 0)
                                        {
                                            dep21 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._21;
                                            if (dep21 == null || dep21 == 0)
                                            {
                                                dep21 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._21;
                                                if (dep21 == null || dep21 == 0)
                                                {
                                                    dep21 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._21;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep22 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._22;
                    if (dep22 == null || dep22 == 0)
                    {
                        dep22 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._22;
                        if (dep22 == null || dep22 == 0)
                        {
                            dep22 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._22;
                            if (dep22 == null || dep22 == 0)
                            {
                                dep22 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._22;
                                if (dep22 == null || dep22 == 0)
                                {
                                    dep22 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._22;
                                    if (dep22 == null || dep22 == 0)
                                    {
                                        dep22 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._22;
                                        if (dep22 == null || dep22 == 0)
                                        {
                                            dep22 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._22;
                                            if (dep22 == null || dep22 == 0)
                                            {
                                                dep22 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._22;
                                                if (dep22 == null || dep22 == 0)
                                                {
                                                    dep22 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._22;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep23 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._23;
                    if (dep23 == null || dep23 == 0)
                    {
                        dep23 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._23;
                        if (dep23 == null || dep23 == 0)
                        {
                            dep23 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._23;
                            if (dep23 == null || dep23 == 0)
                            {
                                dep23 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._23;
                                if (dep23 == null || dep23 == 0)
                                {
                                    dep23 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._23;
                                    if (dep23 == null || dep23 == 0)
                                    {
                                        dep23 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._23;
                                        if (dep23 == null || dep23 == 0)
                                        {
                                            dep23 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._23;
                                            if (dep23 == null || dep23 == 0)
                                            {
                                                dep23 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._23;
                                                if (dep23 == null || dep23 == 0)
                                                {
                                                    dep23 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._23;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep24 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._24;
                    if (dep24 == null || dep24 == 0)
                    {
                        dep24 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._24;
                        if (dep24 == null || dep24 == 0)
                        {
                            dep24 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._24;
                            if (dep24 == null || dep24 == 0)
                            {
                                dep24 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._24;
                                if (dep24 == null || dep24 == 0)
                                {
                                    dep24 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._24;
                                    if (dep24 == null || dep24 == 0)
                                    {
                                        dep24 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._24;
                                        if (dep24 == null || dep24 == 0)
                                        {
                                            dep24 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._24;
                                            if (dep24 == null || dep24 == 0)
                                            {
                                                dep24 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._24;
                                                if (dep24 == null || dep24 == 0)
                                                {
                                                    dep24 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._24;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep25 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._25;
                    if (dep25 == null || dep25 == 0)
                    {
                        dep25 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._25;
                        if (dep25 == null || dep25 == 0)
                        {
                            dep25 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._25;
                            if (dep25 == null || dep25 == 0)
                            {
                                dep25 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._25;
                                if (dep25 == null || dep25 == 0)
                                {
                                    dep25 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._25;
                                    if (dep25 == null || dep25 == 0)
                                    {
                                        dep25 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._25;
                                        if (dep25 == null || dep25 == 0)
                                        {
                                            dep25 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._25;
                                            if (dep25 == null || dep25 == 0)
                                            {
                                                dep25 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._25;
                                                if (dep25 == null || dep25 == 0)
                                                {
                                                    dep25 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._25;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep26 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._26;
                    if (dep26 == null || dep26 == 0)
                    {
                        dep26 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._26;
                        if (dep26 == null || dep26 == 0)
                        {
                            dep26 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._26;
                            if (dep26 == null || dep26 == 0)
                            {
                                dep26 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._26;
                                if (dep26 == null || dep26 == 0)
                                {
                                    dep26 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._26;
                                    if (dep26 == null || dep26 == 0)
                                    {
                                        dep26 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._26;
                                        if (dep26 == null || dep26 == 0)
                                        {
                                            dep26 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._26;
                                            if (dep26 == null || dep26 == 0)
                                            {
                                                dep26 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._26;
                                                if (dep26 == null || dep26 == 0)
                                                {
                                                    dep26 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._26;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep27 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._27;
                    if (dep27 == null || dep27 == 0)
                    {
                        dep27 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._27;
                        if (dep27 == null || dep27 == 0)
                        {
                            dep27 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._27;
                            if (dep27 == null || dep27 == 0)
                            {
                                dep27 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._27;
                                if (dep27 == null || dep27 == 0)
                                {
                                    dep27 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._27;
                                    if (dep27 == null || dep27 == 0)
                                    {
                                        dep27 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._27;
                                        if (dep27 == null || dep27 == 0)
                                        {
                                            dep27 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._27;
                                            if (dep27 == null || dep27 == 0)
                                            {
                                                dep27 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._27;
                                                if (dep27 == null || dep27 == 0)
                                                {
                                                    dep27 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._27;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep28 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._28;
                    if (dep28 == null || dep28 == 0)
                    {
                        dep28 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._28;
                        if (dep28 == null || dep28 == 0)
                        {
                            dep28 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._28;
                            if (dep28 == null || dep28 == 0)
                            {
                                dep28 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._28;
                                if (dep28 == null || dep28 == 0)
                                {
                                    dep28 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._28;
                                    if (dep28 == null || dep28 == 0)
                                    {
                                        dep28 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._28;
                                        if (dep28 == null || dep28 == 0)
                                        {
                                            dep28 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._28;
                                            if (dep28 == null || dep28 == 0)
                                            {
                                                dep28 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._28;
                                                if (dep28 == null || dep28 == 0)
                                                {
                                                    dep28 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._28;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep29 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._29;
                    if (dep29 == null || dep29 == 0)
                    {
                        dep29 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._29;
                        if (dep29 == null || dep29 == 0)
                        {
                            dep29 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._29;
                            if (dep29 == null || dep29 == 0)
                            {
                                dep29 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._29;
                                if (dep29 == null || dep29 == 0)
                                {
                                    dep29 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._29;
                                    if (dep29 == null || dep29 == 0)
                                    {
                                        dep29 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._29;
                                        if (dep29 == null || dep29 == 0)
                                        {
                                            dep29 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._29;
                                            if (dep29 == null || dep29 == 0)
                                            {
                                                dep29 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._29;
                                                if (dep29 == null || dep29 == 0)
                                                {
                                                    dep29 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._29;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep30 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._30;
                    if (dep30 == null || dep30 == 0)
                    {
                        dep30 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._30;
                        if (dep30 == null || dep30 == 0)
                        {
                            dep30 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._30;
                            if (dep30 == null || dep30 == 0)
                            {
                                dep30 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._30;
                                if (dep30 == null || dep30 == 0)
                                {
                                    dep30 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._30;
                                    if (dep30 == null || dep30 == 0)
                                    {
                                        dep30 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._30;
                                        if (dep30 == null || dep30 == 0)
                                        {
                                            dep30 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._30;
                                            if (dep30 == null || dep30 == 0)
                                            {
                                                dep30 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._30;
                                                if (dep30 == null || dep30 == 0)
                                                {
                                                    dep30 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._30;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var dep31 = empdata.Where(x => x.DataType == "Out9" && x.Month == finalmonth).FirstOrDefault()._31;
                    if (dep31 == null || dep31 == 0)
                    {
                        dep31 = empdata.Where(x => x.DataType == "Out8" && x.Month == finalmonth).FirstOrDefault()._31;
                        if (dep31 == null || dep31 == 0)
                        {
                            dep31 = empdata.Where(x => x.DataType == "Out7" && x.Month == finalmonth).FirstOrDefault()._31;
                            if (dep31 == null || dep31 == 0)
                            {
                                dep31 = empdata.Where(x => x.DataType == "Out6" && x.Month == finalmonth).FirstOrDefault()._31;
                                if (dep31 == null || dep31 == 0)
                                {
                                    dep31 = empdata.Where(x => x.DataType == "Out5" && x.Month == finalmonth).FirstOrDefault()._31;
                                    if (dep31 == null || dep31 == 0)
                                    {
                                        dep31 = empdata.Where(x => x.DataType == "Out4" && x.Month == finalmonth).FirstOrDefault()._31;
                                        if (dep31 == null || dep31 == 0)
                                        {
                                            dep31 = empdata.Where(x => x.DataType == "Out3" && x.Month == finalmonth).FirstOrDefault()._31;
                                            if (dep31 == null || dep31 == 0)
                                            {
                                                dep31 = empdata.Where(x => x.DataType == "Out2" && x.Month == finalmonth).FirstOrDefault()._31;
                                                if (dep31 == null || dep31 == 0)
                                                {
                                                    dep31 = empdata.Where(x => x.DataType == "Out1" && x.Month == finalmonth).FirstOrDefault()._31;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var departureList = new List<decimal>();
                    departureList.Add(dep1);
                    departureList.Add(dep2);
                    departureList.Add(dep3);
                    departureList.Add(dep4);
                    departureList.Add(dep5);
                    departureList.Add(dep6);
                    departureList.Add(dep7);
                    departureList.Add(dep8);
                    departureList.Add(dep9);
                    departureList.Add(dep10);
                    departureList.Add(dep11);
                    departureList.Add(dep12);
                    departureList.Add(dep13);
                    departureList.Add(dep14);
                    departureList.Add(dep15);
                    departureList.Add(dep16);
                    departureList.Add(dep17);
                    departureList.Add(dep18);
                    departureList.Add(dep19);
                    departureList.Add(dep20);
                    departureList.Add(dep21);
                    departureList.Add(dep22);
                    departureList.Add(dep23);
                    departureList.Add(dep24);
                    departureList.Add(dep25);
                    departureList.Add(dep26);
                    departureList.Add(dep27);
                    departureList.Add(dep28);
                    departureList.Add(dep29);
                    departureList.Add(dep30);
                    departureList.Add(dep31);

                    var lists = new List<decimal>();
                    var one = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._1;
                    lists.Add(one);
                    var two = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._2;
                    lists.Add(two);
                    var three = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._3;
                    lists.Add(three);
                    var four = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._4;
                    lists.Add(four);
                    var five = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._5;
                    lists.Add(five);
                    var six = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._6;
                    lists.Add(six);
                    var seven = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._7;
                    lists.Add(seven);
                    var eight = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._8;
                    lists.Add(eight);
                    var nine = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._9;
                    lists.Add(nine);
                    var ten = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._10;
                    lists.Add(ten);
                    var eleven = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._11;
                    lists.Add(eleven);
                    var twelve = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._12;
                    lists.Add(twelve);
                    var thirteen = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._13;
                    lists.Add(thirteen);
                    var fourteen = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._14;
                    lists.Add(fourteen);
                    var fifteen = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._15;
                    lists.Add(fifteen);
                    var sixteen = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._16;
                    lists.Add(sixteen);
                    var seventeen = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._17;
                    lists.Add(seventeen);
                    var eighteen = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._18;
                    lists.Add(eighteen);
                    var nineteen = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._19;
                    lists.Add(nineteen);
                    var twenty = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._20;
                    lists.Add(twenty);
                    var twentyone = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._21;
                    lists.Add(twentyone);
                    var twentytwo = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._22;
                    lists.Add(twentytwo);
                    var twentythree = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._23;
                    lists.Add(twentythree);
                    var twentyfour = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._24;
                    lists.Add(twentyfour);
                    var twentyfive = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._25;
                    lists.Add(twentyfive);
                    var twentysix = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._26;
                    lists.Add(twentysix);
                    var twentyseven = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._27;
                    lists.Add(twentyseven);
                    var twentyeight = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._28;
                    lists.Add(twentyeight);
                    var twentynine = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._29;
                    lists.Add(twentynine);
                    var thirty = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._30;
                    lists.Add(thirty);
                    var thirtyone = empdata.Where(x => x.DataType == "WorkingHrs").FirstOrDefault()._31;
                    lists.Add(thirtyone);

                    string data = "";
                    int lbl = 0;
                    string label = "";
                    foreach (var item in lists)
                    {
                        data = data + item + ",";
                        lbl++;
                        label = label + lbl + ",";
                    }

                    data = data.Remove(data.Length - 1);
                    label = label.Remove(label.Length - 1);

                    string arrivals = "";
                    foreach (var item in arrivalList)
                    {
                        arrivals = arrivals + item + ",";
                    }
                    arrivals = arrivals.Remove(arrivals.Length - 1);

                    string departures = "";
                    foreach (var item in departureList)
                    {
                        departures = departures + item + ",";
                    }
                    departures = departures.Remove(departures.Length - 1);

                    var result = new { data = data, label = label, arrivals = arrivals, departures = departures };
                    return Json(result);
                }
                else
                {
                    return Json(null);
                }
            }
            else
            {
                return Json(null);
            }
        }

        public IActionResult EmployeeLeaveReport()
        {
            var userList = (from userData in _context.ApplicationUser
                            join ur in _context.UserRoles on userData.Id equals ur.UserId
                            join role in _context.Roles on ur.RoleId equals role.Id
                            where role.Name == "Employee"
                            select new UserDropdownListViewModel
                            {
                                Userid = userData.Id,
                                FullName = userData.FirstName + " " + userData.LastName,
                            }).ToList();

            ViewBag.UserList = new SelectList(userList, "Userid", "FullName");

            return View();
        }

        [HttpGet]
        public IActionResult EmployeeAttendanceInfo(string id, string month, string year)
        {
            int EmpCode = _context.ApplicationUser.Where(x => x.Id == id).FirstOrDefault().EmpCode;

            int daysInMonth = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
            var monthyear = month + "/" + year;
            List<EmployeAttedenceReportVM> employeAttedences = new List<EmployeAttedenceReportVM>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                var data = SaveManualAttendance(EmpCode, day, monthyear);
                if (data.ReportType == 1)
                {
                    data.ReportType = data.ReportType == 1 ? 1 : 0;
                }

                if (data.ReportType != null)
                {
                    employeAttedences.Add(data);
                }
            }
            if (employeAttedences.Count == 0)
            {
                employeAttedences = null;
            }
            return View("~/Views/MonthlyReports/ManualEmployee.cshtml", employeAttedences);
        }


        private EmployeAttedenceReportVM SaveManualAttendance(int empCode, int day, string month)
        {
            EmployeAttedenceReportVM data = new EmployeAttedenceReportVM();
            DataSet ds = new DataSet();

            string spName = "dbo.GetEmployeeMonthlyReport";

            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand(spName, connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeCode", empCode);
                cmd.Parameters.AddWithValue("@Day", day);
                cmd.Parameters.AddWithValue("@Month", month);

                da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    data.Day = Convert.ToInt32(dt.Rows[0]["Day"].ToString());
                    data.Month = dt.Rows[0]["Month"].ToString();
                    data.EmployeeCode = Convert.ToInt32(!String.IsNullOrEmpty(dt.Rows[0]["EmployeeCode"].ToString()) ? dt.Rows[0]["EmployeeCode"].ToString() : "0");
                    data.ArrTim = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["In1"].ToString()) ? dt.Rows[0]["In1"].ToString() : "0");
                    data.Out1 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["Out1"].ToString()) ? dt.Rows[0]["Out1"].ToString() : "0");
                    data.In2 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["In2"].ToString()) ? dt.Rows[0]["In2"].ToString() : "0");

                    data.Out2 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["Out2"].ToString()) ? dt.Rows[0]["Out2"].ToString() : "0");
                    data.In3 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["In3"].ToString()) ? dt.Rows[0]["In3"].ToString() : "0");

                    data.Out3 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["Out3"].ToString()) ? dt.Rows[0]["Out3"].ToString() : "0");
                    data.In4 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["In4"].ToString()) ? dt.Rows[0]["In4"].ToString() : "0");

                    data.Out4 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["Out4"].ToString()) ? dt.Rows[0]["Out4"].ToString() : "0");
                    data.In5 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["In5"].ToString()) ? dt.Rows[0]["In5"].ToString() : "0");

                    data.Out5 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["Out1"].ToString()) ? dt.Rows[0]["Out5"].ToString() : "0");
                    data.In6 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["In2"].ToString()) ? dt.Rows[0]["In6"].ToString() : "0");

                    data.Out6 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["Out6"].ToString()) ? dt.Rows[0]["Out6"].ToString() : "0");
                    data.In7 = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["In7"].ToString()) ? dt.Rows[0]["In7"].ToString() : "0");

                    data.DepTim = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["Out9"].ToString()) ? dt.Rows[0]["Out9"].ToString() : "0");
                    data.WorkingHrs = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["WorkingHrs"].ToString()) ? dt.Rows[0]["WorkingHrs"].ToString() : "0");
                    data.ReportType = Convert.ToDecimal(!String.IsNullOrEmpty(dt.Rows[0]["ReportType"].ToString()) ? dt.Rows[0]["ReportType"].ToString() : null);
                }
                else
                {
                    data = new EmployeAttedenceReportVM();
                    return data;
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }


            return data;

        }
    }
}
