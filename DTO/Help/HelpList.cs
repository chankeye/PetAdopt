using System.Collections.Generic;

namespace PetAdopt.DTO.Help
{
    public class HelpItem
    {
        public int Id { get; set; }

        public string Photo { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Message { get; set; }

        public string Area { get; set; }

        public string Classes { get; set; }
    }

    public class HelpList
    {
        public List<HelpItem> List { get; set; }

        public int Count { get; set; }
    }
}