using System.Collections.Generic;

namespace PetAdopt.DTO.Animal
{
    public class AnimalMessageItem
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Message { get; set; }

        public string Account { get; set; }
    }

    public class AnimalMessageList
    {
        public List<AnimalMessageItem> List { get; set; }

        public int Count { get; set; }
    }
}