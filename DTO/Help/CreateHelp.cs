namespace PetAdopt.DTO.Help
{
    /// <summary>
    /// 新增救援文章
    /// </summary>
    public class CreateHelp
    {
        /// <summary>
        /// 封面圖
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 分類Id
        /// </summary>
        public short ClassId { get; set; }

        /// <summary>
        /// 地區Id
        /// </summary>
        public short AreaId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
    }
}