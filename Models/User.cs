using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class User
    {
        public User()
        {
            this.OperationInfoes = new List<OperationInfo>();
        }

        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Display { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public System.DateTime Date { get; set; }
        public bool IsDisable { get; set; }
        public virtual ICollection<OperationInfo> OperationInfoes { get; set; }
    }
}
