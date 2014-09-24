using PetAdopt.Controllers;
using PetAdopt.DTO.Animal;
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

        public ActionResult GetAnimalList(int page)
        {
            var animallist = _animalLogic.GetAnimalList(page);

            return Json(animallist);
        }

        public ActionResult Delete(int id)
        {
            var result = _animalLogic.DeleteAnimal(id);

            return Json(result);
        }

        //public ActionResult Edit()
        //{
        //    return View();
        //}

        //public ActionResult EditInit(int id)
        //{
        //    var result = _animalLogic.GetAnimal(id);

        //    return Json(result);
        //}

        public ActionResult AddAnimal(CreateAnimal data)
        {
            var result = _animalLogic.AddAnimal(data);

            return Json(result);
        }
    }
}