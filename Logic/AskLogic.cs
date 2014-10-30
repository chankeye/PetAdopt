using PetAdopt.DTO;
using PetAdopt.DTO.Ask;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public AskList GetAskList(int page = 1, int take = 10, string query = "", bool isLike = true, int classId = -1, int userId = -1)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}, classId={4}, userId={5}", page, take, query, isLike, classId, userId);

            if (page <= 0)
                page = 1;

            if (take < 1)
                take = 10;

            List<AskItem> list;
            var count = 0;

            // 查全部
            if (string.IsNullOrWhiteSpace(query))
            {
                var asks = PetContext.Asks
                    .Select(r => new
                    {
                        r.Id,
                        r.Title,
                        r.OperationInfo,
                        r.Class
                    });

                // 指定誰發佈的
                if (userId != -1)
                {
                    asks = asks
                        .Where(r => r.OperationInfo.UserId == userId);
                }

                // 指定地區
                if (classId != -1)
                {
                    asks = asks
                        .Where(r => r.Class.Id == classId);
                }

                var templist = asks
                    .OrderByDescending(r => r.Id)
                    .Select(r => new
                    {
                        r.Id,
                        r.Title,
                        r.OperationInfo.Date,
                        r.Class.Word
                    })
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                list = templist
                    .Select(r => new AskItem
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Date = r.Date.ToString("yyyy/MM/dd"),
                        Classes = r.Word
                    })
                    .ToList();

                count = asks.Count();
            }
            // 查特定標題
            else
            {
                // 查完全命中的
                if (isLike == false)
                {
                    var asks = PetContext.Asks
                        .Where(r => r.Title == query)
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo,
                            r.Class
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        asks = asks
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    // 指定地區
                    if (classId != -1)
                    {
                        asks = asks
                            .Where(r => r.Class.Id == classId);
                    }

                    var templist = asks
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo.Date,
                            r.Class.Word
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new AskItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString("yyyy/MM/dd"),
                            Classes = r.Word
                        })
                        .ToList();

                    count = asks.Count();
                }
                // 查包含的
                else
                {
                    var asks = PetContext.Asks
                        .Where(r => r.Title.Contains(query))
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo,
                            r.Class
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        asks = asks
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    // 指定地區
                    if (classId != -1)
                    {
                        asks = asks
                            .Where(r => r.Class.Id == classId);
                    }

                    var templist = asks
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo.Date,
                            r.Class.Word
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new AskItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString("yyyy/MM/dd"),
                            Classes = r.Word
                        })
                        .ToList();

                    count = asks.Count();
                }
            }

            var result = new AskList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 取得問與答
        /// </summary>
        /// <param name="id">Ask.Id</param>
        /// <returns></returns>
        public IsSuccessResult<GetAsk> GetAsk(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var ask = PetContext.Asks
                .Include(r => r.Class)
                .Include(r => r.OperationInfo.User)
                .SingleOrDefault(r => r.Id == id);
            if (ask == null)
                return new IsSuccessResult<GetAsk>("找不到此問與答");

            return new IsSuccessResult<GetAsk>
            {
                ReturnObject = new GetAsk
                {
                    Title = ask.Title,
                    Message = ask.Message,
                    ClassId = ask.ClassId,
                    Class = ask.Class.Word,
                    Date = ask.OperationInfo.Date.ToString("yyyy-MM-dd"),
                    UserDisplay = ask.OperationInfo.User.Display
                }
            };
        }

        /// <summary>
        /// 取得留言列表
        /// </summary>
        /// <param name="id">Ask.Id</param>
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆資料</param>
        /// <returns></returns>
        public AskMessageList GetMessageList(int id, int page = 1, int take = 10)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, id:{2}", page, take, id);

            if (page <= 0)
                page = 1;

            if (take < 1)
                take = 10;

            var messages = PetContext.Asks
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

            var list = temp.Select(r => new AskMessageItem
            {
                Id = r.Id,
                Message = r.Message,
                Date = r.Date.ToString("yyyy-MM-dd"),
                Account = r.Account
            })
            .ToList();

            var count = messages.Count();
            return new AskMessageList
            {
                List = list,
                Count = count
            };
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
                PetContext.Messages.RemoveRange(ask.Messages);
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

        /// <summary>
        /// 刪除留言
        /// </summary>
        /// <param name="id">Ask.id</param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public IsSuccessResult DeleteMessage(int id, int messageId)
        {
            var log = GetLogger();
            log.Debug("id:{0}, messageId:{1}", id, messageId);

            var result = new IsSuccessResult();

            var ask = PetContext.Asks.SingleOrDefault(r => r.Id == id);
            if (ask == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此問與答";
                return result;
            }

            var message = ask.Messages.SingleOrDefault(r => r.Id == messageId);
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
        /// 新增問與答
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<AskItem> AddAsk(CreateAsk data)
        {
            var log = GetLogger();
            log.Debug("title: {0}, message:{1}, classId:{2}", data.Title, data.Message, data.ClassId);

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<AskItem>("標題請勿傳入空值");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult<AskItem>("內容請勿傳入空值");
            data.Message = data.Message.Trim();

            var isAny = PetContext.Asks.Any(r => r.Title == data.Title);
            if (isAny)
                return new IsSuccessResult<AskItem>(string.Format("已經有 {0} 這個問與答了", data.Title));

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult<AskItem>("請選擇正確的分類");

            try
            {
                var ask = PetContext.Asks.Add(new Ask
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

                return new IsSuccessResult<AskItem>
                {
                    ReturnObject = new AskItem
                    {
                        Id = ask.Id,
                        Title = ask.Title,
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult<AskItem>("發生不明錯誤，請稍候再試");
            }
        }

        /// <summary>
        /// 修改問與答
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult EditAsk(int id, CreateAsk data)
        {
            var log = GetLogger();
            log.Debug("title: {0}, message:{1}, classId:{2}, id:{3}", data.Title, data.Message, data.ClassId, id);

            var ask = PetContext.Asks.SingleOrDefault(r => r.Id == id);
            if (ask == null)
                return new IsSuccessResult("找不到此問與答");

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult("請輸入內容");
            data.Message = data.Message.Trim();

            var isAny = PetContext.Asks.Any(r => r.Title == data.Title && r.Id != id);
            if (isAny)
                return new IsSuccessResult(string.Format("已經有 {0} 這個問與答了", data.Title));

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult("請選擇正確的分類");

            if (ask.Title == data.Title && ask.Message == data.Message && ask.ClassId == data.ClassId)
                return new IsSuccessResult();

            try
            {
                ask.Title = data.Title;
                ask.Message = data.Message;
                ask.ClassId = data.ClassId;

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
        /// <param name="id">Ask.Id</param>
        /// <param name="message">留言內容</param>
        /// <returns></returns>
        public IsSuccessResult AddMessage(int id, string message)
        {
            var log = GetLogger();
            log.Debug("id: {0}, message: {1}", id, message);

            if (string.IsNullOrWhiteSpace(message))
                return new IsSuccessResult("請輸入留言");
            message = message.Trim();

            var ask = PetContext.Asks.SingleOrDefault(r => r.Id == id);
            if (ask == null)
                return new IsSuccessResult("找不到此問題，暫時無法留言");

            try
            {
                ask.Messages.Add(new Message
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