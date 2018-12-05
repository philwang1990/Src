using System;

namespace KKday.Web.B2D.EC.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorType ErrorType { get; set; }

        public string ErrorMessage { get; set; } = "";
    }

    public enum ErrorType
    {
        Invalid_Common,
        Invalid_Market,
        Order_Fail
    }
}