using System.Collections.Generic;

namespace PetAdopt.DTO.Shelters
{
    public class SheltersItem
    {
        public int Id { get; set; }

        public string Photo { get; set; }

        public string Name { get; set; }

        public string Area { get; set; }
    }

    public class SheltersList
    {
        public List<SheltersItem> List { get; set; }

        public int Count { get; set; }
    }
}