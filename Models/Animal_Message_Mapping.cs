using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class Animal_Message_Mapping
    {
        public int MessageId { get; set; }
        public int AnimalId { get; set; }
        public virtual Animal Animal { get; set; }
    }
}
