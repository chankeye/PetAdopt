using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Help
    {
        public Help()
        {
            this.Messages = new List<Message>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public short ClassId { get; set; }
        public int AreaId { get; set; }
        public string Address { get; set; }
        public int OperationId { get; set; }
        public string CoverPhoto { get; set; }
        public virtual Area Area { get; set; }
        public virtual Class Class { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
