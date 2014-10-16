using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Controllers
{
    [Authorize]
    public class BlogController : _BaseController
    {
        #region _blogLogic
        /// <summary>
        /// BlogLogic
        /// </summary>
        BlogLogic _blogLogic
        {
            get
            {
                if (@blogLogic == null)
                    @blogLogic = new BlogLogic(GetOperation());
                return @blogLogic;
            }
        }
        BlogLogic @blogLogic;
        #endregion //_blogLogic

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
        public ActionResult GetBlogList(int page, int take, string query, bool isLike)
        {
            var bloglist = _blogLogic.GetBlogList(page, take, query, isLike);

            return Json(bloglist);
        }

        [AllowAnonymous]
        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = _blogLogic.GetMessageList(id, page, take);

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
            var result = _blogLogic.GetBlog(id);

            return Json(result);
        }

        /*public ActionResult Delete(int id)
        {
            var result = _blogLogic.DeleteBlog(id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = _blogLogic.DeleteMessage(id, messageId);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = _blogLogic.GetBlog(id);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult AddBlog(CreateBlog data)
        {
            var result = _blogLogic.AddBlog(data);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult EditBlog(int id, CreateBlog data)
        {
            var result = _blogLogic.EditBlog(id, data);

            return Json(result);
        }*/
    }
}