using PetAdopt.Controllers;
using PetAdopt.DTO.User;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class UserController : _BaseController
    {
        #region _newsLogic
        /// <summary>
        /// _userLogic
        /// </summary>
        UserLogic _userLogic
        {
            get
            {
                if (@userLogic == null)
                    @userLogic = new UserLogic(GetOperation());
                return @userLogic;
            }
        }
        UserLogic @userLogic;
        #endregion //_userLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserList(int page, int take, string query, bool isLike)
        {
            var newslist = _userLogic.GetUserList(page, take, query, isLike);

            return Json(newslist);
        }

        public ActionResult Delete(int id)
        {
            var result = _userLogic.DeleteUser(id);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = _userLogic.GetUser(id);

            return Json(result);
        }

        public ActionResult AddUser(CreateUser data)
        {
            var result = _userLogic.AddUser(data);

            return Json(result);
        }

        public ActionResult EditUser(int id, CreateUser data)
        {
            var result = _userLogic.EditUser(id, data);

            return Json(result);
        }
    }
}