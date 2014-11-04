namespace PetAdopt.DTO
{
    /// <summary>
    /// 首頁輪播圖
    /// </summary>
    public class Carousel
    {
        /// <summary>
        /// Animal.Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 封面圖
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 安樂死日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 分類
        /// </summary>
        public string Class { get; set; }
    }
}