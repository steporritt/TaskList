using System;
using System.Collections.Generic;

namespace TaskList.Models
{
    public partial class Attachment
    {
        public Attachment()
        {
            TaskAttachment = new HashSet<TaskAttachment>();
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public byte[] FileData { get; set; }

        public virtual ICollection<TaskAttachment> TaskAttachment { get; set; }
    }
}
