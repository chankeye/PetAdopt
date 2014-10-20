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
        #endregion //_userLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserList(int page, int take, string query, bool isLike)
        {
            var newslist = UserLogic.GetUserList(page, take, query, isLike);

            return Json(newslist);
        }

        public ActionResult Delete(int id)
        {
            var result = UserLogic.DeleteUser(id);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = UserLogic.GetUser(id);

            return Json(result);
        }

        public ActionResult AddUser(CreateUser data)
        {
            var result = UserLogic.AddUser(data);

            return Json(result);
        }

        public ActionResult EditUser(int id, CreateUser data)
        {
            var result = UserLogic.EditUser(id, data);

            return Json(result);
        }
    }
}