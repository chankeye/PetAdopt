using System;

namespace PetAdopt.DTO.Animal
{
    public class EditAnimal
    {
        public int Id { get; set; }

        public string Photo { get; set; }

        public string Title { get; set; }

        public short? Age { get; set; }

        public string Introduction { get; set; }

        public string Shelters { get; set; }

        public short ClassId { get; set; }

        public short StatusId { get; set; }

        public short? AreaId { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}