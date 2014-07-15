using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Message
    {
        public Message()
        {
            this.Knowledge_Message_Mapping = new List<Knowledge_Message_Mapping>();
        }

        public int Id { get; set; }
        public string Message1 { get; set; }
        public int OperationId { get; set; }
        public virtual ICollection<Knowledge_Message_Mapping> Knowledge_Message_Mapping { get; set; }
    }
}
