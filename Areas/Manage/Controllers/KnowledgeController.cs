using PetAdopt.Controllers;
using PetAdopt.DTO;
using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Areas.Manage.Controllers
{
    [Authorize]
    public class KnowledgeController : _BaseController
    {
        #region _knowledgeLogic
        /// <summary>
        /// HelpLogic
        /// </summary>
        KnowledgeLogic _knowledgeLogic
        {
            get
            {
                if (@knowledgeLogic == null)
                    @knowledgeLogic = new KnowledgeLogic(GetOperation());
                return @knowledgeLogic;
            }
        }
        KnowledgeLogic @knowledgeLogic;
        #endregion //_knowledgeLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetKnowledgeList(int page)
        {
            var newslist = _knowledgeLogic.GetKnowledgeList(page);

            return Json(newslist);
        }

        public ActionResult Delete(int id)
        {
            var result = _knowledgeLogic.DeleteKnowledge(id);

            return Json(result);
        }
    }
}