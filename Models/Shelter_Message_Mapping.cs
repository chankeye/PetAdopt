using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Shelter_Message_Mapping
    {
        public int MessageId { get; set; }
        public int ShelterId { get; set; }
        public virtual Shelter Shelter { get; set; }
    }
}
