using System.Collections.Generic;

namespace PetAdopt.DTO.Activity
{
    public class ActivityMessageItem
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Message { get; set; }

        public string Account { get; set; }
    }

    public class ActivityMessageList
    {
        public List<ActivityMessageItem> List { get; set; }

        public int Count { get; set; }
    }
}