using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Blog_Message_Mapping
    {
        public int MessageId { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
