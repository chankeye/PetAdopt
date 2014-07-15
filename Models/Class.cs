using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Class
    {
        public Class()
        {
            this.Animals = new List<Animal>();
            this.Asks = new List<Ask>();
            this.Blogs = new List<Blog>();
            this.Helps = new List<Help>();
            this.Knowledges = new List<Knowledge>();
        }

        public short Id { get; set; }
        public string Word { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual ICollection<Ask> Asks { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Help> Helps { get; set; }
        public virtual ICollection<Knowledge> Knowledges { get; set; }
    }
}
