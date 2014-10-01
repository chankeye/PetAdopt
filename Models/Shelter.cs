using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Shelter
    {
        public Shelter()
        {
            this.Animals = new List<Animal>();
            this.Messages = new List<Message>();
            this.Pictures = new List<Picture>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Url { get; set; }
        public short AreaId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int OperationId { get; set; }
        public string CoverPhoto { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual Area Area { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
    }
}
