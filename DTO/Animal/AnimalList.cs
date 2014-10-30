using System.Collections.Generic;

namespace PetAdopt.DTO.Animal
{
    public class AnimalItem
    {
        public int Id { get; set; }

        public string Photo { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Introduction { get; set; }

        public string Area { get; set; }

        public string Classes { get; set; }

        public string Status { get; set; }
    }

    public class AnimalList
    {
        public List<AnimalItem> List { get; set; }

        public int Count { get; set; }
    }
}