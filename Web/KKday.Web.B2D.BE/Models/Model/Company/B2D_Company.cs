using System;
namespace KKday.Web.B2D.BE.Models.Company
{
    public class B2D_Company
    {
        public Int64 XID { get; set; } //公司流水號
        public string STATUS { get; set; } //審核狀態(00待審中/01已核准/02審核未通過/03待補件)
        public string STATUS_DESC { get; set; } //審核狀態名稱
        public string COMP_COOPERATION { get; set; } //合作方式(00串接API/01平台)
        public string PAYMENT_TYPE { get; set; } //付款方式(00信用卡逐筆結/01額度付款)

        public Int64 MANAGER_ACCOUNT_XID { get; set; } //主帳號流水號
        public Int64 PARENT_COMP_XID { get; set; } //母公司流水號

        public string COMP_NAME { get; set; } //公司名稱
        public string COMP_URL { get; set; } //公司網址
        public string COMP_LICENSE { get; set; } //公司憑證(營業執照)
        public string COMP_LICENSE_2 { get; set; } //公司憑證(旅行社許可證)

        public string COMP_LOCALE { get; set; } //公司指定語系
        public string COMP_CURRENCY { get; set; } //公司指定幣別
        public string COMP_INVOICE { get; set; } //公司統編(台灣限定)
        public string COMP_COUNTRY { get; set; } //公司國碼

        public string COMP_TEL_COUNTRY_CODE { get; set; } //公司電話國碼
        public string COMP_TEL { get; set; } //公司電話 
        public string COMP_ADDRESS { get; set; } //公司地址
        public string CHARGE_MAN_FIRST { get; set; } //公司負責人名字
        public string CHARGE_MAN_LAST { get; set; } //公司負責人姓氏

        public string CREDITCARD_NO { get; set; } //信用卡號
        public string CREDITCARD_VALID { get; set; } //信用卡效期 yyyy-mm
        public string CREDITCARD_CVC { get; set; } //信用卡背面三碼

        public string CONTACT_USER { get; set; } //公司聯絡窗口
        public string CONTACT_USER_EMAIL { get; set; }
        public string FINANCE_USER { get; set; } //公司財務窗口
        public string SALES_USER { get; set; } //公司業務窗口

    }
}
