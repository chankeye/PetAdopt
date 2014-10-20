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
        AskLogic AskLogic
        {
            get
            {
                if (_askLogic == null)
                    _askLogic = new AskLogic(GetOperation());
                return _askLogic;
            }
        }
        AskLogic _askLogic;
        #endregion //_askLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAskList(int page, int take, string query, bool isLike)
        {
            var asklist = AskLogic.GetAskList(page, take, query, isLike);

            return Json(asklist);
        }

        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = AskLogic.GetMessageList(id, page, take);

            return Json(result);
        }

        public ActionResult Delete(int id)
        {
            var result = AskLogic.DeleteAsk(id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = AskLogic.DeleteMessage(id, messageId);

            return Json(result);
        }

        public ActionResult AddAsk(CreateAsk data)
        {
            var result = AskLogic.AddAsk(data);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = AskLogic.GetAsk(id);

            return Json(result);
        }

        public ActionResult EditAsk(int id, CreateAsk data)
        {
            var result = AskLogic.EditAsk(id, data);

            return Json(result);
        }
    }
}