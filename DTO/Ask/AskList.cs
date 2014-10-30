using System.Collections.Generic;

namespace PetAdopt.DTO.Ask
{
    public class AskItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Classes { get; set; }
    }

    public class AskList
    {
        public List<AskItem> List { get; set; }

        public int Count { get; set; }
    }
}