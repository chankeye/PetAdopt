using PetAdopt.DTO;
using PetAdopt.DTO.Animal;
using PetAdopt.Models;
using System;
using System.Linq;

namespace PetAdopt.Logic
{
    public class AnimalLogic : _BaseLogic
    {
        /// <summary>
        /// 認養動物Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public AnimalLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得動物列表
        /// </summary>
        /// <returns></returns>
        public AnimalList GetAnimalList(int page)
        {
            var log = GetLogger();
            log.Debug("page:{0}", page);

            var list = PetContext.Animals
                .Select(r => new AnimalItem
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();

            var count = PetContext.Animals.Count();
            var result = new AnimalList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 刪除動物
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteAnimal(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var animal = PetContext.Animals.SingleOrDefault(r => r.Id == id);
            if (animal == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此動物資訊";
                return result;
            }

            try
            {
                PetContext.Animals.Remove(animal);
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
        /// 新增認養動物
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<AnimalItem> AddAnimal(CreateAnimal data)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, title: {1}, introduction:{2}, areaId:{3}, address:{4}, phone:{5}, classId:{6}, sheltersId:{7}, startDate:{8}, endDate:{9}, statusId:{10}, age:{11}",
                data.Photo, data.Title, data.Introduction, data.AreaId, data.Address, data.Phone, data.ClassId, data.SheltersId, data.StartDate, data.EndDate, data.StartDate, data.Age);

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<AnimalItem>("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Introduction))
                return new IsSuccessResult<AnimalItem>("請輸入介紹");
            data.Introduction = data.Introduction.Trim();

            if (data.EndDate.HasValue)
            {
                if(data.EndDate < data.StartDate)
                    return new IsSuccessResult<AnimalItem>("安樂死日期不得在認養日期之前");
            }

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult<AnimalItem>("請選擇正確的分類");

            var hasStatus = PetContext.Status.Any(r => r.Id == data.StatusId);
            if (hasStatus == false)
                return new IsSuccessResult<AnimalItem>("請選擇正確的狀態");

            if (data.SheltersId.HasValue)
            {
                var hasShelters = PetContext.Shelters.Any(r => r.Id == data.SheltersId);
                if (hasShelters == false)
                    return new IsSuccessResult<AnimalItem>("找不到此收容所編號");
            }
            else
            {
                if(data.AreaId.HasValue == false)
                    return new IsSuccessResult<AnimalItem>("請選擇地區");
                else
                {
                    var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
                    if (hasArea == false)
                        return new IsSuccessResult<AnimalItem>("請選擇正確的地區");
                }

                if(string.IsNullOrWhiteSpace(data.Phone))
                    return new IsSuccessResult<AnimalItem>("請輸入電話");
                data.Phone = data.Phone.Trim();

                if (string.IsNullOrWhiteSpace(data.Address))
                    return new IsSuccessResult<AnimalItem>("請輸入地址");
                data.Address = data.Address.Trim();
            }

            if (string.IsNullOrWhiteSpace(data.Photo) == false)
                data.Photo = data.Photo.Trim();

            var isAny = PetContext.Animals.Any(r => r.Title == data.Title);
            if (isAny)
                return new IsSuccessResult<AnimalItem>(string.Format("已經有 {0} 這個認養資訊了", data.Title));

            try
            {
                var activity = PetContext.Animals.Add(new Animal
                {
                    CoverPhoto = data.Photo,
                    Title = data.Title,
                    Introduction = data.Introduction,
                    Address = data.Address,
                    AreaId = data.AreaId,
                    ClassId = data.ClassId,
                    SheltersId = data.SheltersId,
                    Phone = data.Phone,
                    StartDate = data.StartDate,
                    EndDate = data.EndDate,
                    Age = data.Age,
                    StatusId = data.StatusId,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.Now,
                        UserId = GetOperationInfo().UserId
                    }
                });
                PetContext.SaveChanges();

                return new IsSuccessResult<AnimalItem>
                {
                    ReturnObject = new AnimalItem
                    {
                        Id = activity.Id,
                        Title = activity.Title,
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult<AnimalItem>("發生不明錯誤，請稍候再試");
            }
        }
    }
}