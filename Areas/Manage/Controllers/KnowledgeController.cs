using PetAdopt.Controllers;
using PetAdopt.DTO.Knowledge;
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

        public ActionResult GetKnowledgeList(int page, int take, string query, bool isLike)
        {
            var newslist = _knowledgeLogic.GetKnowledgeList(page, take, query, isLike);

            return Json(newslist);
        }

        public ActionResult Delete(int id)
        {
            var result = _knowledgeLogic.DeleteKnowledge(id);

            return Json(result);
        }

        public ActionResult AddKnowledge(CreateKnowledge data)
        {
            var result = _knowledgeLogic.AddKnowledge(data);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = _knowledgeLogic.GetKnowledge(id);

            return Json(result);
        }

        public ActionResult EditKnowledge(int id, CreateKnowledge data)
        {
            var result = _knowledgeLogic.EditKnowledge(id, data);

            return Json(result);
        }
    }
}