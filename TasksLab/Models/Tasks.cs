using System;
using System.Collections.Generic;

namespace TasksLab.Models
{
    public partial class Tasks
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int TaskStatus { get; set; }
        public DateTime DueDate { get; set; }

        public virtual TaskStatus TaskStatusNavigation { get; set; }
    }
}
