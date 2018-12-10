using System;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Model.Product;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using KKday.Web.B2D.BE.App_Code;

namespace KKday.Web.B2D.EC.Models.Repostory.Product
{
    public static class ProductRepostory
    {
        //查商品
        public static ProductforEcModel getProdDtl(long companyXid, string state, string lang, string currency, string prodoid, ProdTitleModel title)
        {
            try
            {
                return ApiHelper.getProdDtl(companyXid, state, lang, currency, prodoid, title);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"error-getprodDtl ex:{ex.ToString()}");
                throw new Exception("error-getprodDtl ex:" + ex.ToString());

            }
        }

        //查套餐
        public static PackageModel getProdPkg(long companyXid, string state, string lang, string currency, string prodoid, ProdTitleModel title)
        {
            try
            {
                return ApiHelper.getProdPkg(companyXid, state, lang, currency, prodoid, title);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"error-getProdPkg ex:{ex.ToString()}");
                throw new Exception("error-getProdPkg ex:" + ex.ToString());

            }
        }

        public static ProductforEcModel getProdOtherInfo(ProductforEcModel prod, string oid, string lang, string currency, Dictionary<string, string> uikey)
        {

            //所需時間 如果tourDays <1 就是用tourHours
            int tourDays = Convert.ToInt32(prod.days.ToString());
            int tourHours = Convert.ToInt32(prod.hours.ToString());
            prod.durationStr = (tourDays < 1) ? tourHours + uikey["common_hours"] : tourDays + uikey["common_days"];

            //確認時間
            int confirmHours = Convert.ToInt32(prod.confirm_order_time);
            switch (confirmHours)
            {
                case 0:
                    prod.confirmInfo = uikey["product_index_imm_confirm"]; break;
                case 24:
                    prod.confirmInfo = uikey["product_index_confirm_hr_24"]; break;
                case 48:
                    prod.confirmInfo = uikey["product_index_confirm_hr_48"]; break;
                case 72:
                    prod.confirmInfo = uikey["product_index_confirm_hr_72"]; break;
            }

            //取消政策 （在bookingbar中 ...)
            switch (prod.policy_no)
            {
                case "1":
                    prod.policyInfo = uikey["common_free_cancellation"]; break;
                case "2":
                    prod.policyInfo = uikey["common_no_refund"]; break;
                case "3":
                    prod.policyInfo = uikey["common_special_cancel_fee"]; break;//common_cancellation_policy
            }

            prod = setCancelPolicy(prod, uikey);
            return prod;

        }

        //取消政策條列化
        public static ProductforEcModel setCancelPolicy(ProductforEcModel prod, Dictionary<string, string> uiKey)
        {
            List<CancelPolicy> policyList = new List<CancelPolicy>();
            CancelPolicy policy = null;

            int i = 0;
            int preDate = 0;

            foreach (Policy p in prod.policy_list)
            {
                policy = new CancelPolicy();
                policy.sort = i;
                policy.ed = Convert.ToInt32(p.days);
                policy.sd = (i == 0) ? 0 : preDate + 1;

                if (p.is_over)
                {
                    policy.showRange = policy.ed + uiKey["common_days_more"].ToString();
                }
                else
                {
                    policy.showRange = $"{policy.sd}~{policy.ed}" + uiKey["common_days_prior"].ToString();
                }
                policy.showPercent = p.fee == 0 ? uiKey["product_index_refundable"].ToString() : p.fee == 100 ? uiKey["product_index_non_refundable"] : p.fee + "%";

                policyList.Add(policy);
                preDate = policy.ed;
                i++;
            }

            policyList = policyList.OrderByDescending(x => x.sort).ToList();
            prod.cancelPolicyList = policyList;

            return prod;
        }

        //預設標題挖字處理
        public static ProdTitleModel getProdTitle(Dictionary<string, string> uiKey)
        {
            ProdTitleModel title = new ProdTitleModel();

            title.common_per_person = uiKey["common_per_person"];
            title.product_index_product_oid = uiKey["product_index_product_oid"];
            title.product_index_see_more_photo = uiKey["product_index_see_more_photo"];
            title.common_duration = uiKey["common_duration"];
            title.common_location = uiKey["common_location"];
            title.common_guide_can_speak = uiKey["common_guide_can_speak"];
            title.product_index_option_title = uiKey["product_index_option_title"];
            title.product_index_select_date = uiKey["product_index_select_date"];
            title.product_index_option_detail = uiKey["product_index_option_detail"];
            title.common_experience = uiKey["common_experience"];
            title.common_timetable = uiKey["common_timetable"];
            title.product_index_price_detail = uiKey["product_index_price_detail"];
            title.product_index_voucher_type = uiKey["product_index_voucher_type"];
            title.product_index_place = uiKey["product_index_place"];
            title.common_reminder = uiKey["common_reminder"];
            title.common_cancellation_policy = uiKey["common_cancellation_policy"];
            title.common_map = uiKey["common_map"];
            title.common_experience_point = uiKey["common_experience_point"];
            title.common_special_cancel_fee = uiKey["common_special_cancel_fee"];
            title.product_index_select = uiKey["product_index_select"];
            title.common_cancel_policy_explanation = uiKey["common_cancel_policy_explanation"];
            title.common_cancel_date = uiKey["common_cancel_date"];
            title.common_cancellation_fee = uiKey["common_cancellation_fee"];

            title.common_days_more = uiKey["common_days_more"];
            title.common_days_prior = uiKey["common_days_prior"];

            title.product_index_event = uiKey["product_index_event"];


            //bookin bar
            title.common_booking = uiKey["common_booking"];
            title.product_index_coming_soon = uiKey["product_index_coming_soon"];

            //價錢
            title.common_total = uiKey["common_total"];
            title.common_adult = uiKey["common_adult"];
            title.common_child = uiKey["common_child"];
            title.common_infant = uiKey["common_infant"];
            title.common_elder = uiKey["common_elder"];

            //單位
            title.product_index_unit2_01 = uiKey["product_index_unit2_01"];
            title.product_index_unit2_02 = uiKey["product_index_unit2_02"];
            title.product_index_unit2_03 = uiKey["product_index_unit2_03"];
            title.product_index_unit2_04 = uiKey["product_index_unit2_04"];
            title.product_index_unit2_05 = uiKey["product_index_unit2_05"];
            title.product_index_unit2_06 = uiKey["product_index_unit2_06"];

            //套餐選擇
            title.product_index_has_been_sold_out = uiKey["product_index_has_been_sold_out"];
            title.product_index_check_availablity = uiKey["product_index_check_availablity"];



            title.common_confirmation = uiKey["common_confirmation"];
            title.common_payment = uiKey["common_payment"];
            title.common_done = uiKey["common_done"];

            title.booking_step1_contact = uiKey["booking_step1_contact"];
            title.booking_step1_contact_firstname = uiKey["booking_step1_contact_firstname"];
            title.booking_step1_contact_lastname = uiKey["booking_step1_contact_lastname"];

            title.booking_step1_local_firstname_placeholder = uiKey["booking_step1_local_firstname_placeholder"];
            title.booking_step1_local_lastname_placeholder = uiKey["booking_step1_local_lastname_placeholder"];

            title.booking_step1_cus_cusBirthday = uiKey["booking_step1_cus_cusBirthday"];
            title.booking_step1_cus_countryName = uiKey["booking_step1_cus_countryName"];
            title.booking_step1_cus_countryName_placeholder = uiKey["booking_step1_cus_countryName_placeholder"];

            title.common_nationality = uiKey["common_nationality"];
            title.booking_step1_contact_tel = uiKey["booking_step1_contact_tel"];
            title.booking_step1_contact_email = uiKey["booking_step1_contact_email"];
            title.booking_step1_update_profile = uiKey["booking_step1_update_profile"];
            title.common_next_step = uiKey["common_next_step"];

            title.booking_step1_traveler_information = uiKey["booking_step1_traveler_information"];
            title.booking_step1_lead_traveler = uiKey["booking_step1_lead_traveler"];
            title.booking_step1_chose_contacted_member = uiKey["booking_step1_chose_contacted_member"];
            title.booking_step1_cust_data_passport_english_firstname = uiKey["booking_step1_cust_data_passport_english_firstname"];

            title.booking_step1_cust_data_passport_english_lastname = uiKey["booking_step1_cust_data_passport_english_lastname"];
            title.booking_step1_cus_cusGender = uiKey["booking_step1_cus_cusGender"];
            title.booking_step1_cust_data_passport_english_lastname_placeholder = uiKey["booking_step1_cust_data_passport_english_lastname_placeholder"];
            title.booking_step1_cust_data_passport_english_firstname_placeholder = uiKey["booking_step1_cust_data_passport_english_firstname_placeholder"];

            title.booking_step1_cus_passportId = uiKey["booking_step1_cus_passportId"];
            title.booking_step1_cus_passportId_placeholder = uiKey["booking_step1_cus_passportId_placeholder"];

            title.booking_step1_cust_data_passport_exp_date = uiKey["booking_step1_cust_data_passport_exp_date"];

            title.booking_step1_cust_data_local_firstname = uiKey["booking_step1_cust_data_local_firstname"];
            title.booking_step1_cust_data_local_firstname_placeholder = uiKey["booking_step1_cust_data_local_firstname_placeholder"];
            title.booking_step1_cust_data_local_lastname = uiKey["booking_step1_cust_data_local_lastname"];
            title.booking_step1_cust_data_local_lastname_placeholder = uiKey["booking_step1_cust_data_local_lastname_placeholder"];

            title.booking_step1_cust_data_tw_identity_number = uiKey["booking_step1_cust_data_tw_identity_number"];
            title.booking_step1_cust_data_tw_identity_number_placeholder = uiKey["booking_step1_cust_data_tw_identity_number_placeholder"];

            title.booking_step1_cust_data_hk_mo_identity_number = uiKey["booking_step1_cust_data_hk_mo_identity_number"];
            title.booking_step1_cust_data_hk_mo_identity_number_placeholder = uiKey["booking_step1_cust_data_hk_mo_identity_number_placeholder"];

            title.booking_step1_cust_data_mtp_number = uiKey["booking_step1_cust_data_mtp_number"];
            title.booking_step1_cust_data_mtp_number_placeholder = uiKey["booking_step1_cust_data_mtp_number_placeholder"];

            title.booking_step1_cust_data_height = uiKey["booking_step1_cust_data_height"];
            title.booking_step1_cust_data_height_unit_01 = uiKey["booking_step1_cust_data_height_unit_01"];
            title.booking_step1_cust_data_height_unit_02 = uiKey["booking_step1_cust_data_height_unit_02"];
            title.booking_step1_cust_data_unit = uiKey["booking_step1_cust_data_unit"];

            title.booking_step1_cust_data_weight = uiKey["booking_step1_cust_data_weight"];
            title.booking_step1_cust_data_weight_unit_01 = uiKey["booking_step1_cust_data_weight_unit_01"];
            title.booking_step1_cust_data_weight_unit_02 = uiKey["booking_step1_cust_data_weight_unit_02"];

            title.booking_step1_cust_data_shoe_size = uiKey["booking_step1_cust_data_shoe_size"];
            title.booking_step1_cust_data_shoe_size_placeholder = uiKey["booking_step1_cust_data_shoe_size_placeholder"];
            title.booking_step1_cust_data_shoe_size_man = uiKey["booking_step1_cust_data_shoe_size_man"];
            title.booking_step1_cust_data_shoe_size_woman = uiKey["booking_step1_cust_data_shoe_size_woman"];
            title.booking_step1_cust_data_shoe_size_child = uiKey["booking_step1_cust_data_shoe_size_child"];
            title.booking_step1_cust_data_shoe_size_tip = uiKey["booking_step1_cust_data_shoe_size_tip"];

            title.booking_step1_cust_data_glass_diopter = uiKey["booking_step1_cust_data_glass_diopter"];
            title.booking_step1_cust_data_glass_diopter_placeholder = uiKey["booking_step1_cust_data_glass_diopter_placeholder"];
            title.booking_step1_cust_data_do_not_need = uiKey["booking_step1_cust_data_do_not_need"];

            title.booking_step1_cust_data_meal = uiKey["booking_step1_cust_data_meal"];
            title.booking_step1_cust_data_meal_placeholder = uiKey["booking_step1_cust_data_meal_placeholder"];
            title.booking_step1_cust_data_exclude_food = uiKey["booking_step1_cust_data_exclude_food"];
            title.booking_step1_cust_data_meal_tip = uiKey["booking_step1_cust_data_meal_tip"];
            title.booking_step1_cust_data_is_food_allergy = uiKey["booking_step1_cust_data_is_food_allergy"];

            title.common_select_set = uiKey["common_select_set"];
            title.common_male = uiKey["common_male"];
            title.common_female = uiKey["common_female"];
            title.booking_step1_save_member_data = uiKey["booking_step1_save_member_data"];
            title.common_guide_lang = uiKey["common_guide_lang"];
            title.booking_step1_product_guide_lang = uiKey["booking_step1_product_guide_lang"];
            title.booking_step1_shuttle_data = uiKey["booking_step1_shuttle_data"];
            title.booking_step1_shuttle_data = uiKey["booking_step1_shuttle_data"];
            title.booking_step1_shuttle_data_shuttle_date = uiKey["booking_step1_shuttle_data_shuttle_date"];
            title.booking_step1_shuttle_data_pick_up_location = uiKey["booking_step1_shuttle_data_pick_up_location"];
            title.booking_step1_shuttle_data_drop_off_location = uiKey["booking_step1_shuttle_data_drop_off_location"];
            title.booking_step1_shuttle_data_pick_up_location_placeholder = uiKey["booking_step1_shuttle_data_pick_up_location_placeholder"];
            title.booking_step1_shuttle_data_drop_off_location_placeholder = uiKey["booking_step1_shuttle_data_drop_off_location_placeholder"];
            title.booking_step1_order_note = uiKey["booking_step1_order_note"];
            title.booking_step1_order_note_tip = uiKey["booking_step1_order_note_tip"];
            title.booking_step1_use_coupon = uiKey["booking_step1_use_coupon"];
            title.booking_step1_have_discount_code = uiKey["booking_step1_have_discount_code"];
            title.booking_step1_dont_use = uiKey["booking_step1_dont_use"];
            title.booking_step1_coupon_code = uiKey["booking_step1_coupon_code"];
            title.booking_step1_btn_coupon = uiKey["booking_step1_btn_coupon"];

            title.booking_step1_please_select_payment_method = uiKey["booking_step1_please_select_payment_method"];
            title.payment_pmch_name_CITI_CREDITCARD = uiKey["payment_pmch_name_CITI_CREDITCARD"];
            title.booking_step1_amount_of_money = uiKey["booking_step1_amount_of_money"];
            title.payment_pmch_info_remind_twd = uiKey["payment_pmch_info_remind_twd"];
            title.payment_pmch_info_remind_hkd = uiKey["payment_pmch_info_remind_hkd"];
            title.payment_pmch_info_remind_usd = uiKey["payment_pmch_info_remind_usd"];
            title.payment_pmch_info_fee_remind = uiKey["payment_pmch_info_fee_remind"];
            title.common_card_holder_name = uiKey["common_card_holder_name"];
            title.booking_step1_enter_card_holder_name = uiKey["booking_step1_enter_card_holder_name"];
            title.common_credit_card_num = uiKey["common_credit_card_num"];
            title.common_expire_date = uiKey["common_expire_date"];
            title.common_next = uiKey["common_next"];



            title.booking_step1_flight_info_arrival_airport = uiKey["booking_step1_flight_info_arrival_airport"];
            title.booking_step1_flight_info_arrival_info = uiKey["booking_step1_flight_info_arrival_info"];
            title.booking_step1_flight_info_arrival_airport_placeholder = uiKey["booking_step1_flight_info_arrival_airport_placeholder"];
            title.booking_step1_flight_info_terminal_no = uiKey["booking_step1_flight_info_terminal_no"];
            title.booking_step1_flight_info_terminal_no_placeholder = uiKey["booking_step1_flight_info_terminal_no_placeholder"];
            title.booking_step1_flight_info_airline = uiKey["booking_step1_flight_info_airline"];
            title.booking_step1_flight_info_airline_placeholder = uiKey["booking_step1_flight_info_airline_placeholder"];
            title.booking_step1_flight_info_flight_no = uiKey["booking_step1_flight_info_flight_no"];
            title.booking_step1_flight_info_flight_no_placeholder = uiKey["booking_step1_flight_info_flight_no_placeholder"];
            title.booking_step1_flight_info_arrival_time = uiKey["booking_step1_flight_info_arrival_time"];
            title.booking_step1_flight_info_arrival_time_placeholder = uiKey["booking_step1_flight_info_arrival_time_placeholder"];
            title.booking_step1_flight_info_departure_info = uiKey["booking_step1_flight_info_departure_info"];
            title.booking_step1_flight_info_departure_airport = uiKey["booking_step1_flight_info_departure_airport"];
            title.booking_step1_flight_info_departure_airport_placeholder = uiKey["booking_step1_flight_info_departure_airport_placeholder"];
            title.booking_step1_flight_info_departure_time = uiKey["booking_step1_flight_info_departure_time"];
            title.booking_step1_flight_info_departure_time_placeholder = uiKey["booking_step1_flight_info_departure_time_placeholder"];
            title.booking_step1_flight_info_flight_type = uiKey["booking_step1_flight_info_flight_type"];
            title.booking_step1_flight_info_flight_type_placeholder = uiKey["booking_step1_flight_info_flight_type_placeholder"];
            title.booking_step1_flight_info_domestic_routes = uiKey["booking_step1_flight_info_domestic_routes"];
            title.booking_step1_flight_info_international_routes = uiKey["booking_step1_flight_info_international_routes"];
            title.common_hr = uiKey["common_hr"];
            title.common_min = uiKey["common_min"];
            title.common_yes = uiKey["common_yes"];
            title.common_no = uiKey["common_no"];
            title.booking_step1_is_visa_required = uiKey["booking_step1_is_visa_required"];

            title.booking_step1_shuttle_data_shuttle_date_placeholder = uiKey["booking_step1_shuttle_data_shuttle_date_placeholder"];
            title.booking_step1_shuttle_data_pick_up_time = uiKey["booking_step1_shuttle_data_pick_up_time"];
            title.booking_step1_shuttle_data_pick_up_time_placeholder = uiKey["booking_step1_shuttle_data_pick_up_time_placeholder"];
            title.booking_step1_shuttle_data_designated_location = uiKey["booking_step1_shuttle_data_designated_location"];
            title.booking_step1_shuttle_data_designated_location_placeholder = uiKey["booking_step1_shuttle_data_designated_location_placeholder"];
            title.booking_step1_shuttle_data_charter_route = uiKey["booking_step1_shuttle_data_charter_route"];
            title.booking_step1_shuttle_data_charter_route_placeholder = uiKey["booking_step1_shuttle_data_charter_route_placeholder"];
            title.booking_step1_shuttle_data_custom_routes = uiKey["booking_step1_shuttle_data_custom_routes"];
            title.booking_step1_shuttle_data_custom_routes_placeholder = uiKey["booking_step1_shuttle_data_custom_routes_placeholder"];
            title.common_add = uiKey["common_add"];
            title.booking_step1_shuttle_data_custom_routes_note_1 = uiKey["booking_step1_shuttle_data_custom_routes_note_1"];
            title.booking_step1_shuttle_data_custom_routes_note_2 = uiKey["booking_step1_shuttle_data_custom_routes_note_2"];

            title.booking_step1_other_data = uiKey["booking_step1_other_data"];
            title.booking_step1_other_data_mobile_model_number = uiKey["booking_step1_other_data_mobile_model_number"];
            title.booking_step1_other_data_imei = uiKey["booking_step1_other_data_imei"];
            title.booking_step1_other_data_activation_date = uiKey["booking_step1_other_data_activation_date"];
            title.booking_step1_other_data_activation_date_placeholder = uiKey["booking_step1_other_data_activation_date_placeholder"];

            title.booking_step1_rent_car = uiKey["booking_step1_rent_car"];
            title.booking_step1_rent_car_pick_up_office = uiKey["booking_step1_rent_car_pick_up_office"];
            title.booking_step1_rent_car_pick_up_office_placeholder = uiKey["booking_step1_rent_car_pick_up_office_placeholder"];
            title.booking_step1_rent_car_pick_up_date = uiKey["booking_step1_rent_car_pick_up_date"];
            title.booking_step1_rent_car_pick_up_date_placeholder = uiKey["booking_step1_rent_car_pick_up_date_placeholder"];
            title.booking_step1_rent_car_is_need_free_wifi = uiKey["booking_step1_rent_car_is_need_free_wifi"];
            title.booking_step1_rent_car_is_need_free_gps = uiKey["booking_step1_rent_car_is_need_free_gps"];
            title.common_need = uiKey["common_need"];
            title.common_no_need = uiKey["common_no_need"];
            title.booking_step1_rent_car_drop_off_office = uiKey["booking_step1_rent_car_drop_off_office"];
            title.booking_step1_rent_car_drop_off_office_placeholder = uiKey["booking_step1_rent_car_drop_off_office_placeholder"];
            title.booking_step1_rent_car_drop_off_date = uiKey["booking_step1_rent_car_drop_off_date"];
            title.booking_step1_rent_car_drop_off_date_placeholder = uiKey["booking_step1_rent_car_drop_off_date_placeholder"];
            title.booking_step1_car_psgr = uiKey["booking_step1_car_psgr"];
            title.booking_step1_car_psgr_carry_luggage_quantity = uiKey["booking_step1_car_psgr_carry_luggage_quantity"];
            title.booking_step1_car_psgr_carry_luggage = uiKey["booking_step1_car_psgr_carry_luggage"];
            title.booking_step1_car_psgr_checked_luggage = uiKey["booking_step1_car_psgr_checked_luggage"];
            title.booking_step1_car_psgr_child_seat_quantity = uiKey["booking_step1_car_psgr_child_seat_quantity"];
            title.booking_step1_car_psgr_suitable_for_age = uiKey["booking_step1_car_psgr_suitable_for_age"];
            title.booking_step1_car_psgr_supplier_provided = uiKey["booking_step1_car_psgr_supplier_provided"];
            title.common_years_old = uiKey["common_years_old"];
            title.booking_step1_car_psgr_self_provided = uiKey["booking_step1_car_psgr_self_provided"];
            title.booking_step1_car_psgr_infant_seat_quantity = uiKey["booking_step1_car_psgr_infant_seat_quantity"];
            title.booking_step1_send_data = uiKey["booking_step1_send_data"];
            title.booking_step1_send_data_receiver_name = uiKey["booking_step1_send_data_receiver_name"];
            title.booking_step1_send_data_receiver_first_name = uiKey["booking_step1_send_data_receiver_first_name"];
            title.booking_step1_send_data_receiver_firstname = uiKey["booking_step1_send_data_receiver_firstname"];
            title.booking_step1_send_data_receiver_firstname_placeholder = uiKey["booking_step1_send_data_receiver_firstname_placeholder"];
            title.booking_step1_send_data_receiver_lastname = uiKey["booking_step1_send_data_receiver_lastname"];
            title.booking_step1_send_data_receiver_lastname_placeholder = uiKey["booking_step1_send_data_receiver_lastname_placeholder"];
            title.booking_step1_send_data_receive_address = uiKey["booking_step1_send_data_receive_address"];
            title.booking_step1_send_data_receive_address_country = uiKey["booking_step1_send_data_receive_address_country"];
            title.booking_step1_send_data_receive_address_country_placeholder = uiKey["booking_step1_send_data_receive_address_country_placeholder"];
            title.booking_step1_send_data_receive_address_city = uiKey["booking_step1_send_data_receive_address_city"];
            title.booking_step1_send_data_receive_address_city_placeholder = uiKey["booking_step1_send_data_receive_address_city_placeholder"];
            title.booking_step1_send_data_zip_colde = uiKey["booking_step1_send_data_zip_colde"];
            title.booking_step1_send_data_receive_address_detail = uiKey["booking_step1_send_data_receive_address_detail"];
            title.booking_step1_send_data_receive_address_placeholder = uiKey["booking_step1_send_data_receive_address_placeholder"];
            title.booking_step1_send_data_receiver_tel = uiKey["booking_step1_send_data_receiver_tel"];
            title.booking_step1_send_data_receiver_tel_placeholder = uiKey["booking_step1_send_data_receiver_tel_placeholder"];
            title.booking_step1_send_data_hotel_name = uiKey["booking_step1_send_data_hotel_name"];
            title.product_index_voucher_type = uiKey["product_index_voucher_type"];
            title.booking_step1_send_data_hotel_tel = uiKey["booking_step1_send_data_hotel_tel"];
            title.booking_step1_send_data_hotel_tel_placeholder = uiKey["booking_step1_send_data_hotel_tel_placeholder"];
            title.booking_step1_send_data_hotel_address = uiKey["booking_step1_send_data_hotel_address"];
            title.booking_step1_send_data_buyer_passport_english_firstname = uiKey["booking_step1_send_data_buyer_passport_english_firstname"];
            title.booking_step1_send_data_buyer_passport_english_firstname_placeholder = uiKey["booking_step1_send_data_buyer_passport_english_firstname_placeholder"];
            title.booking_step1_send_data_buyer_passport_english_lastname = uiKey["booking_step1_send_data_buyer_passport_english_lastname"];
            title.booking_step1_send_data_buyer_passport_english_lastname_placeholder = uiKey["booking_step1_send_data_buyer_passport_english_lastname_placeholder"];
            title.booking_step1_send_data_buyer_local_firstname = uiKey["booking_step1_send_data_buyer_local_firstname"];
            title.booking_step1_send_data_buyer_local_firstname_placeholder = uiKey["booking_step1_send_data_buyer_local_firstname_placeholder"];
            title.booking_step1_send_data_buyer_local_lastname = uiKey["booking_step1_send_data_buyer_local_lastname"];
            title.booking_step1_send_data_buyer_local_lastname_placeholder = uiKey["booking_step1_send_data_buyer_local_lastname_placeholder"];
            title.booking_step1_send_data_booking_website = uiKey["booking_step1_send_data_booking_website"];
            title.booking_step1_send_data_booking_website_placeholder = uiKey["booking_step1_send_data_booking_website_placeholder"];
            title.booking_step1_send_data_booking_order_no = uiKey["booking_step1_send_data_booking_order_no"];
            title.booking_step1_send_data_booking_order_no_placeholder = uiKey["booking_step1_send_data_booking_order_no_placeholder"];
            title.booking_step1_send_data_check_in_date = uiKey["booking_step1_send_data_check_in_date"];
            title.booking_step1_send_data_check_in_date_placeholder = uiKey["booking_step1_send_data_check_in_date_placeholder"];
            title.booking_step1_send_data_check_out_date = uiKey["booking_step1_send_data_check_out_date"];
            title.booking_step1_send_data_check_out_date_placeholder = uiKey["booking_step1_send_data_check_out_date_placeholder"];
            title.booking_step1_contact_data = uiKey["booking_step1_contact_data"];
            title.booking_step1_contact_data_firstname = uiKey["booking_step1_contact_data_firstname"];
            title.booking_step1_contact_data_firstname_placeholder = uiKey["booking_step1_contact_data_firstname_placeholder"];
            title.booking_step1_contact_data_lastname = uiKey["booking_step1_contact_data_lastname"];
            title.booking_step1_contact_data_lastname_placeholder = uiKey["booking_step1_contact_data_lastname_placeholder"];
            title.booking_step1_contact_data_contact_tel = uiKey["booking_step1_contact_data_contact_tel"];
            title.booking_step1_contact_data_contact_tel_placeholder = uiKey["booking_step1_contact_data_contact_tel_placeholder"];
            title.booking_step1_contact_data_contact_app = uiKey["booking_step1_contact_data_contact_app"];
            title.booking_step1_contact_data_contact_app_placeholder = uiKey["booking_step1_contact_data_contact_app_placeholder"];
            title.booking_step1_contact_data_contact_app_account = uiKey["booking_step1_contact_data_contact_app_account"];
            title.booking_step1_other_data_exchange_location = uiKey["booking_step1_other_data_exchange_location"];
            title.booking_step1_other_data_exchange_location_placeholder = uiKey["booking_step1_other_data_exchange_location_placeholder"];
            title.common_have = uiKey["common_have"];
            title.common_have_not = uiKey["common_have_not"];


            title.booking_step1_shuttle_data_customized_shuttle_time = uiKey["booking_step1_shuttle_data_customized_shuttle_time"];
            title.booking_step1_shuttle_data_customized_charter_route = uiKey["booking_step1_shuttle_data_customized_charter_route"];

            //error
            title.booking_step1_required_error = uiKey["booking_step1_required_error"];
            title.booking_step1_length_error_1 = uiKey["booking_step1_length_error_1"];
            title.booking_step1_length_error_2 = uiKey["booking_step1_length_error_2"];
            title.booking_step1_english_error = uiKey["booking_step1_english_error"];


            //active 
            title.common_options = uiKey["common_options"];
            title.common_date = uiKey["common_date"];
            title.common_guest = uiKey["common_guest"];
            title.common_order_num_of_travellers = uiKey["common_order_num_of_travellers"];
            title.order_show_event_time = uiKey["order_show_event_time"];
            //event
            title.booking_step1_event_backup = uiKey["booking_step1_event_backup"];
            title.booking_step1_backup_event_data_number = uiKey["booking_step1_backup_event_data_number"];
            title.product_productlist_choose_date = uiKey["product_productlist_choose_date"];

            return title;
        }


        //套餐日期 一開始先全抓
        public static List<PkgDateforEcModel> getProdPkgDate(PackageModel pkg, string lang, string currency, Dictionary<string, string> uikey, out string allCanUseDate)
        {
            List<PkgDateforEcModel> pkgDateList = new List<PkgDateforEcModel>();
            string allCanUseDateTemp = "";

            //要判斷是否是0000
            PkgSaleDateModel sale_dates = pkg.sale_dates;

            foreach (SaleDt s in sale_dates.saleDt)
            {
                string pkgOid = s.pkg_no.ToString();
                DateTime day = DateTimeTool.yyyyMMdd2DateTime(s.sale_day);
                //string day = s.sale_day.ToString();

                //先建立ProductPkgDateModel ,有新的就加
                string[] pkgid = pkgOid.Split(",");
                foreach (string id in pkgid)
                {
                    var result = pkgDateList.Where(x => x.pkgOid.ToString() == id);

                    if (result.Count() > 0)
                    {
                        foreach (PkgDateforEcModel pd in result)
                        {
                            pd.day = pd.day + day.ToString("yyyy-MM-dd") + ",";
                        }
                    }
                    else
                    {
                        PkgDateforEcModel pd = new PkgDateforEcModel();
                        pd.pkgOid = id;
                        pd.day = day.ToString("yyyy-MM-dd") + ",";
                        pkgDateList.Add(pd);
                    }
                }
                allCanUseDateTemp = allCanUseDateTemp + day.ToString("yyyy-MM-dd") + ",";
            }

            //去最後逗號
            foreach (PkgDateforEcModel pkgDate in pkgDateList)
            {
                if (pkgDate.day.Length != 0)
                {
                    pkgDate.day = pkgDate.day.Substring(0, pkgDate.day.Length - 1);
                }
            }

            allCanUseDate = allCanUseDateTemp.Length > 0 ? allCanUseDateTemp.Substring(0, allCanUseDateTemp.Length - 1) : allCanUseDateTemp;

            return pkgDateList;
        }

        //套餐&日期整合
        public static PackageModel InitPkg(prodQury prodQury, ProdTitleModel title, PackageModel pkgs, List<PkgDateforEcModel> prodPkgDateList)
        {
            if (prodQury.selDate == "")//第一次進來
            {
                foreach (PkgDetailModel pkg in pkgs.pkgs)
                {
                    string pkgOid = pkg.pkg_no;

                    foreach (PkgDateforEcModel pkgDate in prodPkgDateList)
                    {
                        if (pkgOid.Equals(pkgDate.pkgOid))
                        {
                            pkg.pkgDate = pkgDate;
                        }
                    }
                }
            }
            else
            {
                //如果有 selDate要依selDate決定可用的套餐
                foreach (PkgDetailModel pkg in pkgs.pkgs)
                {
                    string pkgOid = pkg.pkg_no;
                    //先確認有沒有在可售區間 -晚點寫,要改

                    if (pkg.status == "N")
                    {
                        pkg.chkDateCanSell = "3"; //3己售罄
                        pkg.NoSellTextShow = title.product_index_has_been_sold_out; // "已售罄";
                    }
                    else
                    {
                        foreach (PkgDateforEcModel pkgDate in prodPkgDateList)
                        {
                            if (pkgOid.Equals(pkgDate.pkgOid))
                            {
                                if (pkgDate.day.IndexOf(prodQury.selDate.Replace("/", "-")) > -1)
                                {
                                    if (pkg.status == "Y")
                                    {
                                        pkg.chkDateCanSell = "1";// 1上架日期可賣
                                    }
                                }
                                else
                                {
                                    pkg.chkDateCanSell = "2"; //2上架日期不可賣
                                    //pkg.NoSellTextShow = title.product_index_check_availablity; //"此日期無法購買";
                                    pkg.NoSellTextShow = title.product_index_has_been_sold_out; // "已售罄";
                                }
                                pkg.pkgDate = pkgDate;
                            }
                        }
                    }
                }

            }

            return pkgs;
        }

        //套餐&日期整合
        public static PackageModel InitPkg2(prodQury prodQury, ProdTitleModel title, PackageModel pkgs, List<PkgDateforEcModel> prodPkgDateList)
        {
            if (prodQury.selDate == "")//第一次進來
            {
                foreach (PkgDetailModel pkg in pkgs.pkgs)
                {
                    string pkgOid = pkg.pkg_no;

                    foreach (PkgDateforEcModel pkgDate in prodPkgDateList)
                    {
                        if (pkgOid.Equals(pkgDate.pkgOid))
                        {
                            pkg.pkgDate = pkgDate;
                        }
                    }
                }
            }
            else
            {
                //如果有 selDate要依selDate決定可用的套餐
                foreach (PkgDetailModel pkg in pkgs.pkgs)
                {
                    string pkgOid = pkg.pkg_no;
                    //先確認有沒有在可售區間 -晚點寫,要改

                    if (pkg.status == "N")
                    {
                        pkg.chkDateCanSell = "3"; //3己售罄
                        pkg.NoSellTextShow = title.product_index_has_been_sold_out; // "已售罄";
                    }
                    else
                    {
                        //foreach (PkgDateforEcModel pkgDate in prodPkgDateList)
                        {
                            //if (prodPkgDateList.Select(x=>x.day))
                            {
                                //if (pkgDate.day.IndexOf(prodQury.selDate.Replace("/", "-")) > -1)
                                {
                                    if (pkg.status == "Y")
                                    {
                                        pkg.chkDateCanSell = "1";// 1上架日期可賣
                                    }
                                }
                                //else
                                {
                                    pkg.chkDateCanSell = "2"; //2上架日期不可賣
                                    //pkg.NoSellTextShow = title.product_index_check_availablity; //"此日期無法購買";
                                    pkg.NoSellTextShow = title.product_index_has_been_sold_out; // "已售罄";
                                }
                                //pkg.pkgDate = pkgDate;
                            }
                        }
                    }
                }

            }

            return pkgs;
        }


        //模組
        public static ProductModuleModel getProdModule(long B2dXid, string state, string lang, string currency, string prodoid, string pkgoid, ProdTitleModel title)
        {
            try
            {
                return ApiHelper.getProdModule(B2dXid, state, lang, currency, prodoid, pkgoid, title);
            }
            catch (Exception ex)
            {
                var error = ex;
                throw new Exception(title.result_code_9990);
            }

        }

        //
        //public static JObject  getModuleBooking(string oid, string lang, string currency) 
        //{
        //KKapiHelper api = new KKapiHelper();
        //var obj = (JObject)api.callKKapiProdModuleBooking(lang, currency, oid);

        //finishStatus 一定要是9
        //customerDataType 01 入住代表人 02 每個人
        //把已選到的場次時間去掉，以免 step1 備選場次又被選到
        //拿寄送資料的城市清單

        //return obj;
        //}

        public static PkgEventsModel getEvent(long companyXid, string state, string lang, string currency, string prodoid, string pkgoid, ProdTitleModel title)
        {
            try
            {
                return ApiHelper.getPkgEvent(companyXid, state, lang, currency, prodoid, pkgoid, title);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"error-getprodDtl ex:{ex.ToString()}");
                throw new Exception("error-getprodDtl ex:" + ex.ToString());

            }
        }

    }
}
