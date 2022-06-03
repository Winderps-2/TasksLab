using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Renci.SshNet.Messages.Authentication;
using TasksLab.Models;

namespace TasksLab.Controllers
{
    public class TasksController : Controller
    {
        private readonly TasksLabContext _context;
        public TasksController (TasksLabContext context)
        {
            _context = context;
        }

        public IActionResult List([FromQuery] int page = 0, [FromQuery] int limit = 20)
        {
            var tasksList = _context.Tasks.Skip(page * limit).Take(limit).ToList();
            return View(tasksList);
        }

        public IActionResult Welcome()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTask([FromForm] string taskName = "", [FromForm] string description = "", [FromForm] string dueDate = "")
        {
            int taskStatus = 1;
            try
            {
                DateTime myDueDate = DateTime.Parse(dueDate);
                _context.Tasks.Add(new Tasks {TaskName = taskName, TaskDescription = description, DueDate = myDueDate,TaskStatus = taskStatus});
                _context.SaveChanges();
                return RedirectToAction("List", "Tasks");
            }
            catch
            {
                return RedirectToAction("List", "Tasks");
            }
        }
        
        [HttpPost]
        public async Task<JsonResult> Delete([FromForm] int taskId)
        {
            Tasks t = _context.Tasks.FirstOrDefault(x => x.TaskId == taskId);
            
            if (t != null)
            {
                _context.Tasks.Remove(t);
                await _context.SaveChangesAsync();
                
                return Json(
                    new AJAXResult(true, "")
                );
            }
            return Json(
                new AJAXResult(false, "error")
            );
        }
        
        [HttpPost]
        public async Task<JsonResult> Complete([FromForm] int taskId)
        {
            Tasks t = _context.Tasks.FirstOrDefault(x => x.TaskId == taskId);
            
            if (t != null)
            {
                t.TaskStatus = _context.TaskStatus.FirstOrDefault((x) => x.StatusName == "Complete").StatusId;
                _context.Tasks.Update(t);
                await _context.SaveChangesAsync();
                
                return Json(
                    new AJAXResult(true, "")
                );
            }
            return Json(
                new AJAXResult(false, "error")
            );
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}