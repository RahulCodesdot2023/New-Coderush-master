using coderush;
using coderush.Controllers;
using coderush.Data;
using coderush.Models;
using CodesDotHRMS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodesDotHRMS.Controllers
{
    public class HolidayListController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HolidayListController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            //_hostingEnvironment = hostingEnvironment;
        }
        public IActionResult HolidayIndex()
        {
            var data = new List<HolidayListViewModel>();
            data = (from h in _context.HolidayList.OrderByDescending(x => x.Id)
                    where !h.Isdelete
                    select new HolidayListViewModel
                    {
                        Id = h.Id,
                        Name = h.Name,
                        Day = h.Day,
                        Date = h.Date,


                    }).ToList();

            return View(data);
        }

        [HttpGet]
        public IActionResult Form()
        {

            HolidayListViewModel newHoliday = new HolidayListViewModel();

            return View(newHoliday);
        }
        [HttpPost]
        public IActionResult SubmitForm([Bind("Id", "Name", "Day", "Date")] HolidayListViewModel holiday)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = holiday.Id });
                }

                HolidayList newHoliday = new HolidayList();

                var exitdata = _context.HolidayList.Where(x => x.Name == holiday.Name).FirstOrDefault();
                if (exitdata == null)
                {
                    newHoliday.Name = holiday.Name;
                    newHoliday.Day = holiday.Day;
                    newHoliday.Date = holiday.Date;

                    _context.HolidayList.Add(newHoliday);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new HolidayList item successfully.";
                    return RedirectToAction(nameof(Form));
                }
                else
                {
                    if (exitdata.Isdelete == true)
                    {
                        exitdata.Isdelete = false;
                        _context.HolidayList.Update(exitdata);
                        _context.SaveChanges();

                        TempData[StaticString.StatusMessage] = "Create new HolidayList item successfully.";
                        return RedirectToAction(nameof(Form));

                    }
                    TempData[StaticString.StatusMessage] = "Data is already exist";
                    return RedirectToAction(nameof(Form));
                }
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form));
            }

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var holiday = _context.HolidayList.Where(x => x.Id.Equals(id)).FirstOrDefault();
            return View(holiday);

        }

        [HttpGet]
        public IActionResult DeleteHoliday(int id)
        {
            try
            {
                var deleteholiday = _context.HolidayList.Where(x => x.Id.Equals(id)).FirstOrDefault();
                if (deleteholiday == null)
                {
                    return NotFound();
                }
                deleteholiday.Isdelete = true;
                _context.HolidayList.Update(deleteholiday);
                _context.SaveChanges();
                return Json(new { issuccess = true, message = "Holiday deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = "Something went wrong." });
            }
        }

        [HttpPost]
        public IActionResult AddHoliday(HolidayList holiday)
        {
            try
            {
                HolidayList newHoliday = new HolidayList
                {
                    Name = holiday.Name,
                    Day = holiday.Day,
                    Date = holiday.Date
                };
                _context.HolidayList.Add(newHoliday);
                _context.SaveChanges();
                return Json(new { issuccess = true, message = "Holiday added successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = "Something went wrong." });
            }
        }

        [HttpGet]
        public IActionResult GetHoliday(int id)
        {
            try
            {
                var data = _context.HolidayList.Where(x => x.Id == id).FirstOrDefault();
                return Json(new { issuccess = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = "Something went wrong." });
            }
        }

        [HttpPost]
        public IActionResult UpdateHoliday(HolidayList holiday)
        {
            try
            {
                var data = _context.HolidayList.Where(x => x.Id == holiday.Id).FirstOrDefault();
                if (data.Isdelete == true)
                {
                    data.Isdelete = false;
                }
                data.Name = holiday.Name;
                data.Day = holiday.Day;
                data.Date = holiday.Date;
                _context.HolidayList.Update(data);
                _context.SaveChanges();
                return Json(new { issuccess = true, message = "Holiday updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, message = "Something went wrong." });
            }
        }

    }
}
