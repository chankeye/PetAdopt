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
                result.IsSuccess = false;
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
            if (isAny)
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

        /// <summary>
        /// 取得狀態列表
        /// </summary>
        /// <returns></returns>
        public List<StatusItem> GetStatusList()
        {
            var log = GetLogger();
            log.Debug("GetStatusList in");

            var statuses = PetContext
                .Status
                .Select(r => new StatusItem
                {
                    Id = r.Id,
                    Word = r.Word
                })
                .ToList();

            return statuses;
        }

        /// <summary>
        /// 刪除狀態
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteStatus(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var status = PetContext
                .Status
                .Where(r => r.Id == id)
                .SingleOrDefault();
            if (status == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此送養狀態";
                return result;
            }

            PetContext.Status.Remove(status);
            PetContext.SaveChanges();
            return result;
        }

        /// <summary>
        /// 新增狀態
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<StatusItem> AddStatus(string word)
        {
            var log = GetLogger();
            log.Debug("word: {0}", word);

            var result = new IsSuccessResult<StatusItem>();

            if (string.IsNullOrWhiteSpace(word))
                return new IsSuccessResult<StatusItem>("送養狀態請勿傳入空值");
            word = word.Trim();

            var isAny = PetContext.Status.Any(r => r.Word == word);
            if (isAny)
                return new IsSuccessResult<StatusItem>(string.Format("已經有 {0} 這個送養狀態了", word));

            var status = PetContext.Status.Add(new Status
            {
                Word = word
            });
            PetContext.SaveChanges();

            return new IsSuccessResult<StatusItem>
            {
                ReturnObject = new StatusItem
                {
                    Id = status.Id,
                    Word = status.Word
                }
            };
        }

        /// <summary>
        /// 取得種類列表
        /// </summary>
        /// <returns></returns>
        public List<ClassItem> GetClassList()
        {
            var log = GetLogger();
            log.Debug("GetClassList in");

            var classes = PetContext
                .Classes
                .Select(r => new ClassItem
                {
                    Id = r.Id,
                    Word = r.Word
                })
                .ToList();

            return classes;
        }

        /// <summary>
        /// 刪除種類
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteClass(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var clas = PetContext
                .Classes
                .Where(r => r.Id == id)
                .SingleOrDefault();
            if (clas == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此動物種類";
                return result;
            }

            PetContext.Classes.Remove(clas);
            PetContext.SaveChanges();
            return result;
        }

        /// <summary>
        /// 新增種類
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<ClassItem> AddClass(string word)
        {
            var log = GetLogger();
            log.Debug("word: {0}", word);

            var result = new IsSuccessResult<ClassItem>();

            if (string.IsNullOrWhiteSpace(word))
                return new IsSuccessResult<ClassItem>("動物種類請勿傳入空值");
            word = word.Trim();

            var isAny = PetContext.Classes.Any(r => r.Word == word);
            if (isAny)
                return new IsSuccessResult<ClassItem>(string.Format("已經有 {0} 這個動物種類了", word));

            var clas = PetContext.Classes.Add(new Class
            {
                Word = word
            });
            PetContext.SaveChanges();

            return new IsSuccessResult<ClassItem>
            {
                ReturnObject = new ClassItem
                {
                    Id = clas.Id,
                    Word = clas.Word
                }
            };
        }
    }
}