using System;
namespace KKday.Web.B2D.BE.Models.Model.Common
{ 
    [Serializable]
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
