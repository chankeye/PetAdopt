using PetAdopt.DTO;
using PetAdopt.DTO.News;
using PetAdopt.Models;
using PetAdopt.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace PetAdopt.Logic
{
    public class NewsLogic : _BaseLogic
    {
        /// <summary>
        /// 最新消息Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public NewsLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得最新消息列表
        /// </summary>
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆資料</param>
        /// <param name="query">查詢條件(只能查標題)</param>
        /// <param name="isLike">非完全比對</param>
        /// <param name="userId">指定某user發佈的</param>
        /// <returns></returns>
        public NewsList GetNewsList(int page = 1, int take = 10, string query = "", bool isLike = true, int areaId = -1, int userId = -1)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}, areaId={4}, userId={5}", page, take, query, isLike, areaId, userId);

            if (page < 1)
                page = 1;

            if (take < 1)
                take = 10;

            List<NewsItem> list;
            var count = 0;

            // 查全部
            if (string.IsNullOrWhiteSpace(query))
            {
                var newslist = PetContext.News
                    .Select(r => new
                    {
                        r.Id,
                        r.CoverPhoto,
                        r.Title,
                        r.Message,
                        r.OperationInfo,
                        r.Area
                    });

                // 指定誰發佈的
                if (userId != -1)
                {
                    newslist = newslist
                        .Where(r => r.OperationInfo.UserId == userId);
                }

                // 指定地區
                if (areaId != -1)
                {
                    newslist = newslist
                        .Where(r => r.Area.Id == areaId);
                }

                var templist = newslist
                    .OrderByDescending(r => r.Id)
                    .Select(r => new
                    {
                        r.Id,
                        r.CoverPhoto,
                        r.Title,
                        r.Message,
                        r.OperationInfo.Date,
                        r.Area.Word
                    })
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                list = templist
                    .Select(r => new NewsItem
                    {
                        Id = r.Id,
                        Photo = r.CoverPhoto,
                        Title = r.Title,
                        Message = r.Message,
                        Date = r.Date.ToString("yyyy/MM/dd"),
                        Area = r.Word
                    })
                    .ToList();

                count = newslist.Count();
            }
            // 查特定標題
            else
            {
                // 查完全命中的
                if (isLike == false)
                {
                    var newslist = PetContext.News
                        .Where(r => r.Title == query)
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Message,
                            r.OperationInfo,
                            r.Area
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        newslist = newslist
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    // 指定地區
                    if (areaId != -1)
                    {
                        newslist = newslist
                            .Where(r => r.Area.Id == areaId);
                    }

                    var templist = newslist
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Message,
                            r.OperationInfo.Date,
                            r.Area.Word
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new NewsItem
                        {
                            Id = r.Id,
                            Photo = r.CoverPhoto,
                            Title = r.Title,
                            Message = r.Message,
                            Date = r.Date.ToString("yyyy/MM/dd"),
                            Area = r.Word
                        })
                        .ToList();

                    count = newslist.Count();
                }
                // 查包含的
                else
                {
                    var newslist = PetContext.News
                        .Where(r => r.Title.Contains(query))
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Message,
                            r.OperationInfo,
                            r.Area
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        newslist = newslist
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    // 指定地區
                    if (areaId != -1)
                    {
                        newslist = newslist
                            .Where(r => r.Area.Id == areaId);
                    }

                    var templist = newslist
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Message,
                            r.OperationInfo.Date,
                            r.Area.Word
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new NewsItem
                        {
                            Id = r.Id,
                            Photo = r.CoverPhoto,
                            Title = r.Title,
                            Message = r.Message,
                            Date = r.Date.ToString("yyyy/MM/dd"),
                            Area = r.Word
                        })
                        .ToList();

                    count = newslist.Count();
                }
            }

            var result = new NewsList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 取得最新消息
        /// </summary>
        /// <param name="id">News.Id</param>
        /// <returns></returns>
        public IsSuccessResult<GetNews> GetNews(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var news = PetContext.News
                .Include(r => r.Area)
                .Include(r => r.OperationInfo.User)
                .SingleOrDefault(r => r.Id == id);
            if (news == null)
                return new IsSuccessResult<GetNews>("找不到此消息");

            return new IsSuccessResult<GetNews>
            {
                ReturnObject = new GetNews
                {
                    Photo = news.CoverPhoto,
                    Title = news.Title,
                    Message = news.Message.Replace("\n", "<br>"),
                    AreaId = news.AreaId,
                    Area = news.AreaId.HasValue ? news.Area.Word : null,
                    Url = news.Url,
                    Date = news.OperationInfo.Date.ToString("yyyy-MM-dd"),
                    UserDisplay = news.OperationInfo.User.Display
                }
            };
        }

        /// <summary>
        /// 取得留言列表
        /// </summary>
        /// <param name="id">News.Id</param>
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆資料</param>
        /// <returns></returns>
        public NewsMessageList GetMessageList(int id, int page = 1, int take = 10)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, id:{2}", page, take, id);

            if (page <= 0)
                page = 1;

            if (take < 1)
                take = 10;

            var messages = PetContext.News
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

            var list = temp.Select(r => new NewsMessageItem
            {
                Id = r.Id,
                Message = r.Message,
                Date = r.Date.ToString("yyyy-MM-dd"),
                Account = r.Account
            })
            .ToList();

            var count = messages.Count();
            return new NewsMessageList
            {
                List = list,
                Count = count
            };
        }

        /// <summary>
        /// 刪除最新消息
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteNews(string path, int id, int userId)
        {
            var log = GetLogger();
            log.Debug("path: {0}, id: {1}, userId: {2}", path, id, userId);

            var result = new IsSuccessResult();
            var news = PetContext.News.SingleOrDefault(r => r.Id == id);
            if (news == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此消息";
                return result;
            }

            //檢查權限
            var user = PetContext.Users.SingleOrDefault(r => r.Id == userId);
            if (user == null || user.IsDisable)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "沒有權限";
                return result;
            }
            if (user.IsAdmin == false && news.OperationInfo.UserId != userId)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "沒有權限";
                return result;
            }

            try
            {

                // 有上傳圖片，就把圖片刪掉
                if (string.IsNullOrWhiteSpace(news.CoverPhoto) == false)
                {
                    File.Delete(path + "//" + news.CoverPhoto);
                }

                PetContext.Messages.RemoveRange(news.Messages);
                PetContext.News.Remove(news);
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
        /// <param name="id">News.id</param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public IsSuccessResult DeleteMessage(int id, int messageId, int userId)
        {
            var log = GetLogger();
            log.Debug("id: {0}, messageId: {1}, userId: {2}", id, messageId, userId);

            var result = new IsSuccessResult();

            var news = PetContext.News.SingleOrDefault(r => r.Id == id);
            if (news == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此問與答";
                return result;
            }

            //檢查權限
            var user = PetContext.Users.SingleOrDefault(r => r.Id == userId);
            if (user == null || user.IsDisable)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "沒有權限";
                return result;
            }
            if (user.IsAdmin == false)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "沒有權限";
                return result;
            }

            var message = news.Messages.SingleOrDefault(r => r.Id == messageId);
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
        /// 新增最新消息
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<NewsItem> AddNews(CreateNews data)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, title: {1}, message:{2}, areaId:{3}, url:{4}",
                data.Photo, data.Title, data.Message, data.AreaId, data.Url);

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<NewsItem>("標題請勿傳入空值");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult<NewsItem>("內容請勿傳入空值");
            data.Message = data.Message.Trim();

            if (string.IsNullOrWhiteSpace(data.Photo) == false)
                data.Photo = data.Photo.Trim();

            // Url驗證
            if (string.IsNullOrWhiteSpace(data.Url) == false)
            {
                data.Url = data.Url.Trim();

                if (Regex.IsMatch(data.Url, Constant.PatternUrl) == false)
                    return new IsSuccessResult<NewsItem>("資料來源請輸入正確網址");
            }

            var isAny = PetContext.News.Any(r => r.Title == data.Title);
            if (isAny)
                return new IsSuccessResult<NewsItem>(string.Format("已經有 {0} 這個最新消息了", data.Title));

            if (data.AreaId.HasValue)
            {
                var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
                if (hasArea == false)
                    return new IsSuccessResult<NewsItem>("請選擇正確的地區");
            }

            try
            {
                var news = PetContext.News.Add(new News
                {
                    CoverPhoto = data.Photo,
                    Title = data.Title,
                    Message = data.Message,
                    Url = data.Url,
                    AreaId = data.AreaId,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.Now,
                        UserId = GetOperationInfo().UserId
                    }
                });
                PetContext.SaveChanges();

                return new IsSuccessResult<NewsItem>
                {
                    ReturnObject = new NewsItem
                    {
                        Id = news.Id,
                        Title = news.Title,
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult<NewsItem>("發生不明錯誤，請稍候再試");
            }
        }

        /// <summary>
        /// 修改最新消息
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult EditNews(int id, CreateNews data, int userId)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, title: {1}, message: {2}, areaId: {3}, url: {4}, id: {5}, userId: {6}",
                data.Photo, data.Title, data.Message, data.AreaId, data.Url, id, userId);

            var news = PetContext.News.SingleOrDefault(r => r.Id == id);
            if (news == null)
                return new IsSuccessResult("找不到此最新消息");

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult("請輸入內容");
            data.Message = data.Message.Trim();

            //檢查權限
            var user = PetContext.Users.SingleOrDefault(r => r.Id == userId);
            if (user == null || user.IsDisable)
                return new IsSuccessResult("沒有權限");
            if (user.IsAdmin == false && news.OperationInfo.UserId != userId)
                return new IsSuccessResult("沒有權限");

            if (string.IsNullOrWhiteSpace(data.Photo) == false)
                data.Photo = data.Photo.Trim();

            // Url驗證
            if (string.IsNullOrWhiteSpace(data.Url) == false)
            {
                data.Url = data.Url.Trim();

                if (Regex.IsMatch(data.Url, Constant.PatternUrl) == false)
                    return new IsSuccessResult("資料來源請輸入正確網址");
            }

            var isAny = PetContext.News.Any(r => r.Title == data.Title && r.Id != id);
            if (isAny)
                return new IsSuccessResult(string.Format("已經有 {0} 這個最新消息了", data.Title));

            if (data.AreaId.HasValue)
            {
                var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
                if (hasArea == false)
                    return new IsSuccessResult("請選擇正確的地區");
            }

            if (news.CoverPhoto == data.Photo && news.Title == data.Title && news.Message == data.Message &&
                news.Url == data.Url && news.AreaId == data.AreaId)
            {
                return new IsSuccessResult();
            }

            try
            {
                news.CoverPhoto = data.Photo;
                news.Title = data.Title;
                news.Message = data.Message;
                news.Url = data.Url;
                news.AreaId = data.AreaId;

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
        /// <param name="id">News.Id</param>
        /// <param name="message">留言內容</param>
        /// <returns></returns>
        public IsSuccessResult AddMessage(int id, string message)
        {
            var log = GetLogger();
            log.Debug("id: {0}, message: {1}", id, message);

            if (string.IsNullOrWhiteSpace(message))
                return new IsSuccessResult("請輸入留言");
            message = message.Trim();

            var news = PetContext.News.SingleOrDefault(r => r.Id == id);
            if (news == null)
                return new IsSuccessResult("找不到此活動，暫時無法留言");

            try
            {
                news.Messages.Add(new Message
                {
                    Message1 = message,
                    IsRead = false,
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