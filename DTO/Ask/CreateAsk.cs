namespace PetAdopt.DTO.Ask
{
    /// <summary>
    /// 新增問與答
    /// </summary>
    public class CreateAsk
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
        /// 分類Id
        /// </summary>
        public short ClassId { get; set; }
    }
}