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
        public SheltersList GetSheltersList(int page = 1, int take = 10, string query = "", bool isLike = true, int areaId = -1, int userId = -1)
        {
            var log = GetLogger();
            log.Debug("page:{0}, take:{1}, query={2}, isLike={3}, areaId={4}, userId={5}", page, take, query, isLike, areaId, userId);

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
                        r.Area,
                        r.Introduction,
                        r.OperationInfo
                    });

                // 指定誰發佈的
                if (userId != -1)
                {
                    shelterses = shelterses
                        .Where(r => r.OperationInfo.UserId == userId);
                }

                // 指定地區
                if (areaId != -1)
                {
                    shelterses = shelterses
                        .Where(r => r.Area.Id == areaId);
                }

                var templist = shelterses
                    .OrderByDescending(r => r.Id)
                    .Select(r => new
                    {
                        r.Id,
                        r.Name,
                        r.Introduction,
                        r.Area.Word
                    })
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                list = templist
                    .Select(r => new SheltersItem
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Introduction = r.Introduction,
                        Area = r.Word
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
                            r.Area,
                            r.Introduction,
                            r.OperationInfo
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        shelterses = shelterses
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    // 指定地區
                    if (areaId != -1)
                    {
                        shelterses = shelterses
                            .Where(r => r.Area.Id == areaId);
                    }

                    var templist = shelterses
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.Name,
                            r.Introduction,
                            r.Area.Word
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new SheltersItem
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Introduction = r.Introduction,
                            Area = r.Word
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
                            r.Area,
                            r.Introduction,
                            r.OperationInfo
                        });

                    // 指定誰發佈的
                    if (userId != -1)
                    {
                        shelterses = shelterses
                            .Where(r => r.OperationInfo.UserId == userId);
                    }

                    // 指定地區
                    if (areaId != -1)
                    {
                        shelterses = shelterses
                            .Where(r => r.Area.Id == areaId);
                    }

                    var templist = shelterses
                        .OrderByDescending(r => r.Id)
                        .Select(r => new
                        {
                            r.Id,
                            r.Name,
                            r.Introduction,
                            r.Area.Word
                        })
                        .Skip((page - 1) * take)
                        .Take(take)
                        .ToList();

                    list = templist
                        .Select(r => new SheltersItem
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Introduction = r.Introduction,
                            Area = r.Word
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
                .Include(r => r.OperationInfo.User)
                .SingleOrDefault(r => r.Id == id);
            if (shelters == null)
                return new IsSuccessResult<GetShelters>("找不到此收容所");

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
                    Phone = shelters.Phone,
                    Date = shelters.OperationInfo.Date.ToString("yyyy-MM-dd"),
                    UserDisplay = shelters.OperationInfo.User.Display
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
        public IsSuccessResult DeleteShelters(string path, int id, int userId)
        {
            var log = GetLogger();
            log.Debug("path: {0}, id: {1}, userId: {2}", path, id, userId);

            var result = new IsSuccessResult();
            var shelters = PetContext.Shelters
                .Include(r => r.Messages)
                .SingleOrDefault(r => r.Id == id);
            if (shelters == null)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "找不到此收容所資訊";
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
            if (user.IsAdmin == false && shelters.OperationInfo.UserId != userId)
            {
                result.IsSuccess = false;
                result.ErrorMessage = "沒有權限";
                return result;
            }

            if (shelters.Animals.Any())
            {
                result.IsSuccess = false;
                result.ErrorMessage = "收容所已有待認養動物，無法刪除";
                return result;
            }

            try
            {
                // 有上傳圖片，就把圖片刪掉
                if (string.IsNullOrWhiteSpace(shelters.CoverPhoto) == false)
                {
                    File.Delete(path + "//" + shelters.CoverPhoto);
                }

                PetContext.Messages.RemoveRange(shelters.Messages);
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
        public IsSuccessResult DeleteMessage(int id, int messageId, int userId)
        {
            var log = GetLogger();
            log.Debug("id: {0}, messageId: {1}, userId: {2}", id, messageId, userId);

            var result = new IsSuccessResult();

            var shelters = PetContext.Shelters.SingleOrDefault(r => r.Id == id);
            if (shelters == null)
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

            var lastId = PetContext.Shelters.Select(r => r.Id).OrderByDescending(r => r).FirstOrDefault();

            try
            {
                var shelters = PetContext.Shelters.Add(new Shelter
                {
                    Id = lastId + 1,
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
        public IsSuccessResult EditShelters(int id, CreateShelters data, int userId)
        {
            var log = GetLogger();
            log.Debug("photo: {0}, name: {1}, intorduction: {2}, areaId: {3}, address: {4}, phone: {5}, url: {6}, id: {7}, userId: {8}",
                data.Photo, data.Name, data.Introduction, data.AreaId, data.Address, data.Phone, data.Url, id, userId);

            var shelters = PetContext.Shelters.SingleOrDefault(r => r.Id == id);
            if (shelters == null)
                return new IsSuccessResult("找不到此收容所");

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

            //檢查權限
            var user = PetContext.Users.SingleOrDefault(r => r.Id == userId);
            if (user == null || user.IsDisable)
                return new IsSuccessResult<SheltersItem>("沒有權限");
            if (user.IsAdmin == false && shelters.OperationInfo.UserId != userId)
                return new IsSuccessResult<SheltersItem>("沒有權限");

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