﻿using coderush.Models;
using coderush.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 


namespace coderush.Services.Security
{
    public interface ICommon
    {
        Task CreateDefaultSuperAdmin();

        List<String> GetAllRoles();

        List<ApplicationUser> GetAllMembers();

        ApplicationUser GetMemberByApplicationId(string applicationId);

        Task<ApplicationUser> CreateApplicationUser(ApplicationViewModel applicationUser, string password);

        List<ApplicationUser> GetAllEmployee(DateTime monthFirstDate, bool isActive = true);


    }
}
