using System.Collections.Generic;

namespace PetAdopt.DTO.Knowledge
{
    public class KnowledgeItem
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class KnowledgeList
    {
        public List<KnowledgeItem> List { get; set; }

        public int Count { get; set; }
    }
}