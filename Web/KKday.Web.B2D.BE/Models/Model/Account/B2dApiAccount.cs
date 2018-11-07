using System;
namespace KKday.Web.B2D.BE.Models.Model.Account
{
    [Serializable]
    public class B2dApiAccount : B2dAccount
    {
        //public Int64 COMPANY_XID { get; set; }    //所屬公司流水號
        public string SOURCE { get; set; }        //來源
        public string TOKEN { get; set; }         //API TOKEN
    }
}
