using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Activity
    {
        public Activity()
        {
            this.Activity_Message_Mapping = new List<Activity_Message_Mapping>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Nullable<short> AreaId { get; set; }
        public string Address { get; set; }
        public int OperationId { get; set; }
        public string CoverPoto { get; set; }
        public virtual Area Area { get; set; }
        public virtual ICollection<Activity_Message_Mapping> Activity_Message_Mapping { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
    }
}
