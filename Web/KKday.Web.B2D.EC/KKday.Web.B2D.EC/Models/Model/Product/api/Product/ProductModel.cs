using System;
using System.Collections.Generic;

namespace KKday.Web.B2D.EC.Models.Model.Product
{
    public class ProductModel : ProductBaseModel
    {
        public string guid { get; set; } // Session ID 確保30分鐘內訂購 seddionID放到redis
        public string result { get; set; }
        public string result_msg { get; set; }
        //public string prod_no { get; set; }//商品編號 (跟ProductBase共用)
        //public string prod_name { get; set; }//商品名稱 (跟ProductBase共用)
        //public string prod_currency { get; set; }//商品進貨幣別(跟ProductBase共用)
        public string prod_name_main_lang { get; set; }//商品主語系商品名稱
        //public string prod_type { get; set; }//商品大分類 M01-M08 (跟ProductBase共用)
        public string prod_type_name { get; set; }//商品大分類名稱 M01-M08
        //public string[] tag { get; set; }//小分類 (跟ProductBase共用)
        //public string tag_name { get; set; }//小分類 ex：戶外活動,熱門票券..
        public string main_lang { get; set; } //主語系
        public string cost_type { get; set; } //成本類別
        //public string introduction { get; set; } //簡述  (跟ProductBase共用)
        public string prod_desc { get; set; } //行程說明
        public string prod_tips { get; set; } //溫馨提醒
        public string prod_map_note { get; set; } //主要目的地
        //public string prod_sup_note { get; set; } //供應商商品備註
        public string is_search { get; set; } //是否開放搜尋
        public string apply_status { get; set; } //審核狀態
        public string status { get; set; } // 銷售狀態(00上架 01下架)

        public string policy_no { get; set; } //取消規則,1:REFOUND_ALL 不扣手續費，退全額（包含當天取消者）2：REFOUND_NO 無法退費 3:REFOUND_BY_RULE 指定手續費
        public List<Policy> policy_list { get; set; } //取消規定列表
        public string is_pkg { get; set; } //是否有無套餐
        public string is_tour { get; set; } //是否有無行程
        public List<Tour> tour_list { get; set; } //行程列表
        public List<ProvideMeal> meal_list { get; set; } //餐食列表
        //public int days { get; set; } //產品所需時間(天) (跟ProductBase共用)
        //public int hours { get; set; } //產品所需時間(時) (跟ProductBase共用)
        public string timezone { get; set; } //時區
        public List<GuideLanguage> guide_lang_list { get; set; } //導覽語言(跟ProductModule共用)
        public List<ArrivalMapInfo> arr_map_info_list { get; set; } //目的地列表 !!!!!!!!!!!!!!!!!!!!!!!!!!!
        public int confirm_order_time { get; set; } //回覆訂購結果 
        public string online_s_date { get; set; } //上架起日
        public string online_e_date { get; set; } //下架起日
        public string before_order_day { get; set; } //訂購前置日
        //public string prod_img_url { get; set; }//商品主畫面圖片url (跟ProductBase共用)
        public List<Images> img_list { get; set; } //商品最上方圖片點擊區

        public string finishStep { get; set; }//編輯步驟  kkday:"PO,PT,PD,TE,PS,PP,PC,SC,PG,CD,76058"
        public string left_step { get; set; }
        public ProdCommentInfo prod_comment_info { get; set; }//商品評價資訊 kkday：prodUrlInfo

        //public double minPrice { get; set; }//當時幣別最低成人售價(特價) "multipricePlatform":"01" 的 minPrice B2D>>套價  "multipricePlatform":"01" 的 minPrice
        //public double minSalePrice { get; set; }//當時幣別最低成人售價(原價) "multipricePlatform":"01" 的 minSalePrice B2D>>牌價

        public string order_email { get; set; }//訂單處理人email
        //public string disease_note { get; set; }//疾病提醒注意事項 >> 與Remind 合併


        public string prod_hander { get; set; }//供應商

        public List<Remind> remind_list { get; set; }//注意事項
        public List<Video> video_list { get; set; }//商品介紹影片
        public List<CostDetail> cost_detail_list { get; set; }//費用明細
        public TktExpire tkt_expire { get; set; }//票券效期
        public List<MeetingPoint> meeting_point_list { get; set; }//機場集合地點  商品為 M03 時使用


        public List<Location> voucher_locations { get; set; }//憑證領取地點
        public string voucher_desc { get; set; }//憑證類型種類敘述

        public ProdMarketing prod_mkt { get; set; }

        public MeetingPointMap meeting_point_map { get; set; } //接送範圍
    }


    public class Policy
    {
        public int days { get; set; }
        public bool is_over { get; set; }//是否是以上天數都套用
        public int fee { get; set; }//手續費%數 0-100
    }

    public class Tour
    {
        //public int tour_xid { get; set; } //行程xid
        public int tour_day { get; set; } //行程第幾天
        public int tour_sort_seq { get; set; } //排序
        public string tour_desc { get; set; }//說明
        public string time_desc { get; set; } //時間說明
        public string photo_url { get; set; } //圖片url
        //public int photo_xid { get; set; }//圖片xid


    }
    public class ProvideMeal
    {
        public int tour_day { get; set; } //行程第幾天提供餐食
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
        public string latitude { get; set; }//目的地 緯度
        public string longitude { get; set; }//目的地 經度
        public string latlong_type { get; set; } //經緯度類型
        public string latlong_desc { get; set; } //經緯度描述

        public int latlong_xid { get; set; } //經緯度序號
        public string map_type { get; set; }//地圖類型


    }
    public class Images
    {
        public string auth_name { get; set; } //授權名稱  (Photo by "xxxxxxx")
        public string is_main_img { get; set; } //是否為主圖 (Y/N)
        public string img_desc { get; set; }//照片說明
        //public string img_xid { get; set; }//序號
        public int img_sort { get; set; }//順序
        public string img_url { get; set; } //url
        public string img_kkday_url { get; set; } //kkday url
        public string is_auth_cc { get; set; } //是否cc授權
        public string is_commerce { get; set; } //是否商業用途
        public string share_type { get; set; } //分享狀態（Y/N/A）(是/否/不限) 預設為A

    }

    public class ProdCommentInfo
    {
        public string avg_scores { get; set; } //平均分數
        public string total_scores { get; set; } //評論星星加總
        public string click_count { get; set; }//點擊瀏覽數
        public string comment_record { get; set; }//評論總筆數
        public string keyword { get; set; }//商品網址關鍵字
        public string sales_qty { get; set; } //銷售量
        public string prod_url_oid { get; set; } //商品網址編號 url 的oid

    }

    public class Remind
    {
        //public int remind_xid { get; set; }//序號
        public string remind_desc { get; set; }//說明

    }

    public class Video
    {
        public int xid { get; set; }//序號
        public string lang_code { get; set; }//語言
        public string vidoe_url { get; set; }//url
    }

    public class CostDetail
    {
        public int detail_xid { get; set; }//序號
        public string detail_desc { get; set; }//說明
        public string detail_type { get; set; }//包含/不包含 (INC/NO_INC)
    }

    public class TktExpire
    {
        public string exp_type { get; set; }//效期選項
        public string exp_open_date { get; set; }//開票幾日/月/年有效
        public string exp_s_date { get; set; }//起始區間
        public string exp_e_date { get; set; }//結束區間
    }

    public class MeetingPoint
    {
        //public int time_id { get; set; }//集合地點時間序號
        public string airport_code { get; set; }//機場代碼
        public string terminal { get; set; }//航廈
        public string meeting_point { get; set; }//集合地點
        public string img_url { get; set; }//集合地圖片url

    }


    public class Location
    {
        public string id { get; set; }
        public List<BusinessHour> businessHours { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string note { get; set; }

    }

    public class From
    {
        public int hour { get; set; }
        public int minute { get; set; }
    }

    public class To
    {
        public int hour { get; set; }
        public int minute { get; set; }
    }

    public class BusinessHour
    {
        public string weekDays { get; set; }
        public From from { get; set; }
        public To to { get; set; }
    }

    public class ProdMarketing
    {
        public bool is_ec_show { get; set; }
        public bool is_ec_sale { get; set; }
        public string purchase_type { get; set; }
        public string purchase_type_name { get; set; }
        public bool is_search { get; set; }
        public bool is_show { get; set; }
    }

    public class MeetingPointMap
    {
        public string mapAddress { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string zoomLevel { get; set; }
        public string imageUrl { get; set; }
    }
}
