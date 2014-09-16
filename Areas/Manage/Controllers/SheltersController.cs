using PetAdopt.Controllers;
using PetAdopt.DTO;
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

        public ActionResult GetSheltersList()
        {
            var shelterslist = _sheltersLogic.GetSheltersList();

            return Json(shelterslist);
        }

        public ActionResult Delete(int id)
        {
            var result = _sheltersLogic.DeleteShelters(id);

            return Json(result);
        }
    }
}