using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Status
    {
        public Status()
        {
            this.Animals = new List<Animal>();
        }

        public short Id { get; set; }
        public string Word { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
    }
}
