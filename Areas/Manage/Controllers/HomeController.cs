using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetAdopt.Controllers;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class HomeController : _BaseController
    {
        // GET: Manage/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}