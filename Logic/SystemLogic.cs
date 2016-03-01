using System;
using PetAdopt.DTO;
using PetAdopt.DTO.System;
using PetAdopt.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PetAdopt.Logic
{
    public class SystemLogic : _BaseLogic
    {
        /// <summary>
        /// 系統參數Logic
        /// </summary>
        /// <param name="operation">操作者資訊</param>
        public SystemLogic(Operation operation) : base(operation) { }

        /// <summary>
        /// 取得地區列表
        /// </summary>
        /// <returns></returns>
        public List<AreaItem> GetAreaList()
        {
            var log = GetLogger();
            log.Debug("GetAreaList in");

            var areas = PetContext
                .Areas
                .Select(r => new AreaItem
                {
                    Id = r.Id,
                    Word = r.Word
                })
                .ToList();

            return areas;
        }

        /// <summary>
        /// 刪除地區
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteArea(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var area = PetContext.Areas.SingleOrDefault(r => r.Id == id);
            if (area == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此區域";
                return result;
            }

            try
            {
                PetContext.Areas.Remove(area);
                PetContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);

                result.IsSuccess = false;
                result.ErrorMessage = "已在使用中，無法刪除";
                return result;
            }

        }

        /// <summary>
        /// 新增地區
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<AreaItem> AddArea(string word, int id = 0)
        {
            var log = GetLogger();
            log.Debug("word: {0}", word);

            var result = new IsSuccessResult<AreaItem>();

            if (string.IsNullOrWhiteSpace(word))
                return new IsSuccessResult<AreaItem>("區域請勿傳入空值");
            word = word.Trim();

            var isAny = PetContext.Areas.Any(r => r.Word == word);
            if (isAny)
                return new IsSuccessResult<AreaItem>(string.Format("已經有 {0} 這個區域了", word));

            if (id == 0)
                id = PetContext.Areas.Select(r => r.Id).OrderByDescending(r => r).FirstOrDefault() + 1;

            var area = PetContext.Areas.Add(new Area
            {
                Id = id,
                Word = word
            });
            PetContext.SaveChanges();

            return new IsSuccessResult<AreaItem>
            {
                ReturnObject = new AreaItem
                {
                    Id = area.Id,
                    Word = area.Word
                }
            };
        }

        /// <summary>
        /// 取得狀態列表
        /// </summary>
        /// <returns></returns>
        public List<StatusItem> GetStatusList()
        {
            var log = GetLogger();
            log.Debug("GetStatusList in");

            var statuses = PetContext
                .Status
                .Select(r => new StatusItem
                {
                    Id = r.Id,
                    Word = r.Word
                })
                .ToList();

            return statuses;
        }

        /// <summary>
        /// 刪除狀態
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteStatus(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var status = PetContext.Status.SingleOrDefault(r => r.Id == id);
            if (status == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此送養狀態";
                return result;
            }

            try
            {
                PetContext.Status.Remove(status);
                PetContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);

                result.IsSuccess = false;
                result.ErrorMessage = "已在使用中，無法刪除";
                return result;
            }
        }

        /// <summary>
        /// 新增狀態
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<StatusItem> AddStatus(string word)
        {
            var log = GetLogger();
            log.Debug("word: {0}", word);

            var result = new IsSuccessResult<StatusItem>();

            if (string.IsNullOrWhiteSpace(word))
                return new IsSuccessResult<StatusItem>("送養狀態請勿傳入空值");
            word = word.Trim();

            var isAny = PetContext.Status.Any(r => r.Word == word);
            if (isAny)
                return new IsSuccessResult<StatusItem>(string.Format("已經有 {0} 這個送養狀態了", word));

            var status = PetContext.Status.Add(new Status
            {
                Word = word
            });
            PetContext.SaveChanges();

            return new IsSuccessResult<StatusItem>
            {
                ReturnObject = new StatusItem
                {
                    Id = status.Id,
                    Word = status.Word
                }
            };
        }

        /// <summary>
        /// 取得種類列表
        /// </summary>
        /// <returns></returns>
        public List<ClassItem> GetClassList()
        {
            var log = GetLogger();
            log.Debug("GetClassList in");

            var classes = PetContext
                .Classes
                .Select(r => new ClassItem
                {
                    Id = r.Id,
                    Word = r.Word
                })
                .ToList();

            return classes;
        }

        /// <summary>
        /// 刪除種類
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult DeleteClass(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var result = new IsSuccessResult();
            var clas = PetContext.Classes.SingleOrDefault(r => r.Id == id);
            if (clas == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此動物種類";
                return result;
            }

            try
            {
                PetContext.Classes.Remove(clas);
                PetContext.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);

                result.IsSuccess = false;
                result.ErrorMessage = "已在使用中，無法刪除";
                return result;
            }
        }

        /// <summary>
        /// 新增種類
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult<ClassItem> AddClass(string word)
        {
            var log = GetLogger();
            log.Debug("word: {0}", word);

            var result = new IsSuccessResult<ClassItem>();

            if (string.IsNullOrWhiteSpace(word))
                return new IsSuccessResult<ClassItem>("動物種類請勿傳入空值");
            word = word.Trim();

            var isAny = PetContext.Classes.Any(r => r.Word == word);
            if (isAny)
                return new IsSuccessResult<ClassItem>(string.Format("已經有 {0} 這個動物種類了", word));

            var clas = PetContext.Classes.Add(new Class
            {
                Word = word
            });
            PetContext.SaveChanges();

            return new IsSuccessResult<ClassItem>
            {
                ReturnObject = new ClassItem
                {
                    Id = clas.Id,
                    Word = clas.Word
                }
            };
        }

        /// <summary>
        /// 把圖片刪掉
        /// </summary>
        /// <param name="path">根目錄</param>
        /// <param name="photo">圖片名</param>
        public void DeletePhoto(string path, string photo)
        {
            File.Delete(path + "//" + photo);
        }

        public void InitSystemParameter()
        {
            AddArea("臺北市", 2);
            AddArea("新北市", 3);
            AddArea("基隆市", 4);
            AddArea("宜蘭縣", 5);
            AddArea("桃園縣", 6);
            AddArea("新竹縣", 7);
            AddArea("新竹市", 8);
            AddArea("苗栗縣", 9);
            AddArea("臺中市", 10);
            AddArea("彰化縣", 11);
            AddArea("南投縣", 12);
            AddArea("雲林縣", 13);
            AddArea("嘉義縣", 14);
            AddArea("嘉義市", 15);
            AddArea("臺南市", 16);
            AddArea("高雄市", 17);
            AddArea("屏東縣", 18);
            AddArea("花蓮縣", 19);
            AddArea("臺東縣", 20);
            AddArea("澎湖縣", 21);
            AddArea("金門縣", 22);
            AddArea("連江縣", 23);

            foreach (var item in Utilities.Constant.InitClasses)
                AddClass(item);

            foreach (var item in Utilities.Constant.InitStatuses)
                AddStatus(item);
        }

        public SiteInfo GetSiteInformation()
        {
            var result = new SiteInfo()
            {
                ActivityCount = PetContext.Activities.Count(),
                AnimalCount = PetContext.Animals.Count(),
                AskCount = PetContext.Asks.Count(),
                BlogCount = PetContext.Blogs.Count(),
                HelpCount = PetContext.Helps.Count(),
                KnowledgeCount = PetContext.Knowledges.Count(),
                NewsCount = PetContext.News.Count(),
                ShelterCount = PetContext.Shelters.Count(),
                MemberCount = PetContext.Users.Count()
            };

            return result;
        }
    }
}