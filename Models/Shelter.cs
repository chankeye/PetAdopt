using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Shelter
    {
        public Shelter()
        {
            this.Animals = new List<Animal>();
            this.Shelter_Message_Mapping = new List<Shelter_Message_Mapping>();
            this.Shelter_Picture_Mapping = new List<Shelter_Picture_Mapping>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Url { get; set; }
        public short AearId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int OperationId { get; set; }
        public string CoverPoto { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual Area Area { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
        public virtual ICollection<Shelter_Message_Mapping> Shelter_Message_Mapping { get; set; }
        public virtual ICollection<Shelter_Picture_Mapping> Shelter_Picture_Mapping { get; set; }
    }
}
