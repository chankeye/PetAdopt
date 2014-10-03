﻿using PetAdopt.Controllers;
using PetAdopt.DTO.Help;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class HelpController : _BaseController
    {
        #region _helpLogic
        /// <summary>
        /// HelpLogic
        /// </summary>
        HelpLogic _helpLogic
        {
            get
            {
                if (@helpLogic == null)
                    @helpLogic = new HelpLogic(GetOperation());
                return @helpLogic;
            }
        }
        HelpLogic @helpLogic;
        #endregion //_helpLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetHelpList(int page, int take, string query, bool isLike)
        {
            var helplist = _helpLogic.GetHelpList(page, take, query, isLike);

            return Json(helplist);
        }

        public ActionResult GetMessageList(int id, int page, int take)
        {
            var result = _helpLogic.GetMessageList(id, page, take);

            return Json(result);
        }

        public ActionResult Delete(int id)
        {
            var result = _helpLogic.DeleteHelp(Server.MapPath("~/Content/uploads"), id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = _helpLogic.DeleteMessage(id, messageId);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = _helpLogic.GetHelp(id);

            return Json(result);
        }

        public ActionResult AddHelp(CreateHelp data)
        {
            var result = _helpLogic.AddHelp(data);

            return Json(result);
        }

        public ActionResult EditHelp(int id, CreateHelp data)
        {
            var result = _helpLogic.EditHelp(id, data);

            return Json(result);
        }
    }
}