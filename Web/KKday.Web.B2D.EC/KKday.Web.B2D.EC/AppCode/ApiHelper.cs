using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using StackExchange.Redis;
using KKday.Web.B2D.EC.Models.Model.Product;
using KKday.Web.B2D.EC.Models.Model.Booking.api;
using KKday.Web.B2D.EC.Models.Model.Pmch;
using KKday.Web.B2D.EC.Models.Model.Booking;

namespace KKday.Web.B2D.EC.AppCode
{
    public static class ApiHelper
    {

        public static ProductModuleModel getProdModule(long B2dXid, string state, string lang, string currency, string prodoid, string pkgoid, ProdTitleModel title)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                var pathUrl = Website.Instance.Configuration["B2DApiUrl:apiUri"];

                string result;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{pathUrl}Product/QueryModule");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                ApiRequest setting = new ApiRequest();

                setting.company_xid = B2dXid.ToString();
                setting.current_currency = currency;
                setting.locale_lang = lang;
                setting.prod_no = prodoid;
                setting.pkg_no = pkgoid;
                setting.state = state;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(setting);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //result = "{\"reasult\":\"0000\",\"reasult_msg\":\"正確\",\"module_type\":[\"PMDL_CAR_PSGR\",\"PMDL_CONTACT_DATA\",\"PMDL_CUST_DATA\",\"PMDL_FLIGHT_INFO\",\"PMDL_RENT_CAR\",\"PMDL_SEND_DATA\",\"PMDL_SIM_WIFI\",\"PMDL_VENUE\",\"GUIDE_LANGAGE\"],\"module_cust_data\":{\"is_require\":true,\"cus_type\":\"02\",\"englis_name\":{\"is_require_FirstName\":true,\"is_require_LastName\":true},\"local_name\":{},\"gender\":{\"is_require\":true,\"gender_list\":[{\"type\":\"F\",\"type_name\":\"女性\"},{\"type\":\"M\",\"type_name\":\"男性\"}]},\"nationality\":{\"is_require\":true,\"nation_list\":[{\"country_local_name\":\"不丹\",\"country_code\":\"BT\"},{\"country_local_name\":\"阿富汗\",\"country_code\":\"AF\"},{\"country_local_name\":\"阿爾巴尼亞\",\"country_code\":\"AL\"},{\"country_local_name\":\"阿爾及利亞\",\"country_code\":\"DZ\"},{\"country_local_name\":\"安道爾\",\"country_code\":\"AD\"},{\"country_local_name\":\"安哥拉\",\"country_code\":\"AO\"},{\"country_local_name\":\"安圭拉\",\"country_code\":\"AI\"},{\"country_local_name\":\"安地卡及巴布達\",\"country_code\":\"AG\"},{\"country_local_name\":\"阿根廷\",\"country_code\":\"AR\"},{\"country_local_name\":\"亞美尼亞\",\"country_code\":\"AM\"},{\"country_local_name\":\"澳洲\",\"country_code\":\"AU\"},{\"country_local_name\":\"奥地利\",\"country_code\":\"AT\"},{\"country_local_name\":\"亞塞拜然\",\"country_code\":\"AZ\"},{\"country_local_name\":\"巴哈馬\",\"country_code\":\"BS\"},{\"country_local_name\":\"巴林\",\"country_code\":\"BH\"},{\"country_local_name\":\"孟加拉\",\"country_code\":\"BD\"},{\"country_local_name\":\"巴貝多\",\"country_code\":\"BB\"},{\"country_local_name\":\"白俄羅斯\",\"country_code\":\"BY\"},{\"country_local_name\":\"比利時\",\"country_code\":\"BE\"},{\"country_local_name\":\"貝里斯\",\"country_code\":\"BZ\"},{\"country_local_name\":\"貝南\",\"country_code\":\"BJ\"},{\"country_local_name\":\"百慕達\",\"country_code\":\"BM\"},{\"country_local_name\":\"玻利維亞\",\"country_code\":\"BO\"},{\"country_local_name\":\"波札那\",\"country_code\":\"BW\"},{\"country_local_name\":\"巴西\",\"country_code\":\"BR\"},{\"country_local_name\":\"汶萊\",\"country_code\":\"BN\"},{\"country_local_name\":\"保加利亞\",\"country_code\":\"BG\"},{\"country_local_name\":\"布吉納法索\",\"country_code\":\"BF\"},{\"country_local_name\":\"蒲隆地\",\"country_code\":\"BI\"},{\"country_local_name\":\"柬埔寨\",\"country_code\":\"KH\"},{\"country_local_name\":\"喀麥隆\",\"country_code\":\"CM\"},{\"country_local_name\":\"加拿大\",\"country_code\":\"CA\"},{\"country_local_name\":\"中非共和國\",\"country_code\":\"CF\"},{\"country_local_name\":\"查德\",\"country_code\":\"TD\"},{\"country_local_name\":\"智利\",\"country_code\":\"CL\"},{\"country_local_name\":\"中國\",\"country_code\":\"CN\"},{\"country_local_name\":\"哥倫比亞\",\"country_code\":\"CO\"},{\"country_local_name\":\"庫克群島\",\"country_code\":\"CK\"},{\"country_local_name\":\"哥斯大黎加\",\"country_code\":\"CR\"},{\"country_local_name\":\"古巴\",\"country_code\":\"CU\"},{\"country_local_name\":\"賽普勒斯\",\"country_code\":\"CY\"},{\"country_local_name\":\"捷克共和國\",\"country_code\":\"CZ\"},{\"country_local_name\":\"丹麥\",\"country_code\":\"DK\"},{\"country_local_name\":\"吉布地\",\"country_code\":\"DJ\"},{\"country_local_name\":\"多明尼加共和國\",\"country_code\":\"DO\"},{\"country_local_name\":\"厄瓜多\",\"country_code\":\"EC\"},{\"country_local_name\":\"埃及\",\"country_code\":\"EG\"},{\"country_local_name\":\"薩爾瓦多\",\"country_code\":\"SV\"},{\"country_local_name\":\"愛沙尼亞\",\"country_code\":\"EE\"},{\"country_local_name\":\"衣索比亞\",\"country_code\":\"ET\"},{\"country_local_name\":\"索馬利亞聯邦共和國\",\"country_code\":\"SO\"},{\"country_local_name\":\"斐濟\",\"country_code\":\"FJ\"},{\"country_local_name\":\"芬蘭\",\"country_code\":\"FI\"},{\"country_local_name\":\"法國\",\"country_code\":\"FR\"},{\"country_local_name\":\"法屬圭亞那\",\"country_code\":\"GF\"},{\"country_local_name\":\"法屬玻里尼西亞\",\"country_code\":\"PF\"},{\"country_local_name\":\"加彭\",\"country_code\":\"GA\"},{\"country_local_name\":\"甘比亞\",\"country_code\":\"GM\"},{\"country_local_name\":\"喬治亞\",\"country_code\":\"GE\"},{\"country_local_name\":\"德國\",\"country_code\":\"DE\"},{\"country_local_name\":\"迦納\",\"country_code\":\"GH\"},{\"country_local_name\":\"直布羅陀\",\"country_code\":\"GI\"},{\"country_local_name\":\"希臘\",\"country_code\":\"GR\"},{\"country_local_name\":\"格瑞那達\",\"country_code\":\"GD\"},{\"country_local_name\":\"關島\",\"country_code\":\"GU\"},{\"country_local_name\":\"瓜地馬拉\",\"country_code\":\"GT\"},{\"country_local_name\":\"幾內亞\",\"country_code\":\"GN\"},{\"country_local_name\":\"蓋亞那\",\"country_code\":\"GY\"},{\"country_local_name\":\"海地\",\"country_code\":\"HT\"},{\"country_local_name\":\"宏都拉斯\",\"country_code\":\"HN\"},{\"country_local_name\":\"香港\",\"country_code\":\"HK\"},{\"country_local_name\":\"匈牙利\",\"country_code\":\"HU\"},{\"country_local_name\":\"冰島\",\"country_code\":\"IS\"},{\"country_local_name\":\"印度\",\"country_code\":\"IN\"},{\"country_local_name\":\"印尼\",\"country_code\":\"ID\"},{\"country_local_name\":\"伊朗\",\"country_code\":\"IR\"},{\"country_local_name\":\"伊拉克\",\"country_code\":\"IQ\"},{\"country_local_name\":\"愛爾蘭\",\"country_code\":\"IE\"},{\"country_local_name\":\"以色列\",\"country_code\":\"IL\"},{\"country_local_name\":\"義大利\",\"country_code\":\"IT\"},{\"country_local_name\":\"牙買加\",\"country_code\":\"JM\"},{\"country_local_name\":\"日本\",\"country_code\":\"JP\"},{\"country_local_name\":\"約旦\",\"country_code\":\"JO\"},{\"country_local_name\":\"哈薩克\",\"country_code\":\"KZ\"},{\"country_local_name\":\"肯亞\",\"country_code\":\"KE\"},{\"country_local_name\":\"科威特\",\"country_code\":\"KW\"},{\"country_local_name\":\"吉爾吉斯坦\",\"country_code\":\"KG\"},{\"country_local_name\":\"寮國\",\"country_code\":\"LA\"},{\"country_local_name\":\"拉脫維亞\",\"country_code\":\"LV\"},{\"country_local_name\":\"黎巴嫩\",\"country_code\":\"LB\"},{\"country_local_name\":\"賴索托\",\"country_code\":\"LS\"},{\"country_local_name\":\"賴比瑞亞\",\"country_code\":\"LR\"},{\"country_local_name\":\"利比亞\",\"country_code\":\"LY\"},{\"country_local_name\":\"列支敦斯登\",\"country_code\":\"LI\"},{\"country_local_name\":\"立陶宛\",\"country_code\":\"LT\"},{\"country_local_name\":\"盧森堡\",\"country_code\":\"LU\"},{\"country_local_name\":\"澳門\",\"country_code\":\"MO\"},{\"country_local_name\":\"馬達加斯加\",\"country_code\":\"MG\"},{\"country_local_name\":\"馬拉威\",\"country_code\":\"MW\"},{\"country_local_name\":\"馬來西亞\",\"country_code\":\"MY\"},{\"country_local_name\":\"馬爾地夫\",\"country_code\":\"MV\"},{\"country_local_name\":\"馬利\",\"country_code\":\"ML\"},{\"country_local_name\":\"馬爾他\",\"country_code\":\"MT\"},{\"country_local_name\":\"模里西斯\",\"country_code\":\"MU\"},{\"country_local_name\":\"墨西哥\",\"country_code\":\"MX\"},{\"country_local_name\":\"摩爾多瓦\",\"country_code\":\"MD\"},{\"country_local_name\":\"摩納哥\",\"country_code\":\"MC\"},{\"country_local_name\":\"蒙古\",\"country_code\":\"MN\"},{\"country_local_name\":\"蒙塞拉特\",\"country_code\":\"MS\"},{\"country_local_name\":\"摩洛哥\",\"country_code\":\"MA\"},{\"country_local_name\":\"莫三比克\",\"country_code\":\"MZ\"},{\"country_local_name\":\"緬甸\",\"country_code\":\"MM\"},{\"country_local_name\":\"那米比亞\",\"country_code\":\"NA\"},{\"country_local_name\":\"諾魯\",\"country_code\":\"NR\"},{\"country_local_name\":\"尼泊爾\",\"country_code\":\"NP\"},{\"country_local_name\":\"荷蘭\",\"country_code\":\"NL\"},{\"country_local_name\":\"紐西蘭\",\"country_code\":\"NZ\"},{\"country_local_name\":\"尼加拉瓜\",\"country_code\":\"NI\"},{\"country_local_name\":\"尼日\",\"country_code\":\"NE\"},{\"country_local_name\":\"奈及利亞\",\"country_code\":\"NG\"},{\"country_local_name\":\"北韓\",\"country_code\":\"KP\"},{\"country_local_name\":\"挪威\",\"country_code\":\"NO\"},{\"country_local_name\":\"阿曼\",\"country_code\":\"OM\"},{\"country_local_name\":\"巴基斯坦\",\"country_code\":\"PK\"},{\"country_local_name\":\"巴拿馬\",\"country_code\":\"PA\"},{\"country_local_name\":\"巴布亞紐幾內亞\",\"country_code\":\"PG\"},{\"country_local_name\":\"巴拉圭\",\"country_code\":\"PY\"},{\"country_local_name\":\"秘魯\",\"country_code\":\"PE\"},{\"country_local_name\":\"菲律賓\",\"country_code\":\"PH\"},{\"country_local_name\":\"波蘭\",\"country_code\":\"PL\"},{\"country_local_name\":\"葡萄牙\",\"country_code\":\"PT\"},{\"country_local_name\":\"波多黎各\",\"country_code\":\"PR\"},{\"country_local_name\":\"卡達\",\"country_code\":\"QA\"},{\"country_local_name\":\"剛果共和國\",\"country_code\":\"CG\"},{\"country_local_name\":\"羅馬尼亞\",\"country_code\":\"RO\"},{\"country_local_name\":\"俄羅斯\",\"country_code\":\"RU\"},{\"country_local_name\":\"聖馬里諾\",\"country_code\":\"SM\"},{\"country_local_name\":\"聖多美普林西比\",\"country_code\":\"ST\"},{\"country_local_name\":\"沙烏地阿拉伯\",\"country_code\":\"SA\"},{\"country_local_name\":\"塞內加爾\",\"country_code\":\"SN\"},{\"country_local_name\":\"塞席爾\",\"country_code\":\"SC\"},{\"country_local_name\":\"獅子山\",\"country_code\":\"SL\"},{\"country_local_name\":\"新加坡\",\"country_code\":\"SG\"},{\"country_local_name\":\"斯洛伐克\",\"country_code\":\"SK\"},{\"country_local_name\":\"斯洛維尼亞\",\"country_code\":\"SI\"},{\"country_local_name\":\"索羅門群島\",\"country_code\":\"SB\"},{\"country_local_name\":\"南非\",\"country_code\":\"ZA\"},{\"country_local_name\":\"南韓\",\"country_code\":\"KR\"},{\"country_local_name\":\"西班牙\",\"country_code\":\"ES\"},{\"country_local_name\":\"斯里蘭卡\",\"country_code\":\"LK\"},{\"country_local_name\":\"聖露西亞\",\"country_code\":\"LC\"},{\"country_local_name\":\"聖文森及格瑞那丁\",\"country_code\":\"VC\"},{\"country_local_name\":\"蘇丹\",\"country_code\":\"SD\"},{\"country_local_name\":\"蘇利南\",\"country_code\":\"SR\"},{\"country_local_name\":\"史瓦濟蘭\",\"country_code\":\"SZ\"},{\"country_local_name\":\"瑞典\",\"country_code\":\"SE\"},{\"country_local_name\":\"瑞士\",\"country_code\":\"CH\"},{\"country_local_name\":\"希利亞\",\"country_code\":\"SY\"},{\"country_local_name\":\"台灣\",\"country_code\":\"TW\"},{\"country_local_name\":\"塔吉克\",\"country_code\":\"TJ\"},{\"country_local_name\":\"坦尚尼亞\",\"country_code\":\"TZ\"},{\"country_local_name\":\"泰國\",\"country_code\":\"TH\"},{\"country_local_name\":\"多哥\",\"country_code\":\"TG\"},{\"country_local_name\":\"東加\",\"country_code\":\"TO\"},{\"country_local_name\":\"千里達及托巴哥\",\"country_code\":\"TT\"},{\"country_local_name\":\"突尼西亞\",\"country_code\":\"TN\"},{\"country_local_name\":\"土耳其\",\"country_code\":\"TR\"},{\"country_local_name\":\"土庫曼\",\"country_code\":\"TM\"},{\"country_local_name\":\"烏干達\",\"country_code\":\"UG\"},{\"country_local_name\":\"烏克蘭\",\"country_code\":\"UA\"},{\"country_local_name\":\"阿拉伯聯合大公國\",\"country_code\":\"AE\"},{\"country_local_name\":\"英國\",\"country_code\":\"GB\"},{\"country_local_name\":\"美國\",\"country_code\":\"US\"},{\"country_local_name\":\"烏拉圭\",\"country_code\":\"UY\"},{\"country_local_name\":\"烏茲別克\",\"country_code\":\"UZ\"},{\"country_local_name\":\"委內瑞拉\",\"country_code\":\"VE\"},{\"country_local_name\":\"越南\",\"country_code\":\"VN\"},{\"country_local_name\":\"葉門\",\"country_code\":\"YE\"},{\"country_local_name\":\"尚比亞\",\"country_code\":\"ZM\"},{\"country_local_name\":\"辛巴威\",\"country_code\":\"ZW\"}],\"nationality_id\":{\"is_require_HKMO\":true}},\"birthday\":{\"is_require\":true},\"passport\":{},\"high\":{},\"weight\":{},\"shoe_size\":{\"is_require\":true,\"man\":{\"is_provided\":true,\"unit_list\":[{\"unit_code\":\"03\",\"unit_name\":\"JP/CM\",\"size_range_start\":\"24.5\",\"size_range_end\":\"26.5\"},{\"unit_code\":\"02\",\"unit_name\":\"EU\",\"size_range_start\":\"39.5\",\"size_range_end\":\"41.5\"},{\"unit_code\":\"01\",\"unit_name\":\"US\",\"size_range_start\":\"6\",\"size_range_end\":\"8\"}]},\"woman\":{\"is_provided\":true,\"unit_list\":[{\"unit_code\":\"03\",\"unit_name\":\"JP/CM\",\"size_range_start\":\"22.5\",\"size_range_end\":\"23.5\"},{\"unit_code\":\"02\",\"unit_name\":\"EU\",\"size_range_start\":\"35.5\",\"size_range_end\":\"36.5\"},{\"unit_code\":\"01\",\"unit_name\":\"US\",\"size_range_start\":\"5.5\",\"size_range_end\":\"6.5\"}]},\"child\":{\"is_provided\":true,\"unit_list\":[{\"unit_code\":\"03\",\"unit_name\":\"JP/CM\",\"size_range_start\":\"12\",\"size_range_end\":\"14\"},{\"unit_code\":\"02\",\"unit_name\":\"EU\",\"size_range_start\":\"20\",\"size_range_end\":\"23\"},{\"unit_code\":\"01\",\"unit_name\":\"US\",\"size_range_start\":\"3\",\"size_range_end\":\"4.5\"}]}},\"meal\":{\"is_require\":true,\"meal_list\":[{\"is_provided\":true,\"meal_type\":\"0004\",\"meal_type_name\":\"猶太教餐\"},{\"is_provided\":true,\"meal_type\":\"0002\",\"meal_type_name\":\"素\"},{\"meal_type\":\"0003\",\"meal_type_name\":\"穆斯林餐\"},{\"meal_type\":\"0001\",\"meal_type_name\":\"葷\"}],\"exclude_food\":{\"is_exclude\":true,\"food_list\":[{\"food_type\":\"0006\",\"food_type_name\":\"魚\"},{\"can_exclude\":true,\"food_type\":\"0007\",\"food_type_name\":\"蛋\"},{\"can_exclude\":true,\"food_type\":\"0004\",\"food_type_name\":\"羊\"},{\"food_type\":\"0005\",\"food_type_name\":\"甲殼類\"},{\"can_exclude\":true,\"food_type\":\"0002\",\"food_type_name\":\"豬\"},{\"can_exclude\":true,\"food_type\":\"0003\",\"food_type_name\":\"雞\"},{\"can_exclude\":true,\"food_type\":\"0001\",\"food_type_name\":\"牛\"},{\"food_type\":\"0008\",\"food_type_name\":\"奶\"}],\"allergy_food\":{\"is_require_FoodAllergy\":true}}},\"glass_dopter\":{\"is_require\":true,\"diopter_range_start\":\"300\",\"diopter_range_end\":\"600\"}},\"module_contact_data\":{\"is_require\":true,\"contact_name\":{\"is_require_FirstName\":true,\"is_require_LastName\":true},\"contact_tel\":{\"is_require_TelNumber\":true,\"is_require_TelCountryCode\":true,\"tel_code_list\":[{\"country_tel_code\":\"975\",\"country_tel_info\":\"Bhutan(不丹)\"},{\"country_tel_code\":\"93\",\"country_tel_info\":\"Afghanistan(阿富汗)\"},{\"country_tel_code\":\"355\",\"country_tel_info\":\"Albania(阿爾巴尼亞)\"},{\"country_tel_code\":\"213\",\"country_tel_info\":\"Algeria(阿爾及利亞)\"},{\"country_tel_code\":\"376\",\"country_tel_info\":\"Andorra(安道爾)\"},{\"country_tel_code\":\"244\",\"country_tel_info\":\"Angola(安哥拉)\"},{\"country_tel_code\":\"1264\",\"country_tel_info\":\"Anguilla(安圭拉)\"},{\"country_tel_code\":\"1268\",\"country_tel_info\":\"Antigua and Barbuda(安地卡及巴布達)\"},{\"country_tel_code\":\"54\",\"country_tel_info\":\"Argentina(阿根廷)\"},{\"country_tel_code\":\"374\",\"country_tel_info\":\"Armenia(亞美尼亞)\"},{\"country_tel_code\":\"61\",\"country_tel_info\":\"Australia(澳洲)\"},{\"country_tel_code\":\"43\",\"country_tel_info\":\"Austria(奥地利)\"},{\"country_tel_code\":\"994\",\"country_tel_info\":\"Azerbaijan(亞塞拜然)\"},{\"country_tel_code\":\"1242\",\"country_tel_info\":\"Bahamas(巴哈馬)\"},{\"country_tel_code\":\"973\",\"country_tel_info\":\"Bahrain(巴林)\"},{\"country_tel_code\":\"880\",\"country_tel_info\":\"Bangladesh(孟加拉)\"},{\"country_tel_code\":\"1246\",\"country_tel_info\":\"Barbados(巴貝多)\"},{\"country_tel_code\":\"375\",\"country_tel_info\":\"Belarus(白俄羅斯)\"},{\"country_tel_code\":\"32\",\"country_tel_info\":\"Belgium(比利時)\"},{\"country_tel_code\":\"501\",\"country_tel_info\":\"Belize(貝里斯)\"},{\"country_tel_code\":\"229\",\"country_tel_info\":\"Benin(貝南)\"},{\"country_tel_code\":\"1441\",\"country_tel_info\":\"Bermuda(百慕達)\"},{\"country_tel_code\":\"591\",\"country_tel_info\":\"Bolivia(玻利維亞)\"},{\"country_tel_code\":\"267\",\"country_tel_info\":\"Botswana(波札那)\"},{\"country_tel_code\":\"55\",\"country_tel_info\":\"Brazil(巴西)\"},{\"country_tel_code\":\"673\",\"country_tel_info\":\"Brunei(汶萊)\"},{\"country_tel_code\":\"359\",\"country_tel_info\":\"Bulgaria(保加利亞)\"},{\"country_tel_code\":\"226\",\"country_tel_info\":\"Burkina Faso(布吉納法索)\"},{\"country_tel_code\":\"257\",\"country_tel_info\":\"Burundi(蒲隆地)\"},{\"country_tel_code\":\"855\",\"country_tel_info\":\"Cambodia(柬埔寨)\"},{\"country_tel_code\":\"237\",\"country_tel_info\":\"Cameroon(喀麥隆)\"},{\"country_tel_code\":\"1\",\"country_tel_info\":\"Canada(加拿大)\"},{\"country_tel_code\":\"236\",\"country_tel_info\":\"Central African Republic(中非共和國)\"},{\"country_tel_code\":\"235\",\"country_tel_info\":\"Chad(查德)\"},{\"country_tel_code\":\"56\",\"country_tel_info\":\"Chile(智利)\"},{\"country_tel_code\":\"86\",\"country_tel_info\":\"China(中國)\"},{\"country_tel_code\":\"57\",\"country_tel_info\":\"Colombia(哥倫比亞)\"},{\"country_tel_code\":\"682\",\"country_tel_info\":\"Cook Islands(庫克群島)\"},{\"country_tel_code\":\"506\",\"country_tel_info\":\"Costa Rica(哥斯大黎加)\"},{\"country_tel_code\":\"53\",\"country_tel_info\":\"Cuba(古巴)\"},{\"country_tel_code\":\"357\",\"country_tel_info\":\"Cyprus(賽普勒斯)\"},{\"country_tel_code\":\"420\",\"country_tel_info\":\"Czech Republic(捷克共和國)\"},{\"country_tel_code\":\"45\",\"country_tel_info\":\"Denmark(丹麥)\"},{\"country_tel_code\":\"253\",\"country_tel_info\":\"Djibouti(吉布地)\"},{\"country_tel_code\":\"1890\",\"country_tel_info\":\"Dominican Republic(多明尼加共和國)\"},{\"country_tel_code\":\"593\",\"country_tel_info\":\"Ecuador(厄瓜多)\"},{\"country_tel_code\":\"20\",\"country_tel_info\":\"Egypt(埃及)\"},{\"country_tel_code\":\"503\",\"country_tel_info\":\"El Salvador(薩爾瓦多)\"},{\"country_tel_code\":\"372\",\"country_tel_info\":\"Estonia(愛沙尼亞)\"},{\"country_tel_code\":\"251\",\"country_tel_info\":\"Ethiopia(衣索比亞)\"},{\"country_tel_code\":\"252\",\"country_tel_info\":\"Federal Republic of Somalia(索馬利亞聯邦共和國)\"},{\"country_tel_code\":\"679\",\"country_tel_info\":\"Fiji(斐濟)\"},{\"country_tel_code\":\"358\",\"country_tel_info\":\"Finland(芬蘭)\"},{\"country_tel_code\":\"33\",\"country_tel_info\":\"France(法國)\"},{\"country_tel_code\":\"594\",\"country_tel_info\":\"French Guiana(法屬圭亞那)\"},{\"country_tel_code\":\"689\",\"country_tel_info\":\"French Polynesia(法屬玻里尼西亞)\"},{\"country_tel_code\":\"241\",\"country_tel_info\":\"Gabon(加彭)\"},{\"country_tel_code\":\"220\",\"country_tel_info\":\"Gambia(甘比亞)\"},{\"country_tel_code\":\"995\",\"country_tel_info\":\"Georgia(喬治亞)\"},{\"country_tel_code\":\"49\",\"country_tel_info\":\"Germany(德國)\"},{\"country_tel_code\":\"233\",\"country_tel_info\":\"Ghana(迦納)\"},{\"country_tel_code\":\"350\",\"country_tel_info\":\"Gibraltar(直布羅陀)\"},{\"country_tel_code\":\"30\",\"country_tel_info\":\"Greece(希臘)\"},{\"country_tel_code\":\"1809\",\"country_tel_info\":\"Grenada(格瑞那達)\"},{\"country_tel_code\":\"1671\",\"country_tel_info\":\"Guam(關島)\"},{\"country_tel_code\":\"502\",\"country_tel_info\":\"Guatemala(瓜地馬拉)\"},{\"country_tel_code\":\"224\",\"country_tel_info\":\"Guinea(幾內亞)\"},{\"country_tel_code\":\"592\",\"country_tel_info\":\"Guyana(蓋亞那)\"},{\"country_tel_code\":\"509\",\"country_tel_info\":\"Haiti(海地)\"},{\"country_tel_code\":\"504\",\"country_tel_info\":\"Honduras(宏都拉斯)\"},{\"country_tel_code\":\"852\",\"country_tel_info\":\"Hong Kong(香港)\"},{\"country_tel_code\":\"36\",\"country_tel_info\":\"Hungary(匈牙利)\"},{\"country_tel_code\":\"354\",\"country_tel_info\":\"Iceland(冰島)\"},{\"country_tel_code\":\"91\",\"country_tel_info\":\"India(印度)\"},{\"country_tel_code\":\"62\",\"country_tel_info\":\"Indonesia(印尼)\"},{\"country_tel_code\":\"98\",\"country_tel_info\":\"Iran(伊朗)\"},{\"country_tel_code\":\"964\",\"country_tel_info\":\"Iraq(伊拉克)\"},{\"country_tel_code\":\"353\",\"country_tel_info\":\"Ireland(愛爾蘭)\"},{\"country_tel_code\":\"972\",\"country_tel_info\":\"Israel(以色列)\"},{\"country_tel_code\":\"39\",\"country_tel_info\":\"Italy(義大利)\"},{\"country_tel_code\":\"1876\",\"country_tel_info\":\"Jamaica(牙買加)\"},{\"country_tel_code\":\"81\",\"country_tel_info\":\"Japan(日本)\"},{\"country_tel_code\":\"962\",\"country_tel_info\":\"Jordan(約旦)\"},{\"country_tel_code\":\"327\",\"country_tel_info\":\"Kazakhstan(哈薩克)\"},{\"country_tel_code\":\"254\",\"country_tel_info\":\"Kenya(肯亞)\"},{\"country_tel_code\":\"965\",\"country_tel_info\":\"Kuwait(科威特)\"},{\"country_tel_code\":\"331\",\"country_tel_info\":\"Kyrgyzstan(吉爾吉斯坦)\"},{\"country_tel_code\":\"856\",\"country_tel_info\":\"Laos(寮國)\"},{\"country_tel_code\":\"371\",\"country_tel_info\":\"Latvia(拉脫維亞)\"},{\"country_tel_code\":\"961\",\"country_tel_info\":\"Lebanon(黎巴嫩)\"},{\"country_tel_code\":\"266\",\"country_tel_info\":\"Lesotho(賴索托)\"},{\"country_tel_code\":\"231\",\"country_tel_info\":\"Liberia(賴比瑞亞)\"},{\"country_tel_code\":\"218\",\"country_tel_info\":\"Libya(利比亞)\"},{\"country_tel_code\":\"423\",\"country_tel_info\":\"Liechtenstein(列支敦斯登)\"},{\"country_tel_code\":\"370\",\"country_tel_info\":\"Lithuania(立陶宛)\"},{\"country_tel_code\":\"352\",\"country_tel_info\":\"Luxembourg(盧森堡)\"},{\"country_tel_code\":\"853\",\"country_tel_info\":\"Macau(澳門)\"},{\"country_tel_code\":\"261\",\"country_tel_info\":\"Madagascar(馬達加斯加)\"},{\"country_tel_code\":\"265\",\"country_tel_info\":\"Malawi(馬拉威)\"},{\"country_tel_code\":\"60\",\"country_tel_info\":\"Malaysia(馬來西亞)\"},{\"country_tel_code\":\"960\",\"country_tel_info\":\"Maldives(馬爾地夫)\"},{\"country_tel_code\":\"223\",\"country_tel_info\":\"Mali(馬利)\"},{\"country_tel_code\":\"356\",\"country_tel_info\":\"Malta(馬爾他)\"},{\"country_tel_code\":\"230\",\"country_tel_info\":\"Mauritius(模里西斯)\"},{\"country_tel_code\":\"52\",\"country_tel_info\":\"Mexico(墨西哥)\"},{\"country_tel_code\":\"373\",\"country_tel_info\":\"Moldova(摩爾多瓦)\"},{\"country_tel_code\":\"377\",\"country_tel_info\":\"Monaco(摩納哥)\"},{\"country_tel_code\":\"976\",\"country_tel_info\":\"Mongolia(蒙古)\"},{\"country_tel_code\":\"1664\",\"country_tel_info\":\"Montserrat(蒙塞拉特)\"},{\"country_tel_code\":\"212\",\"country_tel_info\":\"Morocco(摩洛哥)\"},{\"country_tel_code\":\"258\",\"country_tel_info\":\"Mozambique(莫三比克)\"},{\"country_tel_code\":\"95\",\"country_tel_info\":\"Myanmar(緬甸)\"},{\"country_tel_code\":\"264\",\"country_tel_info\":\"Namibia(那米比亞)\"},{\"country_tel_code\":\"674\",\"country_tel_info\":\"Nauru(諾魯)\"},{\"country_tel_code\":\"977\",\"country_tel_info\":\"Nepal(尼泊爾)\"},{\"country_tel_code\":\"31\",\"country_tel_info\":\"Netherlands(荷蘭)\"},{\"country_tel_code\":\"64\",\"country_tel_info\":\"New Zealand(紐西蘭)\"},{\"country_tel_code\":\"505\",\"country_tel_info\":\"Nicaragua(尼加拉瓜)\"},{\"country_tel_code\":\"227\",\"country_tel_info\":\"Niger(尼日)\"},{\"country_tel_code\":\"234\",\"country_tel_info\":\"Nigeria(奈及利亞)\"},{\"country_tel_code\":\"850\",\"country_tel_info\":\"North Korea(北韓)\"},{\"country_tel_code\":\"47\",\"country_tel_info\":\"Norway(挪威)\"},{\"country_tel_code\":\"968\",\"country_tel_info\":\"Oman(阿曼)\"},{\"country_tel_code\":\"92\",\"country_tel_info\":\"Pakistan(巴基斯坦)\"},{\"country_tel_code\":\"507\",\"country_tel_info\":\"Panama(巴拿馬)\"},{\"country_tel_code\":\"675\",\"country_tel_info\":\"Papua New Guinea(巴布亞紐幾內亞)\"},{\"country_tel_code\":\"595\",\"country_tel_info\":\"Paraguay(巴拉圭)\"},{\"country_tel_code\":\"51\",\"country_tel_info\":\"Peru(秘魯)\"},{\"country_tel_code\":\"63\",\"country_tel_info\":\"Philippines(菲律賓)\"},{\"country_tel_code\":\"48\",\"country_tel_info\":\"Poland(波蘭)\"},{\"country_tel_code\":\"351\",\"country_tel_info\":\"Portugal(葡萄牙)\"},{\"country_tel_code\":\"1787\",\"country_tel_info\":\"Puerto Rico(波多黎各)\"},{\"country_tel_code\":\"974\",\"country_tel_info\":\"Qatar(卡達)\"},{\"country_tel_code\":\"242\",\"country_tel_info\":\"Republic of the Congo(剛果共和國)\"},{\"country_tel_code\":\"40\",\"country_tel_info\":\"Romania(羅馬尼亞)\"},{\"country_tel_code\":\"7\",\"country_tel_info\":\"Russia(俄羅斯)\"},{\"country_tel_code\":\"378\",\"country_tel_info\":\"San Marino(聖馬里諾)\"},{\"country_tel_code\":\"239\",\"country_tel_info\":\"Sao Tome and Principe(聖多美普林西比)\"},{\"country_tel_code\":\"966\",\"country_tel_info\":\"Saudi Arabia(沙烏地阿拉伯)\"},{\"country_tel_code\":\"221\",\"country_tel_info\":\"Senegal(塞內加爾)\"},{\"country_tel_code\":\"248\",\"country_tel_info\":\"Seychelles(塞席爾)\"},{\"country_tel_code\":\"232\",\"country_tel_info\":\"Sierra Leone(獅子山)\"},{\"country_tel_code\":\"65\",\"country_tel_info\":\"Singapore(新加坡)\"},{\"country_tel_code\":\"421\",\"country_tel_info\":\"Slovakia(斯洛伐克)\"},{\"country_tel_code\":\"386\",\"country_tel_info\":\"Slovenia(斯洛維尼亞)\"},{\"country_tel_code\":\"677\",\"country_tel_info\":\"Solomon Islands(索羅門群島)\"},{\"country_tel_code\":\"27\",\"country_tel_info\":\"South Africa(南非)\"},{\"country_tel_code\":\"82\",\"country_tel_info\":\"South Korea(南韓)\"},{\"country_tel_code\":\"34\",\"country_tel_info\":\"Spain(西班牙)\"},{\"country_tel_code\":\"94\",\"country_tel_info\":\"Sri Lanka(斯里蘭卡)\"},{\"country_tel_code\":\"1758\",\"country_tel_info\":\"St. Lucia(聖露西亞)\"},{\"country_tel_code\":\"1784\",\"country_tel_info\":\"St. Vincent and the Grenadines(聖文森及格瑞那丁)\"},{\"country_tel_code\":\"249\",\"country_tel_info\":\"Sudan(蘇丹)\"},{\"country_tel_code\":\"597\",\"country_tel_info\":\"Suriname(蘇利南)\"},{\"country_tel_code\":\"268\",\"country_tel_info\":\"Swaziland(史瓦濟蘭)\"},{\"country_tel_code\":\"46\",\"country_tel_info\":\"Sweden(瑞典)\"},{\"country_tel_code\":\"41\",\"country_tel_info\":\"Switzerland(瑞士)\"},{\"country_tel_code\":\"963\",\"country_tel_info\":\"Syria(希利亞)\"},{\"country_tel_code\":\"886\",\"country_tel_info\":\"Taiwan(台灣)\"},{\"country_tel_code\":\"992\",\"country_tel_info\":\"Tajikistan(塔吉克)\"},{\"country_tel_code\":\"255\",\"country_tel_info\":\"Tanzania(坦尚尼亞)\"},{\"country_tel_code\":\"66\",\"country_tel_info\":\"Thailand(泰國)\"},{\"country_tel_code\":\"228\",\"country_tel_info\":\"Togo(多哥)\"},{\"country_tel_code\":\"676\",\"country_tel_info\":\"Tonga(東加)\"},{\"country_tel_code\":\"1809\",\"country_tel_info\":\"Trinidad and Tobago(千里達及托巴哥)\"},{\"country_tel_code\":\"216\",\"country_tel_info\":\"Tunisia(突尼西亞)\"},{\"country_tel_code\":\"90\",\"country_tel_info\":\"Turkey(土耳其)\"},{\"country_tel_code\":\"993\",\"country_tel_info\":\"Turkmenistan(土庫曼)\"},{\"country_tel_code\":\"256\",\"country_tel_info\":\"Uganda(烏干達)\"},{\"country_tel_code\":\"380\",\"country_tel_info\":\"Ukraine(烏克蘭)\"},{\"country_tel_code\":\"971\",\"country_tel_info\":\"United Arab Emirates(阿拉伯聯合大公國)\"},{\"country_tel_code\":\"44\",\"country_tel_info\":\"United Kingdom(英國)\"},{\"country_tel_code\":\"1\",\"country_tel_info\":\"United States of America(美國)\"},{\"country_tel_code\":\"598\",\"country_tel_info\":\"Uruguay(烏拉圭)\"},{\"country_tel_code\":\"233\",\"country_tel_info\":\"Uzbekistan(烏茲別克)\"},{\"country_tel_code\":\"58\",\"country_tel_info\":\"Venezuela(委內瑞拉)\"},{\"country_tel_code\":\"84\",\"country_tel_info\":\"Vietnam(越南)\"},{\"country_tel_code\":\"967\",\"country_tel_info\":\"Yemen(葉門)\"},{\"country_tel_code\":\"260\",\"country_tel_info\":\"Zambia(尚比亞)\"},{\"country_tel_code\":\"263\",\"country_tel_info\":\"Zimbabwe(辛巴威)\"}]},\"contact_app\":{\"is_require\":true,\"app_type_list\":[{\"app_type\":\"0006\",\"app_name\":\"SnapChat\"},{\"app_type\":\"0007\",\"app_name\":\"Facebook Messenger\"},{\"is_supported\":true,\"app_type\":\"0004\",\"app_name\":\"Kakao\"},{\"is_supported\":true,\"app_type\":\"0005\",\"app_name\":\"Viber\"},{\"app_type\":\"0002\",\"app_name\":\"WhatsApp\"},{\"app_type\":\"0003\",\"app_name\":\"WeChat\"},{\"app_type\":\"0001\",\"app_name\":\"Line\"},{\"app_type\":\"0008\",\"app_name\":\"Twitter\"}],\"is_require_AppAccount\":true}},\"module_sim_wifi\":{\"is_require\":true,\"mobile_modle_no\":{\"is_require\":true},\"mobile_IMEI\":{\"is_require\":true},\"activation_date\":{\"is_require\":true}},\"module_car_pasgr\":{\"is_require\":true,\"adul_qty\":{\"is_require\":true,\"age_range_start\":12,\"age_range_end\":99},\"child_qty\":{\"is_require\":true,\"age_range_start\":2,\"age_range_end\":11},\"infant_qty\":{\"is_require\":true,\"age_range_start\":0,\"age_range_end\":2},\"child_safety_seat\":{\"is_require_supplierProvided\":true,\"is_require_selfProvided\":true,\"age_range_start\":2,\"age_range_end\":11},\"infant_safety_seat\":{\"is_require_supplierProvided\":true,\"is_require_selfProvided\":true,\"age_range_start\":0,\"age_range_end\":2},\"carry_luggage_qty\":{\"is_require\":true},\"checked_luggage_qty\":{\"is_require\":true}},\"module_flight_info\":{\"is_require\":true,\"arrival\":{\"is_require_FlightType\":true,\"is_require_Date\":true,\"is_require_Hour\":true,\"is_require_Minute\":true,\"is_require_Airline\":true,\"is_require_FlightNo\":true,\"is_require_TerminalNo\":true,\"is_need_ApplyVisa\":true},\"departure\":{\"is_require_FlightType\":true,\"is_require_Date\":true,\"is_require_Hour\":true,\"is_require_Minute\":true,\"is_require_Airline\":true,\"is_require_FlightNo\":true,\"is_require_TerminalNo\":true}},\"module_send_data\":{\"is_require\":true,\"receiver_name\":{\"is_require_FirstName\":true,\"is_require_LastName\":true},\"receiver_tel\":{\"is_require_TelNumber\":true,\"is_require_TelCountryCode\":true,\"tel_code_list\":[{\"country_tel_code\":\"975\",\"country_tel_info\":\"Bhutan(不丹)\"},{\"country_tel_code\":\"93\",\"country_tel_info\":\"Afghanistan(阿富汗)\"},{\"country_tel_code\":\"355\",\"country_tel_info\":\"Albania(阿爾巴尼亞)\"},{\"country_tel_code\":\"213\",\"country_tel_info\":\"Algeria(阿爾及利亞)\"},{\"country_tel_code\":\"376\",\"country_tel_info\":\"Andorra(安道爾)\"},{\"country_tel_code\":\"244\",\"country_tel_info\":\"Angola(安哥拉)\"},{\"country_tel_code\":\"1264\",\"country_tel_info\":\"Anguilla(安圭拉)\"},{\"country_tel_code\":\"1268\",\"country_tel_info\":\"Antigua and Barbuda(安地卡及巴布達)\"},{\"country_tel_code\":\"54\",\"country_tel_info\":\"Argentina(阿根廷)\"},{\"country_tel_code\":\"374\",\"country_tel_info\":\"Armenia(亞美尼亞)\"},{\"country_tel_code\":\"61\",\"country_tel_info\":\"Australia(澳洲)\"},{\"country_tel_code\":\"43\",\"country_tel_info\":\"Austria(奥地利)\"},{\"country_tel_code\":\"994\",\"country_tel_info\":\"Azerbaijan(亞塞拜然)\"},{\"country_tel_code\":\"1242\",\"country_tel_info\":\"Bahamas(巴哈馬)\"},{\"country_tel_code\":\"973\",\"country_tel_info\":\"Bahrain(巴林)\"},{\"country_tel_code\":\"880\",\"country_tel_info\":\"Bangladesh(孟加拉)\"},{\"country_tel_code\":\"1246\",\"country_tel_info\":\"Barbados(巴貝多)\"},{\"country_tel_code\":\"375\",\"country_tel_info\":\"Belarus(白俄羅斯)\"},{\"country_tel_code\":\"32\",\"country_tel_info\":\"Belgium(比利時)\"},{\"country_tel_code\":\"501\",\"country_tel_info\":\"Belize(貝里斯)\"},{\"country_tel_code\":\"229\",\"country_tel_info\":\"Benin(貝南)\"},{\"country_tel_code\":\"1441\",\"country_tel_info\":\"Bermuda(百慕達)\"},{\"country_tel_code\":\"591\",\"country_tel_info\":\"Bolivia(玻利維亞)\"},{\"country_tel_code\":\"267\",\"country_tel_info\":\"Botswana(波札那)\"},{\"country_tel_code\":\"55\",\"country_tel_info\":\"Brazil(巴西)\"},{\"country_tel_code\":\"673\",\"country_tel_info\":\"Brunei(汶萊)\"},{\"country_tel_code\":\"359\",\"country_tel_info\":\"Bulgaria(保加利亞)\"},{\"country_tel_code\":\"226\",\"country_tel_info\":\"Burkina Faso(布吉納法索)\"},{\"country_tel_code\":\"257\",\"country_tel_info\":\"Burundi(蒲隆地)\"},{\"country_tel_code\":\"855\",\"country_tel_info\":\"Cambodia(柬埔寨)\"},{\"country_tel_code\":\"237\",\"country_tel_info\":\"Cameroon(喀麥隆)\"},{\"country_tel_code\":\"1\",\"country_tel_info\":\"Canada(加拿大)\"},{\"country_tel_code\":\"236\",\"country_tel_info\":\"Central African Republic(中非共和國)\"},{\"country_tel_code\":\"235\",\"country_tel_info\":\"Chad(查德)\"},{\"country_tel_code\":\"56\",\"country_tel_info\":\"Chile(智利)\"},{\"country_tel_code\":\"86\",\"country_tel_info\":\"China(中國)\"},{\"country_tel_code\":\"57\",\"country_tel_info\":\"Colombia(哥倫比亞)\"},{\"country_tel_code\":\"682\",\"country_tel_info\":\"Cook Islands(庫克群島)\"},{\"country_tel_code\":\"506\",\"country_tel_info\":\"Costa Rica(哥斯大黎加)\"},{\"country_tel_code\":\"53\",\"country_tel_info\":\"Cuba(古巴)\"},{\"country_tel_code\":\"357\",\"country_tel_info\":\"Cyprus(賽普勒斯)\"},{\"country_tel_code\":\"420\",\"country_tel_info\":\"Czech Republic(捷克共和國)\"},{\"country_tel_code\":\"45\",\"country_tel_info\":\"Denmark(丹麥)\"},{\"country_tel_code\":\"253\",\"country_tel_info\":\"Djibouti(吉布地)\"},{\"country_tel_code\":\"1890\",\"country_tel_info\":\"Dominican Republic(多明尼加共和國)\"},{\"country_tel_code\":\"593\",\"country_tel_info\":\"Ecuador(厄瓜多)\"},{\"country_tel_code\":\"20\",\"country_tel_info\":\"Egypt(埃及)\"},{\"country_tel_code\":\"503\",\"country_tel_info\":\"El Salvador(薩爾瓦多)\"},{\"country_tel_code\":\"372\",\"country_tel_info\":\"Estonia(愛沙尼亞)\"},{\"country_tel_code\":\"251\",\"country_tel_info\":\"Ethiopia(衣索比亞)\"},{\"country_tel_code\":\"252\",\"country_tel_info\":\"Federal Republic of Somalia(索馬利亞聯邦共和國)\"},{\"country_tel_code\":\"679\",\"country_tel_info\":\"Fiji(斐濟)\"},{\"country_tel_code\":\"358\",\"country_tel_info\":\"Finland(芬蘭)\"},{\"country_tel_code\":\"33\",\"country_tel_info\":\"France(法國)\"},{\"country_tel_code\":\"594\",\"country_tel_info\":\"French Guiana(法屬圭亞那)\"},{\"country_tel_code\":\"689\",\"country_tel_info\":\"French Polynesia(法屬玻里尼西亞)\"},{\"country_tel_code\":\"241\",\"country_tel_info\":\"Gabon(加彭)\"},{\"country_tel_code\":\"220\",\"country_tel_info\":\"Gambia(甘比亞)\"},{\"country_tel_code\":\"995\",\"country_tel_info\":\"Georgia(喬治亞)\"},{\"country_tel_code\":\"49\",\"country_tel_info\":\"Germany(德國)\"},{\"country_tel_code\":\"233\",\"country_tel_info\":\"Ghana(迦納)\"},{\"country_tel_code\":\"350\",\"country_tel_info\":\"Gibraltar(直布羅陀)\"},{\"country_tel_code\":\"30\",\"country_tel_info\":\"Greece(希臘)\"},{\"country_tel_code\":\"1809\",\"country_tel_info\":\"Grenada(格瑞那達)\"},{\"country_tel_code\":\"1671\",\"country_tel_info\":\"Guam(關島)\"},{\"country_tel_code\":\"502\",\"country_tel_info\":\"Guatemala(瓜地馬拉)\"},{\"country_tel_code\":\"224\",\"country_tel_info\":\"Guinea(幾內亞)\"},{\"country_tel_code\":\"592\",\"country_tel_info\":\"Guyana(蓋亞那)\"},{\"country_tel_code\":\"509\",\"country_tel_info\":\"Haiti(海地)\"},{\"country_tel_code\":\"504\",\"country_tel_info\":\"Honduras(宏都拉斯)\"},{\"country_tel_code\":\"852\",\"country_tel_info\":\"Hong Kong(香港)\"},{\"country_tel_code\":\"36\",\"country_tel_info\":\"Hungary(匈牙利)\"},{\"country_tel_code\":\"354\",\"country_tel_info\":\"Iceland(冰島)\"},{\"country_tel_code\":\"91\",\"country_tel_info\":\"India(印度)\"},{\"country_tel_code\":\"62\",\"country_tel_info\":\"Indonesia(印尼)\"},{\"country_tel_code\":\"98\",\"country_tel_info\":\"Iran(伊朗)\"},{\"country_tel_code\":\"964\",\"country_tel_info\":\"Iraq(伊拉克)\"},{\"country_tel_code\":\"353\",\"country_tel_info\":\"Ireland(愛爾蘭)\"},{\"country_tel_code\":\"972\",\"country_tel_info\":\"Israel(以色列)\"},{\"country_tel_code\":\"39\",\"country_tel_info\":\"Italy(義大利)\"},{\"country_tel_code\":\"1876\",\"country_tel_info\":\"Jamaica(牙買加)\"},{\"country_tel_code\":\"81\",\"country_tel_info\":\"Japan(日本)\"},{\"country_tel_code\":\"962\",\"country_tel_info\":\"Jordan(約旦)\"},{\"country_tel_code\":\"327\",\"country_tel_info\":\"Kazakhstan(哈薩克)\"},{\"country_tel_code\":\"254\",\"country_tel_info\":\"Kenya(肯亞)\"},{\"country_tel_code\":\"965\",\"country_tel_info\":\"Kuwait(科威特)\"},{\"country_tel_code\":\"331\",\"country_tel_info\":\"Kyrgyzstan(吉爾吉斯坦)\"},{\"country_tel_code\":\"856\",\"country_tel_info\":\"Laos(寮國)\"},{\"country_tel_code\":\"371\",\"country_tel_info\":\"Latvia(拉脫維亞)\"},{\"country_tel_code\":\"961\",\"country_tel_info\":\"Lebanon(黎巴嫩)\"},{\"country_tel_code\":\"266\",\"country_tel_info\":\"Lesotho(賴索托)\"},{\"country_tel_code\":\"231\",\"country_tel_info\":\"Liberia(賴比瑞亞)\"},{\"country_tel_code\":\"218\",\"country_tel_info\":\"Libya(利比亞)\"},{\"country_tel_code\":\"423\",\"country_tel_info\":\"Liechtenstein(列支敦斯登)\"},{\"country_tel_code\":\"370\",\"country_tel_info\":\"Lithuania(立陶宛)\"},{\"country_tel_code\":\"352\",\"country_tel_info\":\"Luxembourg(盧森堡)\"},{\"country_tel_code\":\"853\",\"country_tel_info\":\"Macau(澳門)\"},{\"country_tel_code\":\"261\",\"country_tel_info\":\"Madagascar(馬達加斯加)\"},{\"country_tel_code\":\"265\",\"country_tel_info\":\"Malawi(馬拉威)\"},{\"country_tel_code\":\"60\",\"country_tel_info\":\"Malaysia(馬來西亞)\"},{\"country_tel_code\":\"960\",\"country_tel_info\":\"Maldives(馬爾地夫)\"},{\"country_tel_code\":\"223\",\"country_tel_info\":\"Mali(馬利)\"},{\"country_tel_code\":\"356\",\"country_tel_info\":\"Malta(馬爾他)\"},{\"country_tel_code\":\"230\",\"country_tel_info\":\"Mauritius(模里西斯)\"},{\"country_tel_code\":\"52\",\"country_tel_info\":\"Mexico(墨西哥)\"},{\"country_tel_code\":\"373\",\"country_tel_info\":\"Moldova(摩爾多瓦)\"},{\"country_tel_code\":\"377\",\"country_tel_info\":\"Monaco(摩納哥)\"},{\"country_tel_code\":\"976\",\"country_tel_info\":\"Mongolia(蒙古)\"},{\"country_tel_code\":\"1664\",\"country_tel_info\":\"Montserrat(蒙塞拉特)\"},{\"country_tel_code\":\"212\",\"country_tel_info\":\"Morocco(摩洛哥)\"},{\"country_tel_code\":\"258\",\"country_tel_info\":\"Mozambique(莫三比克)\"},{\"country_tel_code\":\"95\",\"country_tel_info\":\"Myanmar(緬甸)\"},{\"country_tel_code\":\"264\",\"country_tel_info\":\"Namibia(那米比亞)\"},{\"country_tel_code\":\"674\",\"country_tel_info\":\"Nauru(諾魯)\"},{\"country_tel_code\":\"977\",\"country_tel_info\":\"Nepal(尼泊爾)\"},{\"country_tel_code\":\"31\",\"country_tel_info\":\"Netherlands(荷蘭)\"},{\"country_tel_code\":\"64\",\"country_tel_info\":\"New Zealand(紐西蘭)\"},{\"country_tel_code\":\"505\",\"country_tel_info\":\"Nicaragua(尼加拉瓜)\"},{\"country_tel_code\":\"227\",\"country_tel_info\":\"Niger(尼日)\"},{\"country_tel_code\":\"234\",\"country_tel_info\":\"Nigeria(奈及利亞)\"},{\"country_tel_code\":\"850\",\"country_tel_info\":\"North Korea(北韓)\"},{\"country_tel_code\":\"47\",\"country_tel_info\":\"Norway(挪威)\"},{\"country_tel_code\":\"968\",\"country_tel_info\":\"Oman(阿曼)\"},{\"country_tel_code\":\"92\",\"country_tel_info\":\"Pakistan(巴基斯坦)\"},{\"country_tel_code\":\"507\",\"country_tel_info\":\"Panama(巴拿馬)\"},{\"country_tel_code\":\"675\",\"country_tel_info\":\"Papua New Guinea(巴布亞紐幾內亞)\"},{\"country_tel_code\":\"595\",\"country_tel_info\":\"Paraguay(巴拉圭)\"},{\"country_tel_code\":\"51\",\"country_tel_info\":\"Peru(秘魯)\"},{\"country_tel_code\":\"63\",\"country_tel_info\":\"Philippines(菲律賓)\"},{\"country_tel_code\":\"48\",\"country_tel_info\":\"Poland(波蘭)\"},{\"country_tel_code\":\"351\",\"country_tel_info\":\"Portugal(葡萄牙)\"},{\"country_tel_code\":\"1787\",\"country_tel_info\":\"Puerto Rico(波多黎各)\"},{\"country_tel_code\":\"974\",\"country_tel_info\":\"Qatar(卡達)\"},{\"country_tel_code\":\"242\",\"country_tel_info\":\"Republic of the Congo(剛果共和國)\"},{\"country_tel_code\":\"40\",\"country_tel_info\":\"Romania(羅馬尼亞)\"},{\"country_tel_code\":\"7\",\"country_tel_info\":\"Russia(俄羅斯)\"},{\"country_tel_code\":\"378\",\"country_tel_info\":\"San Marino(聖馬里諾)\"},{\"country_tel_code\":\"239\",\"country_tel_info\":\"Sao Tome and Principe(聖多美普林西比)\"},{\"country_tel_code\":\"966\",\"country_tel_info\":\"Saudi Arabia(沙烏地阿拉伯)\"},{\"country_tel_code\":\"221\",\"country_tel_info\":\"Senegal(塞內加爾)\"},{\"country_tel_code\":\"248\",\"country_tel_info\":\"Seychelles(塞席爾)\"},{\"country_tel_code\":\"232\",\"country_tel_info\":\"Sierra Leone(獅子山)\"},{\"country_tel_code\":\"65\",\"country_tel_info\":\"Singapore(新加坡)\"},{\"country_tel_code\":\"421\",\"country_tel_info\":\"Slovakia(斯洛伐克)\"},{\"country_tel_code\":\"386\",\"country_tel_info\":\"Slovenia(斯洛維尼亞)\"},{\"country_tel_code\":\"677\",\"country_tel_info\":\"Solomon Islands(索羅門群島)\"},{\"country_tel_code\":\"27\",\"country_tel_info\":\"South Africa(南非)\"},{\"country_tel_code\":\"82\",\"country_tel_info\":\"South Korea(南韓)\"},{\"country_tel_code\":\"34\",\"country_tel_info\":\"Spain(西班牙)\"},{\"country_tel_code\":\"94\",\"country_tel_info\":\"Sri Lanka(斯里蘭卡)\"},{\"country_tel_code\":\"1758\",\"country_tel_info\":\"St. Lucia(聖露西亞)\"},{\"country_tel_code\":\"1784\",\"country_tel_info\":\"St. Vincent and the Grenadines(聖文森及格瑞那丁)\"},{\"country_tel_code\":\"249\",\"country_tel_info\":\"Sudan(蘇丹)\"},{\"country_tel_code\":\"597\",\"country_tel_info\":\"Suriname(蘇利南)\"},{\"country_tel_code\":\"268\",\"country_tel_info\":\"Swaziland(史瓦濟蘭)\"},{\"country_tel_code\":\"46\",\"country_tel_info\":\"Sweden(瑞典)\"},{\"country_tel_code\":\"41\",\"country_tel_info\":\"Switzerland(瑞士)\"},{\"country_tel_code\":\"963\",\"country_tel_info\":\"Syria(希利亞)\"},{\"country_tel_code\":\"886\",\"country_tel_info\":\"Taiwan(台灣)\"},{\"country_tel_code\":\"992\",\"country_tel_info\":\"Tajikistan(塔吉克)\"},{\"country_tel_code\":\"255\",\"country_tel_info\":\"Tanzania(坦尚尼亞)\"},{\"country_tel_code\":\"66\",\"country_tel_info\":\"Thailand(泰國)\"},{\"country_tel_code\":\"228\",\"country_tel_info\":\"Togo(多哥)\"},{\"country_tel_code\":\"676\",\"country_tel_info\":\"Tonga(東加)\"},{\"country_tel_code\":\"1809\",\"country_tel_info\":\"Trinidad and Tobago(千里達及托巴哥)\"},{\"country_tel_code\":\"216\",\"country_tel_info\":\"Tunisia(突尼西亞)\"},{\"country_tel_code\":\"90\",\"country_tel_info\":\"Turkey(土耳其)\"},{\"country_tel_code\":\"993\",\"country_tel_info\":\"Turkmenistan(土庫曼)\"},{\"country_tel_code\":\"256\",\"country_tel_info\":\"Uganda(烏干達)\"},{\"country_tel_code\":\"380\",\"country_tel_info\":\"Ukraine(烏克蘭)\"},{\"country_tel_code\":\"971\",\"country_tel_info\":\"United Arab Emirates(阿拉伯聯合大公國)\"},{\"country_tel_code\":\"44\",\"country_tel_info\":\"United Kingdom(英國)\"},{\"country_tel_code\":\"1\",\"country_tel_info\":\"United States of America(美國)\"},{\"country_tel_code\":\"598\",\"country_tel_info\":\"Uruguay(烏拉圭)\"},{\"country_tel_code\":\"233\",\"country_tel_info\":\"Uzbekistan(烏茲別克)\"},{\"country_tel_code\":\"58\",\"country_tel_info\":\"Venezuela(委內瑞拉)\"},{\"country_tel_code\":\"84\",\"country_tel_info\":\"Vietnam(越南)\"},{\"country_tel_code\":\"967\",\"country_tel_info\":\"Yemen(葉門)\"},{\"country_tel_code\":\"260\",\"country_tel_info\":\"Zambia(尚比亞)\"},{\"country_tel_code\":\"263\",\"country_tel_info\":\"Zimbabwe(辛巴威)\"}]},\"receiver_address\":{\"is_require_Country\":true,\"is_require_City\":true,\"is_require_ZipCode\":true,\"is_require_Address\":true,\"country_list\":[{\"id\":\"A01-001\",\"name\":\"台灣\",\"cities\":[{\"id\":\"A01-001-00001\",\"name\":\"台北\"},{\"id\":\"A01-001-00005\",\"name\":\"花蓮\"},{\"id\":\"A01-001-00008\",\"name\":\"桃園\"},{\"id\":\"A01-001-00002\",\"name\":\"台中\"},{\"id\":\"A01-001-00004\",\"name\":\"高雄\"},{\"id\":\"A01-001-00014\",\"name\":\"嘉義\"},{\"id\":\"A01-001-00009\",\"name\":\"新竹\"},{\"id\":\"A01-001-00006\",\"name\":\"新北市\"},{\"id\":\"A01-001-00003\",\"name\":\"台南\"},{\"id\":\"A01-001-00010\",\"name\":\"苗栗\"},{\"id\":\"A01-001-00013\",\"name\":\"雲林\"},{\"id\":\"A01-001-00011\",\"name\":\"彰化\"},{\"id\":\"A01-001-00012\",\"name\":\"南投\"},{\"id\":\"A01-001-00016\",\"name\":\"墾丁\"},{\"id\":\"A01-001-00017\",\"name\":\"宜蘭\"},{\"id\":\"A01-001-00018\",\"name\":\"台東\"},{\"id\":\"A01-001-00019\",\"name\":\"澎湖\"},{\"id\":\"A01-001-00015\",\"name\":\"屏東\"},{\"id\":\"A01-001-00023\",\"name\":\"綠島\"},{\"id\":\"A01-001-00022\",\"name\":\"蘭嶼\"},{\"id\":\"A01-001-00026\",\"name\":\"基隆\"},{\"id\":\"A01-001-00020\",\"name\":\"金門\"},{\"id\":\"A01-001-99999\",\"name\":\"所有城市\"}]}]},\"send_to_hotel\":{\"is_provided\":true,\"send_to_hotel_info\":{\"is_require_HotelName\":true,\"is_require_HotelAddress\":true,\"is_require_HotelTel\":true,\"is_require_BuyerPassportEnglishFirstName\":true,\"is_require_BuyerPassportEnglishLastName\":true,\"is_require_BuyerLocalFirstName\":true,\"is_require_BuyerLocalLastName\":true,\"is_require_BookingOrderNo\":true,\"is_require_BookingWebsite\":true,\"is_require_CheckOutDate\":true,\"is_require_CheckInDate\":true}}},\"module_rent_car\":{\"is_require\":true,\"rent_type\":\"03\",\"driver_shuttle\":{\"charterRoute\":{\"is_require\":true,\"route_list\":[{\"sort\":1,\"id\":\"20181016_h8qr0\",\"routeEng\":\"水金九路線\",\"routeLocal\":\"水金九路線（在地語系）\"},{\"sort\":2,\"id\":\"20181016_d84jt\",\"routeEng\":\"不水金九路線\",\"routeLocal\":\"不水金九路線（在地語系）\"}],\"route_custom\":{\"is_require\":true,\"is_require_Location\":true,\"route_limit\":2}}}},\"module_venue_info\":{\"is_require\":true,\"venue_type\":\"04\",\"is_require_Date\":true,\"designated_location_list\":[{\"sort\":1,\"id\":\"20181004_9hcgx\",\"location_name\":\"西門町\",\"location_address\":\"漢口街\",\"time_range_start\":\"8:0\",\"time_range_end\":\"9:0\"}],\"designated_by_customer\":{\"pick_up\":{\"is_require_Location\":true,\"time\":{\"is_require\":true,\"custom\":{\"is_allow\":true,\"time_range_start\":\"1:30\",\"time_range_end\":\"3:40\"},\"time_list\":[{\"id\":\"20181016_f17e4\",\"hour\":\"1\",\"minute\":\"20\"},{\"id\":\"20181016_2ndbe\",\"hour\":\"2\",\"minute\":\"30\"}]}},\"drop_off\":{\"is_require_Location\":true}}},\"module_guide_lang_list\":[{\"lang_code\":\"en\",\"lang_name\":\"English\"},{\"lang_code\":\"ja\",\"lang_name\":\"日本語\"},{\"lang_code\":\"zh-tw\",\"lang_name\":\"中文\"}]}";
                ProductModuleModel module = JsonConvert.DeserializeObject<ProductModuleModel>(result);

                return module;
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"apiHelpler_getProdModule_err:{ JsonConvert.SerializeObject(ex.Message.ToString())}");
                throw new Exception(title.result_code_9990);
            }
        }

        public static ProductforEcModel getProdDtl(long B2dXid, string state, string lang, string currency, string prodoid, ProdTitleModel title)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                 delegate (object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                 { return true; };

                var pathUrl = Website.Instance.Configuration["B2DApiUrl:apiUri"];

                string result;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{pathUrl}product/QueryProduct");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                ApiRequest setting = new ApiRequest();

                setting.company_xid = B2dXid.ToString();
                setting.current_currency = currency;
                setting.locale_lang = lang;
                setting.prod_no = prodoid;
                setting.state = state;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(setting);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //result = "{\"reasult\":\"0000\",\"reasult_msg\":\"正確\",\"prod_type_name\":\"一日旅遊\",\"main_lang\":\"zh-tw\",\"cost_type\":\"NET\",\"prod_desction\":\"詳細內容\",\"prod_tips\":\"\",\"prod_map_note\":\"備註( to traveler)\",\"is_search\":\"Y\",\"apply_status\":\"4\",\"status\":\"00\",\"policy_no\":\"2\",\"policy_list\":[],\"is_tour\":\"N\",\"tour_list\":[],\"meal_list\":[],\"guide_lang_list\":[{\"lang_code\":\"en\",\"lang_name\":\"English\"},{\"lang_code\":\"ja\",\"lang_name\":\"日本語\"},{\"lang_code\":\"zh-tw\",\"lang_name\":\"中文\"}],\"arr_map_info_list\":[{\"photo_url\":\"//s1.kkday.com/images/product/20159/location/20181004014711_vKhFt.png\",\"photo_desc\":\"\",\"zoom\":14,\"latitude\":\"25.109187\",\"longitude\":\"121.8462979\",\"latlong_type\":\"ARR\",\"latlong_desc\":\"九份\"}],\"confirm_order_time\":24,\"online_s_date\":\"20181004000000\",\"online_e_date\":\"20190630000000\",\"before_order_day\":\"2\",\"img_list\":[{\"auth_name\":\"                                                                                                                                                                                                        \",\"is_main_img\":\"Y\",\"img_desc\":\"\",\"img_url\":\"//s1.kkday.com/images/product/20159/20181004014454_pvYSh.jpg\",\"img_kkday_url\":\"/image/get/s1.kkday.com/product_20159/20181004014454_pvYSh/jpg\",\"is_auth_cc\":\"N\",\"is_commerce\":\"Y\",\"share_type\":\"A\"}],\"finishStep\":\"PO,PT,PD,PS,PP,PC,SC,PG,CD,77568\",\"prod_comment_info\":{\"avg_scores\":\"0\",\"total_scores\":\"0\",\"click_count\":\"0\",\"comment_record\":\"0\",\"keyword\":\"水金九,金瓜石,九份\",\"sales_qty\":\"0\",\"prod_url_oid\":\"20159\"},\"order_email\":\"phil651105@gmail.com\",\"remind_list\":[{\"remind_desc\":\"當參加人數未達最少出團人數之2人時，將於使用日前1天發出取消旅遊的email通知。\"},{\"remind_desc\":\"因交通、天氣等不可抗力因素所引起的時間延誤，造成部份行程景點取消時，請您主動聯絡客服，我們將會為您辦理部份退款。\"},{\"remind_desc\":\"不建議患有下列疾病或其他不宜受到過度刺激的遊客參加此項目：高血壓,心臟病\"},{\"remind_desc\":\"落石\"},{\"remind_desc\":\"雷擊\"}],\"video_list\":[],\"cost_detail_list\":[{\"detail_desc\":\"費用不包含\",\"detail_type\":\"NO_INC\"},{\"detail_desc\":\"費用不包含2\",\"detail_type\":\"NO_INC\"},{\"detail_desc\":\"費用包含\",\"detail_type\":\"INC\"},{\"detail_desc\":\"費用包含2\",\"detail_type\":\"INC\"}],\"tkt_expire\":{\"exp_type\":\"\",\"exp_open_date\":\"\",\"exp_s_date\":\"\",\"exp_e_date\":\"\"},\"meeting_point_list\":[],\"voucher_locations\":[],\"voucher_desc\":\"\",\"prod_mkt\":{\"is_ec_show\":true,\"is_ec_sale\":true,\"purchase_type\":\"\",\"purchase_type_name\":\"\",\"is_search\":true,\"is_show\":true},\"prod_no\":20159,\"prod_name\":\"浪漫水金九 （正）\",\"b2c_price\":210.0,\"b2d_price\":204.0,\"prod_currency\":\"TWD\",\"prod_img_url\":\"浪漫水金九 （正）\",\"introduction\":\"商品概述\",\"prod_type\":\"M01\",\"tag\":[\"TAG_1_3\",\"TAG_3_1\",\"TAG_3_3\"],\"countries\":[{\"id\":\"A01-001\",\"name\":\"台灣\",\"cities\":[{\"id\":\"A01-001-00002\",\"name\":\"台中\"},{\"id\":\"A01-001-00001\",\"name\":\"台北\"}]}]}";
                ProductforEcModel obj = JsonConvert.DeserializeObject<ProductforEcModel>(result);

                return obj;

            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"apiHelpler_getProdDtl_err:{ JsonConvert.SerializeObject(ex.Message.ToString())}");
                throw new Exception(title.result_code_9990);
            }
        }

        public static PackageModel getProdPkg(long B2dXid, string state, string lang, string currency, string prodoid, ProdTitleModel title)
        {

            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                var pathUrl = Website.Instance.Configuration["B2DApiUrl:apiUri"];

                string result;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{pathUrl}product/Querypackage");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                ApiRequest setting = new ApiRequest();

                setting.company_xid = B2dXid.ToString();
                setting.current_currency = currency;
                setting.locale_lang = lang;
                setting.prod_no = prodoid;
                setting.state = state;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(setting);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //result = "{\"result\":\"0000\",\"result_msg\":\"正確\",\"cost_calc_type\":\"NET\",\"sale_dates\":{\"result\":\"0000\",\"result_msg\":\"正確\",\"saleDt\":[{\"pkg_no\":\"77568\",\"sale_day\":\"20181023\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181024\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181025\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181030\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181031\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181101\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181106\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181107\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181108\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181113\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181114\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181115\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181120\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181121\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181122\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181127\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181128\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181129\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181204\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181205\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181206\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181211\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181212\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181213\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181218\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181219\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181220\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181225\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181226\"},{\"pkg_no\":\"77568\",\"sale_day\":\"20181227\"}]},\"pkgs\":[{\"pkg_no\":\"77568\",\"pkg_name\":\"浪漫水金九\",\"desc_items\":[{\"desc\":\"浪漫水金九-銷售組合概述\",\"id\":\"20181004dwwub\"}],\"online_s_date\":\"20181004080000\",\"online_e_date\":\"20190201075959\",\"weekDays\":\"2,3,4\",\"is_unit_pirce\":\"浪漫水金九\",\"price1\":204.0,\"price1_org\":210.52632,\"prcie1_org_net\":200.0,\"prcie1_profit_rate\":5.0,\"prcie1_comm_rate\":0.0,\"prcie1_age_range\":\"12~99\",\"price2\":102.0,\"price2_org\":105.26316,\"prcie2_org_net\":100.0,\"prcie2_profit_rate\":5.0,\"prcie2_comm_rate\":0.0,\"prcie2_age_range\":\"2~11\",\"price3_org\":0.0,\"prcie3_org_net\":0.0,\"prcie3_profit_rate\":0.0,\"prcie3_comm_rate\":0.0,\"price3_age_range\":\"~\",\"price4_org\":0.0,\"prcie4_org_net\":0.0,\"prcie4_profit_rate\":0.0,\"prcie4_comm_rate\":0.0,\"price4_age_range\":\"~\",\"status\":\"Y\",\"min_book_qty\":1,\"max_book_qty\":10,\"isMultiple\":\"N\",\"book_qty\":\"1,2,3,4,5,6,7,8,9,10\",\"unit\":\"01\",\"unit_txt\":\"人\",\"unit_qty\":1,\"pickupTp\":\"\",\"pickupTpTxt\":\"\",\"is_hl\":\"N\",\"is_event\":\"N\",\"module_setting\":{\"flight_info_type\":{\"value\":\"00\"},\"send_info_type\":{\"value\":\"00\",\"country_code\":\"\"},\"voucher_valid_info\":{\"valid_period_type\":\"\",\"before_specific_date\":\"\"}}}]}";
                PackageModel obj = JsonConvert.DeserializeObject<PackageModel>(result);

                return obj;
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"apiHelpler_getProdPkg_err:{ JsonConvert.SerializeObject(ex.Message.ToString())}");
                throw new Exception(title.result_code_9990);
            }
        }

        public static PkgEventsModel getPkgEvent(long B2dXid, string state, string lang, string currency, string prodoid, string pkgoid, ProdTitleModel title)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
           delegate (object s, X509Certificate certificate,
           X509Chain chain, SslPolicyErrors sslPolicyErrors)
           { return true; };

                var pathUrl = Website.Instance.Configuration["B2DApiUrl:apiUri"];

                string result;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{pathUrl}product/QueryEvent");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                ApiRequest setting = new ApiRequest();

                setting.company_xid = B2dXid.ToString();
                setting.current_currency = currency;
                setting.locale_lang = lang;
                setting.prod_no = prodoid;
                setting.pkg_no = pkgoid;
                //setting.state = state;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(setting);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                PkgEventsModel obj = JsonConvert.DeserializeObject<PkgEventsModel>(result);

                return obj;
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"apiHelpler_getPkgEvent_err:{ JsonConvert.SerializeObject(ex.Message.ToString())}");
                throw new Exception(title.result_code_9990);
            }
        }

        //寫入b2b 訂單
        public static insB2dOrderResult insB2dOrder(B2dOrderModel orders, ProdTitleModel title)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
           delegate (object s, X509Certificate certificate,
           X509Chain chain, SslPolicyErrors sslPolicyErrors)
           { return true; };

                var pathUrl = Website.Instance.Configuration["B2DApiUrl:apiUri"];

                string result;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{pathUrl}Booking/InsertOrder");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(orders);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

                insB2dOrderResult obj = JsonConvert.DeserializeObject<insB2dOrderResult>(result);

                return obj;
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"apiHelpler_insB2dOrder_err:{ JsonConvert.SerializeObject(ex.Message.ToString())}");
                throw new Exception(title.result_code_9990);
            }
        }

        //寫入kkday 訂單
        public static JObject orderNew(DataModel data, ProdTitleModel title)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
           delegate (object s, X509Certificate certificate,
           X509Chain chain, SslPolicyErrors sslPolicyErrors)
           { return true; };

                var pathUrl = Website.Instance.Configuration["B2DApiUrl:apiUri"];

                string result;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{pathUrl}booking/bookingStep1");

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(data);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                var obj = JObject.Parse(result);

                return obj;
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"apiHelpler_orderNew_err:{ JsonConvert.SerializeObject(ex.Message.ToString())}");
                throw new Exception(title.result_code_9990);
            }
        }

        //PMCH List 要呈現在頁面上可以選擇的付款方式～
        public static PmchLstResponse getPaymentListRes(List<Country> countries, string prodOid,
                                   string bookinSdate, string bookingEdate, string goSdate, string goEdate,
                                    string memCountryCd, string lang, string mainCat, string ip, string prodHander, string currency, ProdTitleModel title)
        {

            ServicePointManager.ServerCertificateValidationCallback =
           delegate (object s, X509Certificate certificate,
           X509Chain chain, SslPolicyErrors sslPolicyErrors)
           {
               return true;
           };

            try
            {
                //PmchSslRequest call = new PmchSslRequest();
                //call.ipaddress = "192.168.1.1";
                //call.apiKey = "kkdayapi";
                //call.userOid = "1";
                //call.ver = "1.0.1";

                //CallJsonGetPayList j = new CallJsonGetPayList();

                List<payTypeValue> conditionList = new List<payTypeValue>();

                conditionList.Add(new payTypeValue() { type = "01", value = countries[0].id.Split('-')[0].ToString() }); //continent
                conditionList.Add(new payTypeValue() { type = "02", value = countries[0].id }); //country

                foreach (City c in countries[0].cities)
                {

                    conditionList.Add(new payTypeValue() { type = "03", value = c.id }); //city
                }

                conditionList.Add(new payTypeValue() { type = "04", value = prodOid }); //product_oid
                conditionList.Add(new payTypeValue() { type = "05", value = bookinSdate }); //book_s_date 2018-10-23
                conditionList.Add(new payTypeValue() { type = "06", value = bookingEdate }); //book_e_date 2018-10-23
                conditionList.Add(new payTypeValue() { type = "07", value = goSdate }); //go_s_date 2018-10-23
                conditionList.Add(new payTypeValue() { type = "08", value = goEdate }); //go_e_date  2018-10-23  
                conditionList.Add(new payTypeValue() { type = "09", value = "member" }); //operator
                conditionList.Add(new payTypeValue() { type = "10", value = memCountryCd }); //member_country
                conditionList.Add(new payTypeValue() { type = "11", value = lang }); //web_lang
                conditionList.Add(new payTypeValue() { type = "12", value = mainCat }); //main_cat
                conditionList.Add(new payTypeValue() { type = "13", value = ip }); //ip_address
                conditionList.Add(new payTypeValue() { type = "14", value = prodHander }); //order_handler
                conditionList.Add(new payTypeValue() { type = "15", value = currency }); //分銷商幣別
                conditionList.Add(new payTypeValue() { type = "16", value = "SAFARI" }); //browser
                conditionList.Add(new payTypeValue() { type = "17", value = "Macintosh" }); //device
                                                                                            //conditionList.Add(new payTypeValue() { type = "18", value = prodContinent }); //system 不知給什麼
                conditionList.Add(new payTypeValue() { type = "19", value = "B2D_WEB" }); //source_code

                //j.conditionList = conditionList;
                //call.json = j;

                string result;
                var pathUrl = Website.Instance.Configuration["B2DApiUrl:apiUri"];
                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{pathUrl}Booking/paymentList"); ;

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(conditionList);

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();

                }

                PmchLstResponse pmchList = JsonConvert.DeserializeObject<PmchLstResponse>(result);

                if (pmchList.isSuccess == false)
                {
                    throw new Exception("no pmch List");
                }

                return pmchList;
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"apiHelpler_getPaymentListRes_err:{ JsonConvert.SerializeObject(ex.Message.ToString())}");
                if (ex.Message.ToString().Equals("no pmch List"))
                {
                    throw new Exception("no pmch List");
                }
                else
                {
                    throw new Exception(title.result_code_9990);
                }
            }
        }


        //PaymentValid
        public static Boolean PaymentValid(string id, string jsondata)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
           delegate (object s, X509Certificate certificate,
           X509Chain chain, SslPolicyErrors sslPolicyErrors)
           { return true; };

                var pathUrl = Website.Instance.Configuration["B2DApiUrl:apiUri"];

                string result;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{pathUrl}Final/Step3?mid=" + id + "&jsondata=" + jsondata);

                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Method = "GET";

                //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                //{
                //    string json = JsonConvert.SerializeObject(data);

                //    streamWriter.Write(json);
                //    streamWriter.Flush();
                //    streamWriter.Close();
                //}

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //var obj = JObject.Parse(result);

                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"apiHelpler_PaymentValid_err:{ JsonConvert.SerializeObject(ex.Message.ToString())}");
                return false;
            }

        }


        public class ApiRequest
        {
            public string company_xid { get; set; }
            public string locale_lang { get; set; }
            public string current_currency { get; set; }
            public string prod_no { get; set; }
            public string pkg_no { get; set; }
            public string state { get; set; }
        }
    }
}
