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
    public class SystemController : _BaseController
    {
        #region _userLogic
        /// <summary>
        /// UserLogic
        /// </summary>
        SystemLogic _systemLogic
        {
            get
            {
                if (@systemLogic == null)
                    @systemLogic = new SystemLogic(GetOperation());
                return @systemLogic;
            }
        }
        SystemLogic @systemLogic;
        #endregion //_userLogic

        // GET: Manage/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAreaList()
        {
            var areas = _systemLogic.GetAreaList();

            return Json(areas);
        }

        public ActionResult DeleteArea(int id)
        {
            var result = _systemLogic.DeleteArea(id);

            return Json(result);
        }

        public ActionResult AddArea(string word)
        {
            var result = _systemLogic.AddArea(word);

            return Json(result);
        }
    }
}