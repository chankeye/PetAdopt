using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Activity_Message_Mapping
    {
        public int MessageId { get; set; }
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
    }
}
