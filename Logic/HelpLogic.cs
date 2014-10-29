using PetAdopt.DTO;
using PetAdopt.DTO.Help;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;
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
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆資料</param>
        /// <param name="query">查詢條件(只能查標題)</param>
        /// <param name="isLike">非完全比對</param>
        /// <param name="userId">指定某user發佈的</param>
        /// <returns></returns>
        public HelpList GetHelpList(int page = 1, int take = 10, string query = "", bool isLike = true, int userId = -1)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}", page, take, query, isLike);

            if (page < 1)
                page = 1;

            if (take < 1)
                take = 10;

            List<HelpItem> list;
            var count = 0;

            // 查全部
            if (string.IsNullOrWhiteSpace(query))
            {
                var helps = PetContext.Helps
                    .Select(r => new
                    {
                        r.Id,
                        r.CoverPhoto,
                        r.Title,
                        r.Message,
                        r.Area.Word,
                        r.OperationInfo
                    });

                // 指定誰發佈的
                if (userId != -1)
                {
                    helps = helps
                        .Where(r => r.OperationInfo.UserId == userId);
                }

                var templist = helps
                    .OrderByDescending(r => r.Id)
                    .Select(r => new
                    {
                        r.Id,
                        r.CoverPhoto,
                        r.Title,
                        r.Message,
                        r.Word,
                        r.OperationInfo.Date
                    })
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                list = templist
                    .Select(r => new HelpItem
                    {
                        Id = r.Id,
                        Photo = r.CoverPhoto,
                        Title = r.Title,
                        Message = r.Message,
                        Area = r.Word,
                        Date = r.Date.ToString("yyyy/MM/dd")
                    })
                    .ToList();

                count = helps.Count();
            }
            // 查特定標題
            else
            {
                // 查完全命中的
                if (isLike == false)
                {
                    var helps = PetContext.Helps
                        .Where(r => r.Title == query)
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Message,
                            r.Area.Word,
                            r.OperationInfo
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        helps = helps
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    var templist = helps
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Message,
                            r.Word,
                            r.OperationInfo.Date
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new HelpItem
                        {
                            Id = r.Id,
                            Photo = r.CoverPhoto,
                            Title = r.Title,
                            Message = r.Message,
                            Area = r.Word,
                            Date = r.Date.ToString("yyyy/MM/dd")
                        })
                        .ToList();

                    count = helps.Count();
                }
                // 查包含的
                else
                {
                    var helps = PetContext.Helps
                        .Where(r => r.Title.Contains(query))
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Message,
                            r.Area.Word,
                            r.OperationInfo
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        helps = helps
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    var templist = helps
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Message,
                            r.Word,
                            r.OperationInfo.Date
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new HelpItem
                        {
                            Id = r.Id,
                            Photo = r.CoverPhoto,
                            Title = r.Title,
                            Message = r.Message,
                            Area = r.Word,
                            Date = r.Date.ToString("yyyy/MM/dd")
                        })
                        .ToList();

                    count = helps.Count();
                }
            }

            var result = new HelpList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 取得救援文章
        /// </summary>
        /// <param name="id">Help.Id</param>
        /// <returns></returns>
        public IsSuccessResult<GetHelp> GetHelp(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var help = PetContext.Helps
                .Include(r => r.Area)
                .Include(r => r.Class)
                .SingleOrDefault(r => r.Id == id);
            if (help == null)
                return new IsSuccessResult<GetHelp>("找不到此救援文章");

            return new IsSuccessResult<GetHelp>
            {
                ReturnObject = new GetHelp
                {
                    Photo = help.CoverPhoto,
                    Title = help.Title,
                    Message = help.Message,
                    Address = help.Address,
                    AreaId = help.AreaId,
                    Area = help.Area.Word,
                    ClassId = help.ClassId,
                    Class = help.Class.Word
                }
            };
        }

        /// <summary>
        /// 取得留言列表
        /// </summary>
        /// <param name="id">Help.Id</param>
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆資料</param>
        /// <returns></returns>
        public HelpMessageList GetMessageList(int id, int page = 1, int take = 10)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, id:{2}", page, take, id);

            if (page <= 0)
                page = 1;

            if (take < 1)
                take = 10;

            var messages = PetContext.Helps
                .Where(r => r.Id == id)
                .SelectMany(r => r.Messages);

            var temp = messages.Select(r => new
            {
                r.Id,
                Message = r.Message1,
                r.OperationInfo.Date,
                r.OperationInfo.User.Account
            })
            .OrderByDescending(r => r.Id)
            .Skip((page - 1) * take)
            .Take(take)
            .ToList();

            var list = temp.Select(r => new HelpMessageItem
            {
                Id = r.Id,
                Message = r.Message,
                Date = r.Date.ToString("yyyy-MM-dd"),
                Account = r.Account
            })
            .ToList();

            var count = messages.Count();
            return new HelpMessageList
            {
                List = list,
                Count = count
            };
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
            if (string.IsNullOrWhiteSpace(help.CoverPhoto) == false)
            {
                File.Delete(path + "//" + help.CoverPhoto);
            }

            try
            {
                PetContext.Messages.RemoveRange(help.Messages);
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
        /// 刪除留言
        /// </summary>
        /// <param name="id">Help.id</param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public IsSuccessResult DeleteMessage(int id, int messageId)
        {
            var log = GetLogger();
            log.Debug("id:{0}, messageId:{1}", id, messageId);

            var result = new IsSuccessResult();

            var help = PetContext.Helps.SingleOrDefault(r => r.Id == id);
            if (help == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此救援文章";
                return result;
            }

            var message = help.Messages.SingleOrDefault(r => r.Id == messageId);
            if (message == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此留言";
                return result;
            }

            try
            {
                PetContext.Messages.Remove(message);
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
            log.Debug("title: {0}, message:{1}, classId:{2}, areaId:{3}, address:{4}, photo:{5}",
                data.Title, data.Message, data.ClassId, data.AreaId, data.Address, data.Photo);

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

            if (string.IsNullOrWhiteSpace(data.Photo) == false)
                data.Photo = data.Photo.Trim();

            try
            {
                var ask = PetContext.Helps.Add(new Help
                {
                    CoverPhoto = data.Photo,
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

        /// <summary>
        /// 修改救援
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult EditHelp(int id, CreateHelp data)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, title: {1}, message:{2}, areaId:{3}, address:{4}, classId:{5}, id:{6}",
                data.Photo, data.Title, data.Message, data.AreaId, data.Address, data.ClassId, id);

            var help = PetContext.Helps.SingleOrDefault(r => r.Id == id);
            if (help == null)
                return new IsSuccessResult("找不到此救援文章");

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult("請輸入內容");
            data.Message = data.Message.Trim();

            if (string.IsNullOrWhiteSpace(data.Address))
                return new IsSuccessResult<HelpItem>("請輸入地址");
            data.Address = data.Address.Trim();

            if (string.IsNullOrWhiteSpace(data.Photo) == false)
                data.Photo = data.Photo.Trim();

            var isAny = PetContext.Helps.Any(r => r.Title == data.Title && r.Id != id);
            if (isAny)
                return new IsSuccessResult(string.Format("已經有 {0} 這個救援文章了", data.Title));

            var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
            if (hasArea == false)
                return new IsSuccessResult("請選擇正確的地區");

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult("請選擇正確的分類");

            if (help.CoverPhoto == data.Photo && help.Title == data.Title && help.Message == data.Message &&
                help.Address == data.Address && help.AreaId == data.AreaId && help.ClassId == data.ClassId)
            {
                return new IsSuccessResult();
            }

            try
            {
                help.CoverPhoto = data.Photo;
                help.Title = data.Title;
                help.Message = data.Message;
                help.Address = data.Address;
                help.AreaId = data.AreaId;
                help.ClassId = data.ClassId;

                PetContext.SaveChanges();

                return new IsSuccessResult();
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult("發生不明錯誤，請稍候再試");
            }
        }

        /// <summary>
        /// 留言
        /// </summary>
        /// <param name="id">Help.Id</param>
        /// <param name="message">留言內容</param>
        /// <returns></returns>
        public IsSuccessResult AddMessage(int id, string message)
        {
            var log = GetLogger();
            log.Debug("id: {0}, message: {1}", id, message);

            if (string.IsNullOrWhiteSpace(message))
                return new IsSuccessResult("請輸入留言");
            message = message.Trim();

            var help = PetContext.Helps.SingleOrDefault(r => r.Id == id);
            if (help == null)
                return new IsSuccessResult("找不到此救援文章，暫時無法留言");

            try
            {
                help.Messages.Add(new Message
                {
                    Message1 = message,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.Now,
                        UserId = GetOperationInfo().UserId
                    }
                });

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