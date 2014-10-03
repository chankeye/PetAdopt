using System.Collections.Generic;

namespace PetAdopt.DTO.Blog
{
    public class BlogMessageItem
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Message { get; set; }

        public string Account { get; set; }
    }

    public class BlogMessageList
    {
        public List<BlogMessageItem> List { get; set; }

        public int Count { get; set; }
    }
}