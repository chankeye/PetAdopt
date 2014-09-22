﻿using PetAdopt.DTO;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
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
        public List<AnimalItem> GetAnimalList()
        {
            var log = GetLogger();
            log.Debug("GetAnimalList in");

            var animallist = PetContext
                .Animals
                .Select(r => new AnimalItem
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToList();

            return animallist;
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
    }
}