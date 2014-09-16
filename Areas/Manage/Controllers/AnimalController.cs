using PetAdopt.Controllers;
using PetAdopt.DTO;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class AnimalController : _BaseController
    {
        #region _animalLogic
        /// <summary>
        /// AnimalLogic
        /// </summary>
        AnimalLogic _animalLogic
        {
            get
            {
                if (@animalLogic == null)
                    @animalLogic = new AnimalLogic(GetOperation());
                return @animalLogic;
            }
        }
        AnimalLogic @animalLogic;
        #endregion //_animalLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAnimalList()
        {
            var animallist = _animalLogic.GetAnimalList();

            return Json(animallist);
        }

        public ActionResult Delete(int id)
        {
            var result = _animalLogic.DeleteAnimal(id);

            return Json(result);
        }
    }
}