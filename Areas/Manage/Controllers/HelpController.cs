using PetAdopt.Controllers;
using PetAdopt.DTO.Help;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetHelpList(int page, int take, string query, bool isLike)
        {
            var helplist = HelpLogic.GetHelpList(page, take, query, isLike);

            return Json(helplist);
        }

        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = HelpLogic.GetMessageList(id, page, take);

            return Json(result);
        }

        public ActionResult Delete(int id)
        {
            var result = HelpLogic.DeleteHelp(Server.MapPath("~/Content/uploads"), id, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = HelpLogic.DeleteMessage(id, messageId, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = HelpLogic.GetHelp(id);

            return Json(result);
        }

        public ActionResult AddHelp(CreateHelp data)
        {
            var result = HelpLogic.AddHelp(data);

            return Json(result);
        }

        public ActionResult EditHelp(int id, CreateHelp data)
        {
            var result = HelpLogic.EditHelp(id, data, LoginInfo.Id);

            return Json(result);
        }
    }
}