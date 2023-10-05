using coderush.Data;
using coderush.Models;
using coderush.Services.Security;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodesDotHRMS.Controllers
{

    [Authorize(Roles = "SuperAdmin,HR")]
    public class PayrollController : Controller
    {
        private readonly ICommon _security;
        private readonly ApplicationDbContext _context;

        public PayrollController(ICommon security, ApplicationDbContext context)
        {
            _security = security;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmployeeInfo(string month, string year, bool? isWFH = false)
        {
            int workingDays = 0;

            DateTime currentDate = DateTime.Today.AddDays(-DateTime.Now.Day);
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

            if (month!="" || month != null && year != "" || year != null)
            {
                string monthFirstDate = "1-" + month + "-" + year;
                currentDate= Convert.ToDateTime(monthFirstDate);
                daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            }

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, day);
                if (currentDateOfMonth.DayOfWeek != DayOfWeek.Saturday && currentDateOfMonth.DayOfWeek != DayOfWeek.Sunday)
                {
                    workingDays++;
                }
            }

            ViewBag.WorkingDayCounter = workingDays;

            var monthyear = currentDate.Year + "-" + currentDate.Month.ToString("D2");
            var holidays = _context.HolidayList.Where(x => x.Date.ToString().Contains(monthyear) && x.Isdelete == false).ToList();
            ViewBag.MonthlyLeaves = holidays.Count();

            var result = _security.GetAllEmployee(currentDate, true).Where(x => x.IsWFH == isWFH).ToList();
            return View("~/Views/Payroll/RegularEmployee.cshtml", result);
        }
    }
}
