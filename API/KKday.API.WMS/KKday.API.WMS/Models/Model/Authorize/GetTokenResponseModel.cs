using System;
namespace KKday.API.WMS.Models.Model.Authorize
{
    public class GetTokenResponseModel
    {
        public string access_token { set; get; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
    }
}
