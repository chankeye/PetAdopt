using PetAdopt.Controllers;
using PetAdopt.DTO.Shelters;
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
        SheltersLogic _sheltersLogic
        {
            get
            {
                if (@sheltersLogic == null)
                    @sheltersLogic = new SheltersLogic(GetOperation());
                return @sheltersLogic;
            }
        }
        SheltersLogic @sheltersLogic;
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
        public ActionResult GetSheltersList(int page, int take, string query, bool isLike)
        {
            var shelterslist = _sheltersLogic.GetSheltersList(page, take, query, isLike);

            return Json(shelterslist);
        }

        [AllowAnonymous]
        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = _sheltersLogic.GetMessageList(id, page, take);

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
            var result = _sheltersLogic.GetShelters(id);

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