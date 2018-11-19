using System;
namespace KKday.API.WMS.Models.DataModel.User
{
    public class RegisterRQModel
    {
        public string COMP_COOP_MODE { get; set; }   //合作方式
        public string PAYMENT_TYPE { get; set; }   //付款方式
        public string GENDER_TITLE { get; set; }   //稱謂
        public string NAME_FIRST { get; set; }     //姓氏
        public string NAME_LAST { get; set; }      //名字
        public string EMAIL { get; set; }          //電子郵件信箱
        public string JOB_TITLE { get; set; }      //職稱
        public string PASSWORD { get; set; }       //密碼
        public string COUNTRY { get; set; }        //國家
        public string COUNTRY_CODE { get; set; }   //國家區碼
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

    [Serializable]
    public class RegisterRSModel
    {
        public string result { get; set; }
        public string result_msg { get; set; }
        public string STATUS { get; set; }
        public Int64 COMPANY_XID { get; set; }
    }
}
