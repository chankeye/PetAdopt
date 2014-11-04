namespace PetAdopt.DTO.Blog
{
    /// <summary>
    /// 修改文章
    /// </summary>
    public class EditBlog
    {
        /// <summary>
        /// Blog.Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 關聯動物Id
        /// </summary>
        public int? AnimalId { get; set; }

        /// <summary>
        /// Class.Id
        /// </summary>
        public short ClassId { get; set; }
    }
}