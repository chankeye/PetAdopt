﻿using PetAdopt.Controllers;
using PetAdopt.DTO.Animal;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Controllers
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

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetAnimalList(int page, int take, string query, bool isLike)
        {
            var animallist = _animalLogic.GetAnimalList(page, take, query, isLike);

            return Json(animallist);
        }

        [AllowAnonymous]
        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = _animalLogic.GetMessageList(id, page, take);

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
            var result = _animalLogic.GetAnimal(id);

            return Json(result);
        }

        /*public ActionResult Delete(int id)
        {
            var result = _animalLogic.DeleteAnimal(Server.MapPath("~/Content/uploads"), id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = _animalLogic.DeleteMessage(id, messageId);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = _animalLogic.GetAnimal(id);

            return Json(result);
        }

        public ActionResult AddAnimal(CreateAnimal data)
        {
            var result = _animalLogic.AddAnimal(data);

            return Json(result);
        }

        public ActionResult EditAnimal(int id, CreateAnimal data)
        {
            var result = _animalLogic.EditAnimal(id, data);

            return Json(result);
        }*/
    }
}