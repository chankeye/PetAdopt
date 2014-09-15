using PetAdopt.Controllers;
using PetAdopt.DTO;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class AskController : _BaseController
    {
        #region _askLogic
        /// <summary>
        /// HelpLogic
        /// </summary>
        AskLogic _askLogic
        {
            get
            {
                if (@askLogic == null)
                    @askLogic = new AskLogic(GetOperation());
                return @askLogic;
            }
        }
        AskLogic @askLogic;
        #endregion //_askLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAskList()
        {
            var newslist = _askLogic.GetAskList();

            return Json(newslist);
        }

        public ActionResult Delete(int id)
        {
            var result = _askLogic.DeleteAsk(id);

            return Json(result);
        }
    }
}