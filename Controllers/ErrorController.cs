using System.Web.Mvc;

namespace PetAdopt.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult PageNotFind()
        {
            return View();
        }

        public ActionResult InternalError()
        {
            return View();
        }
    }
}