using System.Collections.Generic;
using System.Linq;
using PetAdopt.Controllers;
using PetAdopt.DTO.Shelters;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetSheltersList(int page, int take, string query, bool isLike)
        {
            var shelterslist = SheltersLogic.GetSheltersList(page, take, query, isLike);

            return Json(shelterslist);
        }

        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = SheltersLogic.GetMessageList(id, page, take);

            return Json(result);
        }

        public ActionResult Delete(int id)
        {
            var result = SheltersLogic.DeleteShelters(Server.MapPath("~/Content/uploads"), id, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = SheltersLogic.DeleteMessage(id, messageId, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = SheltersLogic.GetShelters(id);

            return Json(result);
        }

        public ActionResult AddShelters(CreateShelters data)
        {
            var result = SheltersLogic.AddShelters(data);

            return Json(result);
        }

        public ActionResult EditShelters(int id, CreateShelters data)
        {
            var result = SheltersLogic.EditShelters(id, data, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult GetSheltersSuggestion(string name)
        {
            var result = SheltersLogic.GetSheltersSuggestion(name);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}