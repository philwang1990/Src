using System;
namespace KKday.Web.B2D.BE.Common.Models
{
    public class IApplicationUser
    {
        public string UserAccount { get; set; }
        public string UserName { get; set; }
    }
     
    /////////////////

    public class KKdayUser : IApplicationUser
    {

    }
 
    /////////////////

    public class DistributorUser : IApplicationUser
    {

    }
}
