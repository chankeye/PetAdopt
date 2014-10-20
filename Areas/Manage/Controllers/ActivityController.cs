using PetAdopt.Controllers;
using PetAdopt.DTO.Activity;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class ActivityController : _BaseController
    {
        #region _activityLogic
        /// <summary>
        /// ActivityLogic
        /// </summary>
        ActivityLogic ActivityLogic
        {
            get
            {
                if (_activityLogic == null)
                    _activityLogic = new ActivityLogic(GetOperation());
                return _activityLogic;
            }
        }
        ActivityLogic _activityLogic;
        #endregion //_activityLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetActivities(int page, int take, string query, bool isLike)
        {
            var newslist = ActivityLogic.GetActivities(page, take, query, isLike);

            return Json(newslist);
        }

        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = ActivityLogic.GetMessageList(id, page, take);

            return Json(result);
        }

        public ActionResult Delete(int id)
        {
            var result = ActivityLogic.DeleteActivity(Server.MapPath("~/Content/uploads"), id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = ActivityLogic.DeleteMessage(id, messageId);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = ActivityLogic.GetActivity(id);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult AddActivity(CreateActivity data)
        {
            var result = ActivityLogic.AddActivity(data);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult EditActivity(int id, CreateActivity data)
        {
            var result = ActivityLogic.EditActivity(id, data);

            return Json(result);
        }
    }
}