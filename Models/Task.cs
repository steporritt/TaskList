using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskList.Models
{
    public partial class Task
    {
        public Task()
        {
            TaskAttachment = new HashSet<TaskAttachment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? StatusId { get; set; }
        public DateTime? DueDate { get; set; }
        public int? AssignedPersonId { get; set; }
        public virtual Person AssignedPerson { get; set; }
        public virtual Status Status { get; set; }
        [NotMapped]
        public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; }
        public virtual ICollection<TaskAttachment> TaskAttachment { get; set; }
    }
}
