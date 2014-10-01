using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Animal
    {
        public Animal()
        {
            this.Blogs = new List<Blog>();
            this.Messages = new List<Message>();
            this.Pictures = new List<Picture>();
        }

        public int Id { get; set; }
        public string CoverPhoto { get; set; }
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
        public string Title { get; set; }
        public virtual Area Area { get; set; }
        public virtual Class Class { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
        public virtual Shelter Shelter { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
