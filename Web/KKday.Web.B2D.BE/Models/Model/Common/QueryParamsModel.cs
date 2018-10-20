using System;

namespace KKday.Web.B2D.BE.Models.Model.Common
{
    [Serializable]
    public class QueryParamsModel
    {
        public string Filter { get; set; } // 搜尋條件
        public string Sorting { get; set; } // 排序條件
        public Pagination Paging { get; set; } // 分頁資料
    }
}
