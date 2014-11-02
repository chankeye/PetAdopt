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
        KnowledgeLogic KnowledgeLogic
        {
            get
            {
                if (_knowledgeLogic == null)
                    _knowledgeLogic = new KnowledgeLogic(GetOperation());
                return _knowledgeLogic;
            }
        }
        KnowledgeLogic _knowledgeLogic;
        #endregion //_knowledgeLogic

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetKnowledgeList(int page, int take, string query, bool isLike)
        {
            var newslist = KnowledgeLogic.GetKnowledgeList(page, take, query, isLike);

            return Json(newslist);
        }

        public ActionResult GetMessageList(int id, int page, int take)
        {
            var asklist = KnowledgeLogic.GetMessageList(id, page, take);

            return Json(asklist);
        }

        public ActionResult Delete(int id)
        {
            var result = KnowledgeLogic.DeleteKnowledge(id, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = KnowledgeLogic.DeleteMessage(id, messageId, LoginInfo.Id);

            return Json(result);
        }

        public ActionResult AddKnowledge(CreateKnowledge data)
        {
            var result = KnowledgeLogic.AddKnowledge(data);

            return Json(result);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditInit(int id)
        {
            var result = KnowledgeLogic.GetKnowledge(id);

            return Json(result);
        }

        public ActionResult EditKnowledge(int id, CreateKnowledge data)
        {
            var result = KnowledgeLogic.EditKnowledge(id, data, LoginInfo.Id);

            return Json(result);
        }
    }
}