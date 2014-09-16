using PetAdopt.Controllers;
using PetAdopt.DTO;
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
        ActivityLogic _activityLogic
        {
            get
            {
                if (@activityLogic == null)
                    @activityLogic = new ActivityLogic(GetOperation());
                return @activityLogic;
            }
        }
        ActivityLogic @activityLogic;
        #endregion //_activityLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetActivities()
        {
            var newslist = _activityLogic.GetActivities();

            return Json(newslist);
        }

        public ActionResult Delete(int id)
        {
            var result = _activityLogic.DeleteActivity(id);

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
        }
    }
}