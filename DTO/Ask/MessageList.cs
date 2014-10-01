using System.Collections.Generic;

namespace PetAdopt.DTO.Ask
{
    public class MessageItem
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Message { get; set; }

        public string Account { get; set; }
    }

    public class MessageList
    {
        public List<MessageItem> List { get; set; }

        public int Count { get; set; }
    }
}