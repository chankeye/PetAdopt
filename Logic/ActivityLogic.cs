using System.IO;
using PetAdopt.DTO;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
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
        public ActivityList GetActivities(int page)
        {
            var log = GetLogger();
            log.Debug("page:{0}", page);

            if (page <= 0)
                page = 1;

            var list = PetContext.Activities
                .Select(r => new ActivityItem
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();

            var count = PetContext.Activities.Count();
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
            if (string.IsNullOrWhiteSpace(activitiy.CoverPoto) == false)
            {
                File.Delete(path + "//" + activitiy.CoverPoto);
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
                    Poto = activity.CoverPoto,
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
            log.Debug("poto: {0}, title: {1}, message:{2}, areaId:{3}, address:{4}", 
                data.Poto, data.Title, data.Message, data.AreaId, data.Address);

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<ActivityItem>("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult<ActivityItem>("請輸入內容");
            data.Message = data.Message.Trim();

            if (string.IsNullOrWhiteSpace(data.Poto) == false)
                data.Poto = data.Poto.Trim();

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
                    CoverPoto = data.Poto,
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
            log.Debug("poto: {0}, title: {1}, message:{2}, areaId:{3}, address:{4}, id:{5}", data.Poto, data.Title, data.Message,
                data.AreaId, data.Address, id);

            var activity = PetContext.Activities.SingleOrDefault(r => r.Id == id);
            if (activity == null)
                return new IsSuccessResult("找不到此最新活動");

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult("請輸入內容");
            data.Message = data.Message.Trim();

            if (string.IsNullOrWhiteSpace(data.Poto) == false)
                data.Poto = data.Poto.Trim();

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

            if (activity.CoverPoto == data.Poto && activity.Title == data.Title && activity.Message == data.Message &&
                activity.Address == data.Address && activity.AreaId == data.AreaId)
            {
                return new IsSuccessResult();
            }

            try
            {
                activity.CoverPoto = data.Poto;
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