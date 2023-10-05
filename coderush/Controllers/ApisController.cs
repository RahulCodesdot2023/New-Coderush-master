using coderush.Data;
using coderush.Models;
using CodesDotHRMS.Models;
using CodesDotHRMS.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace CodesDotHRMS.Controllers
{
  [ApiController]
  public class ApisController : ControllerBase
  {
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public ApisController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        IConfiguration config)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _context = context;
      _config = config;
    }
    [Route("api/LoginApi")]
    public string LoginApi(string Email, string Password)
    {
      var response = new HttpResponseMessage(HttpStatusCode.OK);
      try
      {
        //var ConvertJsonData = JsonConvert.DeserializeObject<MachineData>(JsonData);
        var result = _signInManager.PasswordSignInAsync(Email, Password, false, lockoutOnFailure: true).Result;
        if (result.Succeeded)
        {
          var userdata = _context.ApplicationUser.Where(x => x.Email == Email).FirstOrDefault();

          return userdata.Id;
          //response.Content = new StringContent(JsonConvert.SerializeObject(userdata.Id));
        }
        if (result.IsLockedOut)
        {
          return "User account locked out.";
        }
        else
        {
          return "Invalid login attempt.";
        }
      }
      catch (Exception ex)
      {
      }
      finally
      {
      }
      return "";
    }
    [Route("api/GetFilseByUserid")]
    public List<FIleModel> GetFilseByUserid(string UserId)
    {
      var response = new HttpResponseMessage(HttpStatusCode.OK);
      var StartPath = "C:\\";

      var path = Path.Combine(StartPath, UserId);

      List<FIleModel> AllFiles = new List<FIleModel>();

      if (Directory.Exists(path))
      {
        var user = _userManager.GetUserAsync(User).Result;
        var member = _context.ApplicationUser.Where(x => x.Id.Equals(UserId)).FirstOrDefault();
        var userrols = _userManager.GetRolesAsync(member).Result;
        var isadmin = userrols.Contains("SuperAdmin");

        var connectionstring = _config.GetValue<string>("ConnectionStrings:DefaultConnection").ToString();

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@isadmin", isadmin);
        parameters.Add("@userId", UserId);

        var MasterListDataSet = DbAccess.ExecuteStoredProc("usp_GetProjectList", parameters, connectionstring);

        var _ProjectList = DbAccess.ConvertDataTableToList<ProjectMasterViewModel>(MasterListDataSet.Tables[0]);

        foreach (var item in _ProjectList)
        {
          var ProjectName = item.Id + "_" + item.projectname;
          var projectPath = Path.Combine(path, ProjectName);

          if (Directory.Exists(projectPath))
          {

            var CurentDate = DateTime.Now.ToString("dd-MMM-yyyy");

            var Datepath = Path.Combine(projectPath, CurentDate);

            if (Directory.Exists(Datepath))
            {
              var dir = new DirectoryInfo(Datepath);

              var FileList = dir.GetFiles();
              foreach (var fileitem in FileList)
              {
                var filename = fileitem.Name;
                var filepath = Path.Combine(Datepath, filename);
                AllFiles.Add(new FIleModel() {
                FileName = filename,
                FilePath = filepath
                });
              }
            }

          }

        }
      }
      return AllFiles;

    }
  }
}
