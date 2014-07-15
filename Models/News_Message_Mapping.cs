using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class News_Message_Mapping
    {
        public int MessageId { get; set; }
        public int NewsId { get; set; }
        public virtual News News { get; set; }
    }
}
