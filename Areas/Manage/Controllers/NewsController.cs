using System;
using PetAdopt.Controllers;
using PetAdopt.DTO;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class NewsController : _BaseController
    {
        #region _newsLogic
        /// <summary>
        /// NewsLogic
        /// </summary>
        NewsLogic _newsLogic
        {
            get
            {
                if (@newsLogic == null)
                    @newsLogic = new NewsLogic(GetOperation());
                return @newsLogic;
            }
        }
        NewsLogic @newsLogic;
        #endregion //_newsLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetNewsList()
        {
            var newslist = _newsLogic.GetNewsList();

            return Json(newslist);
        }

        public ActionResult Delete(int id)
        {
            var result = _newsLogic.DeleteNews(Server.MapPath("~/Content/uploads"), id);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = _newsLogic.GetNews(id);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult AddNews(CreateNews data)
        {
            var result = _newsLogic.AddNews(data);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult EditNews(int id, CreateNews data)
        {
            var result = _newsLogic.EditNews(id, data);

            return Json(result);
        }
    }
}