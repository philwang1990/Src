using System;
namespace KKday.Web.B2D.EC.Models.Model.Product
{
    public class ProdTitleModel
    {
        //-------------product
        public string common_per_person { get; set; } //最低價 
        public string product_index_choose_option { get; set; }//立即預訂
        public string product_index_product_oid { get; set; } //商品編號 繁體中文(台灣)
        public string product_index_see_more_photo { get; set; } //查看圖片
        public string common_duration { get; set; } //所需時間
        public string common_location { get; set; } //地點位置
        public string common_guide_can_speak { get; set; } //導覽語言
        public string product_index_option_title { get; set; } //套餐選項
        public string product_index_select_date { get; set; } //請選擇使用日
        public string product_index_select { get; set; } //選擇
        public string product_index_option_detail { get; set; } //option_detail
        public string common_experience { get; set; } //行程說明
        public string common_timetable { get; set; } //行程表
        public string common_timetable_day { get; set; } //第%s天
        public string product_index_event { get; set; } //場次

        public string common_number_of_comment { get; set; } //%s則評論
        public string product_index_visitor { get; set; } //%s位旅客瀏覽過此行程

        public string product_index_price_detail { get; set; } //費用細節
        public string product_index_voucher_type { get; set; } //憑證類型
        public string product_index_place { get; set; } //兌換地點名稱
        public string common_reminder { get; set; } //注意事項
        public string common_cancellation_policy { get; set; } //取消政策
        public string common_map { get; set; } //地圖
        public string common_experience_point { get; set; } //主要目的地
        public string common_special_cancel_fee { get; set; } //購買即視為同意取消政策
        public string common_cancel_policy_explanation { get; set; } //取消時間依照商品時區決定
        public string common_cancel_date { get; set; } //取消日期
        public string common_cancellation_fee { get; set; } //取消費用

        public string common_days_more { get; set; } //天以前
        public string common_days_prior { get; set; } //天
        public string product_index_refundable { get; set; } //全額退款
        public string product_index_non_refundable { get; set; } //不可取消，無法退款


        //booking bar 
        public string common_booking { get; set; } //立即訂購
        public string product_index_coming_soon { get; set; } //即將開賣


        //金額有關
        public string common_total { get; set; } //總金額
        public string common_adult { get; set; } //成人
        public string common_child { get; set; } //兒童
        public string common_infant { get; set; }//嬰兒
        public string common_elder { get; set; } //老人

        //單位
        public string product_index_unit2_01 { get; set; }//每人
        public string product_index_unit2_02 { get; set; }//每張
        public string product_index_unit2_03 { get; set; }//每輛
        public string product_index_unit2_04 { get; set; }//每套
        public string product_index_unit2_05 { get; set; }//每間
        public string product_index_unit2_06 { get; set; }//每天

        //警示
        public string pkgShowChiQtyInfo { get; set; }
        public string product_index_min_event_qty_alert { get; set; } //目前場次數量僅剩%s場
        public string product_index_min_order_adult_qty_alert { get; set; } //成人加老人至少%s人
        public string product_index_min_order_qty_alert { get; set; } //訂購人數至少%s人
        public string product_index_max_order_qty_alert { get; set; } //訂購人數至多%s人

        //product_index_confirm_hr_48 2個工作日內回覆訂購結果
        //product_index_asia_mile_valid} //可累積亞洲萬里通里數

        public string product_index_has_been_sold_out { get; set; }//已售罄
        public string product_index_check_availablity { get; set; }//選擇其他日期

        //地圖相關
        public string common_meeting_point { get; set; }


        //-------------booking

        public string common_confirmation { get; set; } //填寫資料
        public string common_payment { get; set; } //付款
        public string common_done { get; set; } //訂購完成

        //訂購人資料
        public string booking_step1_contact { get; set; } //訂購人資料
        public string booking_step1_contact_firstname { get; set; } //訂購人名
        public string booking_step1_contact_lastname { get; set; } //訂購人姓
        public string booking_step1_local_firstname_placeholder { get; set; } //例：明寧
        public string booking_step1_local_lastname_placeholder { get; set; } //例：陳
        public string common_nationality { get; set; } //國籍
        public string booking_step1_contact_tel { get; set; } //聯絡電話
        public string booking_step1_contact_email { get; set; } //電子郵件信箱
        public string booking_step1_update_profile { get; set; } //同時更新會員資料 ( 應該不會用）
        public string common_next_step { get; set; } //下一步

        //旅客資料
        public string booking_step1_traveler_information { get; set; } //旅客資料
        public string booking_step1_lead_traveler { get; set; } //旅客代表人
        public string booking_step1_custom { get; set; } //旅客
        public string booking_step1_chose_contacted_member { get; set; } //請選擇常用聯絡人 ( 應該不會用）
        public string booking_step1_cust_data_passport_english_firstname { get; set; } //旅客護照名(英文)
        public string booking_step1_cust_data_passport_english_lastname { get; set; } //旅客護照姓(英文)
        public string booking_step1_cus_cusGender { get; set; } //性別
        public string booking_step1_cust_data_passport_english_lastname_placeholder { get; set; } //例：Chen
        public string booking_step1_cust_data_passport_english_firstname_placeholder { get; set; } //例：Ming Ni


        public string common_select_set { get; set; } //請選擇
        public string common_male { get; set; } //男性
        public string common_female { get; set; } //女性

        public string booking_step1_cus_cusBirthday { get; set; } //出生日期

        public string booking_step1_cus_countryName { get; set; } //請選擇國籍
        public string booking_step1_cus_countryName_placeholder { get; set; } //請選擇國籍

        public string booking_step1_cus_passportId { get; set; }//護照號碼
        public string booking_step1_cus_passportId_placeholder { get; set; } //請輸入護照號碼

        public string booking_step1_cust_data_passport_exp_date { get; set; }//護照有效日期

        public string booking_step1_cust_data_local_firstname { get; set; } //本國名
        public string booking_step1_cust_data_local_lastname { get; set; } //本國姓
        public string booking_step1_cust_data_local_firstname_placeholder { get; set; } //例：明寧
        public string booking_step1_cust_data_local_lastname_placeholder { get; set; }//例：陳


        public string booking_step1_cust_data_tw_identity_number { get; set; }//台灣身分證字號
        public string booking_step1_cust_data_tw_identity_number_placeholder { get; set; }//請輸入台灣身分證字號

        public string booking_step1_cust_data_hk_mo_identity_number { get; set; }//回鄉證號
        public string booking_step1_cust_data_hk_mo_identity_number_placeholder { get; set; } //請輸入回鄉證號

        public string booking_step1_cust_data_mtp_number { get; set; } //台胞證字號
        public string booking_step1_cust_data_mtp_number_placeholder { get; set; } //請輸入台灣身分證字號


        public string booking_step1_cust_data_height { get; set; } //旅客身高
        public string booking_step1_cust_data_height_unit_01 { get; set; } //公分
        public string booking_step1_cust_data_height_unit_02 { get; set; } //英呎
        public string booking_step1_cust_data_unit { get; set; } //單位

        public string booking_step1_cust_data_weight { get; set; } //旅客體重
        public string booking_step1_cust_data_weight_unit_01 { get; set; } //公斤
        public string booking_step1_cust_data_weight_unit_02 { get; set; } //磅


        public string booking_step1_cust_data_shoe_size { get; set; } //旅客鞋碼
        public string booking_step1_cust_data_shoe_size_placeholder { get; set; } //請選擇旅客鞋碼
        public string booking_step1_cust_data_shoe_size_man { get; set; } //成人男性鞋碼
        public string booking_step1_cust_data_shoe_size_woman { get; set; } //成人女性鞋碼
        public string booking_step1_cust_data_shoe_size_child { get; set; } //兒童鞋碼
        public string booking_step1_cust_data_shoe_size_tip { get; set; } //如腳掌長低於19CM, 請選擇兒童鞋碼

        public string booking_step1_cust_data_glass_diopter { get; set; } //眼鏡度數
        public string booking_step1_cust_data_glass_diopter_placeholder { get; set; }//請選擇眼鏡度數
        public string booking_step1_cust_data_do_not_need { get; set;} //不需要


        public string booking_step1_cust_data_meal { get; set; } //選擇餐食
        public string booking_step1_cust_data_meal_placeholder { get; set; } //請選擇餐食
        public string booking_step1_cust_data_exclude_food { get; set; } //其他不吃的食物
        public string booking_step1_cust_data_meal_tip { get; set; } //此餐食選項會盡量幫您安排
        public string booking_step1_cust_data_is_food_allergy { get; set; }//是否有過敏食物


        public string booking_step1_save_member_data { get; set; } //儲存聯絡人資料 (應該不會用)
        public string common_guide_lang { get; set; } //導覽語言
        public string booking_step1_product_guide_lang { get; set; } //行程導覽語言

        public string booking_step1_shuttle_data { get; set; } //接送資料
        public string booking_step1_shuttle_data_shuttle_date { get; set; } //接送日期
        public string booking_step1_shuttle_data_pick_up_location { get; set; } //上車地點
        public string booking_step1_shuttle_data_drop_off_location { get; set; } //下車地點
        public string booking_step1_shuttle_data_pick_up_location_placeholder { get; set; } //旅館請務必提供「旅館名稱」和「旅館地址
        public string booking_step1_shuttle_data_drop_off_location_placeholder { get; set; } //旅館請務必提供「旅館名稱」和「旅館地址」

        public string booking_step1_order_note { get; set; } //訂單備註
        public string booking_step1_order_note_tip { get; set; } //備註事項

        //折扣
        public string booking_step1_use_coupon { get; set; } //使用折扣卷 ( 應該不會用）
        public string booking_step1_have_discount_code { get; set; } //我有折扣券( 應該不會用）
        public string booking_step1_dont_use { get; set; } //不使用( 應該不會用( 應該不會用）
        public string booking_step1_coupon_code { get; set; } //請輸入折扣券代碼( 應該不會用）
        public string booking_step1_btn_coupon { get; set; } //使用折扣碼( 應該不會用）

        //付款
        public string booking_step1_please_select_payment_method { get; set; } //請選擇付款方式
        public string payment_pmch_name_CITI_CREDITCARD { get; set; } //信用卡付款
        public string booking_step1_amount_of_money { get; set; } //支付金額 TWD 2,049 要組合
        public string payment_pmch_info_remind_twd { get; set; } //持台灣發卡銀行的信用卡刷卡，不需加收海外交易手續費
        public string payment_pmch_info_remind_hkd { get; set; } //持香港發卡銀行的信用卡刷卡，不需加收海外交易手續費
        public string payment_pmch_info_remind_usd { get; set; } //您將以 %s 支付，您的總費用為 %s 

        public string payment_pmch_info_fee_remind { get; set; } //KKday不收取任何交易手續費用或附加費用。如您發現被收取交易手續費，請洽詢您的發卡銀行
        public string common_card_holder_name { get; set; } //持卡人姓名
        public string booking_step1_enter_card_holder_name { get; set; } //請輸入持卡人姓名
        public string common_credit_card_num { get; set; } //信用卡號碼
        public string common_expire_date { get; set; } //有效期限

        public string common_next { get; set; } //確認送出

        public string booking_step1_flight_info_arrival_airport { get; set; } //抵達機場
        public string booking_step1_flight_info_arrival_info { get; set; } //抵達 %s 班機資訊
        public string booking_step1_flight_info_arrival_airport_placeholder { get; set; }//請選擇抵達機場
        public string booking_step1_flight_info_terminal_no { get; set; } //航廈
        public string booking_step1_flight_info_terminal_no_placeholder { get; set; } //請輸入航廈 (例 2航廈)
        public string booking_step1_flight_info_airline { get; set; } //航空公司
        public string booking_step1_flight_info_airline_placeholder { get; set; }//請輸入航空公司名稱
        public string booking_step1_flight_info_flight_no { get; set; } //航班編號
        public string booking_step1_flight_info_flight_no_placeholder { get; set; }//請輸入航班編號 (例 CI-123)
        public string booking_step1_flight_info_arrival_time { get; set; } //航班抵達時間
        public string booking_step1_flight_info_arrival_time_placeholder { get; set; }//請選擇抵達日期
        public string booking_step1_is_visa_required { get; set; } //是否需要辦理落地簽證
        public string common_yes { get; set; }//是
        public string common_no { get; set; } //否

        //flight
        public string booking_step1_flight_info_departure_info { get; set; } //離開 %s 班機資訊
        public string booking_step1_flight_info_departure_airport { get; set; } //出發機場
        public string booking_step1_flight_info_departure_airport_placeholder { get; set; } //請選擇出發機場
        public string booking_step1_flight_info_departure_time { get; set; }//航班出發時間
        public string booking_step1_flight_info_departure_time_placeholder { get; set; }//請選擇出發日期
        public string booking_step1_flight_info_flight_type { get; set; } //航班類型
        public string booking_step1_flight_info_flight_type_placeholder { get; set; } //請選擇航班類型
        public string booking_step1_flight_info_domestic_routes { get; set; } //國內線
        public string booking_step1_flight_info_international_routes { get; set; } //國際線
        public string common_hr { get; set; } //時
        public string common_min { get; set; } //分

        //shuttle
        public string booking_step1_shuttle_data_shuttle_date_placeholder { get; set; }//請選擇接送日期
        public string booking_step1_shuttle_data_pick_up_time { get; set; } //接送時間
        public string booking_step1_shuttle_data_pick_up_time_placeholder { get; set; }//請選擇接送時間
        public string booking_step1_shuttle_data_designated_location { get; set; }//接送地點
        public string booking_step1_shuttle_data_designated_location_placeholder { get; set; }//請選擇接送地點

        //rentCar
        public string booking_step1_shuttle_data_charter_route { get; set; } //包車路線
        public string booking_step1_shuttle_data_charter_route_placeholder { get; set; }//請選擇包車路線
        public string booking_step1_shuttle_data_custom_routes { get; set; } //自訂行程
        public string booking_step1_shuttle_data_custom_routes_placeholder { get; set; }//請輸入自訂行程地點
        public string common_add { get; set; } //add
        public string booking_step1_shuttle_data_custom_routes_note_1 { get; set; } //本產品最多可輸入
        public string booking_step1_shuttle_data_custom_routes_note_2 { get; set; }//個自訂行程




        //wifi
        public string booking_step1_other_data { get; set; } //其他資料
        public string booking_step1_other_data_mobile_model_number { get; set; }//手機型號
        public string booking_step1_other_data_imei { get; set;  } //IMEI
        public string booking_step1_other_data_activation_date { get; set; } //開通日期
        public string booking_step1_other_data_activation_date_placeholder { get; set; } //請選擇開通日期


        //rentCar
        public string booking_step1_rent_car { get; set; } //租車資料
        public string booking_step1_rent_car_pick_up_office { get; set; }//取車地點
        public string booking_step1_rent_car_pick_up_office_placeholder { get; set; }//請選擇取車地點
        public string booking_step1_rent_car_pick_up_date { get; set; }//取車日期及時間
        public string booking_step1_rent_car_pick_up_date_placeholder { get; set; } //請選擇取車日期
        public string booking_step1_rent_car_is_need_free_wifi { get; set; } //是否需要免費Wifi機
        public string common_need { get; set; } //需要
        public string common_no_need { get; set; } //不需要  
        public string booking_step1_rent_car_is_need_free_gps { get; set; }//是否需要免費GPS
        public string booking_step1_rent_car_drop_off_office { get; set; } //還車地點
        public string booking_step1_rent_car_drop_off_office_placeholder { get; set; } //請選擇還車地點
        public string booking_step1_rent_car_drop_off_date { get; set; }//還車日期及時間
        public string booking_step1_rent_car_drop_off_date_placeholder { get; set; }//請選擇還車日期

        //carPsgr
        public string booking_step1_car_psgr { get; set; } //乘客資料
        public string booking_step1_car_psgr_carry_luggage_quantity { get; set; }//行李數量
        public string booking_step1_car_psgr_carry_luggage { get; set; } //手提行李 (20吋以下)
        public string booking_step1_car_psgr_checked_luggage { get; set; } //托運行李 (21吋以上)
        public string booking_step1_car_psgr_child_seat_quantity { get; set; }//兒童座椅數量
        public string booking_step1_car_psgr_suitable_for_age { get; set; }//適用年齡
        public string common_years_old { get; set; }//歲
        public string booking_step1_car_psgr_supplier_provided { get; set; }//店家提供
        public string booking_step1_car_psgr_self_provided { get; set; }//自備座椅
        public string booking_step1_car_psgr_infant_seat_quantity { get; set; }//嬰兒座椅數量
       
        public string booking_step1_send_data { get; set; } //寄送資料
        public string booking_step1_send_data_receiver_name { get; set; }//收件人姓名
        public string booking_step1_send_data_receiver_first_name { get; set; }//請輸入收件人名字
        public string booking_step1_send_data_receiver_firstname { get; set; } //收件人名
        public string booking_step1_send_data_receiver_firstname_placeholder { get; set; }//例：明寧
        public string booking_step1_send_data_receiver_lastname { get; set; } //收件人姓
        public string booking_step1_send_data_receiver_lastname_placeholder { get; set; }//例：陳

        public string booking_step1_send_data_receive_address { get; set; }//收件人地址

        public string booking_step1_send_data_receive_address_country { get; set; }//收件國家
        public string booking_step1_send_data_receive_address_country_placeholder { get; set; } //請選擇收件國家
        public string booking_step1_send_data_receive_address_city { get; set; }//收件城市
        public string booking_step1_send_data_receive_address_city_placeholder { get; set; } //請選擇收件城市
    
        public string booking_step1_send_data_zip_colde { get; set; } //郵遞區號(港澳區請填0)
        public string booking_step1_send_data_receive_address_detail { get; set; }//詳細地址
        public string booking_step1_send_data_receive_address_placeholder { get; set; } //請輸入收件地址,台灣地址請加上區名 ex:中山區南京北路二段
        public string booking_step1_send_data_receiver_tel { get; set; } //收件人電話
        public string booking_step1_send_data_receiver_tel_placeholder { get; set; } //請輸入聯絡電話

        public string booking_step1_send_data_hotel_name { get; set; }//寄送飯店名稱
        public string booking_step1_send_data_hotel_name_placeholder { get; set; } //請輸入寄送飯店名稱
        public string booking_step1_send_data_hotel_tel { get; set; }//飯店電話
        public string booking_step1_send_data_hotel_tel_placeholder { get; set; } //booking_step1_send_data_hotel_tel_placeholder
        public string booking_step1_send_data_hotel_address { get; set; } //飯店地址
        public string booking_step1_send_data_hotel_address_placeholder { get; set; } //請輸入寄送飯店地址
        public string booking_step1_send_data_buyer_passport_english_firstname { get; set; } //訂房人英文名(同護照)
        public string booking_step1_send_data_buyer_passport_english_firstname_placeholder { get; set; }//例：Ming Ni
        public string booking_step1_send_data_buyer_passport_english_lastname { get; set; }//訂房人英文姓(同護照)
        public string booking_step1_send_data_buyer_passport_english_lastname_placeholder { get; set; }//例：Chan
        public string booking_step1_send_data_buyer_local_firstname { get; set; } //訂房人名
        public string booking_step1_send_data_buyer_local_firstname_placeholder { get; set; }//例：明寧
        public string booking_step1_send_data_buyer_local_lastname { get; set; } //訂房人姓
        public string booking_step1_send_data_buyer_local_lastname_placeholder { get; set; } //例：陳

        public string booking_step1_send_data_booking_website { get; set; } //訂房網站
        public string booking_step1_send_data_booking_website_placeholder { get; set; }//請輸入訂房網站
        public string booking_step1_send_data_booking_order_no { get; set; } //訂房編號
        public string booking_step1_send_data_booking_order_no_placeholder { get; set; }//請輸入訂房編號

        public string booking_step1_send_data_check_in_date { get; set; }//入住日期
        public string booking_step1_send_data_check_in_date_placeholder { get; set; } //請選擇入住日期
        public string booking_step1_send_data_check_out_date { get; set; } //退房日期
        public string booking_step1_send_data_check_out_date_placeholder { get; set; }//請選擇退房日期


        //contactData
        public string booking_step1_contact_data { get; set; } //旅遊期間聯絡人
        public string booking_step1_contact_data_firstname { get; set; } //聯絡人英文名(同護照)
        public string booking_step1_contact_data_firstname_placeholder { get; set; }//例：Ming Ni

        public string booking_step1_contact_data_lastname { get; set; } //聯絡人英文姓(同護照)
        public string booking_step1_contact_data_lastname_placeholder { get; set; } //例：Chen 
        public string booking_step1_contact_data_contact_tel { get; set; } //旅遊期間聯絡電話
        public string common_have { get; set; } //有
        public string common_have_not { get; set; }//沒有
        public string booking_step1_contact_data_contact_tel_placeholder { get; set; }//請輸入聯絡電話
        public string booking_step1_contact_data_contact_app { get; set; } //APP聯絡方式

        public string booking_step1_contact_data_contact_app_placeholder { get; set; } //請選擇
        public string booking_step1_contact_data_contact_app_account { get; set; } //APP 帳號

        //exchange
        public string booking_step1_other_data_exchange_location { get; set; } //取票櫃檯
        public string booking_step1_other_data_exchange_location_placeholder { get; set; } //請選擇取票櫃檯

        //shuttle
        public string booking_step1_shuttle_data_customized_shuttle_time { get; set; } //自訂接送時間
        public string booking_step1_shuttle_data_customized_charter_route { get; set; } //我想要自訂行程

        //error
        public string booking_step1_length_error_1 { get; set; } //至少填入
        public string booking_step1_length_error_2 { get; set; } //個字
        public string booking_step1_required_error { get; set; }//此為必填欄位
        public string booking_step1_english_error { get; set; } //請使用英文輸入

    }



    //提醒
    public class Reminder
    {
        public string remindKey { get; set; }
        public string remindValue { get; set; }
    }

    //取消政策 （只限3）
    public class CancelPolicy
    {
        public int sort { get; set; }
        public int sd { get; set; }
        public int ed { get; set; }
        public string showRange { get; set; }
        public string showPercent { get; set; }
    }


}
