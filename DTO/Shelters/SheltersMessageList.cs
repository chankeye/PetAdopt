using System.Collections.Generic;

namespace PetAdopt.DTO.Shelters
{
    public class SheltersMessageItem
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Message { get; set; }

        public string Account { get; set; }
    }

    public class SheltersMessageList
    {
        public List<SheltersMessageItem> List { get; set; }

        public int Count { get; set; }
    }
}