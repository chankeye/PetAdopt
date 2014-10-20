using PetAdopt.Controllers;
using PetAdopt.Logic;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class AccountController : _BaseController
    {
        /// <summary>
        /// UserLogic
        /// </summary>
        UserLogic UserLogic
        {
            get
            {
                if (_userLogic == null)
                    _userLogic = new UserLogic(GetOperation());
                return _userLogic;
            }
        }
        UserLogic _userLogic;

        /// <summary>
        /// 登入畫面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // 如果已經登入了，就直接幫進入系統，不用看到登入畫面
            if (Request.IsAuthenticated)
                return redirect(returnUrl);

            return View();
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="password">密碼</param>
        /// <param name="remember">記住我?</param>
        /// <param name="returnUrl">重導路徑</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string account, string password, string returnUrl)
        {
            #region 驗證帳密
            var isValid = UserLogic.IsValid(account, password, true);
            if (isValid.IsSuccess == false)
            {
                ModelState.AddModelError("", isValid.ErrorMessage);
                return View();
            }
            #endregion //驗證帳密

            // 取得資訊
            var id = isValid.ReturnObject;
            var user = UserLogic.GetLoginInfo(id);

            #region 登入系統
            var userData = ParseToUserDataString(user);

            var ticket = new FormsAuthenticationTicket(
                1,                      // ticket version
                user.Account,           // authenticated username
                DateTime.Now,           // issueDate
                DateTime.MaxValue,      // expiryDate
                false,                  // true to persist across browser sessions
                userData                // can be used to store additional user data
            );
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authCookie.HttpOnly = true;
            if (ticket.IsPersistent)
                authCookie.Expires = ticket.Expiration;

            Response.Cookies.Add(authCookie);
            #endregion //登入系統

            return redirect(returnUrl);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Manage/Account/Login");
        }

        #region Private Methods
        /// <summary>
        /// 重新導向
        /// </summary>
        /// <param name="returnUrl">重導路徑</param>
        /// <returns></returns>
        ActionResult redirect(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return Redirect("/Manage");
        }
        #endregion //Private Methods

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult ChangePasswordSubmit(string oldPassword, string newPassword)
        {
            var result = UserLogic.ChangePassword(LoginInfo.Id, oldPassword, newPassword);

            if (result.IsSuccess == false)
                return JsonError(result.ErrorMessage);

            return null;
        }
    }
}