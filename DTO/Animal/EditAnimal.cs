using System;

namespace PetAdopt.DTO.Animal
{
    /// <summary>
    /// 修改動物
    /// </summary>
    public class EditAnimal
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
        /// 年齡
        /// </summary>
        public short? Age { get; set; }

        /// <summary>
        /// 介紹
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 在哪個收容所
        /// </summary>
        public string Shelters { get; set; }

        /// <summary>
        /// 分類Id
        /// </summary>
        public short ClassId { get; set; }

        /// <summary>
        /// 狀態Id
        /// </summary>
        public short StatusId { get; set; }

        /// <summary>
        /// 地區Id
        /// </summary>
        public short? AreaId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 送養時間
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 安樂死時間
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}