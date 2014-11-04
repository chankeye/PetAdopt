namespace PetAdopt.DTO.Blog
{
    /// <summary>
    /// 新增文章
    /// </summary>
    public class CreateBlog
    {
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 關聯動物文章標題
        /// </summary>
        public string AnimalTitle { get; set; }

        /// <summary>
        /// Class.Id
        /// </summary>
        public short ClassId { get; set; }
    }
}