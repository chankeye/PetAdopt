using System.Collections.Generic;

namespace PetAdopt.DTO
{
    public class BlogItem
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class BlogList
    {
        public List<BlogItem> List { get; set; }

        public int Count { get; set; }
    }
}