using System;
using KKday.API.WMS.Models.DataModel.User;
using KKday.API.WMS.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace KKday.API.WMS.Controllers
{
    public class UserController
    {
        [HttpPost("RegisterUser")]
        public RegisterRSModel RegisterUser([FromBody]RegisterRQModel register)
        {
            RegisterRSModel status = new RegisterRSModel();

            try
            {
                Website.Instance.logger.Info($"WMS RegisterUser Start! B2D email:{register.EMAIL},pwd:{register.PASSWORD}");
                status = UserRepository.RegisterAccount(register);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
    }
}
