using PetAdopt.Controllers;
using PetAdopt.DTO;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class HelpController : _BaseController
    {
        #region _activityLogic
        /// <summary>
        /// HelpLogic
        /// </summary>
        HelpLogic _helpLogic
        {
            get
            {
                if (@helpLogic == null)
                    @helpLogic = new HelpLogic(GetOperation());
                return @helpLogic;
            }
        }
        HelpLogic @helpLogic;
        #endregion //_helpLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetHelpList()
        {
            var newslist = _helpLogic.GetHelpList();

            return Json(newslist);
        }

        public ActionResult Delete(int id)
        {
            var result = _helpLogic.DeleteHelp(id);

            return Json(result);
        }
    }
}