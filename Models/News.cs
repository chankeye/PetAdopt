using System;
using System.Collections.Generic;

namespace PetAdopt.Models
{
    public partial class News
    {
        public News()
        {
            this.News_Message_Mapping = new List<News_Message_Mapping>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public Nullable<short> AreaId { get; set; }
        public string Url { get; set; }
        public int OperationId { get; set; }
        public string CoverPhoto { get; set; }
        public virtual Area Area { get; set; }
        public virtual OperationInfo OperationInfo { get; set; }
        public virtual ICollection<News_Message_Mapping> News_Message_Mapping { get; set; }
    }
}
