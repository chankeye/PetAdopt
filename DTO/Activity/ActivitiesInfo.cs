using System.Collections.Generic;

namespace PetAdopt.DTO
{
    public class ActivityItem
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class ActivityList
    {
        public List<ActivityItem> List { get; set; }

        public int Count { get; set; }
    }
}