using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Controllers
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

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetAskList(int page, int take, string query, bool isLike, int classId, bool memberOnly = false)
        {
            if (memberOnly)
            {
                var asklist = AskLogic.GetAskList(page, take, query, isLike, classId, LoginInfo.Id);
                return Json(asklist);
            }
            else
            {
                var asklist = AskLogic.GetAskList(page, take, query, isLike, classId);
                return Json(asklist);
            }
        }

        [AllowAnonymous]
        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = AskLogic.GetMessageList(id, page, take);

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
            var result = AskLogic.GetAsk(id);

            return Json(result);
        }

        [AllowAnonymous]
        public ActionResult AddMessage(int id, string message)
        {
            var result = AskLogic.AddMessage(id, message);

            return Json(result);
        }

        /*public ActionResult Delete(int id)
        {
            var result = _askLogic.DeleteAsk(id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = _askLogic.DeleteMessage(id, messageId);

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
        }*/
    }
}