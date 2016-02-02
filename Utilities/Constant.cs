namespace PetAdopt.Utilities
{
    public class Constant
    {
        /// <summary>
        /// 預設的密碼
        /// </summary>
        public const string DefaultPassword = "123456";

        /// <summary>
        /// 帳號必須以英文字母開頭，並且只接受英文、數字與底線
        /// </summary>
        public const string PatternAccount = @"^[A-Za-z]{1}\w{1,}$";

        public const string PatternEmail = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";

        public const string PatternUrl = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";

        public static string[] InitStatuses = new string[] { "開放認養", "已認養", "已安樂死", "其他" };

        public static string[] InitClasses = new string[] { "狗", "貓", "兔", "鼠", "鳥", "其他" };
    }
}