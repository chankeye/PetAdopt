﻿using PetAdopt.DTO;
using PetAdopt.DTO.Animal;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace PetAdopt.Logic
{
    /// <summary>
    /// 動物Logic
    /// </summary>
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
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆</param>
        /// <param name="query">標題查詢</param>
        /// <param name="isLike">模糊比對</param>
        /// <param name="areaId">Area.Id</param>
        /// <param name="classId">Class.Id</param>
        /// <param name="statusId">Status.Id</param>
        /// <param name="userId">User.Id</param>
        /// <returns></returns>
        public AnimalList GetAnimalList(int page = 1, int take = 10, string query = "", bool isLike = true, int areaId = -1, int classId = -1, int statusId = -1, int userId = -1)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}, areaId={4}, classId={5}, statusId={6}, userId={7}", page, take, query, isLike, areaId, classId, statusId, userId);

            if (page < 1)
                page = 1;

            if (take < 1)
                take = 10;

            List<AnimalItem> list;
            var count = 0;

            // 查全部
            if (string.IsNullOrWhiteSpace(query))
            {
                var animals = PetContext.Animals
                    .Select(r => new
                    {
                        r.Id,
                        r.CoverPhoto,
                        r.Title,
                        r.Introduction,
                        r.OperationInfo,
                        r.Area,
                        r.Class,
                        r.Status
                    });

                // 指定誰發佈的
                if (userId != -1)
                {
                    animals = animals
                        .Where(r => r.OperationInfo.UserId == userId);
                }

                // 指定地區
                if (areaId != -1)
                {
                    animals = animals
                        .Where(r => r.Area.Id == areaId);
                }

                // 指定分類
                if (classId != -1)
                {
                    animals = animals
                        .Where(r => r.Class.Id == classId);
                }

                // 指定狀態
                if (statusId != -1)
                {
                    animals = animals
                        .Where(r => r.Status.Id == statusId);
                }

                var templist = animals
                    .OrderByDescending(r => r.Id)
                    .Select(r => new
                    {
                        r.Id,
                        r.CoverPhoto,
                        r.Title,
                        r.Introduction,
                        r.OperationInfo.Date,
                        Area = r.Area.Word,
                        Classes = r.Class.Word,
                        Status = r.Status.Word
                    })
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                list = templist
                    .Select(r => new AnimalItem
                    {
                        Id = r.Id,
                        Photo = r.CoverPhoto,
                        Title = r.Title,
                        Introduction = r.Introduction,
                        Date = r.Date.ToString("yyyy/MM/dd"),
                        Area = r.Area,
                        Classes = r.Classes,
                        Status = r.Status
                    })
                    .ToList();

                count = animals.Count();
            }
            // 查特定標題
            else
            {
                // 查完全命中的
                if (isLike == false)
                {
                    var animals = PetContext.Animals
                        .Where(r => r.Title == query)
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Introduction,
                            r.OperationInfo,
                            r.Area,
                            r.Class,
                            r.Status
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        animals = animals
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    // 指定地區
                    if (areaId != -1)
                    {
                        animals = animals
                            .Where(r => r.Area.Id == areaId);
                    }

                    // 指定分類
                    if (classId != -1)
                    {
                        animals = animals
                            .Where(r => r.Class.Id == classId);
                    }

                    // 指定狀態
                    if (statusId != -1)
                    {
                        animals = animals
                            .Where(r => r.Status.Id == statusId);
                    }

                    var templist = animals
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Introduction,
                            r.OperationInfo.Date,
                            Area = r.Area.Word,
                            Classes = r.Class.Word,
                            Status = r.Status.Word
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new AnimalItem
                        {
                            Id = r.Id,
                            Photo = r.CoverPhoto,
                            Title = r.Title,
                            Introduction = r.Introduction,
                            Date = r.Date.ToString("yyyy/MM/dd"),
                            Area = r.Area,
                            Classes = r.Classes,
                            Status = r.Status
                        })
                        .ToList();

                    count = animals.Count();
                }
                // 查包含的
                else
                {
                    var animals = PetContext.Animals
                        .Where(r => r.Title.Contains(query))
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Introduction,
                            r.OperationInfo,
                            r.Area,
                            r.Class,
                            r.Status
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        animals = animals
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    // 指定地區
                    if (areaId != -1)
                    {
                        animals = animals
                            .Where(r => r.Area.Id == areaId);
                    }

                    // 指定分類
                    if (classId != -1)
                    {
                        animals = animals
                            .Where(r => r.Class.Id == classId);
                    }

                    // 指定狀態
                    if (statusId != -1)
                    {
                        animals = animals
                            .Where(r => r.Status.Id == statusId);
                    }


                    var templist = animals
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.CoverPhoto,
                            r.Title,
                            r.Introduction,
                            r.OperationInfo.Date,
                            Area = r.Area.Word,
                            Classes = r.Class.Word,
                            Status = r.Status.Word
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new AnimalItem
                        {
                            Id = r.Id,
                            Photo = r.CoverPhoto,
                            Title = r.Title,
                            Introduction = r.Introduction,
                            Date = r.Date.ToString("yyyy/MM/dd"),
                            Area = r.Area,
                            Classes = r.Classes,
                            Status = r.Status
                        })
                        .ToList();

                    count = animals.Count();
                }
            }

            var result = new AnimalList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 取得動物資訊
        /// </summary>
        /// <param name="id">Animal.Id</param>
        /// <returns></returns>
        public IsSuccessResult<GetAnimal> GetAnimal(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var animal = PetContext.Animals
                .Include(r => r.Class)
                .Include(r => r.Status)
                .Include(r => r.Area)
                .Include(r => r.OperationInfo.User)
                .SingleOrDefault(r => r.Id == id);

            if (animal == null)
                return new IsSuccessResult<GetAnimal>("找不到此認養動物資訊");

            return new IsSuccessResult<GetAnimal>
            {
                ReturnObject = new GetAnimal
                {
                    Photo = animal.CoverPhoto,
                    Title = animal.Title,
                    Introduction = animal.Introduction.Replace("\n", "<br>"),
                    Address = animal.Address,
                    AreaId = animal.AreaId,
                    ClassId = animal.ClassId,
                    SheltersId = animal.SheltersId,
                    StatusId = animal.StatusId,
                    Area = animal.AreaId.HasValue ? animal.Area.Word : null,
                    Class = animal.Class.Word,
                    Shelters = animal.SheltersId.HasValue ? animal.Shelter.Name : null,
                    Status = animal.Status.Word,
                    Phone = animal.Phone,
                    StartDate = animal.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = animal.EndDate.HasValue ? animal.EndDate.Value.ToString("yyyy-MM-dd") : null,
                    Age = animal.Age,
                    Date = animal.OperationInfo.Date.ToString("yyyy-MM-dd"),
                    UserDisplay = animal.OperationInfo.User.Display
                }
            };
        }

        /// <summary>
        /// 取得留言列表
        /// </summary>
        /// <param name="id">Animal.Id</param>
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆資料</param>
        /// <returns></returns>
        public AnimalMessageList GetMessageList(int id, int page = 1, int take = 10)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, id:{2}", page, take, id);

            if (page <= 0)
                page = 1;

            if (take < 1)
                take = 10;

            var messages = PetContext.Animals
                .Where(r => r.Id == id)
                .SelectMany(r => r.Messages);

            var temp = messages.Select(r => new
            {
                r.Id,
                Message = r.Message1,
                r.OperationInfo.Date,
                r.OperationInfo.User.Account
            })
            .OrderByDescending(r => r.Id)
            .Skip((page - 1) * take)
            .Take(take)
            .ToList();

            var list = temp.Select(r => new AnimalMessageItem
            {
                Id = r.Id,
                Message = r.Message,
                Date = r.Date.ToString("yyyy-MM-dd"),
                Account = r.Account
            })
            .ToList();

            var count = messages.Count();
            return new AnimalMessageList
            {
                List = list,
                Count = count
            };
        }

        /// <summary>
        /// 刪除動物
        /// </summary>
        /// <param name="path">圖片路徑</param>
        /// <param name="id">Animal.Id</param>
        /// <param name="userId">User.Id</param>
        /// <returns></returns>
        public IsSuccessResult DeleteAnimal(string path, int id, int userId)
        {
            var log = GetLogger();
            log.Debug("path: {0}, id: {1}, userId: {2}", path, id, userId);

            var result = new IsSuccessResult();
            var animal = PetContext.Animals.SingleOrDefault(r => r.Id == id);
            if (animal == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此動物資訊";
                return result;
            }

            //檢查權限
            var user = PetContext.Users.SingleOrDefault(r => r.Id == userId);
            if (user == null || user.IsDisable)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "沒有權限";
                return result;
            }
            if (user.IsAdmin == false && animal.OperationInfo.UserId != userId)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "沒有權限";
                return result;
            }

            if (animal.Blogs.Any())
            {
                result.IsSuccess = false;
                result.ErrorMessage = "有關於此動物的文章，無法刪除";
                return result;
            }

            // 有上傳圖片，就把圖片刪掉
            if (string.IsNullOrWhiteSpace(animal.CoverPhoto) == false)
            {
                File.Delete(path + "//" + animal.CoverPhoto);
            }

            try
            {
                PetContext.Messages.RemoveRange(animal.Messages);
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
        /// 刪除留言
        /// </summary>
        /// <param name="id">Animal.Id</param>
        /// <param name="messageId">Message.Id</param>
        /// <param name="userId">User.Id</param>
        /// <returns></returns>
        public IsSuccessResult DeleteMessage(int id, int messageId, int userId)
        {
            var log = GetLogger();
            log.Debug("id: {0}, messageId: {1}, userId: {2}", id, messageId, userId);

            var result = new IsSuccessResult();

            var animal = PetContext.Animals.SingleOrDefault(r => r.Id == id);
            if (animal == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此問與答";
                return result;
            }

            //檢查權限
            var user = PetContext.Users.SingleOrDefault(r => r.Id == userId);
            if (user == null || user.IsDisable)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "沒有權限";
                return result;
            }
            if (user.IsAdmin == false)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "沒有權限";
                return result;
            }

            var message = animal.Messages.SingleOrDefault(r => r.Id == messageId);
            if (message == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此留言";
                return result;
            }

            try
            {
                PetContext.Messages.Remove(message);
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
        /// <param name="data">動物資訊</param>
        /// <returns></returns>
        public IsSuccessResult<AnimalItem> AddAnimal(CreateAnimal data)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, title: {1}, introduction:{2}, areaId:{3}, address:{4}, phone:{5}, classId:{6}, shelters:{7}, startDate:{8}, endDate:{9}, statusId:{10}, age:{11}",
                data.Photo, data.Title, data.Introduction, data.AreaId, data.Address, data.Phone, data.ClassId, data.Shelters, data.StartDate, data.EndDate, data.StartDate, data.Age);

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<AnimalItem>("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Introduction))
                return new IsSuccessResult<AnimalItem>("請輸入介紹");
            data.Introduction = data.Introduction.Trim();

            if (data.EndDate.HasValue)
            {
                if (data.EndDate < data.StartDate)
                    return new IsSuccessResult<AnimalItem>("安樂死日期不得在認養日期之前");
            }

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult<AnimalItem>("請選擇正確的分類");

            var hasStatus = PetContext.Status.Any(r => r.Id == data.StatusId);
            if (hasStatus == false)
                return new IsSuccessResult<AnimalItem>("請選擇正確的狀態");

            int? sheltersId = 0;
            if (string.IsNullOrWhiteSpace(data.Shelters) == false)
            {
                data.Shelters = data.Shelters.Trim();
                sheltersId = PetContext.Shelters
                    .Where(r => r.Name == data.Shelters)
                    .Select(r => r.Id)
                    .SingleOrDefault();
                if (sheltersId == 0)
                    return new IsSuccessResult<AnimalItem>("找不到此收容所，請輸入正確名稱");
            }
            else
            {
                if (data.AreaId.HasValue == false)
                    return new IsSuccessResult<AnimalItem>("請選擇地區");
                else
                {
                    var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
                    if (hasArea == false)
                        return new IsSuccessResult<AnimalItem>("請選擇正確的地區");
                }

                if (string.IsNullOrWhiteSpace(data.Phone))
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
                var animal = PetContext.Animals.Add(new Animal
                {
                    CoverPhoto = data.Photo,
                    Title = data.Title,
                    Introduction = data.Introduction,
                    Address = data.Address,
                    AreaId = data.AreaId,
                    ClassId = data.ClassId,
                    SheltersId = sheltersId == 0 ? null : sheltersId,
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
                        Id = animal.Id,
                        Title = animal.Title,
                    }
                };
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult<AnimalItem>("發生不明錯誤，請稍候再試");
            }
        }

        /// <summary>
        /// 修改認養動物資訊
        /// </summary>
        /// <param name="data">修改動物資訊</param>
        /// <param name="userId">User.Id</param>
        /// <returns></returns>
        public IsSuccessResult EditAnimal(EditAnimal data, int userId)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, title: {1}, introduction: {2}, areaId: {3}, address: {4}, phone: {5}, classId: {6}, shelters: {7}, startDate: {8}, endDate: {9}, statusId: {10}, age: {11}, id: {12}, user: {13}",
                data.Photo, data.Title, data.Introduction, data.AreaId, data.Address, data.Phone, data.ClassId, data.Shelters, data.StartDate, data.EndDate, data.StartDate, data.Age, data.Id, userId);

            var animal = PetContext.Animals.SingleOrDefault(r => r.Id == data.Id);
            if (animal == null)
                return new IsSuccessResult("找不到此認養動物資訊");

            if (string.IsNullOrWhiteSpace(data.Title))
                return new IsSuccessResult<AnimalItem>("請輸入標題");
            data.Title = data.Title.Trim();

            if (string.IsNullOrWhiteSpace(data.Introduction))
                return new IsSuccessResult<AnimalItem>("請輸入介紹");
            data.Introduction = data.Introduction.Trim();

            //檢查權限
            var user = PetContext.Users.SingleOrDefault(r => r.Id == userId);
            if (user == null || user.IsDisable)
                return new IsSuccessResult<AnimalItem>("沒有權限");
            if (user.IsAdmin == false && animal.OperationInfo.UserId != userId)
                return new IsSuccessResult<AnimalItem>("沒有權限");

            if (data.EndDate.HasValue)
            {
                if (data.EndDate < data.StartDate)
                    return new IsSuccessResult<AnimalItem>("安樂死日期不得在認養日期之前");
            }

            var hasClass = PetContext.Classes.Any(r => r.Id == data.ClassId);
            if (hasClass == false)
                return new IsSuccessResult<AnimalItem>("請選擇正確的分類");

            var hasStatus = PetContext.Status.Any(r => r.Id == data.StatusId);
            if (hasStatus == false)
                return new IsSuccessResult<AnimalItem>("請選擇正確的狀態");

            int? sheltersId = 0;
            if (string.IsNullOrWhiteSpace(data.Shelters) == false)
            {
                data.Shelters = data.Shelters.Trim();
                sheltersId = PetContext.Shelters
                    .Where(r => r.Name == data.Shelters)
                    .Select(r => r.Id)
                    .SingleOrDefault();
                if (sheltersId == 0)
                    return new IsSuccessResult<AnimalItem>("找不到此收容所，請輸入正確名稱");
            }
            else
            {
                if (data.AreaId.HasValue == false)
                    return new IsSuccessResult<AnimalItem>("請選擇地區");
                else
                {
                    var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
                    if (hasArea == false)
                        return new IsSuccessResult<AnimalItem>("請選擇正確的地區");
                }

                if (string.IsNullOrWhiteSpace(data.Phone))
                    return new IsSuccessResult<AnimalItem>("請輸入電話");
                data.Phone = data.Phone.Trim();

                if (string.IsNullOrWhiteSpace(data.Address))
                    return new IsSuccessResult<AnimalItem>("請輸入地址");
                data.Address = data.Address.Trim();
            }

            if (string.IsNullOrWhiteSpace(data.Photo) == false)
                data.Photo = data.Photo.Trim();

            var isAny = PetContext.Animals.Any(r => r.Title == data.Title && r.Id != data.Id);
            if (isAny)
                return new IsSuccessResult<AnimalItem>(string.Format("已經有 {0} 這個認養資訊了", data.Title));

            try
            {
                animal.CoverPhoto = data.Photo;
                animal.Title = data.Title;
                animal.Introduction = data.Introduction;
                animal.Address = data.Address;
                animal.AreaId = data.AreaId;
                animal.ClassId = data.ClassId;
                animal.SheltersId = sheltersId == 0 ? null : sheltersId;
                animal.Phone = data.Phone;
                animal.StartDate = data.StartDate;
                animal.EndDate = data.EndDate;
                animal.Age = data.Age;
                animal.StatusId = data.StatusId;

                PetContext.SaveChanges();

                return new IsSuccessResult();
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult("發生不明錯誤，請稍候再試");
            }
        }

        /// <summary>
        /// 留言
        /// </summary>
        /// <param name="id">Animal.Id</param>
        /// <param name="message">留言內容</param>
        /// <returns></returns>
        public IsSuccessResult AddMessage(int id, string message)
        {
            var log = GetLogger();
            log.Debug("id: {0}, message: {1}", id, message);

            if (string.IsNullOrWhiteSpace(message))
                return new IsSuccessResult("請輸入留言");
            message = message.Trim();

            var animal = PetContext.Animals.SingleOrDefault(r => r.Id == id);
            if (animal == null)
                return new IsSuccessResult("找不到此待認養動物，暫時無法留言");

            try
            {
                animal.Messages.Add(new Message
                {
                    Message1 = message,
                    IsRead = false,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.Now,
                        UserId = GetOperationInfo().UserId
                    }
                });

                PetContext.SaveChanges();

                return new IsSuccessResult();
            }
            catch (Exception ex)
            {
                log.Error(ex);

                return new IsSuccessResult("發生不明錯誤，請稍候再試");
            }
        }

        /// <summary>
        /// 取得認養動物的自動完成
        /// </summary>
        /// <param name="title">認養文章標題</param>
        /// <returns></returns>
        public List<Suggestion> GetAnimalSuggestion(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return new List<Suggestion>();
            title = title.Trim();

            var animalList = PetContext.Animals
                .Where(r => r.Title.Contains(title))
                .Take(10)
                .Select(r => new Suggestion
                {
                    Display = r.Title,
                    Value = r.Id.ToString()
                })
                .ToList();

            return animalList;
        }

        /// <summary>
        /// 取得輪播的資訊
        /// </summary>
        /// <returns></returns>
        public List<Carousel> GetCarouselList()
        {
            var data = PetContext.Animals
                .OrderByDescending(r => r.EndDate)
                .Where(r => r.EndDate >= DateTime.Now)
                .Select(r => new
                {
                    r.Id,
                    r.CoverPhoto,
                    r.EndDate,
                    r.Title
                })
                .Take(5)
                .ToList();

            // 沒取到半比，就回傳一個空的list回去
            if (data.Any() == false)
                return new List<Carousel>();

            var result = data
                .Select(r => new Carousel
                {
                    Id = r.Id,
                    Photo = r.CoverPhoto,
                    EndDate = r.EndDate.HasValue ? r.EndDate.Value.ToString("yyyy-MM-dd") : null,
                    Title = r.Title,
                    Class = "item"
                })
                .ToList();


            result.First().Class += " active";
            return result;
        }
    }
}