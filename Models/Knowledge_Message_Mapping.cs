using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Knowledge_Message_Mapping
    {
        public int MessageId { get; set; }
        public int KnowledgeId { get; set; }
        public virtual Knowledge Knowledge { get; set; }
        public virtual Message Message { get; set; }
    }
}
