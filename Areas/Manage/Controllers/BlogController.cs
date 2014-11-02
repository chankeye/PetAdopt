using PetAdopt.Controllers;
using PetAdopt.DTO.Blog;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class BlogController : _BaseController
    {
        #region _blogLogic
        /// <summary>
        /// BlogLogic
        /// </summary>
        BlogLogic BlogLogic
        {
            get
            {
                if (_blogLogic == null)
                    _blogLogic = new BlogLogic(GetOperation());
                return _blogLogic;
            }
        }
        BlogLogic _blogLogic;
        #endregion //_blogLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBlogList(int page, int take, string query, bool isLike)
        {
            var bloglist = BlogLogic.GetBlogList(page, take, query, isLike);

            return Json(bloglist);
        }

        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = BlogLogic.GetMessageList(id, page, take);

            return Json(result);
        }

        public ActionResult Delete(int id)
        {
            var result = BlogLogic.DeleteBlog(id, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = BlogLogic.DeleteMessage(id, messageId, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = BlogLogic.GetBlog(id);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult AddBlog(CreateBlog data)
        {
            var result = BlogLogic.AddBlog(data);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult EditBlog(EditBlog data)
        {
            var result = BlogLogic.EditBlog(data, LoginInfo.Id);

            return Json(result);
        }
    }
}