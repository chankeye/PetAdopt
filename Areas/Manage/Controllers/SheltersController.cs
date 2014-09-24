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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetSheltersList(int page)
        {
            var shelterslist = _sheltersLogic.GetSheltersList(page);

            return Json(shelterslist);
        }

        public ActionResult Delete(int id)
        {
            var result = _sheltersLogic.DeleteShelters(Server.MapPath("~/Content/uploads"), id);

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
        }
    }
}