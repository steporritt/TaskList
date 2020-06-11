using System;
using System.Collections.Generic;

namespace TaskList.Models
{
    public partial class Status
    {
        public Status()
        {
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Task> Task { get; set; }
    }
}
