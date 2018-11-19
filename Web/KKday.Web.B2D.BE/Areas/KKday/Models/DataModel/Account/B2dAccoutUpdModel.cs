using System;
namespace KKday.Web.B2D.BE.Areas.KKday.Models.DataModel.Account
{
    public class B2dAccoutUpdModel
    {
        public Int64 XID { get; set; }
        public string NAME_FIRST { get; set; } //姓氏
        public string NAME_LAST { get; set; }  //名字
        public string GENDER_TITLE { get; set; } //稱謂
        public string JOB_TITLE { get; set; } //職稱
        public string DEPARTMENT { get; set; } //部門
        public bool ENABLE { get; set; }   //是否有效(true/false)
        public string USER_TYPE { get; set; } //帳號權限("00":一般, "01":管理者)  
        public string TEL { get; set; } //聯絡電話 
    }
}
