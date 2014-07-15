using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Animal_Picture_Mapping
    {
        public int PictureId { get; set; }
        public int AnimalId { get; set; }
        public virtual Animal Animal { get; set; }
    }
}
