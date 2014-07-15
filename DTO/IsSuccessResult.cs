namespace PetAdopt.DTO
{
    public class IsSuccessResult
    {
        #region Constructors
        /// <summary>
        /// 成功
        /// </summary>
        public IsSuccessResult()
        {
            IsSuccess = true;
        }

        /// <summary>
        /// 失敗
        /// </summary>
        /// <param name="errorMessage">錯誤訊息</param>
        public IsSuccessResult(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
        }
        #endregion //Constructors

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// 是否成功
    /// </summary>
    /// <typeparam name="T">要回傳的物件型別</typeparam>
    public class IsSuccessResult<T> : IsSuccessResult
    {
        #region Constructors
        /// <summary>
        /// 成功
        /// </summary>
        public IsSuccessResult()
            : base() { }

        /// <summary>
        /// 失敗
        /// </summary>
        /// <param name="errorMessage">錯誤訊息</param>
        public IsSuccessResult(string errorMessage)
            : base(errorMessage)
        {
        }
        #endregion //Constructors

        /// <summary>
        /// 要回傳的物件
        /// </summary>
        public T ReturnObject { get; set; }
    }
}