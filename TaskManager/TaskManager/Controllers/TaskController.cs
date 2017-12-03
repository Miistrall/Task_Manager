using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Context;
using TaskManager.Models;


namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        public EFContext Context { get; set; }

        public TaskController(EFContext context)
        {
            Context = context;
        }

        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var taskListForUser = Context.Users.Include(x => x.Tasks).Single(x => x.UserName == userName).Tasks.OrderBy(x => x.StartDate.Date).ToList();
            return View(taskListForUser);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(TaskModel taskModel)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                Context.Users.Include(x => x.Tasks).Single(x => x.UserName == userName).Tasks.Add(taskModel);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taskModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userName = User.Identity.Name;
            var user = Context.Users.Include(x => x.Tasks).Single(x => x.UserName == userName);
            var task = user.Tasks.Single(x => x.Id == id);
           
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskModel taskModel)
        {
            if (ModelState.IsValid)
            {
                Context.Update(taskModel);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return Edit(taskModel);
        }

        public IActionResult Remove(int id)
        {
            var taskToDelete = Context.Tasks.Single(x => x.Id == id);
            Context.Tasks.Remove(taskToDelete);
            Context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
