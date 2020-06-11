using System;
using System.Collections.Generic;

namespace TaskList.Models
{
    public partial class TaskAttachment
    {
        public int TaskId { get; set; }
        public int AttachmentId { get; set; }

        public virtual Attachment Attachment { get; set; }
        public virtual Task Task { get; set; }
    }
}
