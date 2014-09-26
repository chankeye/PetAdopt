﻿using System.Collections.Generic;
using PetAdopt.DTO;
using PetAdopt.DTO.Activity;
using PetAdopt.Models;
using System;
using System.IO;
using System.Linq;

namespace PetAdopt.Logic
{
    public class ActivityLogic : _BaseLogic
    {
        /// <summary>
        /// 最新活動Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public ActivityLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得最新活動列表
        /// </summary>
        /// <returns></returns>
        public ActivityList GetActivities(int page = 1, int take = 10, string query = "", bool isLike = true)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}", page, take, query, isLike);

            if (page < 1)
                page = 1;

            if (take < 1)
                take = 10;

            List<ActivityItem> list;
            var count = 0;

            // 查全部
            if (string.IsNullOrWhiteSpace(query))
            {
                var activities = PetContext.Activities
                    .Select(r => new
                    {
                        r.Id,
                        r.Title,
                        r.OperationInfo.Date
                    });

                var templist = activities
                    .OrderByDescending(r => r.Id)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                list = templist
                    .Select(r => new ActivityItem
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Date = r.Date.ToString("yyyy/MM/dd")
                    })
                    .ToList();

                count = activities.Count();
            }
            // 查特定標題
            else
            {
                // 查完全命中的
                if (isLike == false)
                {
                    var activities = PetContext.Activities
                        .Where(r => r.Title == query)
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo.Date
                        });

                    var templist = activities
                        .OrderByDescending(r => r.Id)
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new ActivityItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString("yyyy/MM/dd")
                        })
                        .ToList();

                    count = activities.Count();
                }
                // 查包含的
                else
                {
                    var activities = PetContext.Activities
                        .Where(r => r.Title.Contains(query))
                        .Select(r => new
                        {
                            r.Id,
                            r.Title,
                            r.OperationInfo.Date
                        });

                    var templist = activities
                        .OrderByDescending(r => r.Id)
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new ActivityItem
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Date = r.Date.ToString("yyyy/MM/dd")
                        })
                        .ToList();

                    count = activities.Count();
                }
            }

            var result = new ActivityList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 刪除最新活動
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteActivity(string path, int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var activitiy = PetContext.Activities.SingleOrDefault(r => r.Id == id);
            if (activitiy == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此活動";
                return result;
            }

            // 有上傳圖片，就把圖片刪掉
            if (string.IsNullOrWhiteSpace(activitiy.CoverPhoto) == false)
            {
                File.Delete(path + "//" + activitiy.CoverPhoto);
            }

            try
            {
                PetContext.Activities.Remove(activitiy);
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
        /// 取得最新活動
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<CreateActivity> GetActivity(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var activity = PetContext.Activities.SingleOrDefault(r => r.Id == id);
            if (activity == null)
                return new IsSuccessResult<CreateActivity>("找不到此活動");

            return new IsSuccessResult<CreateActivity>
            {
                ReturnObject = new CreateActivity
                {
                    Photo = activity.CoverPhoto,
                    Title = activity.Title,
                    Message = activity.Message,
                    AreaId = activity.AreaId,
                    Address = activity.Address
                }
            };
        }

        /// <summary>
        /// 新增最新活動
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<ActivityItem> AddActivity(CreateActivity data)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, title: {1}, message:{2}, areaId:{3}, address:{4}",
                data.Photo, data.Title, data.Message, data.AreaId, data.Address);

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<ActivityItem>("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult<ActivityItem>("請輸入內容");
            data.Message = data.Message.Trim();

            if (string.IsNullOrWhiteSpace(data.Photo) == false)
                data.Photo = data.Photo.Trim();

            var isAny = PetContext.Activities.Any(r => r.Title == data.Title);
            if (isAny)
                return new IsSuccessResult<ActivityItem>(string.Format("已經有 {0} 這個最新消息了", data.Title));

            if (data.AreaId.HasValue)
            {
                var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
                if (hasArea == false)
                    return new IsSuccessResult<ActivityItem>("請選擇正確的地區");
            }

            if (string.IsNullOrWhiteSpace(data.Address) == false)
                data.Address = data.Address.Trim();

            try
            {
                var activity = PetContext.Activities.Add(new Activity
                {
                    CoverPhoto = data.Photo,
                    Title = data.Title,
                    Message = data.Message,
                    Address = data.Address,
                    AreaId = data.AreaId,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.Now,
                        UserId = GetOperationInfo().UserId
                    }
                });
                PetContext.SaveChanges();

                return new IsSuccessResult<ActivityItem>
                {
                    ReturnObject = new ActivityItem
                    {
                        Id = activity.Id,
                        Title = activity.Title,
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult<ActivityItem>("發生不明錯誤，請稍候再試");
            }
        }

        /// <summary>
        /// 修改最新活動
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult EditActivity(int id, CreateActivity data)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, title: {1}, message:{2}, areaId:{3}, address:{4}, id:{5}",
                data.Photo, data.Title, data.Message, data.AreaId, data.Address, id);

            var activity = PetContext.Activities.SingleOrDefault(r => r.Id == id);
            if (activity == null)
                return new IsSuccessResult("找不到此最新活動");

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult("請輸入內容");
            data.Message = data.Message.Trim();

            if (string.IsNullOrWhiteSpace(data.Photo) == false)
                data.Photo = data.Photo.Trim();

            var isAny = PetContext.Activities.Any(r => r.Title == data.Title && r.Id != id);
            if (isAny)
                return new IsSuccessResult(string.Format("已經有 {0} 這個最新消息了", data.Title));

            if (data.AreaId.HasValue)
            {
                var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
                if (hasArea == false)
                    return new IsSuccessResult("請選擇正確的地區");
            }

            if (string.IsNullOrWhiteSpace(data.Address) == false)
                data.Address = data.Address.Trim();

            if (activity.CoverPhoto == data.Photo && activity.Title == data.Title && activity.Message == data.Message &&
                activity.Address == data.Address && activity.AreaId == data.AreaId)
            {
                return new IsSuccessResult();
            }

            try
            {
                activity.CoverPhoto = data.Photo;
                activity.Title = data.Title;
                activity.Message = data.Message;
                activity.Address = data.Address;
                activity.AreaId = data.AreaId;

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