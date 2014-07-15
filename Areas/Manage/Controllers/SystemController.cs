﻿using System;
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

        public ActionResult GetStatusList()
        {
            var statuses = _systemLogic.GetStatusList();

            return Json(statuses);
        }

        public ActionResult DeleteStatus(int id)
        {
            var result = _systemLogic.DeleteStatus(id);

            return Json(result);
        }

        public ActionResult AddStatus(string word)
        {
            var result = _systemLogic.AddStatus(word);

            return Json(result);
        }

        public ActionResult GetClassList()
        {
            var classes = _systemLogic.GetClassList();

            return Json(classes);
        }

        public ActionResult DeleteClass(int id)
        {
            var result = _systemLogic.DeleteClass(id);

            return Json(result);
        }

        public ActionResult AddClass(string word)
        {
            var result = _systemLogic.AddClass(word);

            return Json(result);
        }
    }
}