using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Controllers
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
        public ActionResult GetActivities(int page, int take, string query, bool isLike, bool memberOnly = false)
        {
            if (memberOnly)
            {
                var newslist = ActivityLogic.GetActivities(page, take, query, isLike, LoginInfo.Id);
                return Json(newslist);
            }
            else
            {
                var newslist = ActivityLogic.GetActivities(page, take, query, isLike);
                return Json(newslist);
            }

        }

        [AllowAnonymous]
        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = ActivityLogic.GetMessageList(id, page, take);

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
            var result = ActivityLogic.GetActivity(id);

            return Json(result);
        }

        /*public ActionResult Delete(int id)
        {
            var result = _activityLogic.DeleteActivity(Server.MapPath("~/Content/uploads"), id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = _activityLogic.DeleteMessage(id, messageId);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = _activityLogic.GetActivity(id);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult AddActivity(CreateActivity data)
        {
            var result = _activityLogic.AddActivity(data);

            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult EditActivity(int id, CreateActivity data)
        {
            var result = _activityLogic.EditActivity(id, data);

            return Json(result);
        }*/
    }
}