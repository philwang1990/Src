using System;
using System.Collections.Generic;

namespace KKday.API.WMS.Models.DataModel.Product
{
    public class ProductResponseModel
    {
        public string prod_no { get; set; }//商品編號
        public string prod_name { get; set; }//商品名稱
        public string prod_name_main_lang { get; set; }//商品主語系商品名稱
        public string prod_type { get; set; }//商品大分類 M01-M08
        public string prod_type_name { get; set; }//商品大分類名稱 M01-M08
        public string tag { get; set; }//小分類 
        public string tag_name { get; set; }//小分類 ex：戶外活動,熱門票券..
        public string main_lang { get; set; } //主語系
        public string cost_type { get; set; } //成本類別
        public string prod_desc1 { get; set; } //簡述
        public string prod_desc2 { get; set; } //行程說明
        public string prod_desc3 { get; set; } //溫馨提醒
        public string prod_desc4 { get; set; } //主要目的地
        public string prod_desc5 { get; set; } //供應商商品備註
        public string is_search { get; set; } //是否開放搜尋
        public string apply_status { get; set; } //審核狀態
        public string status { get; set; } // 銷售狀態(00上架 01下架)
        public string policy_no { get; set; } //取消規則,1:不扣手續費，退全額（包含當天取消者）2：無法退費 3: 指定手續費
        public List<Policy> policy_list { get; set; } //取消規定列表
        public string is_pkg { get; set; } //是否有無套餐
        public string is_tour { get; set; } //是否有無行程
        public List<Tour> tour_list { get; set; } //行程列表(合併[kkday]ScheList / scheMealList)
        public int prod_day { get; set; } //產品所需時間(天)
        public int prod_hour { get; set; } //產品所需時間(時)
        public List<GuideLanguage> guide_lang_list { get; set; } //導覽語言
        public List<ArrivalMapInfo> arr_map_info_list { get; set; } //目的地列表 
        public int confirm_order_time { get; set; } //回覆訂購結果 









        public string sup_no { get; set; }//供應商編號 
        public string sup_name { get; set; }//供應商名稱 
        public Supplier supplier { get; set; }//供應商資訊

    }

    /// <summary>
    /// 再研究
    /// </summary>
    public class Supplier
    {
        public string sup_c_name { get; set; }
        public string sup_e_name { get; set; }
        public string sup_desc { get; set; }
        public string sup_web { get; set; }

    }

    public class Policy
    {
        public int days { get; set; }
        public bool is_over { get; set; }//是否是以上天數都套用
        public int fee { get; set; }//手續費%數
    }

    public class Tour
    {
        public int tour_xid { get; set; } //行程xid
        public int tour_day { get; set; } //行程第幾天
        public string tour_time { get; set; } //時間
        public string tour_desc { get; set; }//說明
        public string tour_sort_seq { get; set; } //排序
        public int photo_xid { get; set; }//圖片xid
        public string photo_url { get; set; } //圖片url
        public string meal_day { get; set; } //行程第幾天提供餐食
        public string is_breakfast { get; set; } // Y/N
        public string is_lunch { get; set; }
        public string is_dinner { get; set; }

    }
    public class GuideLanguage
    {
        public string lang_code { get; set; }
        public string lang_name { get; set; }
    }

    public class ArrivalMapInfo
    {
        public string photo_url { get; set; } //圖片位置
        public string photo_desc { get; set; } //圖片描述
        public int zoom { get; set; } //地圖比例
        public int latlong_xid { get; set; } //經緯度序號
        public string latlong_type { get; set; } //經緯度類型
        public string latlong_desc { get; set; } //經緯度描述
        public string latitude { get; set; }//目的地 緯度
        public string longitude { get; set; }//目的地 經度

        public string mapType { get; set; }//地圖類型

    }
}
