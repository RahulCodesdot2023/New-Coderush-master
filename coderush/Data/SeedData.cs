using coderush;
using coderush.Data;
using coderush.DataEnum;
using coderush.Models;
using CodesDotHRMS.Helper;
using CodesDotHRMS.Models;
using CodesDotHRMS.Models.ViewModels;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodesDotHRMS.Data
{
    public static class SeedData
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        // private readonly ApplicationDbContext _context;

        public static async Task SeedEmployeeManagementAsync(bool clicked = false)
        {
            var host = CreateWebHostBuilder().Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var _roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var _context = services.GetRequiredService<ApplicationDbContext>();
                var configuration = services.GetRequiredService<IConfiguration>();

                // Access configuration values
                var adminUser = configuration.GetSection("AdminUser").Get<AdminUser>();

                var employeeType = _context.Datamaster.Where(x => x.Type == DataSelection.EmployeeType).ToList();
                if (employeeType.Count == 0)
                {
                    employeeType = new List<DataMaster>()
                    {
                        new DataMaster()
                        {
                            Type=DataSelection.EmployeeType,
                            Text=ConstantData.Progressive,
                            Description=ConstantData.Progressive,
                            Isactive=true,
                        },
                        new DataMaster()
                        {
                            Type=DataSelection.EmployeeType,
                            Text=ConstantData.Permanent,
                            Description=ConstantData.Permanent,
                            Isactive=true,
                        },
                    };
                    await _context.Datamaster.AddRangeAsync(employeeType);
                    await _context.SaveChangesAsync();
                }

                var LeaveReasons = _context.Datamaster.Where(x => x.Type == DataSelection.LeaveReasons).ToList();
                if (LeaveReasons.Count == 0)
                {
                    LeaveReasons = new List<DataMaster>()
                    {
                        new DataMaster()
                        {
                            Type=DataSelection.LeaveReasons,
                            Text=ConstantData.Planned,
                            Description=ConstantData.Planned,
                            Isactive=true,
                        },
                        new DataMaster()
                        {
                            Type=DataSelection.LeaveReasons,
                            Text=ConstantData.Unplanned,
                            Description=ConstantData.Unplanned,
                            Isactive=true,
                        },
                        new DataMaster()
                        {
                            Type=DataSelection.LeaveReasons,
                            Text=ConstantData.SickLeave,
                            Description=ConstantData.SickLeave,
                            Isactive=true,
                        },
                        new DataMaster()
                        {
                            Type=DataSelection.LeaveReasons,
                            Text=ConstantData.Others,
                            Description=ConstantData.Others,
                            Isactive=true,
                        },
                    };
                    await _context.Datamaster.AddRangeAsync(LeaveReasons);
                    await _context.SaveChangesAsync();
                }

                var Expenses = _context.Datamaster.Where(x => x.Type == DataSelection.Expenses).ToList();
                if (Expenses.Count == 0)
                {
                    Expenses = new List<DataMaster>()
                    {
                        new DataMaster()
                        {
                            Type=DataSelection.Expenses,
                            Text=ConstantData.DirectExpense,
                            Description=ConstantData.DirectExpense,
                            Isactive=true,
                        },
                        new DataMaster()
                        {
                            Type=DataSelection.Expenses,
                            Text=ConstantData.InDirectExpense,
                            Description=ConstantData.InDirectExpense,
                            Isactive=true,
                        },
                    };
                    await _context.Datamaster.AddRangeAsync(Expenses);
                    await _context.SaveChangesAsync();
                }

                var appRoles = _context.Roles.ToList();
                if (appRoles.Count == 0)
                {
                    appRoles = new List<IdentityRole>()
                    {
                        new IdentityRole
                        {
                            Name=ConstantData.SuperAdmin,
                            NormalizedName=ConstantData.SuperAdmin.ToUpper(),
                        },
                        new IdentityRole
                        {
                            Name=ConstantData.HR,
                            NormalizedName=ConstantData.HR.ToUpper()
                        },
                        new IdentityRole
                        {
                            Name=ConstantData.USER,
                            NormalizedName=ConstantData.USER.ToUpper()
                        },
                        new IdentityRole
                        {
                            Name=ConstantData.Sales,
                            NormalizedName=ConstantData.Sales.ToUpper()
                        },
                        new IdentityRole
                        {
                            Name=ConstantData.TeamAdmin,
                            NormalizedName=ConstantData.TeamAdmin.ToUpper()
                        },
                        new IdentityRole
                        {
                            Name=ConstantData.Employee,
                            NormalizedName=ConstantData.Employee.ToUpper()
                        }
                    };
                    await _context.Roles.AddRangeAsync(appRoles);
                    await _context.SaveChangesAsync();
                }

                var appUsers = _context.ApplicationUser.ToList();
                var appUser = new ApplicationUser();

                if (appUsers.Count == 0)
                {
                    appUser = new ApplicationUser()
                    {
                        FirstName = adminUser.FirstName,
                        LastName = "",
                        UserName = adminUser.Email,
                        NormalizedUserName = adminUser.Email.ToUpper(),
                        Email = adminUser.Email,
                        NormalizedEmail = adminUser.Email.ToUpper(),
                        EmailConfirmed = true,
                        LockoutEnabled = true,
                        AccessFailedCount = 0,
                        isSuperAdmin = true,
                    };
                    var result = await _userManager.CreateAsync(appUser, adminUser.Password);
                    await _userManager.AddToRoleAsync(appUser, ConstantData.SuperAdmin);
                    await _userManager.AddToRoleAsync(appUser, ConstantData.TeamAdmin);
                    await _userManager.AddToRoleAsync(appUser, ConstantData.HR);
                    await _userManager.AddToRoleAsync(appUser, ConstantData.Sales);
                }

                var admin = _userManager.GetUsersInRoleAsync(ConstantData.SuperAdmin).Result.FirstOrDefault();
                var hr = _userManager.GetUsersInRoleAsync(ConstantData.HR).Result.FirstOrDefault();

                DateTime CurrentDate = DateTime.Now;
                DateTime startDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                DateTime LastMonth = DateTime.Now.AddMonths(-1);
                DateTime monthEndDate = CurrentDate.AddMonths(+1).AddDays(-DateTime.Now.Day);

                var listofemployee = (from user in _userManager.Users
                                      join userrole in _context.UserRoles on user.Id equals userrole.UserId
                                      join role in _context.Roles on userrole.RoleId equals role.Id
                                      where role.Name == ConstantData.Employee || role.Name == ConstantData.TeamAdmin
                                      select new LeaveManagementViewModel
                                      {
                                          UserId = user.Id
                                      }).ToList();

                if (listofemployee.Count == 0)
                {
                    return;
                }

                foreach (var emp in listofemployee)
                {
                    var updEmployee = _context.ApplicationUser.Where(x => x.Id == emp.UserId).FirstOrDefault();
                    if (updEmployee.EndOfDate.HasValue && updEmployee.EndOfDate.Value.Date < CurrentDate.Date)
                    {
                        updEmployee.EmailConfirmed = false;
                        _context.ApplicationUser.Update(updEmployee);
                        _context.SaveChanges();
                    }
                }


                List<Models.LeaveManagement> leaveMngList = new List<Models.LeaveManagement>();

                var FirstLogin = _context.LeaveManagement.Where(x => listofemployee.FirstOrDefault().UserId == x.UserId.ToString() && x.FromDate.Month == CurrentDate.Month).FirstOrDefault();
                var updateEmployee = new ApplicationUser();
                if (CurrentDate.Day == 01 || FirstLogin == null || clicked == true)
                {
                    foreach (var user in listofemployee)
                    {
                        var leavemanager = _context.LeaveManagement.Where(x => user.UserId == x.UserId.ToString() && x.FromDate.Month == CurrentDate.Month).FirstOrDefault();
                        if (leavemanager == null)
                        {
                            var old_leavemanager = _context.LeaveManagement.Where(x => user.UserId == x.UserId.ToString() && x.FromDate.Month == LastMonth.Month).FirstOrDefault();

                            decimal leavebucket;

                            var employeeTypeDT = (from users in _userManager.Users
                                                  join types in _context.Datamaster on users.Id equals user.UserId
                                                  where users.EmployeeType == types.Id
                                                  select types.Text).FirstOrDefault();

                            if (employeeTypeDT == ConstantData.Permanent)
                            {
                                if (CurrentDate.Month == 01)
                                {
                                    updateEmployee = _context.ApplicationUser.Where(x => x.Id == user.UserId).FirstOrDefault();
                                    updateEmployee.PaidLeave = 12;
                                    leavebucket = 0;

                                    _context.ApplicationUser.Update(updateEmployee);
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    if (old_leavemanager == null)
                                    {
                                        leavebucket = 0;
                                    }
                                    else if (old_leavemanager.IsMonthlyLeaveLeft == true)
                                    {
                                        leavebucket = old_leavemanager.LeaveBuket + 1;
                                    }
                                    else
                                    {
                                        leavebucket = old_leavemanager.LeaveBuket;
                                    }
                                }

                                if (hr != null)
                                {
                                    leaveMngList.Add(new Models.LeaveManagement()
                                    {
                                        CreatedBy = hr.FirstName + "" + hr.LastName,
                                        CreatedDate = CurrentDate,
                                        FromDate = startDate,
                                        ToDate = monthEndDate,
                                        UserId = new Guid(user.UserId),
                                        IsMonthlyLeaveLeft = true,
                                        LeaveBuket = leavebucket
                                    });
                                }
                                else
                                {
                                    leaveMngList.Add(new Models.LeaveManagement()
                                    {
                                        CreatedBy = admin.FirstName + "" + admin.LastName,
                                        CreatedDate = CurrentDate,
                                        FromDate = startDate,
                                        ToDate = monthEndDate,
                                        UserId = new Guid(user.UserId),
                                        IsMonthlyLeaveLeft = true,
                                        LeaveBuket = leavebucket
                                    });
                                }


                            }
                            else
                            {

                                if (hr != null)
                                {
                                    leaveMngList.Add(new Models.LeaveManagement()
                                    {
                                        CreatedBy = hr.FirstName + "" + hr.LastName,
                                        CreatedDate = CurrentDate,
                                        FromDate = startDate,
                                        ToDate = monthEndDate,
                                        UserId = new Guid(user.UserId),
                                        IsMonthlyLeaveLeft = false,
                                        LeaveBuket = 0
                                    });
                                }
                                else
                                {
                                    leaveMngList.Add(new Models.LeaveManagement()
                                    {
                                        CreatedBy = admin.FirstName + "" + admin.LastName,
                                        CreatedDate = CurrentDate,
                                        FromDate = startDate,
                                        ToDate = monthEndDate,
                                        UserId = new Guid(user.UserId),
                                        IsMonthlyLeaveLeft = false,
                                        LeaveBuket = 0
                                    });
                                }
                            }

                        }

                    }
                    await _context.LeaveManagement.AddRangeAsync(leaveMngList);
                    await _context.SaveChangesAsync();

                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder().UseStartup<Startup>();
        }
    }
}