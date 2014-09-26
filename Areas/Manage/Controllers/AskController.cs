using PetAdopt.Controllers;
using PetAdopt.DTO.Ask;
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

        public ActionResult GetAskList(int page, int take, string query, bool isLike)
        {
            var newslist = _askLogic.GetAskList(page, take, query, isLike);

            return Json(newslist);
        }

        public ActionResult Delete(int id)
        {
            var result = _askLogic.DeleteAsk(id);

            return Json(result);
        }

        public ActionResult AddAsk(CreateAsk data)
        {
            var result = _askLogic.AddAsk(data);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = _askLogic.GetAsk(id);

            return Json(result);
        }

        public ActionResult EditAsk(int id, CreateAsk data)
        {
            var result = _askLogic.EditAsk(id, data);

            return Json(result);
        }
    }
}