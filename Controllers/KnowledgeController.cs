using PetAdopt.Logic;
using System.Web.Mvc;

namespace PetAdopt.Controllers
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

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetKnowledgeList(int page, int take, string query, bool isLike, bool memberOnly = false)
        {
            if (memberOnly)
            {
                var newslist = KnowledgeLogic.GetKnowledgeList(page, take, query, isLike, LoginInfo.Id);
                return Json(newslist);
            }
            else
            {
                var newslist = KnowledgeLogic.GetKnowledgeList(page, take, query, isLike);
                return Json(newslist);
            }
        }

        [AllowAnonymous]
        public ActionResult GetMessageList(int id, int page, int take)
        {
            var asklist = KnowledgeLogic.GetMessageList(id, page, take);

            return Json(asklist);
        }

        [AllowAnonymous]
        public ActionResult Detail()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DetailInit(int id)
        {
            var result = KnowledgeLogic.GetKnowledge(id);

            return Json(result);
        }

        /*public ActionResult Delete(int id)
        {
            var result = _knowledgeLogic.DeleteKnowledge(id);

            return Json(result);
        }

        public ActionResult DeleteMessage(int id, int messageId)
        {
            var result = _knowledgeLogic.DeleteMessage(id, messageId);

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
        }*/
    }
}