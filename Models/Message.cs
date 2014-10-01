using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Message
    {
        public Message()
        {
            this.Activities = new List<Activity>();
            this.Animals = new List<Animal>();
            this.Asks = new List<Ask>();
            this.Blogs = new List<Blog>();
            this.Helps = new List<Help>();
            this.Knowledges = new List<Knowledge>();
            this.News = new List<News>();
            this.Shelters = new List<Shelter>();
        }

        public int Id { get; set; }
        public string Message1 { get; set; }
        public int OperationId { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual ICollection<Ask> Asks { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Help> Helps { get; set; }
        public virtual ICollection<Knowledge> Knowledges { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Shelter> Shelters { get; set; }
    }
}
