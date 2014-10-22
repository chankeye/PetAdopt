using PetAdopt.DTO;
using PetAdopt.DTO.Shelters;
using PetAdopt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆資料</param>
        /// <param name="query">查詢條件(只能查標題)</param>
        /// <param name="isLike">非完全比對</param>
        /// <param name="userId">指定某user發佈的</param>
        /// <returns></returns>
        public SheltersList GetSheltersList(int page = 1, int take = 10, string query = "", bool isLike = true, int userId = -1)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}", page, take, query, isLike);

            if (page < 1)
                page = 1;

            if (take < 1)
                take = 10;

            List<SheltersItem> list;
            var count = 0;

            // 查全部
            if (string.IsNullOrWhiteSpace(query))
            {
                var shelterses = PetContext.Shelters
                    .Select(r => new
                    {
                        r.Id,
                        r.Name,
                        r.Area.Word,
                        r.OperationInfo
                    });

                // 指定誰發佈的
                if (userId != -1)
                {
                    shelterses = shelterses
                        .Where(r => r.OperationInfo.UserId == userId);
                }

                var templist = shelterses
                    .OrderByDescending(r => r.Id)
                    .Select(r => new
                    {
                        r.Id,
                        r.Name,
                        Area = r.Word,
                    })
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                list = templist
                    .Select(r => new SheltersItem
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Area = r.Area
                    })
                    .ToList();

                count = shelterses.Count();
            }
            // 查特定標題
            else
            {
                // 查完全命中的
                if (isLike == false)
                {
                    var shelterses = PetContext.Shelters
                        .Where(r => r.Name == query)
                        .Select(r => new
                        {
                            r.Id,
                            r.Name,
                            r.Area.Word,
                            r.OperationInfo
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        shelterses = shelterses
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    var templist = shelterses
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.Name,
                            Area = r.Word
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new SheltersItem
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Area = r.Area
                        })
                        .ToList();

                    count = shelterses.Count();
                }
                // 查包含的
                else
                {
                    var shelterses = PetContext.Shelters
                        .Where(r => r.Name.Contains(query))
                        .Select(r => new
                        {
                            r.Id,
                            r.Name,
                            r.Area.Word,
                            r.OperationInfo
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        shelterses = shelterses
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    var templist = shelterses
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.Name,
                            Area = r.Word
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new SheltersItem
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Area = r.Area
                        })
                        .ToList();

                    count = shelterses.Count();
                }
            }

            var result = new SheltersList
            {
                List = list,
                Count = count
            };

            return result;
        }

        /// <summary>
        /// 取得收容所資訊
        /// </summary>
        /// <param name="id">Shelters.Id</param>
        /// <returns></returns>
        public IsSuccessResult<GetShelters> GetShelters(int id)
        {
            var log = GetLogger();
            log.Debug("id: {0}", id);

            var shelters = PetContext.Shelters
                .Include(r => r.Area)
                .SingleOrDefault(r => r.Id == id);
            if (shelters == null)
                return new IsSuccessResult<GetShelters>("找不到此活動");

            return new IsSuccessResult<GetShelters>
            {
                ReturnObject = new GetShelters
                {
                    Photo = shelters.CoverPhoto,
                    Name = shelters.Name,
                    Introduction = shelters.Introduction,
                    AreaId = shelters.AreaId,
                    Area = shelters.Area.Word,
                    Address = shelters.Address,
                    Url = shelters.Url,
                    Phone = shelters.Phone
                }
            };
        }

        /// <summary>
        /// 取得留言列表
        /// </summary>
        /// <param name="id">Ask.Id</param>
        /// <param name="page">第幾頁(1是第一頁)</param>
        /// <param name="take">取幾筆資料</param>
        /// <returns></returns>
        public SheltersMessageList GetMessageList(int id, int page = 1, int take = 10)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, id:{2}", page, take, id);

            if (page <= 0)
                page = 1;

            if (take < 1)
                take = 10;

            var messages = PetContext.Shelters
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

            var list = temp.Select(r => new SheltersMessageItem
            {
                Id = r.Id,
                Message = r.Message,
                Date = r.Date.ToString("yyyy-MM-dd"),
                Account = r.Account
            })
            .ToList();

            var count = messages.Count();
            return new SheltersMessageList
            {
                List = list,
                Count = count
            };
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
        /// 刪除留言
        /// </summary>
        /// <param name="id">Shelters.id</param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public IsSuccessResult DeleteMessage(int id, int messageId)
        {
            var log = GetLogger();
            log.Debug("id:{0}, messageId:{1}", id, messageId);

            var result = new IsSuccessResult();

            var shelters = PetContext.Shelters.SingleOrDefault(r => r.Id == id);
            if (shelters == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此問與答";
                return result;
            }

            var message = shelters.Messages.SingleOrDefault(r => r.Id == messageId);
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

        /// <summary>
        /// 修改收容所資訊
        /// </summary>
        /// <returns></returns>
        public IsSuccessResult EditShelters(int id, CreateShelters data)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, name: {1}, intorduction:{2}, areaId:{3}, address:{4}, phone:{5}, url:{6}, id:{7}",
                data.Photo, data.Name, data.Introduction, data.AreaId, data.Address, data.Phone, data.Url, id);

            var shelters = PetContext.Shelters.SingleOrDefault(r => r.Id == id);
            if (shelters == null)
                return new IsSuccessResult("找不到此最新活動");

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

            var isAny = PetContext.Shelters.Any(r => r.Name == data.Name && r.Id != id);
            if (isAny)
                return new IsSuccessResult<SheltersItem>(string.Format("已經有 {0} 這個收容所資料了", data.Name));

            var hasArea = PetContext.Areas.Any(r => r.Id == data.AreaId);
            if (hasArea == false)
                return new IsSuccessResult<SheltersItem>("請選擇正確的地區");

            if (string.IsNullOrWhiteSpace(data.Url) == false)
                data.Url = data.Url.Trim();

            if (shelters.CoverPhoto == data.Photo && shelters.Name == data.Name && shelters.Introduction == data.Introduction &&
                shelters.Address == data.Address && shelters.AreaId == data.AreaId && shelters.Phone == data.Phone && shelters.Url == data.Url)
            {
                return new IsSuccessResult();
            }

            try
            {
                shelters.CoverPhoto = data.Photo;
                shelters.Name = data.Name;
                shelters.Introduction = data.Introduction;
                shelters.Address = data.Address;
                shelters.Url = data.Url;
                shelters.Phone = data.Phone;
                shelters.AreaId = data.AreaId;

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
        /// <param name="id">Shelters.Id</param>
        /// <param name="message">留言內容</param>
        /// <returns></returns>
        public IsSuccessResult AddMessage(int id, string message)
        {
            var log = GetLogger();
            log.Debug("id: {0}, message: {1}", id, message);

            if (string.IsNullOrWhiteSpace(message))
                return new IsSuccessResult("請輸入留言");
            message = message.Trim();

            var shelters = PetContext.Shelters.SingleOrDefault(r => r.Id == id);
            if (shelters == null)
                return new IsSuccessResult("找不到此活動，暫時無法留言");

            try
            {
                shelters.Messages.Add(new Message
                {
                    Message1 = message,
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
        /// 取得收容所的自動完成
        /// </summary>
        /// <param name="name">收容所名稱</param>
        /// <returns></returns>
        public List<Suggestion> GetSheltersSuggestion(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<Suggestion>();
            name = name.Trim();

            var sheltersList = PetContext.Shelters
                .Where(r => r.Name.Contains(name))
                .Take(10)
                .Select(r => new Suggestion
                {
                    Display = r.Name,
                    Value = r.Id.ToString()
                })
                .ToList();

            return sheltersList;
        }
    }
}