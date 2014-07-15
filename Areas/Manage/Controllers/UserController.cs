using PetAdopt.Controllers;
using PetAdopt.DTO;
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

        public ActionResult GetUserList()
        {
            var newslist = _userLogic.GetUserList();

            return Json(newslist);
        }

        public ActionResult DeleteUser(int id)
        {
            var result = _userLogic.DeleteUser(id);

            return Json(result);
        }

        public ActionResult AddUser(CreateUser data)
        {
            var result = _userLogic.AddUser(data);

            return Json(result);
        }
    }
}