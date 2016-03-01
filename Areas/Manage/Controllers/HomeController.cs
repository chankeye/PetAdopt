using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetAdopt.Controllers;
using PetAdopt.Logic;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class HomeController : _BaseController
    {
        #region _helpLogic
        /// <summary>
        /// SystemLogic
        /// </summary>
        SystemLogic SystemLogic
        {
            get
            {
                if (_systemLogic == null)
                    _systemLogic = new SystemLogic(GetOperation());
                return _systemLogic;
            }
        }
        SystemLogic _systemLogic;
        #endregion // _systemLogic

        // GET: Manage/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetHomeInfo()
        {
            var homeInfo = SystemLogic.GetSiteInformation();

            return Json(homeInfo);
        }
    }
}