using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Renci.SshNet.Messages.Authentication;
using TasksLab.Models;

namespace TasksLab.Controllers
{
    public class TasksController : Controller
    {
        private readonly TasksLabContext _context;

        public TasksController(TasksLabContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            var tabs = _context.StatusTabs.ToList();
            return View(tabs);
        }

        public async Task<JsonResult> ListAJAX([FromQuery] int page = 0, [FromQuery] int limit = 20, [FromQuery] int tab = 1)
        {
            return Json(new
            {
                success = true,
                html = await this.RenderViewAsync("TaskList", 
                    _context.Tasks.Include(x=>x.TaskStatusNavigation)
                        .Where(x=>x.TaskStatusNavigation.StatusTab == tab)
                        .Skip(page * limit)
                        .Take(limit)
                        .ToList()
                    ,
                    true)
            });
        }

        public IActionResult Welcome()
        {
            return View();
        }
            
        [HttpGet]
        public async Task<JsonResult> AddTask()
        {
            return Json(new
            {
                success=true, 
                html=await this.RenderViewAsync<Object>("AddTask", null, true)
            });
        }

        [HttpPost]
        public async Task<JsonResult> AddTask([FromForm] string taskName = "", [FromForm] string description = "", [FromForm] string dueDate = "")
        {
            int taskStatus = 1;
            try
            {
                DateTime myDueDate = DateTime.Parse(dueDate);
                _context.Tasks.Add(new Tasks {TaskName = taskName, TaskDescription = description, DueDate = myDueDate,TaskStatus = taskStatus});
                await _context.SaveChangesAsync();
                return Json(new {
                    success = true,
                    html = ""
                });
            }
            catch
            {
                return Json(new {
                    success = false,
                    html = ""
                });
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
            Tasks t = await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == taskId);
            
            if (t != null)
            {
                t.TaskStatus = (await _context.TaskStatus.FirstOrDefaultAsync((x) => x.StatusName == "Complete")).StatusId;
                _context.Tasks.Update(t);
                await _context.SaveChangesAsync();
                
                return Json(new
                    {
                        success=true,
                        html=await this.RenderViewAsync("TaskListItem", await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == taskId), true)
                    }
                );
            }
            return Json(new
                {
                    success=false,
                    html=""
                }
            );
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}