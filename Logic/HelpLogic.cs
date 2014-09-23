using System.IO;
using PetAdopt.DTO;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetAdopt.Logic
{
    public class HelpLogic : _BaseLogic
    {
        /// <summary>
        /// 即刻救援Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public HelpLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得即刻救援列表
        /// </summary>
        /// <returns></returns>
        public HelpList GetHelpList(int page)
        {
            var log = GetLogger();
            log.Debug("page:{0}", page);

            if (page <= 0)
                page = 1;

            var list = PetContext.Helps
                .Select(r => new HelpItem
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();

            var count = PetContext.Helps.Count();
            var result = new HelpList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 刪除即刻救援
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteHelp(string path, int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var help = PetContext.Helps.SingleOrDefault(r => r.Id == id);
            if (help == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此救援文章";
                return result;
            }

            // 有上傳圖片，就把圖片刪掉
            if (string.IsNullOrWhiteSpace(help.CoverPoto) == false)
            {
                File.Delete(path + "//" + help.CoverPoto);
            }

            try
            {
                PetContext.Helps.Remove(help);
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
        /// 新增救援
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<HelpItem> AddHelp(CreateHelp data)
        {
            var log = GetLogger();
            log.Debug("title: {0}, message:{1}, classId:{2}, areaId:{3}, address:{4}, poto:{5}",
                data.Title, data.Message, data.ClassId, data.AreaId, data.Address, data.Poto);

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<HelpItem>("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult<HelpItem>("請輸入內容");
            data.Message = data.Message.Trim();

            if (string.IsNullOrWhiteSpace(data.Address))
                return new IsSuccessResult<HelpItem>("請輸入地址");
            data.Address = data.Address.Trim();

            var isAny = PetContext.Helps.Any(r => r.Title == data.Title);
            if (isAny)
                return new IsSuccessResult<HelpItem>(string.Format("已經有 {0} 這個救援了", data.Title));

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult<HelpItem>("請選擇正確的分類");

            var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
            if (hasArea == false)
                return new IsSuccessResult<HelpItem>("請選擇正確的地區");

            if (string.IsNullOrWhiteSpace(data.Poto) == false)
                data.Poto = data.Poto.Trim();

            try
            {
                var ask = PetContext.Helps.Add(new Help
                {
                    CoverPoto = data.Poto,
                    Title = data.Title,
                    Message = data.Message,
                    ClassId = data.ClassId,
                    AreaId = data.AreaId,
                    Address = data.Address,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.Now,
                        UserId = GetOperationInfo().UserId
                    }
                });
                PetContext.SaveChanges();

                return new IsSuccessResult<HelpItem>
                {
                    ReturnObject = new HelpItem
                    {
                        Id = ask.Id,
                        Title = ask.Title,
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult<HelpItem>("發生不明錯誤，請稍候再試");
            }
        }
    }
}