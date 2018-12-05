using System;
namespace KKday.Web.B2D.BE.Models.Model.Company
{
    public class B2dCompany
    {
        public Int64 XID { get; set; } //公司流水號
        public Int64 MANAGER_ACCOUNT_XID { get; set; } //主帳號流水號
        public Int64 PARENT_COMP_XID { get; set; } //母公司流水號

        public string STATUS { get; set; } //審核狀態(00=待審中/01=已核准/02=未通過/03=待補件)
        public string STATUS_DESC { get; set; } //審核狀態名稱
        public string COMP_COOP_MODE { get; set; } //合作方式(00=ALL,01=API,02=WEB)
        public string PAYMENT_TYPE { get; set; } //付款方式(01=逐筆結/02=額度付款)
        public string COMP_NAME { get; set; } //公司名稱
        public string COMP_URL { get; set; } //公司網址
        public string COMP_LOCALE { get; set; } //公司指定語系
        public string COMP_CURRENCY { get; set; } //公司指定幣別
        public string COMP_INVOICE { get; set; } //公司統編(台灣限定)
        public string COMP_COUNTRY { get; set; } //公司所在國家
        public string COMP_TEL_COUNTRY_CODE { get; set; } //公司電話國碼
        public string COMP_TEL { get; set; } //公司電話 
        public string COMP_ADDRESS { get; set; } //公司地址
        public string CONTACT_USER { get; set; } //聯絡窗口
        public string CONTACT_USER_EMAIL { get; set; } //聯絡窗口電子郵件
        public string FINANCE_USER { get; set; } //財務窗口
        public string SALES_USER { get; set; } //業務窗口 

        public string CHARGE_MAN_FIRST { get; set; } //公司負責人名字
        public string CHARGE_MAN_LAST { get; set; } //公司負責人姓氏

        public string COMP_LICENSE { get; set; } //公司憑證(營業執照)
        public string COMP_LICENSE_2 { get; set; } //公司憑證(旅行社許可證)

        public string CREDITCARD_NO { get; set; } //信用卡號
        public string CREDITCARD_VALID { get; set; } //信用卡效期 yyyy-mm
        public string CREDITCARD_CVC { get; set; } //信用卡背面三碼 

    }
}
