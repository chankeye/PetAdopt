using PetAdopt.DTO;
using PetAdopt.DTO.Knowledge;
using PetAdopt.Models;
using System;
using System.Linq;

namespace PetAdopt.Logic
{
    public class KnowledgeLogic : _BaseLogic
    {
        /// <summary>
        /// 相關知識Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public KnowledgeLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得知識列表
        /// </summary>
        /// <returns></returns>
        public KnowledgeList GetKnowledgeList(int page)
        {
            var log = GetLogger();
            log.Debug("page:{0}", page);

            var list = PetContext.Knowledges
                .Select(r => new KnowledgeItem
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();

            var count = PetContext.Knowledges.Count();
            var result = new KnowledgeList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 刪除知識
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteKnowledge(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var knowledge = PetContext.Knowledges.SingleOrDefault(r => r.Id == id);
            if (knowledge == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此知識文章";
                return result;
            }

            try
            {
                PetContext.Knowledges.Remove(knowledge);
                PetContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);

                result.IsSuccess = false;
                result.ErrorMessage = "發生不明錯誤，請稍候再試";
                return result;
            }
        }

        /// <summary>
        /// 取得知識
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<CreateKnowledge> GetKnowledge(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var knowledge = PetContext.Knowledges.SingleOrDefault(r => r.Id == id);
            if (knowledge == null)
                return new IsSuccessResult<CreateKnowledge>("找不到此知識");

            return new IsSuccessResult<CreateKnowledge>
            {
                ReturnObject = new CreateKnowledge
                {
                    Title = knowledge.Title,
                    Message = knowledge.Message,
                    ClassId = knowledge.ClassId,
                }
            };
        }

        /// <summary>
        /// 新增知識
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<KnowledgeItem> AddKnowledge(CreateKnowledge data)
        {
            var log = GetLogger();
            log.Debug("title: {0}, message:{1}, classId:{2}", data.Title, data.Message, data.ClassId);

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<KnowledgeItem>("標題請勿傳入空值");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult<KnowledgeItem>("內容請勿傳入空值");
            data.Message = data.Message.Trim();

            var isAny = PetContext.Knowledges.Any(r => r.Title == data.Title);
            if (isAny)
                return new IsSuccessResult<KnowledgeItem>(string.Format("已經有 {0} 這個知識了", data.Title));

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult<KnowledgeItem>("請選擇正確的分類");

            try
            {
                var ask = PetContext.Knowledges.Add(new Knowledge
                {
                    Title = data.Title,
                    Message = data.Message,
                    ClassId = data.ClassId,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.Now,
                        UserId = GetOperationInfo().UserId
                    }
                });
                PetContext.SaveChanges();

                return new IsSuccessResult<KnowledgeItem>
                {
                    ReturnObject = new KnowledgeItem
                    {
                        Id = ask.Id,
                        Title = ask.Title,
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult<KnowledgeItem>("發生不明錯誤，請稍候再試");
            }
        }

        /// <summary>
        /// 修改知識
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult EditKnowledge(int id, CreateKnowledge data)
        {
            var log = GetLogger();
            log.Debug("title: {0}, message:{1}, classId:{2}, id:{3}", data.Title, data.Message, data.ClassId, id);

            var knowledge = PetContext.Knowledges.SingleOrDefault(r => r.Id == id);
            if (knowledge == null)
                return new IsSuccessResult("找不到此知識");

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult("請輸入內容");
            data.Message = data.Message.Trim();

            var isAny = PetContext.Knowledges.Any(r => r.Title == data.Title && r.Id != id);
            if (isAny)
                return new IsSuccessResult(string.Format("已經有 {0} 這個知識了", data.Title));

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult("請選擇正確的分類");

            if (knowledge.Title == data.Title && knowledge.Message == data.Message && knowledge.ClassId == data.ClassId)
                return new IsSuccessResult();

            try
            {
                knowledge.Title = data.Title;
                knowledge.Message = data.Message;
                knowledge.ClassId = data.ClassId;

                PetContext.SaveChanges();

                return new IsSuccessResult();
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult("發生不明錯誤，請稍候再試");
            }
        }
    }
}