using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using KKday.API.WMS.AppCode.DAL;
using KKday.API.WMS.AppCode.Proxy;

using KKday.API.WMS.Models.DataModel.Authorize;
using KKday.API.WMS.Models.DataModel.User;
using KKday.API.WMS.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : Controller {

        /// <summary>
        /// The redis cache.
        /// </summary>
        private readonly IDistributedCache redisCache;

        public AuthorizeController(IDistributedCache redisCache) {
            this.redisCache = redisCache;
        }

        /// <summary>
        /// Auths the user.
        /// </summary>
        /// <returns>The user.</returns>
        /// <param name="email">Email.</param>
        /// <param name="password">Password.</param>
        //[HttpGet("AuthUser")]
        //public ApiUserModel AuthUser(string email,string password)
        //{

        //   // string token = "";
        //    ApiUserModel user = new ApiUserModel();

        //    try
        //    {
        //        Website.Instance.logger.Info($"WMS AuthUser Start! B2D email:{email},pwd:{password}");
        //        //1. 從IS4取使用者的門票
        //       // GetTokenResponseModel response = AuthProxy.getToke(account, password);
        //      //  token = response.access_token ?? response.error_description;

        //        //1. 從DB抓使用者資訊
        //        user = UserRepository.GetUser(email, password);

        //        //3. 把使用者的資訊轉成byte 存進去redis快取
        //      //  var userByte = ObjectToByteArray(user);
               
        //      //  redisCache.Set("wms.api.token", userByte, 
        //       //                new DistributedCacheEntryOptions() {
        //       //     AbsoluteExpiration = DateTime.Now.AddHours(24)
        //            //設定過期時間，時間一到快取立刻就被移除
        //       // });
            
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return user;
        //}

        /// <summary>
        /// Auths the API user.
        /// </summary>
        /// <returns>The API user.</returns>
        /// <param name="email">Email.</param>
        [HttpGet("AuthApiUser")]
        public B2dAccountModel AuthApiUser(string email)
        {

            B2dAccountModel ApiUser = new B2dAccountModel();

            try
            {
                Website.Instance.logger.Info($"WMS AuthApiUser Start! B2D email:{email}");
                //從DB抓使用者資訊
                ApiUser = UserRepository.AuthApiAccount(email);

            }
            catch (Exception ex)
            {
                ApiUser.result = "10";
                ApiUser.result_msg = "system error:" + ex.Message;
            }
            return ApiUser;
        }

        //將物件轉為ByteArray
        //private byte[] ObjectToByteArray(Object aum) {
        //    if (aum == null)
        //        return null;
        //    BinaryFormatter bf = new BinaryFormatter();
        //    MemoryStream ms = new MemoryStream();
        //    bf.Serialize(ms, aum);
        //    return ms.ToArray();

        //}

        //[HttpGet("authToken")]
        //public string authToken(string token)
        //{
        //    string result = "";
        //    HttpClient client = new HttpClient();

        //    try
        //    {

        //        var cacheBytes = redisCache.Get("wms.api.token");

        //        if (cacheBytes != null) {

        //            token = System.Text.Encoding.UTF8.GetString(cacheBytes);

        //            //Token 驗證的語法
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //            HttpResponseMessage response = client.GetAsync("http://192.168.2.83:5009/api/values").Result;
        //            result = response.Content.ReadAsStringAsync().Result;

        //        } else {
        //            result = "系統閒置過久，請重新登入！";
        //        }

        //    } catch (Exception ex)
        //    {
        //        result = ex.Message;

        //    }
        //    return result;
        //}

        [HttpGet("AuthUser")]
        public B2dAccountModel AuthUser(string email, string password)
        {

            B2dAccountModel user = new B2dAccountModel();

            try
            {
                Website.Instance.logger.Info($"WMS AuthUser Start! B2D email:{email},pwd:{password}");
                user = UserRepository.AuthAccount(email, password);
            }
            catch (Exception ex)
            {
                user.result = "10";
                user.result_msg = "system error:" + ex.Message;

            }
            return user;
        }

    }
}
