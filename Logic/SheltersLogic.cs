using PetAdopt.DTO;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
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
        public List<SheltersItem> GetSheltersList()
        {
            var log = GetLogger();
            log.Debug("GetSheltersList in");

            var shelterslist = PetContext
                .Shelters
                .Select(r => new SheltersItem
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToList();

            return shelterslist;
        }

        /// <summary>
        /// 刪除收容所
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteShelters(int id)
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
    }
}