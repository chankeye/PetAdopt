using System.Collections.Generic;

namespace PetAdopt.DTO.Animal
{
    public class AnimalItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }
    }

    public class AnimalList
    {
        public List<AnimalItem> List { get; set; }

        public int Count { get; set; }
    }
}