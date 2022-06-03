using System;
using System.Collections.Generic;

namespace TasksLab.Models
{
    public partial class TaskStatus
    {
        public TaskStatus()
        {
            Tasks = new HashSet<Tasks>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusColorHex { get; set; }
        public int StatusTab { get; set; }

        public virtual StatusTabs StatusTabNavigation { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
