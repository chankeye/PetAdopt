using System.IO;
using PetAdopt.DTO;
using PetAdopt.DTO.Shelters;
using PetAdopt.Models;
using System;
using System.Linq;

namespace PetAdopt.Logic
{
    public class SheltersLogic : _BaseLogic
    {
        /// <summary>
        /// 收容所Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public SheltersLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得收容所列表
        /// </summary>
        /// <returns></returns>
        public SheltersList GetSheltersList(int page)
        {
            var log = GetLogger();
            log.Debug("page:{0}", page);

            if (page <= 0)
                page = 1;

            var list = PetContext.Shelters
                .Select(r => new SheltersItem
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();

            var count = PetContext.Shelters.Count();
            var result = new SheltersList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 刪除收容所
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteShelters(string path, int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var shelters = PetContext.Shelters.SingleOrDefault(r => r.Id == id);
            if (shelters == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此收容所資訊";
                return result;
            }

            // 有上傳圖片，就把圖片刪掉
            if (string.IsNullOrWhiteSpace(shelters.CoverPhoto) == false)
            {
                File.Delete(path + "//" + shelters.CoverPhoto);
            }

            try
            {
                PetContext.Shelters.Remove(shelters);
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
        /// 新增收容所
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<SheltersItem> AddShelters(CreateShelters data)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, name: {1}, intorduction:{2}, areaId:{3}, address:{4}, phone:{5}, url:{6}",
                data.Photo, data.Name, data.Introduction, data.AreaId, data.Address, data.Phone, data.Url);

            if (string.IsNullOrWhiteSpace(data.Name))
                return new IsSuccessResult<SheltersItem>("請輸入收容所名稱");
            data.Name = data.Name.Trim();

            if (string.IsNullOrWhiteSpace(data.Introduction))
                return new IsSuccessResult<SheltersItem>("請輸入收容所介紹");
            data.Introduction = data.Introduction.Trim();

            if (string.IsNullOrWhiteSpace(data.Address))
                return new IsSuccessResult<SheltersItem>("請輸入收容所地址");
            data.Address = data.Address.Trim();

            if (string.IsNullOrWhiteSpace(data.Phone))
                return new IsSuccessResult<SheltersItem>("請輸入收容所電話");
            data.Phone = data.Phone.Trim();

            if (string.IsNullOrWhiteSpace(data.Photo) == false)
                data.Photo = data.Photo.Trim();

            var isAny = PetContext.Shelters.Any(r => r.Name == data.Name);
            if (isAny)
                return new IsSuccessResult<SheltersItem>(string.Format("已經有 {0} 這個收容所資料了", data.Name));

            var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
            if (hasArea == false)
                return new IsSuccessResult<SheltersItem>("請選擇正確的地區");

            if (string.IsNullOrWhiteSpace(data.Url) == false)
                data.Url = data.Url.Trim();

            try
            {
                var shelters = PetContext.Shelters.Add(new Shelter
                {
                    CoverPhoto = data.Photo,
                    Name = data.Name,
                    Introduction = data.Introduction,
                    Address = data.Address,
                    AreaId = data.AreaId,
                    Phone = data.Phone,
                    Url = data.Url,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.Now,
                        UserId = GetOperationInfo().UserId
                    }
                });
                PetContext.SaveChanges();

                return new IsSuccessResult<SheltersItem>
                {
                    ReturnObject = new SheltersItem
                    {
                        Id = shelters.Id,
                        Name = shelters.Name,
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult<SheltersItem>("發生不明錯誤，請稍候再試");
            }
        }
    }
}