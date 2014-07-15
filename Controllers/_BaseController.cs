using Newtonsoft.Json;
using PetAdopt.DTO;
using System;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Security;

namespace PetAdopt.Controllers
{
    public abstract class _BaseController : Controller
    {
        /// <summary>
        /// 取得client的IP
        /// </summary>
        /// <returns></returns>
        /*protected string GetClientIP()
        {
            if (Request.ServerVariables["HTTP_VIA"] == null)
            {
                return Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else
            {
                return Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
        }*/

        /// <summary>
        /// 將物件轉換為可存在cookie中UserData的字串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string ParseToUserDataString<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 將存在cookie中的UserData讀出成指定的型別的物件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T ReadUserData<T>()
        {
            if (!Request.IsAuthenticated)
                throw new UnauthorizedAccessException("尚未登入");

            // 讀cookie的UserData
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var userData = FormsAuthentication.Decrypt(authCookie.Value).UserData;

            return JsonConvert.DeserializeObject<T>(userData);
        }

        /// <summary>
        /// 從Cache取得值，沒有就撈並且寫入Cache
        /// </summary>
        /// <typeparam name="T">回傳型別</typeparam>
        /// <param name="key">Cache的key</param>
        /// <param name="getValue">如果cache沒有的話，取得值的方法</param>
        /// <param name="absoluteExpiration">如果cache沒有的話，設定cache的失效時間</param>
        /// <returns></returns>
        protected T GetFromCache<T>(string key, Func<T> getValue, DateTime absoluteExpiration)
        {
            // 嘗試取得值
            var result = HttpRuntime.Cache.Get(key);

            // 取不到就撈，並且設定cache
            if (result == null)
            {
                result = getValue();
                HttpRuntime.Cache.Insert(
                    key,
                    result,
                    null,
                    absoluteExpiration,
                    Cache.NoSlidingExpiration
                    );
            }

            if (!(result is T))
                throw new Exception(string.Format("Cache型別錯誤!請檢查是否有其他地方使用\"{0}\"這個key", key));

            return (T)result;
        }

        /// <summary>
        /// 登入資訊
        /// </summary>
        public LoginInfo LoginInfo
        {
            get
            {
                if (_loginInfo == null)
                    _loginInfo = ReadUserData<LoginInfo>();
                return _loginInfo;
            }
        }
        private LoginInfo _loginInfo;

        /// <summary>
        /// 取得操作者資訊
        /// </summary>
        /// <returns></returns>
        protected Operation GetOperation()
        {
            var result = new Operation();

            if (Request.IsAuthenticated == false)
            {
                result.Display = "_guest";
            }
            else
            {
                result.Display = User.Identity.Name;
                result.UserId = LoginInfo.Id;
            }

            return result;
        }

        /// <summary>
        /// JsonResult - 發生錯誤
        /// </summary>
        /// <param name="errorMessage">錯誤訊息</param>
        /// <returns></returns>
        protected ActionResult JsonError(string errorMessage)
        {
            return Json(new
            {
                isSuccess = false,
                errorMessage = errorMessage
            });
        }

        /// <summary>
        /// 目前登入的使用者有沒有某種權限
        /// </summary>
        /// <returns></returns>
        protected bool HasAuthority()
        {
            if (LoginInfo.IsAdmin)
                return true;
            else
                return false;
        }
    }
}