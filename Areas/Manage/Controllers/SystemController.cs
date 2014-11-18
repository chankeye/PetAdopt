using PetAdopt.Controllers;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class SystemController : _BaseController
    {
        #region _systemLogic
        /// <summary>
        /// SystemLogic
        /// </summary>
        SystemLogic SystemLogic
        {
            get
            {
                if (_systemLogic == null)
                    _systemLogic = new SystemLogic(GetOperation());
                return _systemLogic;
            }
        }
        SystemLogic _systemLogic;
        #endregion //_systemLogic

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetAreaList()
        {
            var areas = SystemLogic.GetAreaList();

            return Json(areas);
        }

        public ActionResult DeleteArea(int id)
        {
            var result = SystemLogic.DeleteArea(id);

            return Json(result);
        }

        public ActionResult AddArea(string word)
        {
            var result = SystemLogic.AddArea(word);

            return Json(result);
        }

        [AllowAnonymous]
        public ActionResult GetStatusList()
        {
            var statuses = SystemLogic.GetStatusList();

            return Json(statuses);
        }

        public ActionResult DeleteStatus(int id)
        {
            var result = SystemLogic.DeleteStatus(id);

            return Json(result);
        }

        public ActionResult AddStatus(string word)
        {
            var result = SystemLogic.AddStatus(word);

            return Json(result);
        }

        [AllowAnonymous]
        public ActionResult GetClassList()
        {
            var classes = SystemLogic.GetClassList();

            return Json(classes);
        }

        public ActionResult DeleteClass(int id)
        {
            var result = SystemLogic.DeleteClass(id);

            return Json(result);
        }

        public ActionResult AddClass(string word)
        {
            var result = SystemLogic.AddClass(word);

            return Json(result);
        }

        public void DeletePhoto(string photo)
        {
            SystemLogic.DeletePhoto(Server.MapPath("~/Content/uploads"), photo);
        }
    }
}