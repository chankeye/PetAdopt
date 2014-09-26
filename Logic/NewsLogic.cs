using System.Collections.Generic;
using PetAdopt.DTO;
using PetAdopt.DTO.News;
using PetAdopt.Models;
using PetAdopt.Utilities;
using System;
using System.IO;
using System.Linq;
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
        /// <returns></returns>
        public NewsList GetNewsList(int page = 1, int take = 10, string query = "", bool isLike = true)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}", page, take, query, isLike);

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
                        r.Title,
                        r.OperationInfo.Date
                    });

                var templist = newslist
                    .OrderByDescending(r => r.Id)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                list = templist
                    .Select(r => new NewsItem
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Date = r.Date.ToString("yyyy/MM/dd")
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
                            r.Title,
                            r.OperationInfo.Date
                        });

                    var templist = newslist
                        .OrderByDescending(r => r.Id)
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new NewsItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString("yyyy/MM/dd")
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
                            r.Title,
                            r.OperationInfo.Date
                        });

                    var templist = newslist
                        .OrderByDescending(r => r.Id)
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new NewsItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString("yyyy/MM/dd")
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
        /// 刪除最新消息
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteNews(string path, int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var news = PetContext.News.SingleOrDefault(r => r.Id == id);
            if (news == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此消息";
                return result;
            }

            // 有上傳圖片，就把圖片刪掉
            if (string.IsNullOrWhiteSpace(news.CoverPhoto) == false)
            {
                File.Delete(path + "//" + news.CoverPhoto);
            }

            try
            {
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
        /// 取得最新消息
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<CreateNews> GetNews(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var news = PetContext.News.SingleOrDefault(r => r.Id == id);
            if (news == null)
                return new IsSuccessResult<CreateNews>("找不到此消息");

            return new IsSuccessResult<CreateNews>
            {
                ReturnObject = new CreateNews
                {
                    Photo = news.CoverPhoto,
                    Title = news.Title,
                    Message = news.Message,
                    AreaId = news.AreaId,
                    Url = news.Url
                }
            };
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

                if (Regex.IsMatch(data.Url, Constant.PatternEmail) == false)
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
        public IsSuccessResult EditNews(int id, CreateNews data)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, title: {1}, message:{2}, areaId:{3}, url:{4}, id:{5}",
                data.Photo, data.Title, data.Message, data.AreaId, data.Url, id);

            var news = PetContext.News.SingleOrDefault(r => r.Id == id);
            if (news == null)
                return new IsSuccessResult("找不到此最新消息");

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult("請輸入內容");
            data.Message = data.Message.Trim();

            if (string.IsNullOrWhiteSpace(data.Photo) == false)
                data.Photo = data.Photo.Trim();

            // Url驗證
            if (string.IsNullOrWhiteSpace(data.Url) == false)
            {
                data.Url = data.Url.Trim();

                if (Regex.IsMatch(data.Url, Constant.PatternEmail) == false)
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
    }
}