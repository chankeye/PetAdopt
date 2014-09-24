using System.Collections.Generic;

namespace PetAdopt.DTO.News
{
    public class NewsItem
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class NewsList
    {
        public List<NewsItem> List { get; set; }

        public int Count { get; set; }
    }
}