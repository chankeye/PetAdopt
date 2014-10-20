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
        public ActionResult GetAskList(int page, int take, string query, bool isLike, bool memberOnly = false)
        {
            if (memberOnly)
            {
                var asklist = _askLogic.GetAskList(page, take, query, isLike, LoginInfo.Id);
                return Json(asklist);
            }
            else
            {
                var asklist = _askLogic.GetAskList(page, take, query, isLike);
                return Json(asklist);
            }
        }

        [AllowAnonymous]
        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = _askLogic.GetMessageList(id, page, take);

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
            var result = _askLogic.GetAsk(id);

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