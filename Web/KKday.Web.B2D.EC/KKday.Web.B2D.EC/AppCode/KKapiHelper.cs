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
using KKday.Web.B2D.EC.Models.Model.Booking;
using KKday.Web.B2D.EC.Models.Model.Product;
using KKday.Web.B2D.EC.Models.Model.Pmch;
using KKday.Web.B2D.EC.Models.Model.Account;

namespace KKday.Web.B2D.EC.AppCode
{
    public class KKapiHelper
    {
    //    public JObject callKKapiProdInfo(string lang, string currency, string oid)
    //    {
    //        ServicePointManager.ServerCertificateValidationCallback =
    //        delegate (object s, X509Certificate certificate,
    //        X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //        { return true; };

    //        string result;

    //        if (RedisHelper.getProdInfotoRedis("bid:test:prodinfo" + oid) != null)
    //        {
    //            result = RedisHelper.getProdInfotoRedis("bid:test:prodinfo" + oid);
    //        }
    //        else
    //        {
    //            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://api.sit.kkday.com/api/product/info/fe/v1/" + oid);

    //            httpWebRequest.ContentType = "application/json";
    //            httpWebRequest.Method = "POST";

    //            CallModules call = new CallModules();
    //            call.ipaddress = "192.168.1.1";
    //            call.apiKey = "kkdayapi";
    //            call.userOid = "1";
    //            call.ver = "1.0.1";
    //            call.locale = lang;
    //            call.currency = currency;

    //            CallJson j = new CallJson();

    //            j.infoType = "ALL";
    //            j.cleanCache = "N";
    //            j.multipricePlatform = "01";

    //            call.json = j;

    //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
    //            {
    //                string json = JsonConvert.SerializeObject(call);

    //                streamWriter.Write(json);
    //                streamWriter.Flush();
    //                streamWriter.Close();
    //            }

    //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

    //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //            {
    //                result = streamReader.ReadToEnd();
    //            }

    //            RedisHelper.SetProdInfotoRedis(result, "bid:test:prodinfo" + oid, 30);
    //        }


    //        //result = "{\"content\":{\"msg\":\"正確\",\"product\":{\"applyStatus\":\"4\",\"arrImgUrl\":\"//s1.kkday.com/images/product/17379/location/20180723081816_Avr06.png\",\"arrLatitude\":\"25.1098695\",\"arrLatlongDesc\":\"台灣新北市瑞芳區基山街九份老街\",\"arrLongitude\":\"121.8451827\",\"arrMapType\":\"LATLONG\",\"arrPhotoDesc\":\"\",\"arrZoomLv\":16,\"avgScores\":0,\"begSaleDt\":\"20180704000000\",\"begSaleDtGmt\":\"20180703160000\",\"clickCnt\":0,\"confirmHour\":\"24\",\"costCalcMethod\":\"NET\",\"countRec\":0,\"cutOfDay\":\"1\",\"diseaseRemind\":\"D001,D002,D003\",\"endSaleDt\":\"20181231235900\",\"finishStep\":\"PO,PT,PD,PS,PP,PC,SC,PG,CD,42206,42517\",\"gatherNote\":\"\",\"guideLang\":\"en,ja,zh-tw\",\"guideLangMap\":{\"zh-tw\":\"中文\",\"ja\":\"日本語\",\"en\":\"English\"},\"imgUrl\":\"//s1.kkday.com/images/product/17379/20180723081529_8876F.png\",\"introduction\":\"商品概述\",\"isOptimized\":\"\",\"isPackage\":\"Y\",\"isSche\":\"Y\",\"isSearch\":\"N\",\"keyWord\":\"金瓜石,九份,廢煙道\",\"mainCat\":\"M07\",\"mainCatStr\":\"私人導遊\",\"masterLang\":\"zh-tw\",\"needLogin\":true,\"orderEmail\":\"phil.chang1@kkday.com\",\"orderNum\":0,\"policyNo\":\"3\",\"prodCurrCd\":\"USD\",\"prodMarketingCountryCd\":[\"MY\",\"TW\",\"CN\",\"TH\",\"KR\",\"PH\",\"SG\",\"ID\",\"JP\",\"KK\",\"HK\"],\"prodOid\":\"17379\",\"productDesc\":\"詳細內容\",\"productName\":\"浪漫水金九\",\"productNameMaster\":\"浪漫水金九\",\"productTips\":\"\",\"promoTagArray\":[\"my||||||\",\"tw||||||\",\"cn||||||\",\"th||||||\",\"kr||||||\",\"ph||||||\",\"sg|||暢銷|||HOT\",\"id||||||\",\"jp||||||\",\"kk||||||\",\"hk||||||\"],\"recScoresMap\":{\"1\":0,\"2\":0,\"3\":0,\"4\":0,\"5\":0},\"saleStatus\":\"00\",\"stillStepToGo\":0,\"supplierName\":\"台北帝大\",\"supplierNote\":\"廢煙道，茶壺山，無極索道，本山六坑，無言山丘\",\"supplierOid\":\"1942\",\"tagCd\":\"TAG_1_1,TAG_1_3,TAG_3_1\",\"timezone\":\"d67d1493ba7c7efb78a30ebd07a6240f\",\"totalScores\":0,\"tourDays\":2,\"tourHours\":0,\"wishlistCnt\":0,\"minPrice\":329.0,\"minSalePrice\":329.0,\"isDisplayPrice\":\"N\",\"marketingIsShow\":true,\"kkdayImgUrl\":\"/image/get/s1.kkday.com/product_17379/20180723081529_8876F/png\"},\"policyList\":[{\"policy\":{\"days\":1,\"isOver\":false,\"percent\":100}},{\"policy\":{\"days\":4,\"isOver\":false,\"percent\":80}},{\"policy\":{\"days\":10,\"isOver\":false,\"percent\":60}},{\"policy\":{\"days\":20,\"isOver\":false,\"percent\":50}},{\"policy\":{\"days\":30,\"isOver\":false,\"percent\":40}},{\"policy\":{\"days\":31,\"isOver\":true,\"percent\":30}}],\"incDataList\":[],\"cityList\":[{\"city\":{\"cityCd\":\"A01-001-00003\",\"cityName\":\"台南\",\"countryCd\":\"A01-001\",\"countryName\":\"台灣\",\"prodCityOid\":19280}},{\"city\":{\"cityCd\":\"A01-001-00002\",\"cityName\":\"台中\",\"countryCd\":\"A01-001\",\"countryName\":\"台灣\",\"prodCityOid\":19279}},{\"city\":{\"cityCd\":\"A01-001-00001\",\"cityName\":\"台北\",\"countryCd\":\"A01-001\",\"countryName\":\"台灣\",\"prodCityOid\":19224}}],\"scheMealList\":[],\"pubRemind\":{\"A\":{\"checked\":\"Y\",\"value\":\"1\"},\"B\":{\"checked\":\"Y\",\"value\":\"1\"},\"C\":\"N\",\"D\":{\"checked\":\"N\",\"value\":\"\",\"value2\":\"\"}},\"prodMan\":[{\"user_email\":\"phil.chang@kkday.com\"}],\"prodGroupLst\":[],\"result\":\"0000\",\"arrList\":[{\"latlong\":{\"imgUrl\":\"//s1.kkday.com/images/product/17379/location/20180723081816_Avr06.png\",\"zoomLv\":16,\"latlongOid\":27223,\"latitude\":\"25.1098695\",\"latlongType\":\"ARR\",\"latlongDesc\":\"台灣新北市瑞芳區基山街九份老街\",\"mapType\":\"LATLONG\",\"photoDesc\":\"\",\"longitude\":\"121.8451827\"}}],\"scheList\":[{\"sche\":{\"daySeq\":1,\"photoOid\":0,\"photoUrl\":\"\",\"scheDesc\":\"test\",\"scheOid\":70090,\"sortSeq\":1,\"timeDesc\":\"0105\"}}],\"pkgStatusList\":[{\"pkg\":{\"pkgDesc\":\"頂級水金九\",\"sortSeq\":5,\"isFinish\":\"Y\",\"pkgOid\":42206,\"status\":\"Y\",\"unit\":\"人\"}},{\"pkg\":{\"pkgDesc\":\"標配水金九\",\"sortSeq\":6,\"isFinish\":\"Y\",\"pkgOid\":42517,\"status\":\"Y\",\"unit\":\"人\"}}],\"prodUrlInfo\":{\"avgScores\":0,\"clickCnt\":0,\"countRec\":0,\"keyword\":\"金瓜石,九份,廢煙道\",\"orderNum\":0,\"prodUrlOid\":17379,\"recScoresMap\":{\"1\":0,\"2\":0,\"3\":0,\"4\":0,\"5\":0},\"totalScores\":0},\"videoList\":[],\"supplier\":{\"avgScores\":0,\"countRec\":0,\"descript\":\"測試\",\"logoUrl\":\"/image/get/s1.kkday.com/supplier/default_image/png\",\"orderHandler\":\"KKDAY\",\"serviceEmail\":\"\",\"serviceTel\":\"0229227777\",\"serviceTelArea\":\"886\",\"supplierName\":\"台北帝大\",\"supplierNameEng\":\"\",\"webside\":\"http://wwww.hinnn.com\"},\"isAsiaMile\":true,\"detailList\":[{\"detail\":{\"desc\":\"費用不包含\",\"detailOid\":209269,\"detailType\":\"NO_INC\"}},{\"detail\":{\"desc\":\"費用包含\",\"detailOid\":209268,\"detailType\":\"INC\"}}],\"tkExpSetting\":{\"expTp\":\"\",\"expNum\":\"\",\"expSt\":\"\",\"expEd\":\"\"},\"meetingPointList\":[],\"prodMarketing\":{\"is_ec_show\":true,\"is_ec_sale\":true,\"purchase_type\":\"\",\"purchase_type_name\":\"\",\"is_search\":true,\"is_show\":true},\"imgList\":[{\"img\":{\"authName\":\"\",\"defaultImg\":\"Y\",\"imgDesc\":\"\",\"imgOid\":116670,\"imgSeq\":0,\"imgUrl\":\"//s1.kkday.com/images/product/17379/20180723081529_8876F.png\",\"isCcAuth\":\"N\",\"isCommerce\":\"Y\",\"shareType\":\"A\",\"usageTag\":\"U01\",\"kkdayImgUrl\":\"/image/get/s1.kkday.com/product_17379/20180723081529_8876F/png\"}}],\"remindList\":[{\"remind\":{\"desc\":\"其它提醒\",\"detailOid\":209529}},{\"remind\":{\"desc\":\"其他提醒2\",\"detailOid\":209567}},{\"remind\":{\"desc\":\"其他提醒3\",\"detailOid\":209568}}]}}";

    //        var obj = JObject.Parse(result);

    //        return obj;
    //        //string moduleType = obj["content"]["product"]["modules"][0]["moduleType"].ToString();

    //        //            {
    //        //                "apiKey":"kkdayapi",
    //        //"userOid":"1",
    //        //"ver":"1.0.1",
    //        //"locale":"zh-tw",
    //        //"currency":"USD",
    //        //"ipaddress":"192.168.2.26",
    //        //"json":{
    //        //                    "infoType":"ALL",
    //        //"cleanCache":"N",
    //        //"multipricePlatform":"01"
    //        //}
    //        //}
    //    }

    //    public JObject callKKapiProdPkg(string lang, string currency, string oid)
    //    {
    //        ServicePointManager.ServerCertificateValidationCallback =
    //       delegate (object s, X509Certificate certificate,
    //       X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //       { return true; };



    //        string result;


    //        if (RedisHelper.getProdInfotoRedis("bid:test:prodPkg" + oid) != null)
    //        {
    //            result = RedisHelper.getProdInfotoRedis("bid:test:prodPkg" + oid);

    //        }
    //        else
    //        {
    //            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://api.sit.kkday.com/api/product/pkg/" + oid);

    //            httpWebRequest.ContentType = "application/json";
    //            httpWebRequest.Method = "POST";

    //            CallModules call = new CallModules();
    //            call.ipaddress = "192.168.1.1";
    //            call.apiKey = "kkdayapi";
    //            call.userOid = "1";
    //            call.ver = "1.0.1";
    //            call.locale = lang;
    //            call.currency = currency;

    //            CallJson2 j = new CallJson2();

    //            j.packageStatus = "ALL";
    //            j.packageOid = "";
    //            j.multipricePlatform = "01";

    //            call.json = j;

    //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
    //            {
    //                string json = JsonConvert.SerializeObject(call);

    //                streamWriter.Write(json);
    //                streamWriter.Flush();
    //                streamWriter.Close();
    //            }

    //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

    //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //            {
    //                result = streamReader.ReadToEnd();
    //            }

    //            RedisHelper.SetProdInfotoRedis(result, "bid:test:prodPkg" + oid, 30);

    //        }


    //        //result = "{\"content\":{\"result\":\"0000\",\"msg\":\"正確\",\"prodCurrCd\":\"USD\",\"packageList\":[{\"productPkg\":{\"pkgOid\":\"42206\",\"pkgName\":\"頂級水金九\",\"pkgDesc\":{\"descItems\":[{\"content\":[{\"desc\":\"銷售組合概述1test\",\"id\":\"20180723x16e1\"},{\"desc\":\"銷售組合概述2test\",\"id\":\"20180917eygsp\"}]}]},\"begValidDt\":\"20180801080000\",\"endValidDt\":\"20181229075959\",\"weekDays\":\"3,4,5\",\"priceType\":\"RANK\",\"price1\":500.0,\"price1USD\":16.01,\"price1Sale\":329.0,\"price1Org\":10.52632,\"price1NetOrg\":10.00000,\"price1GrossRate\":5.00,\"price1CommRate\":0.00,\"isDisplayPrice1\":\"N\",\"price1BegOld\":\"12\",\"price1EndOld\":\"99\",\"price2\":null,\"price2USD\":null,\"price2Sale\":null,\"price2Org\":0.00000,\"price2NetOrg\":0.00000,\"price2GrossRate\":0.00,\"price2CommRate\":0.00,\"isDisplayPrice2\":null,\"price2BegOld\":\"\",\"price2EndOld\":\"\",\"price3\":null,\"price3USD\":null,\"price3Sale\":null,\"price3Org\":0.00000,\"price3NetOrg\":0.00000,\"price3GrossRate\":0.00,\"price3CommRate\":0.00,\"isDisplayPrice3\":null,\"price3BegOld\":\"\",\"price3EndOld\":\"\",\"price4\":null,\"price4USD\":null,\"price4Sale\":null,\"price4Org\":0.00000,\"price4NetOrg\":0.00000,\"price4GrossRate\":0.00,\"price4CommRate\":0.00,\"isDisplayPrice4\":null,\"price4BegOld\":\"\",\"price4EndOld\":\"\",\"status\":\"Y\",\"isFinish\":\"Y\",\"minOrderNum\":1,\"maxOrderNum\":10,\"minOrderQty\":1,\"minOrderAdultQty\":0,\"isMultiple\":\"N\",\"orderQty\":\"1,2,3,4,5,6,7,8,9,10\",\"unit\":\"01\",\"unitTxt\":\"人\",\"unitQty\":1,\"pickupTp\":\"\",\"pickupTpTxt\":\"\",\"hasEvent\":\"N\",\"isBackUp\":\"N\",\"moduleSetting\":{\"flightInfoType\":{\"value\":\"00\"},\"sendInfoType\":{\"value\":\"00\",\"countryCode\":null},\"voucherValidInfo\":{\"validPeriodType\":null,\"beforeSpecificDate\":null,\"afterOrderDate\":null}}}},{\"productPkg\":{\"pkgOid\":\"42517\",\"pkgName\":\"標配水金九\",\"pkgDesc\":{\"descItems\":[{\"content\":[{\"desc\":\"芋圓＋茶壼山\",\"id\":\"20180920c8ccg\"}]}]},\"begValidDt\":\"20180925080000\",\"endValidDt\":\"20181107075959\",\"weekDays\":\"1,2\",\"priceType\":\"RANK\",\"price1\":329.0,\"price1USD\":10.54,\"price1Sale\":329.0,\"price1Org\":10.52632,\"price1NetOrg\":10.00000,\"price1GrossRate\":5.00,\"price1CommRate\":0.00,\"isDisplayPrice1\":\"N\",\"price1BegOld\":\"12\",\"price1EndOld\":\"64\",\"price2\":164.0,\"price2USD\":5.25,\"price2Sale\":164.0,\"price2Org\":5.26316,\"price2NetOrg\":5.00000,\"price2GrossRate\":5.00,\"price2CommRate\":0.00,\"isDisplayPrice2\":\"N\",\"price2BegOld\":\"2\",\"price2EndOld\":\"11\",\"price3\":null,\"price3USD\":null,\"price3Sale\":null,\"price3Org\":0.00000,\"price3NetOrg\":0.00000,\"price3GrossRate\":0.00,\"price3CommRate\":0.00,\"isDisplayPrice3\":null,\"price3BegOld\":\"\",\"price3EndOld\":\"\",\"price4\":null,\"price4USD\":null,\"price4Sale\":null,\"price4Org\":0.00000,\"price4NetOrg\":0.00000,\"price4GrossRate\":0.00,\"price4CommRate\":0.00,\"isDisplayPrice4\":null,\"price4BegOld\":\"\",\"price4EndOld\":\"\",\"status\":\"Y\",\"isFinish\":\"Y\",\"minOrderNum\":1,\"maxOrderNum\":10,\"minOrderQty\":1,\"minOrderAdultQty\":1,\"isMultiple\":\"N\",\"orderQty\":\"1,2,3,4,5,6,7,8,9,10\",\"unit\":\"01\",\"unitTxt\":\"人\",\"unitQty\":1,\"pickupTp\":\"\",\"pickupTpTxt\":\"\",\"hasEvent\":\"N\",\"isBackUp\":\"N\",\"moduleSetting\":{\"flightInfoType\":{\"value\":\"00\"},\"sendInfoType\":{\"value\":\"00\",\"countryCode\":null},\"voucherValidInfo\":{\"validPeriodType\":null,\"beforeSpecificDate\":null,\"afterOrderDate\":null}}}}],\"rateTWD\":31.22495,\"costCalcMethod\":\"NET\"}}";

    //        var obj = JObject.Parse(result);
    //        return obj;

    //    }

    //    public JObject callKKapiProdPkgDate(string lang, string currency, string oid)
    //    {
    //        ServicePointManager.ServerCertificateValidationCallback =
    //       delegate (object s, X509Certificate certificate,
    //       X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //       { return true; };



    //        string result;


    //        if (RedisHelper.getProdInfotoRedis("bid:test:prodPkgDate" + oid) != null)
    //        {
    //            result = RedisHelper.getProdInfotoRedis("bid:test:prodPkgDate" + oid);

    //        }
    //        else
    //        {
    //            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://api.sit.kkday.com/api/1.0/pkg/cal/order/saleDt");

    //            httpWebRequest.ContentType = "application/json";
    //            httpWebRequest.Method = "POST";

    //            CallModules call = new CallModules();
    //            call.ipaddress = "192.168.1.1";
    //            call.apiKey = "kkdayapi";
    //            call.userOid = "1";
    //            call.ver = "1.0.1";
    //            call.locale = lang;
    //            //call.currency = currency;

    //            CallJson3 j = new CallJson3();

    //            j.prodOid = oid;
    //            j.rtnMonth = "12";

    //            call.json = j;

    //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
    //            {
    //                string json = JsonConvert.SerializeObject(call);

    //                streamWriter.Write(json);
    //                streamWriter.Flush();
    //                streamWriter.Close();
    //            }

    //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

    //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //            {
    //                result = streamReader.ReadToEnd();
    //            }

    //            RedisHelper.SetProdInfotoRedis(result, "bid:test:prodPkgDate" + oid, 30);
    //        }

    //        //result = "{\"content\":{\"result\":\"0000\",\"msg\":\"正確\",\"saleDt\":[{\"pkgOidObj\":\"42517\",\"day\":\"20180925\"},{\"pkgOidObj\":\"42206\",\"day\":\"20180926\"},{\"pkgOidObj\":\"42206\",\"day\":\"20180927\"},{\"pkgOidObj\":\"42206\",\"day\":\"20180928\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181001\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181002\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181003\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181004\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181005\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181008\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181009\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181010\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181011\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181012\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181015\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181016\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181017\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181018\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181019\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181022\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181023\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181024\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181025\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181026\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181029\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181030\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181031\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181101\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181102\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181105\"},{\"pkgOidObj\":\"42517\",\"day\":\"20181106\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181107\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181108\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181109\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181114\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181115\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181116\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181121\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181122\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181123\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181128\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181129\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181130\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181205\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181206\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181207\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181212\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181213\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181214\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181219\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181220\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181221\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181226\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181227\"},{\"pkgOidObj\":\"42206\",\"day\":\"20181228\"}],\"pkgMiniPrice\":10.53000,\"pkgGoDt\":\"20180925\",\"pkgOid\":42517,\"pkgTodayDt\":\"20180921\"}}";


    //        var obj = JObject.Parse(result);
    //        return obj;
    //    }


    //    public JObject callKKapiProdModuleBooking(string lang, string currency, string oid)
    //    {
    //        ServicePointManager.ServerCertificateValidationCallback =
    //       delegate (object s, X509Certificate certificate,
    //       X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //       { return true; };



    //        string result;

    //        if (RedisHelper.getProdInfotoRedis("bid:test:prodModuleBooking" + oid) != null)
    //        {
    //            result = RedisHelper.getProdInfotoRedis("bid:test:prodModuleBooking" + oid);
    //        }
    //        else
    //        {
    //            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://api.sit.kkday.com/api/product/{oid}/module/get");

    //            httpWebRequest.ContentType = "application/json";
    //            httpWebRequest.Method = "POST";

    //            CallModules call = new CallModules();
    //            call.ipaddress = "192.168.1.1";
    //            call.apiKey = "kkdayapi";
    //            call.userOid = "1";
    //            call.ver = "1.0.1";
    //            call.locale = lang;
    //            //call.currency = currency;

    //            CallJson4 j = new CallJson4();

    //            j.deviceId = "cf40989cc2d187d1c79db8cf0918f1ba";
    //            j.tokenKey = "1b75390e6013433109d89a90a08c62ec";

    //            string[] s = new string[9];
    //            s[0] = "PMDL_CUST_DATA";
    //            s[1] = "PMDL_RENT_CAR";
    //            s[2] = "PMDL_CAR_PSGR";
    //            s[3] = "PMDL_SEND_DATA";
    //            s[4] = "PMDL_SIM_WIFI";
    //            s[5] = "PMDL_CONTACT_DATA";
    //            s[6] = "PMDL_FLIGHT_INFO";
    //            s[7] = "PMDL_VENUE";
    //            s[8] = "PMDL_EXCHANGE";

    //            j.moduleTypes = s;

    //            call.json = j;

    //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
    //            {
    //                string json = JsonConvert.SerializeObject(call);

    //                streamWriter.Write(json);
    //                streamWriter.Flush();
    //                streamWriter.Close();
    //            }

    //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

    //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //            {
    //                result = streamReader.ReadToEnd();
    //            }

    //            RedisHelper.SetProdInfotoRedis(result, "bid:test:prodModuleBooking" + oid, 30);
    //        }


    //        //result = "{\"content\":{\"result\":\"0000\",\"msg\":\"正確\",\"product\":{\"modules\":[{\"moduleType\":\"PMDL_CUST_DATA\",\"finishStatus\":\"9\",\"moduleSetting\":{\"isRequired\":true,\"setting\":{\"customerDataType\":\"01\",\"dataItems\":{\"englishName\":{\"isRequired\":true},\"gender\":{\"isRequired\":true},\"nationality\":{\"isRequired\":false,\"options\":{\"TWIdentityNumber\":{\"isRequired\":false},\"HKMOIdentityNumber\":{\"isRequired\":false},\"MTPNumber\":{\"isRequired\":false}}},\"birthday\":{\"isRequired\":false},\"passportNo\":{\"isRequired\":false,\"options\":{\"passportExpDate\":{\"isRequired\":false}}},\"localName\":{\"isRequired\":false},\"height\":{\"isRequired\":false,\"unit\":\"01\",\"unitName\":\"公分\"},\"weight\":{\"isRequired\":false,\"unit\":\"01\",\"unitName\":\"公斤\"},\"shoeSize\":{\"isRequired\":false,\"options\":{\"man\":{\"isProvided\":false,\"unit\":\"01\",\"sizeRangeStart\":null,\"sizeRangeEnd\":null,\"allowSetting\":[{\"unit\":\"03\",\"min\":\"24\",\"max\":\"33\",\"size\":[24,24.5,25,25.5,26,26.5,27,27.5,28,28.5,29,29.5,30,30.5,31,31.5,32,32.5,33],\"name\":\"JP\"},{\"unit\":\"02\",\"min\":\"39\",\"max\":\"48\",\"size\":[39,39.5,40,40.5,41,41.5,42,42.5,43,43.5,44,44.5,45,45.5,46,46.5,47,47.5,48],\"name\":\"EU\"},{\"unit\":\"01\",\"min\":\"5.5\",\"max\":\"14.5\",\"size\":[5.5,6,6.5,7,7.5,8,8.5,9,9.5,10,10.5,11,11.5,12,12.5,13,13.5,14,14.5],\"name\":\"US\"}]},\"woman\":{\"isProvided\":false,\"unit\":\"01\",\"sizeRangeStart\":null,\"sizeRangeEnd\":null,\"allowSetting\":[{\"unit\":\"03\",\"min\":\"21\",\"max\":\"31\",\"size\":[21,21.5,22,22.5,23,23.5,24,24.5,25,25.5,26,27,28,29,30,31],\"name\":\"JP\"},{\"unit\":\"02\",\"min\":\"34\",\"max\":\"41.5\",\"size\":[34,34.5,35,35.5,36,36.5,37,37.5,38,38.5,39,39.5,40,40.5,41,41.5],\"name\":\"EU\"},{\"unit\":\"01\",\"min\":\"4\",\"max\":\"11.5\",\"size\":[4,4.5,5,5.5,6,6.5,7,7.5,8,8.5,9,9.5,10,10.5,11,11.5],\"name\":\"US\"}]},\"child\":{\"isProvided\":false,\"unit\":\"01\",\"sizeRangeStart\":null,\"sizeRangeEnd\":null,\"allowSetting\":[{\"unit\":\"03\",\"min\":\"11\",\"max\":\"19\",\"size\":[11,12,12.5,13,14,14.5,15,16,16.5,17,18,18.5,19],\"name\":\"JP\"},{\"unit\":\"02\",\"min\":\"19\",\"max\":\"31\",\"size\":[19,20,21,22,23,24,25,26,27,28,29,30,31],\"name\":\"EU\"},{\"unit\":\"01\",\"min\":\"2.5\",\"max\":\"8.5\",\"size\":[2.5,3,3.5,4,4.5,5,5.5,6,6.5,7,7.5,8,8.5],\"name\":\"US\"}]}}},\"meal\":{\"isRequired\":false,\"options\":{\"meals\":[{\"mealType\":\"0004\",\"isProvided\":false,\"mealTypeName\":\"猶太教餐\"},{\"mealType\":\"0002\",\"isProvided\":false,\"mealTypeName\":\"素\"},{\"mealType\":\"0003\",\"isProvided\":false,\"mealTypeName\":\"穆斯林餐\"},{\"mealType\":\"0001\",\"isProvided\":false,\"mealTypeName\":\"葷\"}],\"excludeFood\":{\"isExcluded\":false,\"foods\":[{\"foodType\":\"0006\",\"canExclude\":false,\"foodTypeName\":\"魚\"},{\"foodType\":\"0007\",\"canExclude\":false,\"foodTypeName\":\"蛋\"},{\"foodType\":\"0004\",\"canExclude\":false,\"foodTypeName\":\"羊\"},{\"foodType\":\"0005\",\"canExclude\":false,\"foodTypeName\":\"甲殼類\"},{\"foodType\":\"0002\",\"canExclude\":false,\"foodTypeName\":\"豬\"},{\"foodType\":\"0003\",\"canExclude\":false,\"foodTypeName\":\"雞\"},{\"foodType\":\"0001\",\"canExclude\":false,\"foodTypeName\":\"牛\"},{\"foodType\":\"0008\",\"canExclude\":false,\"foodTypeName\":\"奶\"}],\"foodAllergy\":{\"canExclude\":false}}}},\"glassDiopter\":{\"isRequired\":false,\"diopterRangeStart\":null,\"diopterRangeEnd\":null}}}}},{\"moduleType\":\"PMDL_RENT_CAR\",\"finishStatus\":\"9\",\"moduleSetting\":{\"isRequired\":false,\"setting\":{\"rentCarType\":null,\"dataItems\":{\"rentCar\":{\"offices\":[],\"isProvidedFreeWiFi\":false,\"isProvidedFreeGPS\":false},\"driverShuttle\":{\"charterRoute\":{\"isRequired\":false,\"routes\":[],\"customRoute\":{\"isAllowCustom\":false,\"routeLimit\":null}}}}}}},{\"moduleType\":\"PMDL_CAR_PSGR\",\"finishStatus\":\"9\",\"moduleSetting\":{\"isRequired\":false,\"setting\":{\"dataItems\":{\"qtyAdult\":{\"isRequired\":false,\"ageLimitStart\":null,\"ageLimitEnd\":null},\"qtyChild\":{\"isRequired\":false,\"ageLimitStart\":null,\"ageLimitEnd\":null},\"qtyInfant\":{\"isRequired\":false,\"ageLimitStart\":null,\"ageLimitEnd\":null},\"qtyChildSeat\":{\"isRequired\":false,\"ageLimitStart\":null,\"ageLimitEnd\":null},\"qtyInfantSeat\":{\"isRequired\":false,\"ageLimitStart\":null,\"ageLimitEnd\":null},\"qtyCarryLuggage\":{\"isRequired\":false},\"qtyCheckedLuggage\":{\"isRequired\":false}}}}},{\"moduleType\":\"PMDL_SEND_DATA\",\"finishStatus\":\"9\",\"moduleSetting\":{\"isRequired\":false,\"setting\":{\"dataItems\":{\"receiverName\":{\"isRequired\":false},\"receiverTel\":{\"isRequired\":false},\"sendToCountry\":{\"isRequired\":false,\"receiveAddress\":{\"isRequired\":false},\"countries\":[]},\"sendToHotel\":{\"isProvided\":false,\"options\":{\"hotelName\":{\"isRequired\":false},\"hotelAddress\":{\"isRequired\":false},\"hotelTel\":{\"isRequired\":false},\"buyerPassportEnglishName\":{\"isRequired\":false},\"buyerLocalName\":{\"isRequired\":false},\"bookingOrderNo\":{\"isRequired\":false},\"bookingWebsite\":{\"isRequired\":false},\"checkOutDate\":{\"isRequired\":false},\"checkInDate\":{\"isRequired\":false}}}}}}},{\"moduleType\":\"PMDL_SIM_WIFI\",\"finishStatus\":\"9\",\"moduleSetting\":{\"isRequired\":false,\"setting\":{\"dataItems\":{\"mobileModelNumber\":{\"isRequired\":false},\"mobileIMEI\":{\"isRequired\":false},\"activationDate\":{\"isRequired\":false}}}}},{\"moduleType\":\"PMDL_CONTACT_DATA\",\"finishStatus\":\"9\",\"moduleSetting\":{\"isRequired\":false,\"setting\":{\"dataItems\":{\"contactName\":{\"isRequired\":false},\"contactTel\":{\"isRequired\":false},\"contactApp\":{\"isRequired\":false,\"apps\":[{\"appType\":\"0006\",\"isSupported\":false,\"appTypeName\":\"SnapChat\"},{\"appType\":\"0007\",\"isSupported\":false,\"appTypeName\":\"Facebook Messenger\"},{\"appType\":\"0004\",\"isSupported\":false,\"appTypeName\":\"Kakao\"},{\"appType\":\"0005\",\"isSupported\":false,\"appTypeName\":\"Viber\"},{\"appType\":\"0002\",\"isSupported\":false,\"appTypeName\":\"WhatsApp\"},{\"appType\":\"0003\",\"isSupported\":false,\"appTypeName\":\"WeChat/QQ\"},{\"appType\":\"0001\",\"isSupported\":false,\"appTypeName\":\"Line\"},{\"appType\":\"0008\",\"isSupported\":false,\"appTypeName\":\"Twitter\"},{\"appType\":\"0009\",\"isSupported\":false,\"appTypeName\":\"QQ\"}]}}}}},{\"moduleType\":\"PMDL_FLIGHT_INFO\",\"finishStatus\":\"9\",\"moduleSetting\":{\"isRequired\":true,\"setting\":{\"dataItems\":{\"arrival\":{\"flightType\":{\"isRequired\":false},\"arrivalDatetime\":{\"isRequired\":true},\"airport\":{\"isRequired\":false},\"airline\":{\"isRequired\":false},\"flightNo\":{\"isRequired\":false},\"terminalNo\":{\"isRequired\":false},\"isNeedToApplyVisa\":{\"isRequired\":false}},\"departure\":{\"flightType\":{\"isRequired\":false},\"departureDatetime\":{\"isRequired\":true},\"airport\":{\"isRequired\":false},\"airline\":{\"isRequired\":false},\"flightNo\":{\"isRequired\":false},\"terminalNo\":{\"isRequired\":false},\"haveBeenInCountry\":{\"isRequired\":false}}}}}},{\"moduleType\":\"PMDL_VENUE\",\"finishStatus\":\"9\",\"moduleSetting\":{\"isRequired\":false,\"setting\":{\"venueType\":null,\"dataItems\":{\"meetingPointMap\":{\"mapAddress\":null,\"latitude\":null,\"longitude\":null,\"zoomLevel\":null,\"imageUrl\":null},\"meetingPointImage\":{\"imageUrl\":null,\"imageDesc\":null},\"designatedLocation\":{\"locations\":[]},\"designatedByCustomer\":{\"pickUpLocation\":{\"isRequired\":false,\"options\":{\"pickUpTime\":{\"isRequired\":false,\"times\":[],\"customTime\":{\"isAllowCustom\":false,\"timeRange\":{\"from\":{\"hour\":null,\"minute\":null},\"to\":{\"hour\":null,\"minute\":null}}}}}},\"dropOffLocation\":{\"isRequired\":false}}}}}},{\"moduleType\":\"PMDL_EXCHANGE\",\"finishStatus\":\"9\",\"moduleSetting\":{\"isRequired\":true,\"setting\":{\"exchangeType\":\"01\",\"dataItems\":{\"locations\":[]}}}}]}}}";
    //        var obj = JObject.Parse(result);



    //        var jarray = (JArray)obj["content"]["product"]["modules"];

    //        int jc = jarray.Count;



    //        return obj;
    //    }


    //    public JObject crtOrder(ApiSetting data)
    //    {
    //        ServicePointManager.ServerCertificateValidationCallback =
    //        delegate (object s, X509Certificate certificate,
    //        X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //        { return true; };

    //        string result;

    //        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://api.sit.kkday.com/api/order/new");

    //        httpWebRequest.ContentType = "application/json";
    //        httpWebRequest.Method = "POST";


    //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
    //        {
    //            string json = JsonConvert.SerializeObject(data);

    //            streamWriter.Write(json);
    //            streamWriter.Flush();
    //            streamWriter.Close();
    //        }

    //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

    //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //        {
    //            result = streamReader.ReadToEnd();
    //        }

    //        var obj = JObject.Parse(result);

    //        return obj;
    //    }



    //    public class CallModules
    //    {
    //        public string ipaddress { get; set; }
    //        public string apiKey { get; set; }
    //        public string userOid { get; set; }
    //        public string ver { get; set; }
    //        public string locale { get; set; }
    //        public string currency { get; set; }
    //        public CallJsonA json { get; set; }
    //    }

    //    public abstract class CallJsonA
    //    {

    //    }

    //    public class CallJson : CallJsonA
    //    {
    //        public string infoType { get; set; }
    //        public string cleanCache { get; set; }
    //        public string multipricePlatform { get; set; }
    //    }

    //    public class CallJson2 : CallJsonA
    //    {
    //        public string packageStatus { get; set; }
    //        public string packageOid { get; set; }
    //        public string multipricePlatform { get; set; }
    //    }

    //    public class CallJson3 : CallJsonA
    //    {
    //        public string prodOid { get; set; }
    //        public string rtnMonth { get; set; }
    //    }

    //    public class CallJson4 : CallJsonA
    //    {
    //        public string deviceId { get; set; }
    //        public string tokenKey { get; set; }
    //        public string[] moduleTypes { get; set; }
    //    }


    //    //PMCH List 要呈現在頁面上可以選擇的付款方式～
    //    public void PaymentListReq(List<Country> countries, string prodOid,
    //                               string bookinSdate,string bookingEdate,string goSdate,string goEdate,
    //                                string memCountryCd,string lang,string mainCat,string ip,string prodHander,string currency)
    //    {

    //        ServicePointManager.ServerCertificateValidationCallback =
    //       delegate (object s, X509Certificate certificate,
    //       X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //       { return true; };

    //        PmchSslRequest call = new PmchSslRequest();
    //        call.ipaddress = "192.168.1.1";
    //        call.apiKey = "kkdayapi";
    //        call.userOid = "1";
    //        call.ver = "1.0.1";

    //        CallJsonGetPayList j = new CallJsonGetPayList();

    //        List<payTypeValue> conditionList = new List<payTypeValue>();

    //        conditionList.Add(new payTypeValue() { type = "01", value = countries[0].id.Split('-')[0].ToString() }); //continent
    //        conditionList.Add(new payTypeValue() { type = "02", value = countries[0].id }); //country

    //        foreach (City c in countries[0].cities)
    //        {

    //            conditionList.Add(new payTypeValue() { type = "03", value = c.id }); //city
    //        }

    //        conditionList.Add(new payTypeValue() { type = "04", value = prodOid }); //product_oid
    //        conditionList.Add(new payTypeValue() { type = "05", value = bookinSdate }); //book_s_date 2018-10-23
    //        conditionList.Add(new payTypeValue() { type = "06", value = bookingEdate }); //book_e_date 2018-10-23
    //        conditionList.Add(new payTypeValue() { type = "07", value = goSdate }); //go_s_date 2018-10-23
    //        conditionList.Add(new payTypeValue() { type = "08", value = goEdate }); //go_e_date  2018-10-23  
    //        conditionList.Add(new payTypeValue() { type = "09", value = "member" }); //operator
    //        conditionList.Add(new payTypeValue() { type = "10", value = memCountryCd }); //member_country
    //        conditionList.Add(new payTypeValue() { type = "11", value = lang }); //web_lang
    //        conditionList.Add(new payTypeValue() { type = "12", value = mainCat }); //main_cat
    //        conditionList.Add(new payTypeValue() { type = "13", value = ip }); //ip_address
    //        conditionList.Add(new payTypeValue() { type = "14", value = prodHander }); //order_handler
    //        conditionList.Add(new payTypeValue() { type = "15", value = currency }); //分銷商幣別
    //        conditionList.Add(new payTypeValue() { type = "16", value = "SAFARI" }); //browser
    //        conditionList.Add(new payTypeValue() { type = "17", value = "Macintosh" }); //device
    //        //conditionList.Add(new payTypeValue() { type = "18", value = prodContinent }); //system 不知給什麼
    //        conditionList.Add(new payTypeValue() { type = "19", value = "WEB" }); //source_code

    //        j.conditionList = conditionList;
    //        call.json = j;

    //        string result;
    //        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://pmch.sit.kkday.com/common/channel/list_available");

    //        httpWebRequest.ContentType = "application/json";
    //        httpWebRequest.Method = "POST";

    //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
    //        {
    //            string json = JsonConvert.SerializeObject(call);

    //            streamWriter.Write(json);
    //            streamWriter.Flush();
    //            streamWriter.Close();
    //        }

    //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

    //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //        {
    //            result = streamReader.ReadToEnd();
    //        }

    //    }



    //    //PMCH 驗證
    //    public string PaymentValid(String pmgwTransNo, string pmgwValidToken)
    //    {

    //        ServicePointManager.ServerCertificateValidationCallback =
    //       delegate (object s, X509Certificate certificate,
    //       X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //       { return true; };

    //        PmchSslRequest2 call = new PmchSslRequest2();
    //        call.ipaddress = "192.168.1.1";
    //        call.apiKey = "kkdayapi";
    //        call.userOid = "1";
    //        call.ver = "1.0.1";

    //        CallPmchValidJson j = new CallPmchValidJson();
    //        j.pmgwTransNo = pmgwTransNo;
    //        j.pmgwValidToken = pmgwValidToken;
    //        call.json = j;

    //        string result;
    //        var httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://pmch.sit.kkday.com/common/gateway/token_validate");

    //        httpWebRequest.ContentType = "application/json";
    //        httpWebRequest.Method = "POST";

    //        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
    //        {
    //            string json = JsonConvert.SerializeObject(call);

    //            streamWriter.Write(json);
    //            streamWriter.Flush();
    //            streamWriter.Close();
    //        }

    //        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

    //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //        {
    //            result = streamReader.ReadToEnd();
    //        }


    //        //回覆要再確認是否是true !
    //        var obj = JObject.Parse(result);

    //        string isSuccess = obj["isSuccess"].ToString();

    //        return isSuccess;
    //    }


    //    //PMCH 驗證過了，付款成功，變更訂單狀態為已付款可處 舊版
    //    public void PayUpdSuccessUpdOrder(string orderMid,string pmgwTransNo, PaymentDtl payDtl, CallJsonPay req, PmchSslResponse res, B2dAccount UserData)
    //    {
    //        try
    //        {
    //            ServicePointManager.ServerCertificateValidationCallback =
    //            delegate (object s, X509Certificate certificate,
    //            X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //            { return true; };

    //            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.sit.kkday.com/api/order/payment/success/" + orderMid);

    //            httpWebRequest.ContentType = "application/json";
    //            httpWebRequest.Method = "POST";

    //            string result;

    //            PaySuccessUpdOrderMst mst = new PaySuccessUpdOrderMst();
    //            mst.ipaddress = "192.168.1.1";
    //            mst.apiKey = "kkdayapi";
    //            mst.userOid = "1";
    //            mst.ver = "1.0.1";
    //            mst.locale = "zh-tw";

    //            PaySuccessUpdOrder p = new PaySuccessUpdOrder();

    //            p.memberUuid = UserData.UUID; 
    //            //p.tokenKey = fakeContact.tokenKey;
    //            //p.deviceId = fakeContact.deviceId;
    //            p.currency = payDtl.currency;
    //            p.currTotalPrice = payDtl.currTotalPrice.ToString();
    //            p.is3D = (req.is3D == "0"? false : true);
    //            p.payMethod = payDtl.payMethod;
    //            p.pmgwMethod = res.pmgwMethod;
    //            p.pmgwTransNo = pmgwTransNo;
    //            p.isFraud = "0";

    //            mst.json = p;

    //            //$path = 'order/payment/success/'.$order_mid;
    //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
    //            {
    //                string json = JsonConvert.SerializeObject(mst);
    //                streamWriter.Write(json);
    //                streamWriter.Flush();
    //                streamWriter.Close();
    //            }

    //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

    //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //            {
    //                result = streamReader.ReadToEnd();
    //            }
    //        }
    //        catch(Exception ex)
    //        {
    //            string dd = ex.ToString();

    //        }
    //    }

    //    //PMCH 驗證過了，付款成功，變更訂單狀態為已付款可處 新版
    //    public void PayUpdSuccessUpdOrder2(string orderMid, string pmgwTransNo, PaymentDtl payDtl, CallJsonPay2 req, PmchSslResponse2 res, B2dAccount UserData)
    //    {
    //        try
    //        {
    //            ServicePointManager.ServerCertificateValidationCallback =
    //            delegate (object s, X509Certificate certificate,
    //            X509Chain chain, SslPolicyErrors sslPolicyErrors)
    //            { return true; };

    //            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.sit.kkday.com/api/order/payment/success/" + orderMid);

    //            httpWebRequest.ContentType = "application/json";
    //            httpWebRequest.Method = "POST";

    //            string result;

    //            PaySuccessUpdOrderMst mst = new PaySuccessUpdOrderMst();
    //            mst.ipaddress = "192.168.1.1";
    //            mst.apiKey = "kkdayapi";
    //            mst.userOid = "1";
    //            mst.ver = "1.0.1";
    //            mst.locale = "zh-tw";

    //            PaySuccessUpdOrder p = new PaySuccessUpdOrder();

    //            p.memberUuid = UserData.UUID; 
    //            //p.tokenKey = fakeContact.tokenKey; 
    //            //p.deviceId = fakeContact.deviceId; 
    //            p.currency = payDtl.currency;
    //            p.currTotalPrice = payDtl.currTotalPrice.ToString();
    //            p.is3D = (req.is_3d == "0" ? false : true);
    //            p.payMethod = payDtl.payMethod;
    //            p.pmgwMethod = res.data.pmgw_method;
    //            p.pmgwTransNo = pmgwTransNo;
    //            p.isFraud = "0";

    //            mst.json = p;

    //            //$path = 'order/payment/success/'.$order_mid;
    //            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
    //            {
    //                string json = JsonConvert.SerializeObject(mst);
    //                streamWriter.Write(json);
    //                streamWriter.Flush();
    //                streamWriter.Close();
    //            }

    //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

    //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
    //            {
    //                result = streamReader.ReadToEnd();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            string dd = ex.ToString();

    //        }
    //    }


    }
}
