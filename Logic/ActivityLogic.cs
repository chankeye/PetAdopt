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
        public List<ActivityItem> GetActivities()
        {
            var log = GetLogger();
            log.Debug("GetActivities in");

            var activities = PetContext
                .Activities
                .Select(r => new ActivityItem
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .ToList();

            return activities;
        }

        /// <summary>
        /// 刪除最新活動
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteActivity(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var activitiy = PetContext
                .Activities
                .Where(r => r.Id == id)
                .SingleOrDefault();
            if (activitiy == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此活動";
                return result;
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
        /// 新增最新活動
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<ActivityItem> AddActivity(CreateActivity data)
        {
            var log = GetLogger();
            log.Debug("poto: {0}, title: {1}, message:{2}, areaId:{3}, address:{4}", data.Poto, data.Title, data.Message,
                data.AreaId, data.Address);

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<ActivityItem>("標題請勿傳入空值");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Message))
                return new IsSuccessResult<ActivityItem>("內容請勿傳入空值");
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
    }
}