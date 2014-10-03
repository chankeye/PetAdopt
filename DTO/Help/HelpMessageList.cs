using System.Collections.Generic;

namespace PetAdopt.DTO.Help
{
    public class HelpMessageItem
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Message { get; set; }

        public string Account { get; set; }
    }

    public class HelpMessageList
    {
        public List<HelpMessageItem> List { get; set; }

        public int Count { get; set; }
    }
}