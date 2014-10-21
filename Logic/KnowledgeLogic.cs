using PetAdopt.DTO;
using PetAdopt.DTO.Knowledge;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆資料</param>
        /// <param name="query">查詢條件(只能查標題)</param>
        /// <param name="isLike">非完全比對</param>
        /// <param name="userId">指定某user發佈的</param>
        /// <returns></returns>
        public KnowledgeList GetKnowledgeList(int page = 1, int take = 10, string query = "", bool isLike = true, int userId = -1)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}", page, take, query, isLike);

            if (page < 1)
                page = 1;

            if (take < 1)
                take = 10;

            List<KnowledgeItem> list;
            var count = 0;

            // 查全部
            if (string.IsNullOrWhiteSpace(query))
            {
                var knowledgeItem = PetContext.Knowledges
                    .Select(r => new
                    {
                        r.Id,
                        r.Title,
                        r.OperationInfo
                    });

                // 指定誰發佈的
                if (userId != -1)
                {
                    knowledgeItem = knowledgeItem
                        .Where(r => r.OperationInfo.UserId == userId);
                }

                var templist = knowledgeItem
                    .OrderByDescending(r => r.Id)
                    .Select(r => new
                    {
                        r.Id,
                        r.Title,
                        r.OperationInfo.Date
                    })
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                list = templist
                    .Select(r => new KnowledgeItem
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Date = r.Date.ToString("yyyy/MM/dd")
                    })
                    .ToList();

                count = knowledgeItem.Count();
            }
            // 查特定標題
            else
            {
                // 查完全命中的
                if (isLike == false)
                {
                    var knowledgeItem = PetContext.Knowledges
                        .Where(r => r.Title == query)
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        knowledgeItem = knowledgeItem
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    var templist = knowledgeItem
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo.Date
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new KnowledgeItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString("yyyy/MM/dd")
                        })
                        .ToList();

                    count = knowledgeItem.Count();
                }
                // 查包含的
                else
                {
                    var knowledgeItem = PetContext.Knowledges
                        .Where(r => r.Title.Contains(query))
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        knowledgeItem = knowledgeItem
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    var templist = knowledgeItem
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo.Date
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new KnowledgeItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString("yyyy/MM/dd")
                        })
                        .ToList();

                    count = knowledgeItem.Count();
                }
            }

            var result = new KnowledgeList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 取得知識
        /// </summary>
        /// <param name="id">Knowledge.Id</param>
        /// <returns></returns>
        public IsSuccessResult<GetKnowledge> GetKnowledge(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var knowledge = PetContext.Knowledges
                .Include(r => r.Class)
                .SingleOrDefault(r => r.Id == id);
            if (knowledge == null)
                return new IsSuccessResult<GetKnowledge>("找不到此知識");

            return new IsSuccessResult<GetKnowledge>
            {
                ReturnObject = new GetKnowledge
                {
                    Title = knowledge.Title,
                    Message = knowledge.Message,
                    ClassId = knowledge.ClassId,
                    Class = knowledge.Class.Word,
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
        public KnowledgeMessageList GetMessageList(int id, int page = 1, int take = 10)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, id:{2}", page, take, id);

            if (page <= 0)
                page = 1;

            if (take < 1)
                take = 10;

            var messages = PetContext.Knowledges
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

            var list = temp.Select(r => new KnowledgeMessageItem
            {
                Id = r.Id,
                Message = r.Message,
                Date = r.Date.ToString("yyyy-MM-dd"),
                Account = r.Account
            })
            .ToList();

            var count = messages.Count();
            return new KnowledgeMessageList
            {
                List = list,
                Count = count
            };
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
        /// 刪除留言
        /// </summary>
        /// <param name="id">Knowledge.id</param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public IsSuccessResult DeleteMessage(int id, int messageId)
        {
            var log = GetLogger();
            log.Debug("id:{0}, messageId:{1}", id, messageId);

            var result = new IsSuccessResult();

            var knowledge = PetContext.Knowledges.SingleOrDefault(r => r.Id == id);
            if (knowledge == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此知識文章";
                return result;
            }

            var message = knowledge.Messages.SingleOrDefault(r => r.Id == messageId);
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

        /// <summary>
        /// 留言
        /// </summary>
        /// <param name="id">Knowledge.Id</param>
        /// <param name="message">留言內容</param>
        /// <returns></returns>
        public IsSuccessResult AddMessage(int id, string message)
        {
            var log = GetLogger();
            log.Debug("id: {0}, message: {1}", id, message);

            if (string.IsNullOrWhiteSpace(message))
                return new IsSuccessResult("請輸入留言");
            message = message.Trim();

            var knowledge = PetContext.Knowledges.SingleOrDefault(r => r.Id == id);
            if (knowledge == null)
                return new IsSuccessResult("找不到此活動，暫時無法留言");

            try
            {
                knowledge.Messages.Add(new Message
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