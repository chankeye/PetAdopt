using System.Web.UI.WebControls;
using PetAdopt.DTO;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetAdopt.Logic
{
    public class AskLogic : _BaseLogic
    {
        /// <summary>
        /// 問與答Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public AskLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得問與答列表
        /// </summary>
        /// <returns></returns>
        public AskList GetAskList(int page)
        {
            var log = GetLogger();
            log.Debug("page:{0}", page);

            if (page <= 0)
                page = 1;

            var list = PetContext.Asks
                .Select(r =>new AskItem
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .OrderBy(r => r.Id)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();

            var count = PetContext.Asks.Count();
            var result = new AskList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 刪除問與答
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteAsk(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var ask = PetContext.Asks.SingleOrDefault(r => r.Id == id);
            if (ask == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此問與答";
                return result;
            }

            try
            {
                PetContext.Asks.Remove(ask);
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