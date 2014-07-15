using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Ask_Message_Mapping
    {
        public int MessageId { get; set; }
        public int AskId { get; set; }
        public virtual Ask Ask { get; set; }
    }
}
