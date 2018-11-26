using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.EC.Models.Model.Account
{
    /// <summary>
    /// 基礎會員模組
    /// </summary>

    [Serializable]
    public class UserAccount 
    {
        public Int64 XID { get; set; }   //帳號流水號
        public string UUID { get; set; }   //會員識別流水號 
        public string EMAIL { get; set; }  //電子郵件信箱(帳號)
        public string NAME_FIRST { get; set; } //姓氏
        public string NAME_LAST { get; set; }  //名字
        public string NAME { get; set; } //[顯示用]姓名
        public string GENDER_TITLE { get; set; } //稱謂
        public string JOB_TITLE { get; set; } //職稱
        public string DEPARTMENT { get; set; } //部門
        public bool ENABLE { get; set; }   //是否有效(true/false)
        public string LOCALE { get; set; } //語系
    }

    /// <summary>
    /// KKday員工模組
    /// </summary>

    [Serializable]
    public class KKdayAccount : UserAccount
    {
        public string STAFF_NO { get; set; }
        public string ROLES { get; set; }   //員工角色

    }


    /// <summary>
    /// 分銷商會員
    /// </summary>

    [Serializable]
    public class B2dAccount : UserAccount
    {
        public string USER_TYPE { get; set; } //帳號權限("00":一般, "01":管理者
        public string USER_TYPE_DESC { get; set; } //[顯示用] 帳號權限
        public Int64 COMPANY_XID { get; set; } // 公司代碼
        public string COMPANY_NAME { get; set; } // [顯示用] 公司名稱
        public string TEL_AREA { get; set; } // [顯示用] 國碼
        public string TEL { get; set; } //聯絡電話
        public string CURRENCY { get; set; } //幣別
        public string PASSWORD { get; set; } //密碼
        public string COUNRTY_CODE { get; set; } //國藉
        //public string LANG { get; set; }
        public string KKDAY_CHANNEL_OID { get; set; } //channelOid
    }
}
