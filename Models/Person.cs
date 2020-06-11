using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.Models
{
    public partial class Person
    {
        public Person()
        {
            Task = new HashSet<Task>();
        }

        public int Id { get; set; }
        [NotMapped]
        public string DisplayName { get { return FirstName + ' ' + LastName; } }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Task> Task { get; set; }
    }
}
