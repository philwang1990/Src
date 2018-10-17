using System;
using KKday.Web.B2D.BE.Models.Model.Common;

namespace KKday.Web.B2D.BE.Areas.KKday.Models.DataModel.Company
{
    [Serializable]
    public class LookupModel
    {
        public string Filter { get; set; } // 搜尋條件
        public string Sorting { get; set; } // 排序條件
        public Pagination Paging { get; set; } // 分頁資料
        public bool ForceRequery { get; set; }
    }
}
