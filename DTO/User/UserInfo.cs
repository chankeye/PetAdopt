using System.Collections.Generic;

namespace PetAdopt.DTO
{
    /// <summary>
    /// 使用者資訊
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }
    }

    public class UserItem
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public bool  IsDisable { get; set; }
    }

    public class UserList
    {
        public List<UserItem> List { get; set; }

        public int Count { get; set; }
    }
}