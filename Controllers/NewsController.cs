using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Controllers
{
    [Authorize]
    public class NewsController : _BaseController
    {
        #region _newsLogic
        /// <summary>
        /// NewsLogic
        /// </summary>
        NewsLogic NewsLogic
        {
            get
            {
                if (_newsLogic == null)
                    _newsLogic = new NewsLogic(GetOperation());
                return _newsLogic;
            }
        }
        NewsLogic _newsLogic;
        #endregion //_newsLogic

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
        public ActionResult GetNewsList(int page, int take, string query, bool isLike, bool memberOnly = false)
        {
            if (memberOnly)
            {
                var newslist = NewsLogic.GetNewsList(page, take, query, isLike, LoginInfo.Id);
                return Json(newslist);
            }
            else
            {
                var newslist = NewsLogic.GetNewsList(page, take, query, isLike);
                return Json(newslist);
            }
        }

        [AllowAnonymous]
        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = NewsLogic.GetMessageList(id, page, take);

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
            var result = NewsLogic.GetNews(id);

            return Json(result);
        }

        /*public ActionResult Delete(int id)
        {
            var result = _newsLogic.DeleteNews(Server.MapPath("~/Content/uploads"), id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = _newsLogic.DeleteMessage(id, messageId);

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
        }*/
    }
}