using System.Collections.Generic;
using System.Threading.Tasks;

namespace TasksLab.Models
{
    public class TasksWithStatuses
    {
        public List<Tasks> Tasks
        {
            get;
            internal set;
        }

        public List<TaskStatus> Statuses
        {
            get;
            internal set;
        }
    }
}