using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Blog
    {
        public Blog()
        {
            this.Blog_Message_Mapping = new List<Blog_Message_Mapping>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Nullable<int> AnimalId { get; set; }
        public int OperationId { get; set; }
        public short ClassId { get; set; }
        public virtual Animal Animal { get; set; }
        public virtual Class Class { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
        public virtual ICollection<Blog_Message_Mapping> Blog_Message_Mapping { get; set; }
    }
}
