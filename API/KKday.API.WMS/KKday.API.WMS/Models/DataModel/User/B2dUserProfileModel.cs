using System;
namespace KKday.API.WMS.Models.DataModel.User
{
    public interface B2dUserProfileModel
    {
    }

    [Serializable]
    public class B2dUserProfile : B2dAccount
    {
        public string COUNTRY_CODE { get; set; }
        public string INVOICE_NO { get; set; }
        public string URL { get; set; }
        public string ADDRESS { get; set; }
    }
}
