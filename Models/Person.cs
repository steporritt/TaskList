using System;
using System.Collections.Generic;

namespace TaskList.Models
{
    public partial class Person
    {
        public Person()
        {
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Task> Task { get; set; }
    }
}
