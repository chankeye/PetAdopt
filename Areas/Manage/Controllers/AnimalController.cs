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
        AnimalLogic AnimalLogic
        {
            get
            {
                if (_animalLogic == null)
                    _animalLogic = new AnimalLogic(GetOperation());
                return _animalLogic;
            }
        }
        AnimalLogic _animalLogic;
        #endregion //_animalLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAnimalList(int page, int take, string query, bool isLike)
        {
            var animallist = AnimalLogic.GetAnimalList(page, take, query, isLike);

            return Json(animallist);
        }

        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = AnimalLogic.GetMessageList(id, page, take);

            return Json(result);
        }

        public ActionResult Delete(int id)
        {
            var result = AnimalLogic.DeleteAnimal(Server.MapPath("~/Content/uploads"), id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = AnimalLogic.DeleteMessage(id, messageId);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = AnimalLogic.GetAnimal(id);

            return Json(result);
        }

        public ActionResult AddAnimal(CreateAnimal data)
        {
            var result = AnimalLogic.AddAnimal(data);

            return Json(result);
        }

        public ActionResult EditAnimal(int id, CreateAnimal data)
        {
            var result = AnimalLogic.EditAnimal(id, data);

            return Json(result);
        }

        public ActionResult GetAnimalSuggestion(string title)
        {
            var result = AnimalLogic.GetAnimalSuggestion(title);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}