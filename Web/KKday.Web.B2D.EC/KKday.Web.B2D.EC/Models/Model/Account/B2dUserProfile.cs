using System;
namespace KKday.Web.B2D.EC.Models.Model.Account
{
    [Serializable]
    public class B2dUserProfile : B2dAccount
    {
        public string COUNTRY_CODE { get; set; }
        public string INVOICE_NO { get; set; }
        public string URL { get; set; }
        public string ADDRESS { get; set; }
    }
}
