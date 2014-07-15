using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Help_Message_Mapping
    {
        public int MessageId { get; set; }
        public int HelpId { get; set; }
        public virtual Help Help { get; set; }
    }
}
