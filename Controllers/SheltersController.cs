using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Controllers
{
    [Authorize]
    public class SheltersController : _BaseController
    {
        #region _sheltersLogic
        /// <summary>
        /// SheltersLogic
        /// </summary>
        SheltersLogic SheltersLogic
        {
            get
            {
                if (_sheltersLogic == null)
                    _sheltersLogic = new SheltersLogic(GetOperation());
                return _sheltersLogic;
            }
        }
        SheltersLogic _sheltersLogic;
        #endregion //_sheltersLogic

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
        public ActionResult GetSheltersList(int page, int take, string query, bool isLike, bool memberOnly = false)
        {
            if (memberOnly)
            {
                var shelterslist = SheltersLogic.GetSheltersList(page, take, query, isLike, LoginInfo.Id);
                return Json(shelterslist);
            }
            else
            {
                var shelterslist = SheltersLogic.GetSheltersList(page, take, query, isLike);
                return Json(shelterslist);
            }
        }

        [AllowAnonymous]
        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = SheltersLogic.GetMessageList(id, page, take);

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
            var result = SheltersLogic.GetShelters(id);

            return Json(result);
        }

        public ActionResult AddMessage(int id, string message)
        {
            var result = SheltersLogic.AddMessage(id, message);

            return Json(result);
        }

        /*public ActionResult Delete(int id)
        {
            var result = _sheltersLogic.DeleteShelters(Server.MapPath("~/Content/uploads"), id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = _sheltersLogic.DeleteMessage(id, messageId);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = _sheltersLogic.GetShelters(id);

            return Json(result);
        }

        public ActionResult AddShelters(CreateShelters data)
        {
            var result = _sheltersLogic.AddShelters(data);

            return Json(result);
        }

        public ActionResult EditShelters(int id, CreateShelters data)
        {
            var result = _sheltersLogic.EditShelters(id, data);

            return Json(result);
        }*/
    }
}