﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Services.App
{
    //static class for app pages common information
    public static partial class Pages
    {
        public static class DoneSatus
        {
            public const string ControllerName = "DoneSatus";
            public const string RoleName = "DoneSatus";
            public const string UrlDefault = "/DoneSatus/DoneStatusIndex";
            public const string NavigationName = "Done Satus";
        }
        public static class Hom
        {
            public const string ControllerName = "Dashboard";
            public const string RoleName = "Home";
            public const string UrlDefault = "/Dashboard/Home";
            public const string NavigationName = "Home";
            public const string ActionName = "Home";
        }
        public static class DashboardIndex
        {
            public const string ControllerName = "Dashboard";
            public const string RoleName = "Index";
            public const string UrlDefault = "/Dashboard/DashboardIndex";
            public const string NavigationName = "Dashboard";
            public const string ActionName = "DashboardIndex";
        }
        public static class Schedule
        {
            public const string ControllerName = "ScheduleInterView";
            public const string RoleName = "Schedule InterView";
            public const string UrlDefault = "/ScheduleInterView/ScheduleIndex";
            public const string NavigationName = "Schedule InterView";
        }
        public static class InprogressInterview
        {
            public const string ControllerName = "InprogressInterview";
            public const string RoleName = "InprogressInterview";
            public const string UrlDefault = "/InprogressInterview/InprogressIndex";
            public const string NavigationName = "Inprogress Interview";
        }
        public static class Holiday
        {
            public const string ControllerName = "HolidayList";
            public const string RoleName = "Holiday List";
            public const string UrlDefault = "/HolidayList/HolidayIndex";
            public const string NavigationName = "Holiday List";
        }
        public static class Project
        {
            public const string ControllerName = "ProjectMaster";
            public const string RoleName = "ProjectMaster Index";
            public const string UrlDefault = "/ProjectMaster/ProjectIndex";
            public const string NavigationName = "Projects";
        }
        public static class Contact
        {
            public const string ControllerName = "Contact";
            public const string RoleName = "Contact";
            public const string UrlDefault = "/Contact/ContactIndex";
            public const string NavigationName = "Contact";
        }

        public static class Hire
        {
            public const string ControllerName = "Hire Us";
            public const string RoleName = "Hire";
            public const string UrlDefault = "/Hire/HireIndex";
            public const string NavigationName = "Hire";
        }
        public static class DataMaster
        {
            public const string ControllerName = "DataMaster";
            public const string RoleName = "DataMaster";
            public const string UrlDefault = "/DataMaster/Index";
            public const string NavigationName = "Data";
        }
        //public static class LeadMaster
        //{
        //    public const string ControllerName = "LeadMaster";
        //    public const string RoleName = "LeadMaster";
        //    public const string UrlDefault = "/LeadMaster/LeadIndex";
        //    public const string NavigationName = "Lead";
        //}
        public static class ExpenseMaster
        {
            public const string ControllerName = "ExpenseMaster";
            public const string RoleName = "ExpenseMaster";
            public const string UrlDefault = "/ExpenseMaster/ExpenseIndex";
            public const string NavigationName = "Expense";
        }
        public static class CandidateMaster
        {
            public const string ControllerName = "CandidateMaster";
            public const string RoleName = "CandidateMaster";
            public const string UrlDefault = "/CandidateMaster/CandidateIndex";
            public const string NavigationName = "Candidate";
        }
        public static class LeaveManagement     //new leaveManagement create sidebar menu
        {
            public const string ControllerName = "LaveManagement";
            public const string RoleName = "LeaveManagement";
            public const string UrlDefault = "/LeaveManagement/LeaveManagIndex";
            public const string NavigationName = "Leave Management";
        }

        //public static class ProjectMaster
        //{
        //    public const string ControllerName = "ProjectMaster";
        //    public const string RoleName = "ProjectMaster";
        //    public const string UrlDefault = "/ProjectMaster/ProjectIndex";
        //    public const string NavigationName = "Project";
        //}
        //public static class InvoiceMaster
        //{
        //    public const string ControllerName = "InvoiceMaster";
        //    public const string RoleName = "InvoiceMaster";
        //    public const string UrlDefault = "/InvoiceMaster/InvoiceIndex";
        //    public const string NavigationName = "Invoice";
        //}
        public static class Leavecount
        {
            public const string ControllerName = "Leavecount";
            public const string RoleName = "Leavecount";
            public const string UrlDefault = "/Leavecount/LeaveIndex";
            public const string NavigationName = "Leave";
        }
        public static class Todo
        {
            public const string ControllerName = "Todo";
            public const string RoleName = "Todo";
            public const string UrlDefault = "/Todo/Index";
            public const string NavigationName = "Todo";
        }
        public static class Home
        {
            public const string ControllerName = "Home";
            public const string RoleName = "Home";
            public const string UrlDefault = "/Home/Index";
            public const string NavigationName = "Home";
        }

        public static class EmployeeMaster
        {
            public const string ControllerName = "EmployeeMaster";
            public const string RoleName = "EmployeeMaster";
            public const string UrlDefault = "/EmployeeMaster/Index";
            public const string NavigationName = "Employee Master";
        }

        public static class Role
        {
            public const string ControllerName = "Role";
            public const string RoleName = "Role";
            public const string UrlDefault = "/Role/Index";
            public const string NavigationName = "Role";
        }
        public static class RoleDetails
        {
            public const string ControllerName = "RoleDetails";
            public const string RoleName = "RoleDetails";
            public const string UrlDefault = "/RoleDetails/RoleIndex";
            public const string NavigationName = "Role Details";
        }

        public static class Payroll
        {
            public const string ControllerName = "Payroll";
            public const string RoleName = "Payroll";
            public const string UrlDefault = "/Payroll/Index";
            public const string NavigationName = "Payroll";
        }

        public static class MonthlyReports
        {
            public const string ControllerName = "MonthlyReports";
            public const string RoleName = "MonthlyReports";
            public const string UrlDefault = "/MonthlyReports/Index";
            public const string UrlDefault1 = "/MonthlyReports/EmployeeLeaveReport";
            public const string NavigationName = "Monthly Reports";
            public const string NavigationName1 = "Employee Attendance";
        }

        public static class TimeSheet
        {
            public const string ControllerName = "TimeSheet";
            public const string RoleName = "TimeSheet";
            public const string UrlDefault = "/TimeSheet/Index";
            public const string NavigationName = "Time Sheet";
        }

    }
}
