using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Ask
    {
        public Ask()
        {
            this.Ask_Message_Mapping = new List<Ask_Message_Mapping>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public short ClassId { get; set; }
        public int OperationId { get; set; }
        public virtual Class Class { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
        public virtual ICollection<Ask_Message_Mapping> Ask_Message_Mapping { get; set; }
    }
}
