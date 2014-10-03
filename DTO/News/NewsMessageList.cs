using System.Collections.Generic;

namespace PetAdopt.DTO.News
{
    public class NewsMessageItem
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Message { get; set; }

        public string Account { get; set; }
    }

    public class NewsMessageList
    {
        public List<NewsMessageItem> List { get; set; }

        public int Count { get; set; }
    }
}