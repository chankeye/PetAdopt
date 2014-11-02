using PetAdopt.Controllers;
using PetAdopt.DTO.News;
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetNewsList(int page, int take, string query, bool isLike)
        {
            var newslist = NewsLogic.GetNewsList(page, take, query, isLike);

            return Json(newslist);
        }

        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = NewsLogic.GetMessageList(id, page, take);

            return Json(result);
        }

        public ActionResult Delete(int id)
        {
            var result = NewsLogic.DeleteNews(Server.MapPath("~/Content/uploads"), id, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = NewsLogic.DeleteMessage(id, messageId, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = NewsLogic.GetNews(id);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult AddNews(CreateNews data)
        {
            var result = NewsLogic.AddNews(data);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult EditNews(int id, CreateNews data)
        {
            var result = NewsLogic.EditNews(id, data, LoginInfo.Id);

            return Json(result);
        }
    }
}