using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Animal
    {
        public Animal()
        {
            this.Animal_Message_Mapping = new List<Animal_Message_Mapping>();
            this.Animal_Picture_Mapping = new List<Animal_Picture_Mapping>();
            this.Blogs = new List<Blog>();
        }

        public int Id { get; set; }
        public string CoverPoto { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public Nullable<short> Age { get; set; }
        public Nullable<int> SheltersId { get; set; }
        public Nullable<short> AreaId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public short StatusId { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int OperationId { get; set; }
        public short ClassId { get; set; }
        public virtual Area Area { get; set; }
        public virtual Class Class { get; set; }
        public virtual ICollection<Animal_Message_Mapping> Animal_Message_Mapping { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
        public virtual ICollection<Animal_Picture_Mapping> Animal_Picture_Mapping { get; set; }
        public virtual Shelter Shelter { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
