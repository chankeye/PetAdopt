using PetAdopt.DTO;
using PetAdopt.DTO.Shelters;
using PetAdopt.Models;
using PetAdopt.Utilities;
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
                    Date = TransformTime.UtcToLocalTime(shelters.OperationInfo.Date).ToString("yyyy/MM/dd"),
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
                Date = TransformTime.UtcToLocalTime(r.Date).ToString("yyyy/MM/dd"),
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
        public IsSuccessResult<SheltersItem> AddShelters(CreateShelters data, int id = 0)
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

            if (id == 0)
                id = PetContext.Shelters.Select(r => r.Id).OrderByDescending(r => r).FirstOrDefault() + 1;

            try
            {
                var shelters = PetContext.Shelters.Add(new Shelter
                {
                    Id = id,
                    CoverPhoto = data.Photo,
                    Name = data.Name,
                    Introduction = data.Introduction,
                    Address = data.Address,
                    AreaId = data.AreaId,
                    Phone = data.Phone,
                    Url = data.Url,
                    OperationInfo = new OperationInfo
                    {
                        Date = DateTime.UtcNow,
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
                        Date = DateTime.UtcNow,
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

        public void AddInitShelters()
        {
            AddShelters(new CreateShelters
            {
                Name = "新北市新莊動物之家",
                Introduction = "新北市新莊動物之家",
                AreaId = 3,
                Address = "新北市泰山區楓江路191號(中港抽水站內)",
                Phone = "02-22977814"
            }, 52);

            AddShelters(new CreateShelters
            {
                Name = "新北市新店動物之家 ",
                Introduction = "新北市新店動物之家 ",
                AreaId = 3,
                Address = "新北市新店區安泰路235號",
                Phone = "02-22159462"
            }, 51);

            AddShelters(new CreateShelters
            {
                Name = "新北市板橋動物之家 ",
                Introduction = "新北市板橋動物之家 ",
                AreaId = 3,
                Address = "新北市板橋區華東路1-9號",
                Phone = "02-89662158"
            }, 50);

            AddShelters(new CreateShelters
            {
                Name = "臺北市動物之家",
                Introduction = "臺北市動物之家",
                AreaId = 2,
                Address = "台北市內湖區潭美街852號",
                Phone = "02-87913254",
                Url = "http://www.tcapo.gov.taipei/"
            }, 49);

            AddShelters(new CreateShelters
            {
                Name = "基隆市政府動物保護防疫所寵物銀行",
                Introduction = "基隆市政府動物保護防疫所寵物銀行",
                AreaId = 4,
                Address = "基隆市信義區信二路241號1樓",
                Phone = "02-24280677",
                Url = "http://www.klaphio.gov.tw/"
            }, 48);

            AddShelters(new CreateShelters
            {
                Name = "新北市中和動物之家 ",
                Introduction = "新北市中和動物之家 ",
                AreaId = 3,
                Address = "新北市中和區興南路3段100號",
                Phone = "02-86685547"
            }, 53);

            AddShelters(new CreateShelters
            {
                Name = "新北市三峽動物之家",
                Introduction = "新北市三峽動物之家",
                AreaId = 3,
                Address = "新北市三峽區隆恩街243號",
                Phone = "02-26722143"
            }, 54);

            AddShelters(new CreateShelters
            {
                Name = "新北市淡水動物之家",
                Introduction = "新北市淡水動物之家",
                AreaId = 3,
                Address = "新北市淡水區義山里下圭柔山90-7號旁",
                Phone = "02-26267558"
            }, 55);

            AddShelters(new CreateShelters
            {
                Name = "新北市瑞芳動物之家",
                Introduction = "新北市瑞芳動物之家",
                AreaId = 3,
                Address = "新北市瑞芳區傑魚里八分寮路(垃圾掩埋場旁)",
                Phone = "02-24063481"
            }, 56);

            AddShelters(new CreateShelters
            {
                Name = "新北市五股動物之家",
                Introduction = "新北市五股動物之家",
                AreaId = 3,
                Address = "新北市五股區外寮路9-9號",
                Phone = "02-82925265"
            }, 58);

            AddShelters(new CreateShelters
            {
                Name = "新北市八里動物之家",
                Introduction = "新北市八里動物之家",
                AreaId = 3,
                Address = "新北市八里區長坑里長道坑36號",
                Phone = "02-26194428"
            }, 59);

            AddShelters(new CreateShelters
            {
                Name = "新北市三芝動物之家 ",
                Introduction = "新北市三芝動物之家 ",
                AreaId = 3,
                Address = "新北市三芝區圓山里二坪頂白沙安樂園附近",
                Phone = "02-26362111"
            }, 60);

            AddShelters(new CreateShelters
            {
                Name = "桃園市動物保護教育園區",
                Introduction = "桃園市動物保護教育園區",
                AreaId = 6,
                Address = "桃園市新屋區永興里三鄰大牛欄117號",
                Phone = "03-4861760",
                Url = "http://taw.efarm.org.tw/"
            }, 61);

            AddShelters(new CreateShelters
            {
                Name = "新竹市動物收容所",
                Introduction = "新竹市動物收容所",
                AreaId = 8,
                Address = "新竹市海濱路250號",
                Phone = "03-5368329",
                Url = "http://puppy.hccg.gov.tw/"
            }, 62);

            AddShelters(new CreateShelters
            {
                Name = "新竹縣動物收容所 ",
                Introduction = "新竹縣動物收容所 ",
                AreaId = 7,
                Address = "新竹縣竹北市縣政五路192號",
                Phone = "03-55195484"
            }, 63);

            AddShelters(new CreateShelters
            {
                Name = "苗栗縣北區動物收容中心（竹南鎮公所）",
                Introduction = "苗栗縣北區動物收容中心（竹南鎮公所）",
                AreaId = 9,
                Address = "苗栗縣竹南鎮垃圾衛生掩埋場",
                Phone = "037-465193"
            }, 64);

            AddShelters(new CreateShelters
            {
                Name = "苗栗縣苗中區動物收容中心（苗栗市公所）",
                Introduction = "苗栗縣苗中區動物收容中心（苗栗市公所）",
                AreaId = 9,
                Address = "苗栗縣苗栗市苗栗市第四公墓後垃圾衛生掩埋場內",
                Phone = "037-331910"
            }, 65);

            AddShelters(new CreateShelters
            {
                Name = "苗栗縣南區動物收容中心（苑裡鎮公所）",
                Introduction = "苗栗縣南區動物收容中心（苑裡鎮公所）",
                AreaId = 3,
                Address = "苗栗縣苑裡鎮水坡里(垃圾掩埋場外)",
                Phone = "037-868930"
            }, 66);

            AddShelters(new CreateShelters
            {
                Name = "臺中市南屯園區動物之家",
                Introduction = "臺中市南屯園區動物之家",
                AreaId = 10,
                Address = "台中市南屯區中台路601號(望高寮)",
                Phone = "04-23850949",
                Url = "http://www.animal.taichung.gov.tw/mp.asp?mp=119020"
            }, 67);

            AddShelters(new CreateShelters
            {
                Name = "臺中市后里園區動物之家",
                Introduction = "臺中市后里園區動物之家",
                AreaId = 10,
                Address = "台中市后里區提防路370號",
                Phone = "04-25581480",
                Url = "http://www.animal.taichung.gov.tw/mp.asp?mp=119020"
            }, 68);

            AddShelters(new CreateShelters
            {
                Name = "彰化縣流浪狗中途之家",
                Introduction = "彰化縣流浪狗中途之家",
                AreaId = 11,
                Address = "彰化縣員林鎮大峰里阿寶坑426號(請於彰化縣芬園鄉大彰路一段875巷進入)",
                Phone = "04-8590638",
                Url = "http://www.chcgadcc.gov.tw/"
            }, 69);

            AddShelters(new CreateShelters
            {
                Name = "南投縣公立動物收容所",
                Introduction = "南投縣公立動物收容所",
                AreaId = 12,
                Address = "南投市嶺興路36-1號",
                Phone = "049-2225440"
            }, 70);

            AddShelters(new CreateShelters
            {
                Name = "嘉義市流浪犬收容中心",
                Introduction = "嘉義市流浪犬收容中心",
                AreaId = 15,
                Address = "嘉義市彌陀路環保局停車場",
                Phone = "05-2168661"
            }, 71);

            AddShelters(new CreateShelters
            {
                Name = "嘉義縣流浪犬中途之家",
                Introduction = "嘉義縣流浪犬中途之家",
                AreaId = 14,
                Address = "嘉義縣民雄鄉松山村後山仔37~1號",
                Phone = "05-3620025"
            }, 72);

            AddShelters(new CreateShelters
            {
                Name = "臺南市灣裡站動物之家",
                Introduction = "臺南市灣裡站動物之家",
                AreaId = 16,
                Address = "台南市南區省躬里萬年路580巷92號",
                Phone = "06-2964439",
            }, 73);

            AddShelters(new CreateShelters
            {
                Name = "臺南市善化站動物之家",
                Introduction = "臺南市善化站動物之家",
                AreaId = 16,
                Address = "台南市善化區東昌里東勢寮1-19號(肉品市場旁)",
                Phone = "06-5832399"
            }, 74);

            AddShelters(new CreateShelters
            {
                Name = "高雄市壽山站動物保護教育園區",
                Introduction = "高雄市壽山站動物保護教育園區",
                AreaId = 17,
                Address = "高雄市鼓山區萬壽路350號",
                Phone = "07-5519059"
            }, 75);

            AddShelters(new CreateShelters
            {
                Name = "高雄市燕巢站動物保護教育園區",
                Introduction = "高雄市燕巢站動物保護教育園區",
                AreaId = 17,
                Address = "高雄市燕巢區深水里縣政府深水農場苗圃地",
                Phone = "07-7450413"
            }, 76);

            AddShelters(new CreateShelters
            {
                Name = "屏東縣流浪動物收容所",
                Introduction = "屏東縣流浪動物收容所",
                AreaId = 18,
                Address = "屏東縣內埔鄉老埤村學府路1號",
                Phone = "08-7740588"
            }, 77);

            AddShelters(new CreateShelters
            {
                Name = "宜蘭縣流浪動物中途之家",
                Introduction = "宜蘭縣流浪動物中途之家",
                AreaId = 5,
                Address = "宜蘭縣五結鄉成興村利寶路60號",
                Phone = "03-9602350",
                Url = "http://asms.wsn.com.tw/Eland/webClientMain.aspx?Page=0"
            }, 78);

            AddShelters(new CreateShelters
            {
                Name = "花蓮縣流浪犬中途之家",
                Introduction = "花蓮縣流浪犬中途之家",
                AreaId = 19,
                Address = "花蓮市吉安鄉南濱路一段599號旁巷內",
                Phone = "03-8421452"
            }, 79);

            AddShelters(new CreateShelters
            {
                Name = "臺東縣流浪動物收容中心",
                Introduction = "臺東縣流浪動物收容中心",
                AreaId = 20,
                Address = "台東縣台東市中華路四段861巷350號",
                Phone = "089-362011"
            }, 80);

            AddShelters(new CreateShelters
            {
                Name = "連江縣流浪犬收容中心",
                Introduction = "連江縣流浪犬收容中心",
                AreaId = 23,
                Address = "連江縣南竿鄉清水村101號",
                Phone = "0836-25348"
            }, 81);

            AddShelters(new CreateShelters
            {
                Name = "金門縣動物收容中心",
                Introduction = "金門縣動物收容中心",
                AreaId = 22,
                Address = "金門縣金湖鎮裕民農莊20號",
                Phone = "082-336625",
                Url = "http://www.kinmen.gov.tw/KinmenWeb/wSite/page/1.html"
            }, 82);

            AddShelters(new CreateShelters
            {
                Name = "澎湖縣流浪動物收容中心",
                Introduction = "澎湖縣流浪動物收容中心",
                AreaId = 21,
                Address = "澎湖縣馬公市烏崁里1073地號",
                Phone = "06-9213559"
            }, 83);

            AddShelters(new CreateShelters
            {
                Name = "雲林縣動植物防疫所",
                Introduction = "雲林縣動植物防疫所",
                AreaId = 13,
                Address = "雲林縣斗六市雲林路二段517號",
                Phone = "06-55523250"
            }, 89);

            AddShelters(new CreateShelters
            {
                Name = "臺中市愛心小站",
                Introduction = "臺中市愛心小站",
                AreaId = 10,
                Address = "臺中市",
                Phone = "04-23850949",
                Url = "http://163.29.86.4/ct.asp?xItem=174423&ctNode=8026&mp=119020"
            }, 90);

            AddShelters(new CreateShelters
            {
                Name = "臺中市中途動物醫院",
                Introduction = "臺中市中途動物醫院",
                AreaId = 10,
                Address = "臺中市中途動物醫院",
                Phone = "臺中市中途動物醫院"
            }, 91);

            AddShelters(new CreateShelters
            {
                Name = "新北市政府動物保護防疫處",
                Introduction = "新北市政府動物保護防疫處",
                AreaId = 3,
                Address = "新北市板橋區四川路一段157巷2號",
                Phone = "02-29596353",
                Url = "http://www.ahiqo.ntpc.gov.tw/"
            }, 92);

            AddShelters(new CreateShelters
            {
                Name = "新北市金山動物之家",
                Introduction = "新北市金山動物之家",
                AreaId = 3,
                Address = "新北市金山區永興里大水崛7號",
                Phone = "02-24986784"
            }, 94);
        }
    }
}