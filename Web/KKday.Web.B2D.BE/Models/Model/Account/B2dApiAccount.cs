using System;
namespace KKday.Web.B2D.BE.Models.Model.Account
{
    [Serializable]
    public class B2dApiAccount : B2dAccount
    {
        public string SOURCE { get; set; }        //來源
        public string TOKEN { get; set; }         //API TOKEN
    }
}
