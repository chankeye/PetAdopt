namespace PetAdopt.DTO.Animal
{
    public class GetAnimal
    {
        public string Photo { get; set; }

        public string Title { get; set; }

        public short? Age { get; set; }

        public string Introduction { get; set; }

        public int? SheltersId { get; set; }

        public string Shelters { get; set; }

        public short ClassId { get; set; }

        public string Class { get; set; }

        public short StatusId { get; set; }

        public string Status { get; set; }

        public int? AreaId { get; set; }

        public string Area { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Date { get; set; }

        public string UserDisplay { get; set; }
    }
}