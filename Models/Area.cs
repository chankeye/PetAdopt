using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Area
    {
        public Area()
        {
            this.Activities = new List<Activity>();
            this.Animals = new List<Animal>();
            this.Helps = new List<Help>();
            this.News = new List<News>();
            this.Shelters = new List<Shelter>();
        }

        public short Id { get; set; }
        public string Word { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual ICollection<Help> Helps { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Shelter> Shelters { get; set; }
    }
}
