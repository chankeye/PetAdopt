using PetAdopt.DTO;
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
        public BlogList GetBlogList(int page)
        {
            var log = GetLogger();
            log.Debug("page:{0}", page);

            if (page <= 0)
                page = 1;

            var list = PetContext.Blogs
                .Select(r => new BlogItem
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();

            var count = PetContext.Blogs.Count();
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
    }
}