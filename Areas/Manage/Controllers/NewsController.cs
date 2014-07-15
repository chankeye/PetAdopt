using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetAdopt.Controllers;
using PetAdopt.DTO;
using PetAdopt.Logic;

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

        public ActionResult DeleteNews(int id)
        {
            var result = _newsLogic.DeleteNews(id);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult AddNews(CreateNews data)
        {
            var result = _newsLogic.AddNews(data);

            return Json(result);
        }
    }
}