using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class OperationInfo
    {
        public OperationInfo()
        {
            this.Activities = new List<Activity>();
            this.Animals = new List<Animal>();
            this.Asks = new List<Ask>();
            this.Blogs = new List<Blog>();
            this.Helps = new List<Help>();
            this.Knowledges = new List<Knowledge>();
            this.Messages = new List<Message>();
            this.News = new List<News>();
            this.Shelters = new List<Shelter>();
        }

        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Animal> Animals { get; set; }
        public virtual ICollection<Ask> Asks { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Help> Helps { get; set; }
        public virtual ICollection<Knowledge> Knowledges { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Shelter> Shelters { get; set; }
    }
}
