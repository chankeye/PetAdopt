﻿using PetAdopt.DTO;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
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
        public List<BlogItem> GetBlogList()
        {
            var log = GetLogger();
            log.Debug("GetBlogList in");

            var bloglist = PetContext
                .Blogs
                .Select(r => new BlogItem
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .ToList();

            return bloglist;
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
    }
}