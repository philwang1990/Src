using System;
namespace KKday.Web.B2D.BE.Areas.KKday.Models.DataModel
{
    public class CompanyUpdModel
    {
        public Int64 XID { get; set; }
        public string STATUS { get; set; } //審核狀態(00=待審中/01=已核准/02=未通過/03=待補件) 
        public string COOP_MODE { get; set; } //合作方式(00=ALL,01=API,02=WEB)
        public string PAYMENT_TYPE { get; set; } //付款方式(01=逐筆結/02=額度付款)
        public string NAME { get; set; } //公司名稱
        public string URL { get; set; } //公司網址
        public string LOCALE { get; set; } //公司指定語系
        public string CURRENCY { get; set; } //公司指定幣別
        public string INVOICE { get; set; } //公司統編(台灣限定)
        public string COUNTRY { get; set; } //公司所在國家
        public string TEL_COUNTRY_CODE { get; set; } //公司電話國碼
        public string TEL { get; set; } //公司電話 
        public string ADDRESS { get; set; } //公司地址
        public string CONTACT_USER { get; set; } //聯絡窗口
        public string CONTACT_USER_EMAIL { get; set; } //聯絡窗口電子郵件
        public string FINANCE_USER { get; set; } //財務窗口
        public string SALES_USER { get; set; } //業務窗口 
    }
}
