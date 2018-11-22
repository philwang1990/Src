﻿using System;
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

using KKday.API.WMS.Models.DataModel.Account;
using KKday.API.WMS.Models.Repository.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller {

        /// <summary>
        /// The redis cache.
        /// </summary>
        private readonly IDistributedCache redisCache;

        public AccountController(IDistributedCache redisCache) {
            this.redisCache = redisCache;
        }


        [HttpPost("RegisterIs4User")]
        public ActionResult RegisterIs4User(AccountModel acct)
        {

            return Content(AccountRepository.RegisterIs4User(acct).ToString(), "application/json");
        }


        [HttpPost("UpdateUser")]
        public ActionResult UpdateUser(AccountModel acct)
        {

            return Content(AccountRepository.UpdatetUser(acct).ToString(), "application/json");
        }

        [HttpPost("RegisterUser")]
        public RegisterRSModel RegisterUser([FromBody]RegisterRQModel register)
        {
            RegisterRSModel status = new RegisterRSModel();

            try
            {
                Website.Instance.logger.Info($"WMS RegisterUser Start! B2D email:{register.EMAIL},pwd:{register.PASSWORD}");
                status = AccountRepository.RegisterAccount(register);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

    }
}
