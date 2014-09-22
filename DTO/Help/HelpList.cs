using System.Collections.Generic;

namespace PetAdopt.DTO
{
    public class HelpItem
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class HelpList
    {
        public List<HelpItem> List { get; set; }

        public int Count { get; set; }
    }
}