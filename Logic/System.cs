using System.Web.Helpers;
using System.Web.Util;
using PetAdopt.DTO;
using PetAdopt.Models;
using PetAdopt.Utilities;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PetAdopt.Logic
{
    public class SystemLogic : _BaseLogic
    {
        /// <summary>
        /// 系統參數Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public SystemLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得地區列表
        /// </summary>
        /// <returns></returns>
        public List<AreaItem> GetAreaList()
        {
            var log = GetLogger();
            log.Debug("GetAreaList in");

            var areas = PetContext
                .Areas
                .Select(r => new AreaItem
                {
                    Id = r.Id,
                    Word = r.Word
                })
                .ToList();

            return areas;
        }

        /// <summary>
        /// 刪除地區
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteArea(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var area = PetContext
                .Areas
                .Where(r => r.Id == id)
                .SingleOrDefault();
            if (area == null)
            {
                result.ErrorMessage = "找不到此區域";
                return result;
            }

            PetContext.Areas.Remove(area);
            PetContext.SaveChanges();
            return result;
        }

        /// <summary>
        /// 新增地區
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<AreaItem> AddArea(string word)
        {
            var log = GetLogger();
            log.Debug("word: {0}", word);

            var result = new IsSuccessResult<AreaItem>();

            if (string.IsNullOrWhiteSpace(word))
                return new IsSuccessResult<AreaItem>("區域請勿傳入空值");
            word = word.Trim();

            var isAny = PetContext.Areas.Any(r => r.Word == word);
            if(isAny)
                return new IsSuccessResult<AreaItem>(string.Format("已經有 {0} 這個區域了", word));

            var area = PetContext.Areas.Add(new Area
            {
                Word = word
            });
            PetContext.SaveChanges();

            return new IsSuccessResult<AreaItem>
            {
                ReturnObject = new AreaItem
                {
                    Id = area.Id,
                    Word = area.Word
                }
            };
        }
    }
}