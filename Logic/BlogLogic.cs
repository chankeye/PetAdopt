using PetAdopt.DTO;
using PetAdopt.DTO.Blog;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PetAdopt.Logic
{
    /// <summary>
    /// Blog Logic
    /// </summary>
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
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆</param>
        /// <param name="query">查標題</param>
        /// <param name="isLike">模糊比對</param>
        /// <param name="classId">Class.Id</param>
        /// <param name="userId">User.Id</param>
        /// <returns></returns>
        public BlogList GetBlogList(int page = 1, int take = 10, string query = "", bool isLike = true, int classId = -1, int userId = -1)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}, classId={4}, userId={5}", page, take, query, isLike, classId, userId);

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
                        r.OperationInfo,
                        r.Class
                    });

                // 指定誰發佈的
                if (userId != -1)
                {
                    blogs = blogs
                        .Where(r => r.OperationInfo.UserId == userId);
                }

                // 指定分類
                if (classId != -1)
                {
                    blogs = blogs
                        .Where(r => r.Class.Id == classId);
                }

                var templist = blogs
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
                    .Select(r => new BlogItem
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Date = r.Date.ToString() + " UTC",
                        Classes = r.Word
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
                            r.OperationInfo,
                            r.Class
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        blogs = blogs
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    // 指定分類
                    if (classId != -1)
                    {
                        blogs = blogs
                            .Where(r => r.Class.Id == classId);
                    }

                    var templist = blogs
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
                        .Select(r => new BlogItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString() + " UTC",
                            Classes = r.Word
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
                            r.OperationInfo,
                            r.Class
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        blogs = blogs
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    // 指定分類
                    if (classId != -1)
                    {
                        blogs = blogs
                            .Where(r => r.Class.Id == classId);
                    }

                    var templist = blogs
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
                        .Select(r => new BlogItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString() + " UTC",
                            Classes = r.Word
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
        /// 取得文章
        /// </summary>
        /// <param name="id">Blog.Id</param>
        /// <returns></returns>
        public IsSuccessResult<GetBlog> GetBlog(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var blog = PetContext.Blogs
                .Include(r => r.Class)
                .Include(r => r.OperationInfo.User)
                .SingleOrDefault(r => r.Id == id);
            if (blog == null)
                return new IsSuccessResult<GetBlog>("找不到此文章");

            return new IsSuccessResult<GetBlog>
            {
                ReturnObject = new GetBlog
                {
                    Title = blog.Title,
                    Message = blog.Message,
                    ClassId = blog.ClassId,
                    Class = blog.Class.Word,
                    AnimalId = blog.AnimalId,
                    Date = blog.OperationInfo.Date.ToString() + " UTC",
                    UserDisplay = blog.OperationInfo.User.Display,
                    Animal = blog.AnimalId.HasValue ? blog.Animal.Title : null
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
        public BlogMessageList GetMessageList(int id, int page = 1, int take = 10)
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

            var list = temp.Select(r => new BlogMessageItem
            {
                Id = r.Id,
                Message = r.Message,
                Date = r.Date.ToString() + " UTC",
                Account = r.Account
            })
            .ToList();

            var count = messages.Count();
            return new BlogMessageList
            {
                List = list,
                Count = count
            };
        }

        /// <summary>
        /// 刪除部落格文章
        /// </summary>
        /// <param name="id">Blog.Id</param>
        /// <param name="userId">User.Id</param>
        /// <returns></returns>
        public IsSuccessResult DeleteBlog(int id, int userId)
        {
            var log = GetLogger();
            log.Debug("id: {0}, userId: {1}", id, userId);

            var result = new IsSuccessResult();
            var blog = PetContext.Blogs.SingleOrDefault(r => r.Id == id);
            if (blog == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此部落格文章";
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
            if (user.IsAdmin == false && blog.OperationInfo.UserId != userId)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "沒有權限";
                return result;
            }

            try
            {
                PetContext.Messages.RemoveRange(blog.Messages);
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
        /// <param name="id">Blog.Id</param>
        /// <param name="messageId">Message.Id</param>
        /// <param name="userId">User.Id</param>
        /// <returns></returns>
        public IsSuccessResult DeleteMessage(int id, int messageId, int userId)
        {
            var log = GetLogger();
            log.Debug("id: {0}, messageId: {1}, userId: {2}", id, messageId, userId);

            var result = new IsSuccessResult();

            var blog = PetContext.Blogs.SingleOrDefault(r => r.Id == id);
            if (blog == null)
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
        /// 新增Blog文章
        /// </summary>
        /// <param name="data">文章資訊</param>
        /// <returns></returns>
        public IsSuccessResult<BlogItem> AddBlog(CreateBlog data)
        {
            var log = GetLogger();
            log.Debug("title: {0}, message:{1}, animaltitle:{2}, classId:{3}",
                data.Title, data.Message, data.AnimalTitle, data.ClassId);

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

            int? animalId = 0;
            if (string.IsNullOrWhiteSpace(data.AnimalTitle) == false)
            {
                data.AnimalTitle = data.AnimalTitle.Trim();
                animalId = PetContext.Animals
                    .Where(r => r.Title == data.AnimalTitle)
                    .Select(r => r.Id)
                    .SingleOrDefault();
                if (animalId == 0)
                    return new IsSuccessResult<BlogItem>("請輸入正確的動物文章標題");
            }

            try
            {
                var blog = PetContext.Blogs.Add(new Blog
                {
                    Title = data.Title,
                    Message = data.Message,
                    AnimalId = animalId == 0 ? null : animalId,
                    ClassId = data.ClassId,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.UtcNow,
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
        public IsSuccessResult EditBlog(EditBlog data, int userId)
        {
            var log = GetLogger();
            log.Debug("title: {0}, message: {1}, classId: {2}, animalId: {3}, id: {4}, userId: {5}",
                data.Title, data.Message, data.ClassId, data.AnimalId, data.Id, userId);

            var blog = PetContext.Blogs.SingleOrDefault(r => r.Id == data.Id);
            if (blog == null)
                return new IsSuccessResult("找不到此文章");

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
            if (user.IsAdmin == false && blog.OperationInfo.UserId != userId)
                return new IsSuccessResult("沒有權限");

            var isAny = PetContext.Blogs.Any(r => r.Title == data.Title && r.Id != data.Id);
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

        /// <summary>
        /// 留言
        /// </summary>
        /// <param name="id">Blog.Id</param>
        /// <param name="message">留言內容</param>
        /// <returns></returns>
        public IsSuccessResult AddMessage(int id, string message)
        {
            var log = GetLogger();
            log.Debug("id: {0}, message: {1}", id, message);

            if (string.IsNullOrWhiteSpace(message))
                return new IsSuccessResult("請輸入留言");
            message = message.Trim();

            var blog = PetContext.Blogs.SingleOrDefault(r => r.Id == id);
            if (blog == null)
                return new IsSuccessResult("找不到此文章，暫時無法留言");

            var userId = GetOperationInfo().UserId;
            if (userId == 0)
                return new IsSuccessResult("請先登入後再進行留言");

            try
            {
                blog.Messages.Add(new Message
                {
                    Message1 = message,
                    IsRead = false,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.UtcNow,
                        UserId = userId
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