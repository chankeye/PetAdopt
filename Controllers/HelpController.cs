using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Controllers
{
    [Authorize]
    public class HelpController : _BaseController
    {
        #region _helpLogic
        /// <summary>
        /// HelpLogic
        /// </summary>
        HelpLogic HelpLogic
        {
            get
            {
                if (_helpLogic == null)
                    _helpLogic = new HelpLogic(GetOperation());
                return _helpLogic;
            }
        }
        HelpLogic _helpLogic;
        #endregion //_helpLogic

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetHelpList(int page, int take, string query, bool isLike, bool memberOnly = false)
        {
            if (memberOnly)
            {
                var helplist = HelpLogic.GetHelpList(page, take, query, isLike, LoginInfo.Id);
                return Json(helplist);
            }
            else
            {
                var helplist = HelpLogic.GetHelpList(page, take, query, isLike);
                return Json(helplist);
            }
        }

        [AllowAnonymous]
        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = HelpLogic.GetMessageList(id, page, take);

            return Json(result);
        }

        [AllowAnonymous]
        public ActionResult Detail()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DetailInit(int id)
        {
            var result = HelpLogic.GetHelp(id);

            return Json(result);
        }

        public ActionResult AddMessage(int id, string message)
        {
            var result = HelpLogic.AddMessage(id, message);

            return Json(result);
        }

        /*public ActionResult Delete(int id)
        {
            var result = _helpLogic.DeleteHelp(Server.MapPath("~/Content/uploads"), id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = _helpLogic.DeleteMessage(id, messageId);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = _helpLogic.GetHelp(id);

            return Json(result);
        }

        public ActionResult AddHelp(CreateHelp data)
        {
            var result = _helpLogic.AddHelp(data);

            return Json(result);
        }

        public ActionResult EditHelp(int id, CreateHelp data)
        {
            var result = _helpLogic.EditHelp(id, data);

            return Json(result);
        }*/
    }
}