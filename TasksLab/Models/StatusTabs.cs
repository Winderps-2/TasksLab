using System;
using System.Collections.Generic;

namespace TasksLab.Models
{
    public partial class StatusTabs
    {
        public StatusTabs()
        {
            TaskStatus = new HashSet<TaskStatus>();
        }

        public int TabId { get; set; }
        public string TabDisplayName { get; set; }

        public virtual ICollection<TaskStatus> TaskStatus { get; set; }
    }
}
