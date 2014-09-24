﻿using PetAdopt.Controllers;
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBlogList(int page)
        {
            var bloglist = _blogLogic.GetBlogList(page);

            return Json(bloglist);
        }

        public ActionResult Delete(int id)
        {
            var result = _blogLogic.DeleteBlog(id);

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
        }
    }
}