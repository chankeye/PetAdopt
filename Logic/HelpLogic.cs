﻿using PetAdopt.DTO;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetAdopt.Logic
{
    public class HelpLogic : _BaseLogic
    {
        /// <summary>
        /// 即刻救援Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public HelpLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得即刻救援列表
        /// </summary>
        /// <returns></returns>
        public List<HelpItem> GetHelpList()
        {
            var log = GetLogger();
            log.Debug("GetHelpList in");

            var helplist = PetContext
                .Helps
                .Select(r => new HelpItem
                {
                    Id = r.Id,
                    Title = r.Title
                })
                .ToList();

            return helplist;
        }

        /// <summary>
        /// 刪除即刻救援
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteHelp(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var help = PetContext.Helps.SingleOrDefault(r => r.Id == id);
            if (help == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此救援文章";
                return result;
            }

            try
            {
                PetContext.Helps.Remove(help);
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