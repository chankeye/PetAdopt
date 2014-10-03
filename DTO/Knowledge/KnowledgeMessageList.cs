using System.Collections.Generic;

namespace PetAdopt.DTO.Knowledge
{
    public class KnowledgeMessageItem
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Message { get; set; }

        public string Account { get; set; }
    }

    public class KnowledgeMessageList
    {
        public List<KnowledgeMessageItem> List { get; set; }

        public int Count { get; set; }
    }
}