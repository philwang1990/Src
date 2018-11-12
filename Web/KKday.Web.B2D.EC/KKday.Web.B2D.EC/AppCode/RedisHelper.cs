using System;
using StackExchange.Redis;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace KKday.Web.B2D.EC.AppCode
{
    public static class RedisHelper
    {
        public static Dictionary<string, string> getuiKey(string lang)
        {

            Dictionary<string, string> uikey = getKlingon("frontend", lang);
            Dictionary<string, string> uikey2 = getKlingon("system", lang);

            foreach (var key in uikey2)
            {
                if (!uikey.ContainsKey(key.Key)) uikey.Add(key.Key, key.Value);
            }

            return uikey;
        }

        //挖字專用
        private  static Dictionary<string,string> getKlingon(string webType,string lang )
        {
            try
            {
                //kkredis  
                //string kkredis = Website.Instance.Configuration["RedisLang:SettinRedis_kkday"];
                //ConnectionMultiplexer kkrds = ConnectionMultiplexer.Connect(kkredis);
                //IDatabase db = kkrds.GetDatabase();

                string klingon = "";

                if(webType== "frontend")
                {

                   // klingon = db.StringGet($"common:uiLangList:{webType}:{lang}");
                    klingon = System.IO.File.ReadAllText(@"/Users/zhangfengrong/Documents/Core_Test/KKday.Web.B2D.EC_ok/KKday.Web.B2D.EC/wwwroot/js/kling.txt");

                }
                else 
                {
                    //klingon = db.StringGet($"common:uiLangList:{webType}:{lang}");
                    klingon = "{\"general_find_more\":\"查看更多\",\"test_test_11\":\"dd\",\"test_test_44\":\"4422\",\"api_S007\":\"密碼與確認密碼不同\",\"api_S008\":\"目前不能使用忘記密碼修改密碼\",\"api_S006\":\"原密碼錯誤\",\"api_S103\":\"驗證碼錯誤\",\"api_S102\":\"帳號被鎖定\",\"api_S004\":\"供應商帳號不存在\",\"api_S101\":\"帳號未啟用\",\"api_S005\":\"啟用碼錯誤\",\"api_S105\":\"帳號已經啟用\",\"api_S104\":\"密碼錯誤\",\"api_S011\":\"供應商使用者帳號與Email不符\",\"api_S014\":\"供應商使用者時區不存在\",\"api_S013\":\"供應商使用者資料的供應商編號與傳入的供應商編號不符\",\"api_S002\":\"供應商帳號已存在\",\"api_S001\":\"供應商登入驗證失敗\",\"api_S003\":\"供應商帳號舊密碼不正確\",\"api_S016\":\"自己不能刪除自己\",\"api_S015\":\"操作權限不足\",\"api_S012\":\"供應商資料未審核通過\",\"api_T105\":\"訂單已經完成交易\",\"api_T201\":\"訂單非取消狀態\",\"api_T107\":\"訂單狀態不正確\",\"api_T106\":\"訂單人數不符\",\"api_T103\":\"交易記錄狀態已有回應\",\"api_T702\":\"免收手續費，但無填寫原因\",\"api_T001\":\"供應商登入驗證失敗\",\"api_T104\":\"無法檢示，無權限查看\",\"api_T101\":\"訂單資料不存在\",\"api_T102\":\"無訂單可供取消\",\"api_T701\":\"傳入取消百分比錯誤\",\"api_0010\":\"api key 已失效\",\"api_0012\":\"版本錯誤\",\"api_0023\":\"此欄位不能接受此值\",\"api_0021\":\"會員或供應商值不能都為空\",\"api_0011\":\"api key 與 userOid 不符\",\"api_0020\":\"無此API 權限\",\"api_0000\":\"正確\",\"api_0014\":\"輸入資料不完全，提示未輸入欄位\",\"api_0024\":\"狀態錯誤不得執行\",\"api_9999\":\"系統異常\",\"api_0013\":\"傳入參數錯誤\",\"api_0022\":\"會員或供應商不能都有值\",\"api_M007\":\"查無此帳號，請檢查是否有誤\",\"api_M021\":\"會員啟動碼錯誤\",\"api_M014\":\"會員帳號未啟用\",\"api_M003\":\"會員舊密碼不正確\",\"api_M011\":\"會員使用者帳號與Email不符\",\"api_M008\":\"驗證碼錯誤，會員重設密碼失敗\",\"api_M010\":\"會員輸入資料不完成\",\"api_M016\":\"驗證碼過期\",\"api_M002\":\"此帳號己注冊過\",\"api_M005\":\"感謝您註冊KKDAY,請驗證您的Email訊息,請於申請24小時內點擊email裡的網頁 (URL)進行身份驗證後執行密碼重新設定流程\",\"api_M020\":\"會員已啟用\",\"api_M015\":\"會員帳號為停用\",\"api_M013\":\"此商品己收藏過\",\"api_M051\":\"重覆評論\",\"api_M030\":\"該商品已在收藏清單\",\"api_M006\":\"無此e-mail，請檢查是否有誤\",\"api_M004\":\"會員重設密碼失敗\",\"api_M012\":\"無此收藏紀碌，刪除失敗\",\"api_M001\":\"會員登入驗證失敗\",\"api_M009\":\"因您的電子郵件信箱已經啟用過 ，KKday將寄送重設密碼連結的郵件到您註冊的信箱 。\",\"api_M050\":\"不允許評論\",\"api_C004\":\"信息的供應商User資料不符\",\"api_C001\":\"查無此信息\",\"api_C003\":\"信息的供應商資料不符\",\"api_C005\":\"非會員訊息不允許回覆\",\"api_C002\":\"信息的會員資料不符\",\"api_P009\":\"該集合的資料商品編號不符\",\"api_P023\":\"商品集合資料不存在\",\"api_P001\":\"商品資料不存在\",\"api_P012\":\"該videoOid的資料商品編號不符\",\"api_P019\":\"超出tag2上限數量\",\"api_P017\":\"找不到該筆評論\",\"api_P002\":\"商品狀態不正確\",\"api_P024\":\"該集合的資料商品編號不符\",\"api_P013\":\"商品PHOTO資料不存在\",\"api_P018\":\"超出tag1上限數量\",\"api_P008\":\"商品集合資料不存在\",\"api_P030\":\"opType參數錯誤\",\"api_P025\":\"商品行程資料不存在\",\"api_P014\":\"該imgOid的資料并非為PHOTO資料\",\"api_P021\":\"該集合的型態不會有出發地\",\"api_P010\":\"商品video資料不存在\",\"api_P003\":\"新建立語系必須為預設語系\",\"api_P028\":\"商品行程天數大於旅游天數\",\"api_P007\":\"商品集合資料己經存在\",\"api_P015\":\"該imgOid的資料商品編號不符\",\"api_P029\":\"該scheOid的資料商品編號不符\",\"api_P026\":\"商品行程時間格式錯誤\",\"api_P022\":\" 商品集合資料己經存在\",\"api_P004\":\"商品已刪除\",\"api_P006\":\"商品行程天數不允許修改\",\"api_P011\":\" 該videoOid的資料并非為video資料\",\"api_P027\":\"商品行程天數格式錯誤\",\"api_P005\":\"商品行程資料己經存在\",\"api_P016\":\"訂單己評論過\",\"api_P031\":\"產品已經上架\",\"api_P042\":\"uikey DATA無資料\",\"api_P034\":\"商品審核中不允許修改\",\"api_P036\":\"商品不属於此供應商\",\"api_P035\":\"GATHER_TYPE參數錯誤\",\"api_P041\":\"商品shareType參數錯誤\",\"api_P032\":\"產品已經下架\",\"api_P040\":\"商品時區未設定\",\"api_P037\":\"產品未審查通過\",\"api_P043\":\"商品上下架日期已過\",\"api_P039\":\"商品videoType參數錯誤\",\"api_P038\":\"國家城市不存在或不可存取\",\"api_P033\":\"產品目前審核中或審核通過\",\"api_P110\":\"傳入的出發日期不介於套餐日期之間\",\"api_P106\":\"套餐資料已刪除\",\"api_P102\":\"套餐商品定價類型不存在\",\"api_P109\":\"月曆資料不存在\",\"api_P104\":\"商品費用語系包含/不包含資料不存在\",\"api_P103\":\"商品費用包含/不包含資料不存在\",\"api_P108\":\"非套餐資料不允許新增多筆資料\",\"api_P101\":\"套餐資料不存在\",\"api_P111\":\"該套餐日期不允許銷售\",\"api_P107\":\"非套餐資料不允許修改商品名稱\",\"api_P105\":\"該套餐不屬於此商品\",\"api_CN03\":\"COUPON條件不符合\",\"api_CN10\":\"商品地區不符合\",\"api_CN02\":\"COUPON不可以重覆使用\",\"api_CN09\":\"供應商不符合\",\"api_CN01\":\"COUPON資料不存在\",\"api_CN12\":\"會員編號不符合\",\"api_CN08\":\"商品TAG2不符合\",\"api_CN04\":\"付款方式不符合\",\"api_CN13\":\"訂單金額不符合\",\"api_CN05\":\"信用卡前六碼不符合\",\"api_CN11\":\"商品編號不符合\",\"api_CN07\":\"商品TAG1不符合\",\"api_CN06\":\"商品大分類不符合\",\"form_validation_is_numeric\":\"The %s field must contain only numeric characters.\",\"form_validation_numeric\":\"The %s field must contain only numbers.\",\"form_validation_exact_length\":\"The %s field must be exactly %s characters in length.\",\"form_validation_regex_match\":\"The %s field is not in the correct format.\",\"form_validation_alpha_numeric\":\"The %s field may only contain alpha-numeric characters.\",\"form_validation_min_length\":\"The %s field must be at least %s characters in length.\",\"form_validation_is_natural\":\"The %s field must contain only positive numbers.\",\"form_validation_greater_than\":\"The %s field must contain a number greater than %s.\",\"form_validation_decimal\":\"The %s field must contain a decimal number.\",\"form_validation_valid_ip\":\"The %s field must contain a valid IP.\",\"form_validation_valid_email\":\"The %s field must contain a valid email address.\",\"form_validation_integer\":\"The %s field must contain an integer.\",\"form_validation_alpha\":\"The %s field may only contain alphabetical characters.\",\"form_validation_is_natural_no_zero\":\"The %s field must contain a number greater than zero.\",\"form_validation_isset\":\"The %s field must have a value.\",\"form_validation_valid_url\":\"The %s field must contain a valid URL.\",\"form_validation_valid_emails\":\"The %s field must contain all valid email addresses.\",\"form_validation_alpha_dash\":\"The %s field may only contain alpha-numeric characters, underscores, and dashes.\",\"form_validation_max_length\":\"The %s field can not exceed %s characters in length.\",\"form_validation_less_than\":\"The %s field must contain a number less than %s.\",\"form_validation_matches\":\"The %s field does not match the %s field.\",\"api_P201\":\"此套餐未設定價格\",\"api_P200\":\"此套餐日期未設定\",\"api_P203\":\"此套餐銷售結束日期+訂購前置日小於今天，無法上架\",\"marketing_system_import_failed\":\"\",\"marketing_system_no_data_input\":\"\",\"common_weekday_5\":\"星期六\",\"general_weekday_0\":\"星期一\",\"general_weekday_4\":\"星期五\",\"general_weekday_2\":\"星期三\",\"common_find_more\":\"Find out more\",\"common_info\":\"行程介紹\",\"common_weekday_4\":\"星期五\",\"common_weekday_0\":\"星期一\",\"common_weekday_1\":\"星期二\",\"common_weekday_6\":\"星期日\",\"common_weekday_2\":\"星期三\",\"common_weekday_3\":\"星期四\",\"general_info\":\"系統維護公告\",\"general_weekday_6\":\"星期日\",\"general_weekday_5\":\"星期六\",\"general_weekday_1\":\"星期二\",\"general_weekday_3\":\"星期四\",\"api_P207\":\"此套餐資料未設定完整\",\"payment_pmch_name_FUBON_CREDITCARD\":\"信用卡付款\",\"common_VT02\":\"電子憑證\",\"common_VT01\":\"電子憑證將email至信箱，請自行列印\",\"common_VT03\":\"Voucher Not Required 出示護照即可\",\"common_VT04\":\"請憑兌換憑證＋訂購旅客護照正本\",\"common_VT07\":\"出示訂單編號入場即可\",\"common_VT05\":\"請在指定兌換地點將列印出來的兌換確認單兌換成實體票券\",\"payment_pmch_info_process_notice\":\"交易進行中，請勿關閉或重整頁面。\",\"form_validation_required\":\"此欄位必填\",\"payment_pmch_name_HK_PAYPAL_CREDITCARD_HKD\":\"信用卡付款\",\"product_category_tag_1\":\"Activities\",\"product_category_tag_1_1\":\"Water Activities\",\"product_category_tag_1_2\":\"Outdoor Activities\",\"product_category_tag_1_3\":\"Adventure Activities\",\"product_category_tag_2\":\"Attractions & Shows\",\"product_category_tag_2_1\":\"Sightseeing Tickets & Passes\",\"product_category_tag_2_2\":\"Museums & Galleries\",\"product_category_tag_2_3\":\"Shows & Performances\",\"product_category_tag_2_4\":\"Sporting Events\",\"product_category_tag_2_5\":\"Theme Parks\",\"product_category_tag_3\":\"Experiences\",\"product_category_tag_3_1\":\"Workshops & Classes\",\"product_category_tag_3_2\":\"Local Experiences\",\"product_category_tag_3_3\":\"Health & Welness\",\"product_category_tag_3_4\":\"Unique Experiences\",\"product_category_tag_3_5\":\"Seasonal experiences\",\"product_category_tag_4\":\"Tours\",\"product_category_tag_4_1\":\"City Sightseeing\",\"product_category_tag_4_2\":\"Arts, Culture & History\",\"product_category_tag_4_3\":\"Food, Drink & Nightlife\",\"product_category_tag_4_4\":\"Shopping\",\"product_category_tag_4_5\":\"Private & Custom Tours\",\"product_category_tag_4_6\":\"Day trips & Excursions\",\"product_category_tag_4_7\":\"Multi-day tours\",\"product_category_tag_5\":\"Transportation & Necesseties\",\"product_category_tag_5_1\":\"Ground Transportation & Transfers\",\"product_category_tag_5_2\":\"Ferries & Cruises\",\"product_category_tag_5_3\":\"Private Charter\",\"product_category_tag_5_4\":\"Tickets & Passes\",\"product_category_tag_5_5\":\"Vehicle Rentals\",\"product_category_tag_5_6\":\"Wifi & SIM Card\",\"product_category_tag_5_7\":\"Insurance\",\"product_category_tag_6\":\"Reservations & Vouchers\",\"product_category_tag_6_1\":\"Dining\",\"product_category_tag_6_2\":\"Accommodation\",\"joy_joy_joy\":\"3335556666\",\"payment_pmch_name_CITI_CREDITCARD_3D_SSL\":\"信用卡付款\",\"payment_pmch_name_CITI_CREDITCARD_3D\":\"信用卡付款\",\"payment_pmch_name_CITI_CREDITCARD\":\"信用卡付款\",\"payment_pmch_info_01\":\"其他說明：持台灣發卡銀行的信用卡刷卡，不需加收國外交易手續費\",\"payment_pmch_info_02\":\"\",\"payment_pmch_name_CITI_VN_CREDITCARD\":\"信用卡付款\",\"payment_pmch_name_CITI_MO_CREDITCARD\":\"信用卡付款\",\"payment_pmch_info_03\":\"KKday不收取任何交易手續費用或附加費用。如您發現被收取交易手續費，請洽詢您的發卡銀行。\",\"payment_pmch_info_02_hkd\":\"持香港發卡銀行的信用卡刷卡，不需加收海外交易手續費。\",\"payment_pmch_info_02_twd\":\"持台灣發卡銀行的信用卡刷卡，不需加收海外交易手續費。\",\"payment_pmch_info_02_usd\":\"您將以 %s 支付，您的總費用為 %s 。\",\"payment_pmch_name_CITI_MO_CREDITCARD_3D\":\"信用卡付款\",\"payment_pmch_info_price_detail\":\"您將以 %s 支付，您的總費用為 %s 約等值 %s 。\",\"payment_pmch_info_remind_hkd\":\"持香港發卡銀行的信用卡刷卡，不需加收海外交易手續費。\",\"payment_pmch_info_remind_twd\":\"持台灣發卡銀行的信用卡刷卡，不需加收海外交易手續費。\",\"payment_pmch_info_remind_usd\":\"您將以 %s 支付，您的總費用為 %s 。\",\"payment_pmch_info_fee_remind\":\"KKday不收取任何交易手續費用或附加費用。如您發現被收取交易手續費，請洽詢您的發卡銀行。\",\"payment_pmch_name_HK_PAYPAL_CREDITCARD_USD\":\"信用卡付款\",\"payment_pmch_name_ANDROID_PAY\":\"Android Pay\",\"payment_pmch_name_HK_PAYPAL_HKD\":\"PayPal\",\"payment_pmch_name_HK_PAYPAL_USD\":\"PayPal\",\"payment_pmch_name_PAYPAL\":\"PayPal\",\"payment_pmch_name_CITI_CREDITCARD_3D_FRAUD_PROD\":\"信用卡付款\",\"form_validation_length_between\":\"輸入長度必須介於%s與%s之間\",\"payment_pmch_name_FUBON_CREDITCARD_3D\":\"信用卡付款\",\"common_VT08\":\"請憑電子憑證，並出示旅客資料正本\",\"common_VT09\":\"請直接出示QR Code\",\"payment_pmch_name_TW_LINEPAY_TWD\":\"LINE Pay\"}";
                }
                //fronEnd = System.IO.File.ReadAllText(@"/Users/zhangfengrong/Documents/Core_Test/KKday.Web.B2D.EC_ok/KKday.Web.B2D.EC/wwwroot/js/kling.txt");


                if (klingon == null)
                {
                    //重新reflash klingon
                    //再取一次
                    //mod_commmon  lang_ui refreshUiLang2Redis 

                }
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(klingon);

                return values;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public static string GetPayDtlfromRedis(string orderMid)
        //{
        //    //kkredis  
        //    string kkredis = Website.Instance.Configuration["RedisLang:SettinRedis_kkday"];
        //    ConnectionMultiplexer kkrds = ConnectionMultiplexer.Connect(kkredis);
        //    IDatabase db = kkrds.GetDatabase();
        //    string payDtl = db.StringGet("b2d:ec:payDtl:" + orderMid);
        //    return payDtl;
        //}


        //存到自己的 redis
        public static void SetProdInfotoRedis(string obj ,string redisKey,int expireMinute )
        {
            string kkredis = Website.Instance.Configuration["RedisLang:SettinRedis_bid"];
            ConnectionMultiplexer kkrds = ConnectionMultiplexer.Connect(kkredis);
            IDatabase db = kkrds.GetDatabase();
            db.StringSet(redisKey, obj, TimeSpan.FromMinutes(expireMinute));
        }

        public static string getProdInfotoRedis( string redisKey)
        {
            string kkredis = Website.Instance.Configuration["RedisLang:SettinRedis_bid"];
            ConnectionMultiplexer kkrds = ConnectionMultiplexer.Connect(kkredis);
            IDatabase db = kkrds.GetDatabase();

            string obj = db.StringGet(redisKey);
            return obj;
        }

    }
}
