using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KKday.API.WMS.AppCode.Proxy;

using KKday.API.WMS.Models.DataModel.Authorize;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KKday.API.WMS.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : Controller {


        [HttpGet("GetToken")]
        public string GetToken(string account,string password)
        {
            string rs = "", resutl = "";
          
            try
            {
                GetTokenResponseModel response = AuthProxy.getToke(account, password);
                rs = response.access_token ?? response.error_description;

                var Jresult = new
                {
                    result = rs
                };

                resutl = JsonConvert.SerializeObject(Jresult);

            }
            catch (Exception ex)
            {
                var Jresult = new
                {
                    result = ex.Message
                };

                resutl = JsonConvert.SerializeObject(Jresult);

            }
            return resutl;
        }



        [HttpGet("authToken")]
        public string authToken(string token)
        {
            string result = "";
            HttpClient client = new HttpClient();
            token = @"JhbGciOiJSUzI1NiIsImtpZCI6IjY5OTZkNzVjMjY1ZjE5ODE0NGE3M2EyMTcyYjgxN2Y1IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MzY4MjQ5MjEsImV4cCI6MTUzOTQxNjkyMSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMi44Mzo4MDgwIiwiYXVkIjpbImh0dHA6Ly8xOTIuMTY4LjIuODM6ODA4MC9yZXNvdXJjZXMiLCJzb2NpYWxuZXR3b3JrIl0sImNsaWVudF9pZCI6InNvY2lhbG5ldHdvcmsiLCJzdWIiOiIzIiwiYXV0aF90aW1lIjoxNTM2ODI0OTIxLCJpZHAiOiJsb2NhbCIsInNjb3BlIjpbInNvY2lhbG5ldHdvcmsiXSwiYW1yIjpbImN1c3RvbSJdfQ.1CipxGGn1hax67DAj2CMeLCIm59JulP3lT9qCSDHyADHgdtgWlOUfowLQYZSt5ZuH7kEF6VIVczcqqribY7XkAEuk03oYKAobXuHjh2COaZgrOHp-jk_RUBtQcdLlSpVr6JAi4LL9H2N09YRBEac0qhaWnaw6U8dOWXOGN4W1yUaXEYCMD4Hh_ghbl7CvE_GLNJ0TKvSLD9Agvdgcq1yrNGekUnSIzEvTYa1JUsulvrdgauMF4UDy3_agkszbSwp4UFR-lqWST54hAHpptmMuTvTnGin1y3y7N81tJ1eLXt3dJ5m1L2_T7rMyUZ4XURYpfdwrKFPZ92qSpzpJNlklQ";

            try
            {
                //Token 驗證的語法
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = client.GetAsync("http://192.168.2.83:5009/api/values").Result;
                result = response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception ex)
            {
                result = ex.Message;

            }
            return result;
        }



    }
}
