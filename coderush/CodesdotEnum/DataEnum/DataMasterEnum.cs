﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace coderush.DataEnum
{
    public enum DataSelection
    {
        Expenses = 1,
        LEAVE = 2,
        technologies = 3,
        HRAnnocuncement = 4,
        EmployeeType = 15,
        LeaveReasons=16,
    }
    public enum CandidateTechnologies
    {
        Asp_net = 1,
        Wordpress = 2,
        React = 3,
        Shopify = 4
    }
    public enum Expensestype
    {
        office = 1,
        other = 2,
        system = 3
    }
    public enum Technologies
    {
        Asp_net = 1,
        Wordpress = 2,
        React = 3,
        Shopify = 4
    }
    public enum LeadTechnologies
    {
        Asp_net = 1,
        Wordpress = 2,
        React = 3,
        Shopify = 4
    }
    public enum LeaveStatus
    {
        [Display(Name = "Leave In Process")]
        Inprogress = 0,

        [Display(Name = "Leave Accepted")]
        Accepted = 1,

        [Display(Name = "Leave Is Rejected")]
        Rejected = 2,


    }
    public enum LeaveType
    {
        Half = 1,
        Full = 2
    }


    public enum Weeks
    {
        Monday = 1,
        Tuesday = 2,
        Wedneseday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }
}
