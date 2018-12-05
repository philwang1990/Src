using System;
namespace KKday.Web.B2D.EC.Models.Model.Account
{
    public class RegisterModel
    {
        public string WEB_LOCALE { get; set; }     //顯示用語系

        public string GENDER_TITLE { get; set; }   //稱謂
        public string NAME_FIRST { get; set; }     //姓氏
        public string NAME_LAST { get; set; }      //名字
        public string EMAIL { get; set; }          //電子郵件信箱
        public string JOB_TITLE { get; set; }      //職稱
        public string PASSWORD { get; set; }       //密碼
        public string COUNTRY_CODE { get; set; }   //國家區碼
        public string TIMEZONE { get; set; }       //時區
        public string TEL_CODE { get; set; }       //區碼
        public string TEL { get; set; }            //聯絡電話
        public string LOCALE { get; set; }         //語系
        public string CURRENCY { get; set; }       //幣別
        public string COMPANY_NAME { get; set; }   //公司名稱
        public string INVOICE { get; set; }        //統一編號
        public string URL { get; set; }            //公司網址
        public string ADDRESS { get; set; }        //公司地址
        public string LICENCSE_1 { get; set; }     //憑證一
        public string LICENCSE_2 { get; set; }     //憑證二
        public string USER_UUID { get; set; }      //uuid
    }
}
