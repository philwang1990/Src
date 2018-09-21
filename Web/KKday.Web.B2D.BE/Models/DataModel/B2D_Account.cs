using System;
namespace KKday.Web.B2D.BE.Models.DataModel
{
    public class B2D_Account
    {
        public int XID { get; set; }           //帳號流水號
        public int COMPANY_XID { get; set; }   //所屬公司流水號

        public int ACCOUNT_TYPE { get; set; }  //帳號權限(00管理admin/01一般user)
        public bool ENABLE { get; set; }       //是否有效(Y/N)

        public string ACCOUNT { get; set; }    //登入帳號(email)
        public string PASSWORD { get; set; }   //登入密碼

        public string NAME_FIRST { get; set; } //姓氏
        public string NAME_LAST { get; set; }  //名字
        public string TITLE { get; set; }      //稱謂
        public string DEPARTMENT { get; set; } //部門
        public string EMAIL { get; set; }      //連絡信箱
        public int TEL { get; set; }           //聯絡電話

    }
}
