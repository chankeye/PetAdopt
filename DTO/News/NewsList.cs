using System.Collections.Generic;

namespace PetAdopt.DTO.News
{
    public class NewsItem
    {
        public int Id { get; set; }

        public string Photo { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Message { get; set; }

        public string Area { get; set; }
    }

    public class NewsList
    {
        public List<NewsItem> List { get; set; }

        public int Count { get; set; }
    }
}