namespace PetAdopt.DTO
{
    /// <summary>
    /// 首頁輪播圖
    /// </summary>
    public class Carousel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 封面圖
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        public string EndDate { get; set; }

        /// <summary>
        /// 內文
        /// </summary>
        public string Detail { get; set; }

        public string Alt { get; set; }

        public string ButtonText { get; set; }

        /// <summary>
        /// 分類
        /// </summary>
        public string Class { get; set; }
    }
}