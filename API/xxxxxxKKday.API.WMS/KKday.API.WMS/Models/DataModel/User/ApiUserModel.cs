using System;
using KKday.API.WMS.Models.DataModel.Authorize;

namespace KKday.API.WMS.Models.DataModel.User {

    /// <summary>
    ///  API 使用者資料模型, 資料表來源: 
    /// b2b.b2d_api_account, b2b.b2d_company
    /// </summary>
    [Serializable]
    public class ApiUserModel {

        public string result { get; set; }
        public string result_msg { get; set; }

        public Int64 user_xid { get; set; }
        public string user_name { get; set; } 
        // 使用者姓名 (name_first + name_last)

        public string user_email { get; set; }

        public Int64 company_xid { get; set; }
        public string comapny_name { get; set; }

        public string company_language { get; set; } // 分銷商語系
        public string company_currency { get; set; } // 分銷商幣別
        public string payment_type { get; set; }  // 付款類型
    }
}
