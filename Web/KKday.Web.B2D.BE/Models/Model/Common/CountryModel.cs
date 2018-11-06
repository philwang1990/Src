using System;
namespace KKday.Web.B2D.BE.Models.Model.Common
{
    // 國家與國碼
    public class CountryArea
    {
        public string telArea { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string countryEngName { get; set; }
    }

    //國家與語系
    public class CountryLocale
    {
        public string localeCode { get; set; }
        public string localeName { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
    } 
}
