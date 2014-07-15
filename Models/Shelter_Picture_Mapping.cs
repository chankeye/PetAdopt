using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Shelter_Picture_Mapping
    {
        public int PictureId { get; set; }
        public int ShelterId { get; set; }
        public virtual Shelter Shelter { get; set; }
    }
}
