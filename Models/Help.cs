using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Help
    {
        public Help()
        {
            this.Help_Message_Mapping = new List<Help_Message_Mapping>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public short ClassId { get; set; }
        public short AreaId { get; set; }
        public string Address { get; set; }
        public int OperationId { get; set; }
        public virtual Area Area { get; set; }
        public virtual Class Class { get; set; }
        public virtual ICollection<Help_Message_Mapping> Help_Message_Mapping { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
    }
}
