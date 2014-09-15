using PetAdopt.DTO;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
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
        public List<KnowledgeItem> GetKnowledgeList()
        {
            var log = GetLogger();
            log.Debug("GetKnowledgeList in");

            var knowledgelist = PetContext
                .Knowledges
                .Select(r => new KnowledgeItem
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .ToList();

            return knowledgelist;
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
    }
}