using System.Text.RegularExpressions;
using PetAdopt.DTO;
using PetAdopt.Models;
using PetAdopt.Utilities;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PetAdopt.Logic
{
    public class UserLogic : _BaseLogic
    {
        /// <summary>
        /// 使用者Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public UserLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 驗證帳密
        /// </summary>
        /// <param name="account">廳主帳號</param>
        /// <param name="password">密碼</param>
        /// <param name="isAmdin">是否為管理者</param>
        /// <returns>Master.Id</returns>
        public IsSuccessResult<int> IsValid(string account, string password, bool isAdmin)
        {
            #region 檢查參數
            // account
            if (string.IsNullOrWhiteSpace(account))
                return new IsSuccessResult<int>("帳號不可為空");

            // password
            if (string.IsNullOrWhiteSpace(password))
                return new IsSuccessResult<int>("密碼不可為空");
            #endregion //檢查參數

            // 加密後的密碼
            var passwordEncrypt = Cryptography.EncryptBySHA1(password);

            // 嘗試取得
            var user = PetContext.Users
                .Where(r =>
                    r.Account == account &&
                    r.Password == passwordEncrypt
                )
                .Select(r => new
                {
                    r.Id,
                    r.IsDisable,
                    r.IsAdmin
                })
                .FirstOrDefault();

            // 帳密錯誤
            if (user == null)
                return new IsSuccessResult<int>("帳密錯誤");

            // 停用中
            if (user.IsDisable)
                return new IsSuccessResult<int>("帳號停用中");

            if (isAdmin)
            {
                if (user.IsAdmin == false)
                    return new IsSuccessResult<int>("帳密錯誤");
            }

            return new IsSuccessResult<int>() { ReturnObject = user.Id };
        }

        /// <summary>
        /// 取得登入要用到的資訊
        /// </summary>
        /// <param name="id">Master.Id</param>
        /// <returns>登入使用者的資訊</returns>
        public LoginInfo GetLoginInfo(int id)
        {
            var user = PetContext.Users
                .Where(r => r.Id == id)
                .Select(r => new LoginInfo
                {
                    Id = r.Id,
                    Account = r.Account,
                    IsAdmin = r.IsAdmin
                })
                .Single();

            return user;
        }

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="id">User.Id</param>
        /// <param name="oldPassword">舊密碼</param>
        /// <param name="newPassword">新密碼</param>
        /// <returns>IsSuccess</returns>
        public IsSuccessResult ChangePassword(int id, string oldPassword, string newPassword)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            #region 參數檢查
            // 原密碼不可為空白
            if (string.IsNullOrWhiteSpace(oldPassword))
                return new IsSuccessResult("原密碼不可為空白");
            oldPassword = oldPassword.Trim();
            log.Debug("oldPassword: {0}", oldPassword);

            // 新密碼不可為空白
            if (string.IsNullOrWhiteSpace(newPassword))
                return new IsSuccessResult("新密碼不可為空白");
            newPassword = newPassword.Trim();
            log.Debug("newPassword: {0}", newPassword);

            // 取得使用者帳號
            var user = PetContext.Users
                .SingleOrDefault(r =>
                    r.Id == id
                );
            // 使用者帳號不存在
            if (user == null)
                return new IsSuccessResult("此帳號不存在");

            // 檢查原密碼是否正確
            if (user.Password != Cryptography.EncryptBySHA1(oldPassword))
                return new IsSuccessResult("密碼輸入錯誤，無法更改");

            if (user.IsDisable == true)
                return new IsSuccessResult("此帳號停用中…無法變更密碼");

            // 新舊密碼如果一樣就不用變
            if (oldPassword == newPassword)
            {
                GetLogger().Warn("密碼相同，不須變更");
                return new IsSuccessResult();
            }
            #endregion // 參數檢查

            #region 異動資料庫
            var operationInfo = GetOperationInfo();

            user.Password = Cryptography.EncryptBySHA1(newPassword);

            PetContext.SaveChanges();
            #endregion //異動資料庫

            log.Info("使用者帳號\"{0}\"變更密碼成功", user.Account);

            return new IsSuccessResult(); ;
        }

        /// <summary>
        /// 取得使用者列表
        /// </summary>
        /// <returns></returns>
        public List<UserItem> GetUserList()
        {
            var log = GetLogger();
            log.Debug("GetUserList in");

            var users = PetContext
                .Users
                .Select(r => new UserItem
                {
                    Id = r.Id,
                    Account = r.Account,
                    IsDisable = r.IsDisable
                })
                .ToList();

            return users;
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteUser(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var user = PetContext
                .Users
                .Where(r => r.Id == id)
                .SingleOrDefault();
            if (user == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此使用者";
                return result;
            }

            try
            {
                user.IsDisable = true;
                PetContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);

                result.IsSuccess = false;
                result.ErrorMessage = "發生不明錯誤，請稍候再試";
                return result;
            }
        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<UserItem> AddUser(CreateUser data)
        {
            var log = GetLogger();
            log.Debug("account: {0}, display: {1}, mobile:{2}, email:{3}, isAdmin:{4}", data.Account, data.Display, data.Mobile,
                data.Email, data.IsAdmin);

            if (string.IsNullOrWhiteSpace(data.Account))
                return new IsSuccessResult<UserItem>("請輸入帳號");
            data.Account = data.Account.Trim();
            if (Regex.IsMatch(data.Account, Constant.PatternAccount) == false)
                return new IsSuccessResult<UserItem>("帳號必須以英文字母開頭，並且只接受英文、數字與底線");

            if (string.IsNullOrWhiteSpace(data.Display))
                return new IsSuccessResult<UserItem>("請輸入暱稱");
            data.Display = data.Display.Trim();

            if (string.IsNullOrWhiteSpace(data.Email))
                return new IsSuccessResult<UserItem>("請輸入Email");
            data.Email = data.Email.Trim();

            if (Regex.IsMatch(data.Email, Constant.PatternEmail) == false)
                return new IsSuccessResult<UserItem>("請輸入正確的Email");

            var isAny = PetContext.Users
                .Any(r => r.Account == data.Account);
            if (isAny)
                return new IsSuccessResult<UserItem>(string.Format("已經有 {0} 這個帳號了", data.Account));

            try
            {
                var user = PetContext.Users.Add(new User
                {
                    Account = data.Account,
                    Password = Cryptography.EncryptBySHA1(Constant.DefaultPassword),
                    Display = data.Display,
                    Mobile = data.Mobile,
                    Email = data.Email,
                    IsAdmin = data.IsAdmin,
                    Date = DateTime.Now,
                    IsDisable = false
                });
                PetContext.SaveChanges();

                return new IsSuccessResult<UserItem>
                {
                    ReturnObject = new UserItem
                    {
                        Id = user.Id,
                        Account = user.Account,
                        IsDisable = user.IsDisable
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult<UserItem>("發生不明錯誤，請稍候再試");
            }
        }
    }
}