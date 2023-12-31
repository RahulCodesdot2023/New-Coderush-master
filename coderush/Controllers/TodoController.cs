﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using coderush.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace coderush.Controllers
{
    [Authorize(Roles = "SuperAdmin,HR,Employee")]
    public class TodoController : Controller
    {
        private readonly Services.Security.ICommon _security;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TodoController(Services.Security.ICommon security, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _security = security;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            //_hostingEnvironment = hostingEnvironment;
        }

        //dependency injection through constructor, to directly access services
        //public TodoController(ApplicationDbContext context) {
        //    _context = context;
        //}
        //consume db context service, display all todo items
        public IActionResult Index()
        {
            var tododata = (from Todo in _context.Todo
                                //where Todo.IsDone == true
                            select new TodoViewModel
                            {
                                TodoId = Todo.TodoId,
                                Users = _context.Users.Where(x => x.Id == Todo.Users).Select(x => x.UserName).FirstOrDefault(),
                                TodoItem = Todo.TodoItem,
                                Duedate = Todo.Duedate,
                                CreatedDate = Todo.CreatedDate,
                                FileName = Todo.FileUpload == null ? "" : Todo.FileUpload,
                                IsDone = Todo.IsDone,

                            }).OrderByDescending(x => x.CreatedDate).ToList();
            return View(tododata);

            //var todos = _context.Todo.OrderByDescending(x => x.CreatedDate).ToList();
            //return View(todos);
        }

        //display todo create edit form
        [HttpGet]
        public async Task<IActionResult> Form(string id)
        {
            var userDetail = _userManager.GetUserAsync(User).Result;
            var roleHR = await _userManager.IsInRoleAsync(userDetail, "HR");
            var roleSuperAdmin = await _userManager.IsInRoleAsync(userDetail, "SuperAdmin");
            TodoViewModel newTodo = new TodoViewModel();

            if (!roleHR && !roleSuperAdmin)
                newTodo.Users = userDetail.Id;

            ViewBag.Selectusers = _userManager.Users
                               .Where(w => w.UserName != null)
                              .Select(s => new SelectListItem()
                              {
                                  Text = s.FirstName + " " + s.LastName,
                                  Value = s.Id.ToString()
                              }).ToList();
            //return Json(Selectusers);

            //create new
            if (id == null)
            {
                return View(newTodo);
            }

            //edit todo
            TodoViewModel todo = new TodoViewModel();
            var edittodo = _context.Todo.Where(x => x.TodoId.Equals(id)).FirstOrDefault();
            todo.TodoId = edittodo.TodoId;
            todo.CreatedDate = edittodo.CreatedDate;
            todo.Duedate = edittodo.Duedate;
            todo.Users = edittodo.Users;
            todo.IsDone = edittodo.IsDone;
            todo.TodoItem = edittodo.TodoItem;
            if (todo == null)
            {
                return NotFound();
            }
            return View(todo);
        }

        //post submitted todo data. if todo.TodoId is null then create new, otherwise edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitForm([Bind("TodoId", "TodoItem", "Duedate", "FileUpload", "Users", "IsDone")] TodoViewModel todo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[StaticString.StatusMessage] = "Error: Model state not valid.";
                    return RedirectToAction(nameof(Form), new { id = todo.TodoId ?? "" });
                }

                var user = _userManager.GetUserAsync(User).Result;

                string wwwPath = this._webHostEnvironment.WebRootPath;
                string contentPath = this._webHostEnvironment.ContentRootPath;
                var filename = "";
                if (todo.FileUpload != null)
                {
                    filename = todo.FileUpload.FileName;
                    string path = Path.Combine(this._webHostEnvironment.WebRootPath, "document/Todo");
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}
                    List<string> uploadedFiles = new List<string>();

                    string fileName = Path.GetFileName(todo.FileUpload.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        todo.FileUpload.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                    }
                }
                //create new
                if (todo.TodoId == null)
                {
                    Todo newTodo = new Todo();
                    newTodo.TodoId = Guid.NewGuid().ToString();
                    newTodo.Duedate = todo.Duedate;
                    if (todo.FileUpload != null)
                        newTodo.FileUpload = todo.FileUpload.FileName.ToString();
                    else
                        newTodo.FileUpload = null;
                    newTodo.CreatedDate = DateTime.Now;
                    newTodo.Users = todo.Users;
                    newTodo.TodoItem = todo.TodoItem;
                    newTodo.Duedate = todo.Duedate;
                    newTodo.IsDone = todo.IsDone;
                    _context.Todo.Add(newTodo);
                    _context.SaveChanges();

                    TempData[StaticString.StatusMessage] = "Create new todo item success.";
                    return RedirectToAction(nameof(Form), new { id = todo.TodoId ?? "" });
                }

                //edit existing
                Todo editTodo = new Todo();
                editTodo = _context.Todo.Where(x => x.TodoId.Equals(todo.TodoId)).FirstOrDefault();
                editTodo.TodoItem = todo.TodoItem;
                editTodo.Duedate = todo.Duedate;
                editTodo.Users = todo.Users;
                if (editTodo.FileUpload != null)
                    editTodo.FileUpload = todo.FileUpload.FileName.ToString();
                else
                    editTodo.FileUpload = "";

                editTodo.IsDone = todo.IsDone;
                _context.Update(editTodo);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Edit existing todo item success.";
                return RedirectToAction(nameof(Form), new { id = todo.TodoId ?? "" });
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Form), new { id = todo.TodoId ?? "" });
            }
        }

        //display todo item for deletion
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = _context.Todo.Where(x => x.TodoId.Equals(id)).FirstOrDefault();
            return View(todo);
        }

        //delete submitted todo item if found, otherwise 404                                     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitDelete([Bind("TodoId")] Todo todo)
        {
            try
            {
                var deleteTodo = _context.Todo.Where(x => x.TodoId.Equals(todo.TodoId)).FirstOrDefault();
                if (deleteTodo == null)
                {
                    return NotFound();
                }

                _context.Todo.Remove(deleteTodo);
                _context.SaveChanges();

                TempData[StaticString.StatusMessage] = "Delete todo item success.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData[StaticString.StatusMessage] = "Error: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id = todo.TodoId ?? "" });
            }
        }
    }
}