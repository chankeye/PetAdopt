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

        public const string patternEmail = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
    }
}