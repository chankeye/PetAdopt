using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Picture
    {
        public Picture()
        {
            this.Animals = new List<Animal>();
            this.Shelters = new List<Shelter>();
        }

        public int Id { get; set; }
        public string Position { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual ICollection<Shelter> Shelters { get; set; }
    }
}
