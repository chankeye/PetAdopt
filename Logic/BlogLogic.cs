using System.Collections.Generic;
using System.Threading;
using PetAdopt.DTO;
using PetAdopt.DTO.Ask;
using PetAdopt.DTO.Blog;
using PetAdopt.Models;
using System;
using System.Linq;

namespace PetAdopt.Logic
{
    public class BlogLogic : _BaseLogic
    {
        /// <summary>
        /// BlogLogic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public BlogLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得部落格列表
        /// </summary>
        /// <returns></returns>
        public BlogList GetBlogList(int page = 1, int take = 10, string query = "", bool isLike = true)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}", page, take, query, isLike);

            if (page < 1)
                page = 1;

            if (take < 1)
                take = 10;

            List<BlogItem> list;
            var count = 0;

            // 查全部
            if (string.IsNullOrWhiteSpace(query))
            {
                var blogs = PetContext.Blogs
                    .Select(r => new
                    {
                        r.Id,
                        r.Title,
                        r.OperationInfo.Date
                    });

                var templist = blogs
                    .OrderByDescending(r => r.Id)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                list = templist
                    .Select(r => new BlogItem
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Date = r.Date.ToString("yyyy/MM/dd")
                    })
                    .ToList();

                count = blogs.Count();
            }
            // 查特定標題
            else
            {
                // 查完全命中的
                if (isLike == false)
                {
                    var blogs = PetContext.Blogs
                        .Where(r => r.Title == query)
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo.Date
                        });

                    var templist = blogs
                        .OrderByDescending(r => r.Id)
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new BlogItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString("yyyy/MM/dd")
                        })
                        .ToList();

                    count = blogs.Count();
                }
                // 查包含的
                else
                {
                    var blogs = PetContext.Blogs
                        .Where(r => r.Title.Contains(query))
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo.Date
                        });

                    var templist = blogs
                        .OrderByDescending(r => r.Id)
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new BlogItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString("yyyy/MM/dd")
                        })
                        .ToList();

                    count = blogs.Count();
                }
            }

            var result = new BlogList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 刪除部落格文章
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteBlog(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var blog = PetContext.Blogs.SingleOrDefault(r => r.Id == id);
            if (blog == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此部落格文章";
                return result;
            }

            try
            {
                PetContext.Blogs.Remove(blog);
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
        /// <param name="id">Blog.id</param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public IsSuccessResult DeleteMessage(int id, int messageId)
        {
            var log = GetLogger();
            log.Debug("id:{0}, messageId:{1}", id, messageId);

            var result = new IsSuccessResult();

            var blog = PetContext.Blogs.SingleOrDefault(r => r.Id == id);
            if (blog == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此問與答";
                return result;
            }

            var message = blog.Messages.SingleOrDefault(r => r.Id == messageId);
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
        /// 取得文章
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<CreateBlog> GetBlog(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var blog = PetContext.Blogs.SingleOrDefault(r => r.Id == id);
            if (blog == null)
                return new IsSuccessResult<CreateBlog>("找不到此文章");

            return new IsSuccessResult<CreateBlog>
            {
                ReturnObject = new CreateBlog
                {
                    Title = blog.Title,
                    Message = blog.Message,
                    ClassId = blog.ClassId,
                    AnimalId = blog.AnimalId
                }
            };
        }

        /// <summary>
        /// 取得留言列表
        /// </summary>
        /// <param name="id">Blog.Id</param>
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆資料</param>
        /// <returns></returns>
        public MessageList GetMessageList(int id, int page = 1, int take = 10)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, id:{2}", page, take, id);

            if (page <= 0)
                page = 1;

            if (take < 1)
                take = 10;

            var messages = PetContext.Blogs
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

            var list = temp.Select(r => new MessageItem
            {
                Id = r.Id,
                Message = r.Message,
                Date = r.Date.ToString("yyyy-MM-dd"),
                Account = r.Account
            })
            .ToList();

            var count = messages.Count();
            return new MessageList
            {
                List = list,
                Count = count
            };
        }

        /// <summary>
        /// 新增Blog文章
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<BlogItem> AddBlog(CreateBlog data)
        {
            var log = GetLogger();
            log.Debug("title: {0}, message:{1}, animalId:{2}, classId:{3}",
                data.Title, data.Message, data.AnimalId, data.ClassId);

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<BlogItem>("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult<BlogItem>("請輸入內容");
            data.Message = data.Message.Trim();

            var isAny = PetContext.Activities.Any(r => r.Title == data.Title);
            if (isAny)
                return new IsSuccessResult<BlogItem>(string.Format("已經有 {0} 這個最新消息了", data.Title));

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult<BlogItem>("請選擇正確的分類");

            if (data.AnimalId.HasValue)
            {
                var hasAnimal = PetContext.Animals.Any(r => r.Id == data.AnimalId);
                if (hasAnimal == false)
                    return new IsSuccessResult<BlogItem>("請輸入正確的動物編號");
            }

            try
            {
                var blog = PetContext.Blogs.Add(new Blog
                {
                    Title = data.Title,
                    Message = data.Message,
                    AnimalId = data.AnimalId,
                    ClassId = data.ClassId,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.Now,
                        UserId = GetOperationInfo().UserId
                    }
                });
                PetContext.SaveChanges();

                return new IsSuccessResult<BlogItem>
                {
                    ReturnObject = new BlogItem
                    {
                        Id = blog.Id,
                        Title = blog.Title,
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult<BlogItem>("發生不明錯誤，請稍候再試");
            }
        }

        /// <summary>
        /// 修改部落格文章
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult EditBlog(int id, CreateBlog data)
        {
            var log = GetLogger();
            log.Debug("title: {0}, message:{1}, classId:{2}, animalId:{3}, id:{4}",
                data.Title, data.Message, data.ClassId, data.AnimalId, id);

            var blog = PetContext.Blogs.SingleOrDefault(r => r.Id == id);
            if (blog == null)
                return new IsSuccessResult("找不到此文章");

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult("請輸入內容");
            data.Message = data.Message.Trim();

            var isAny = PetContext.Blogs.Any(r => r.Title == data.Title && r.Id != id);
            if (isAny)
                return new IsSuccessResult(string.Format("已經有 {0} 這篇文章了", data.Title));

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult("請選擇正確的分類");

            if (data.AnimalId.HasValue)
            {
                var hasAnimal = PetContext.Animals.Any(r => r.Id == data.AnimalId);
                if (hasAnimal == false)
                    return new IsSuccessResult("請輸入正確的動物編號");
            }

            if (blog.Title == data.Title && blog.Message == data.Message &&
                blog.ClassId == data.ClassId && blog.AnimalId == data.AnimalId)
            {
                return new IsSuccessResult();
            }

            try
            {
                blog.Title = data.Title;
                blog.Message = data.Message;
                blog.ClassId = data.ClassId;
                blog.AnimalId = data.AnimalId;

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