using System;
using System.Collections.Generic;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.Models.DataModel.Product;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using KKday.API.WMS.Models.DataModel.Package;
using KKday.API.WMS.Models.Repository.Discount;
using KKday.API.WMS.AppCode;
using KKday.API.WMS.Models.Repository.Common;

namespace KKday.API.WMS.Models.Repository.Product {
    public class ProductRepository {
        public ProductRepository() {
        }

        //code language type
        static string _VOUCHER_EXCHANGE_TYPE = "VOUCHER_EXCHANGE_TYPE";
        static string _SEARCH_TYPE = "PRODUCT";


        private readonly IRedisHelper _redisCache;
        public ProductRepository(IRedisHelper redisCache) {

            _redisCache = redisCache;
        }

        /// <summary>
        /// Gets the prod dtl.
        /// </summary>
        /// <returns>The prod dtl.</returns>
        /// <param name="queryRQ">Query rq.</param>
        public ProductModel GetProdDtl(QueryProductModel queryRQ) {
            ProductModel product = new ProductModel();
            JObject obj = null, objModule = null, objExTypeLang = null;
            DataModel.Discount.DiscountRuleModel disc = null;
            DiscountRepository dis = new DiscountRepository(_redisCache);
            try {
                //商品黑名單過濾
                //抓商品是否為黑名單
                bool isBlack = dis.GetProdBlackWhite(queryRQ.prod_no);

                if (isBlack) {
                    product.result = "10";
                    product.result_msg = $"Bad Request:Product-{queryRQ.prod_no} is not available";
                    return product;
                }

                //data from kkday api
                obj = ProdProxy.getProd(queryRQ);
                objModule = ProdProxy.getModule(queryRQ);
                objExTypeLang = CommonProxy.getCodeLang(queryRQ, ProductRepository._VOUCHER_EXCHANGE_TYPE);


                if (obj["content"]["result"].ToString() != "0000") {
                    product.result = obj["content"]["result"].ToString();
                    product.result_msg = $"kkday product api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception($"kkday product api response msg is not correct! {obj["content"]["msg"].ToString()}");
                }

                product.result = obj["content"]["result"].ToString();
                product.result_msg = obj["content"]["msg"].ToString();
                product.prod_no = (int)obj["content"]["product"]["prodOid"];
                product.prod_name = obj["content"]["product"]["productName"].ToString();
                product.prod_img_url = obj["content"]["product"]["productName"].ToString();
                product.prod_type = obj["content"]["product"]["mainCat"].ToString();
                product.prod_type_name = obj["content"]["product"]["mainCatStr"].ToString();
                product.prod_currency = obj["content"]["product"]["prodCurrCd"].ToString();
                product.tag = obj["content"]["product"]["tagCd"].ToString().Split(',');
                product.cost_type = obj["content"]["product"]["costCalcMethod"].ToString();
                product.main_lang = obj["content"]["product"]["masterLang"].ToString();
                product.introduction = obj["content"]["product"]["introduction"].ToString();
                product.prod_desc = obj["content"]["product"]["productDesc"].ToString();
                product.prod_tips = obj["content"]["product"]["productTips"].ToString();
                product.prod_map_note = obj["content"]["product"]["gatherNote"].ToString();
                product.is_search = obj["content"]["product"]["isSearch"].ToString();
                product.apply_status = obj["content"]["product"]["applyStatus"].ToString();
                product.status = obj["content"]["product"]["saleStatus"].ToString();
                product.policy_no = obj["content"]["product"]["policyNo"].ToString();  //1:不扣手續費，退全額（包含當天取消者） 2:取消訂單，將收取所有實際產生費用 3:取消時間依照商品時區決定
                product.is_tour = obj["content"]["product"]["isSche"].ToString();
                product.days = (int)obj["content"]["product"]["tourDays"];
                product.hours = (int)obj["content"]["product"]["tourHours"];
                product.confirm_order_time = (int)obj["content"]["product"]["confirmHour"];
                product.online_s_date = obj["content"]["product"]["begSaleDt"].ToString();
                product.online_e_date = obj["content"]["product"]["endSaleDt"].ToString();
                product.before_order_day = obj["content"]["product"]["cutOfDay"].ToString();
                product.finishStep = obj["content"]["product"]["finishStep"].ToString();


                ProdCommentInfo comment = new ProdCommentInfo();
                comment.total_scores = obj["content"]["prodUrlInfo"]["totalScores"].ToString();
                comment.avg_scores = obj["content"]["prodUrlInfo"]["avgScores"].ToString();
                comment.click_count = obj["content"]["prodUrlInfo"]["clickCnt"].ToString();
                comment.comment_record = obj["content"]["prodUrlInfo"]["countRec"].ToString();
                comment.keyword = obj["content"]["prodUrlInfo"]["keyword"].ToString();
                comment.sales_qty = obj["content"]["prodUrlInfo"]["orderNum"].ToString();
                comment.prod_url_oid = obj["content"]["prodUrlInfo"]["prodUrlOid"].ToString();
                product.prod_comment_info = comment;

                product.b2c_price = (double)obj["content"]["product"]["minSalePrice"];
                product.b2d_price = dis.GetCompanyDiscPrice(Int64.Parse(queryRQ.company_xid), queryRQ.current_currency, (double)obj["content"]["product"]["minPrice"], queryRQ.prod_no, obj["content"]["product"]["mainCat"].ToString(), ProductRepository._SEARCH_TYPE, null, ref disc);

                product.order_email = obj["content"]["product"]["orderEmail"].ToString();
                product.prod_hander = obj["content"]["supplier"]["orderHandler"].ToString();


                TktExpire tkt = new TktExpire();
                tkt.exp_type = obj["content"]["tkExpSetting"]["expTp"].ToString();
                tkt.exp_open_date = obj["content"]["tkExpSetting"]["expNum"].ToString();
                tkt.exp_s_date = obj["content"]["tkExpSetting"]["expSt"].ToString();
                tkt.exp_e_date = obj["content"]["tkExpSetting"]["expEd"].ToString();
                product.tkt_expire = tkt;

                ProdMarketing pmkt = new ProdMarketing();
                pmkt.is_ec_sale = (bool)obj["content"]["prodMarketing"]["is_ec_sale"];
                pmkt.is_ec_show = (bool)obj["content"]["prodMarketing"]["is_ec_show"];
                pmkt.is_search = (bool)obj["content"]["prodMarketing"]["is_search"];
                pmkt.is_show = (bool)obj["content"]["prodMarketing"]["is_show"];
                pmkt.purchase_type = obj["content"]["prodMarketing"]["purchase_type"].ToString();
                pmkt.purchase_type_name = obj["content"]["prodMarketing"]["purchase_type_name"].ToString();
                product.prod_mkt = pmkt;

                //product 之外的list 或 object
                //取消規定
                List<Policy> polList = new List<Policy>();
                Policy policy = null;
                if (obj["content"]["policyList"] != null) {
                    JArray policy_items = (JArray)obj["content"]["policyList"];

                    foreach (var item in policy_items) {
                        policy = new Policy();
                        policy.days = (int)item["policy"]["days"];
                        policy.fee = (int)item["policy"]["percent"];
                        policy.is_over = (bool)item["policy"]["isOver"];
                        polList.Add(policy);

                    }

                    product.policy_list = polList;
                }
                //行程表
                List<Tour> tourList = new List<Tour>();
                Tour tour = null;
                if (obj["content"]["scheList"] != null) {
                    JArray sche_items = (JArray)obj["content"]["scheList"];


                    foreach (var item in sche_items) {
                        tour = new Tour();
                        tour.tour_day = (int)item["sche"]["daySeq"];
                        tour.time_desc = (string)item["sche"]["timeDesc"];
                        tour.tour_desc = (string)item["sche"]["scheDesc"];
                        tour.tour_sort_seq = (int)item["sche"]["sortSeq"];
                        tour.photo_url = (string)item["sche"]["photoUrl"];
                        tourList.Add(tour);
                    }

                    product.tour_list = tourList;
                }

                //行程餐食
                List<ProvideMeal> mealList = new List<ProvideMeal>();
                ProvideMeal meal = null;
                if (obj["content"]["scheMealList"] != null) {
                    JArray meal_items = (JArray)obj["content"]["scheMealList"];

                    foreach (var item in meal_items) {
                        meal = new ProvideMeal();
                        meal.tour_day = (int)item["day"];
                        meal.is_breakfast = (string)item["meal"]["breakfast"];
                        meal.is_lunch = (string)item["meal"]["lunch"];
                        meal.is_dinner = (string)item["meal"]["dinner"];
                        mealList.Add(meal);
                    }

                    product.meal_list = mealList;
                }

                //導覽語言
                List<GuideLanguage> langList = new List<GuideLanguage>();
                GuideLanguage lang = null;
                if (obj["content"]["product"]["guideLang"] != null && (string)obj["content"]["product"]["guideLang"] != "") {
                    //多筆
                    if (((string)obj["content"]["product"]["guideLang"]).Contains(',')) {
                        string[] guide_langs = ((string)obj["content"]["product"]["guideLang"]).Split(',');
                        foreach (string guide_lang in guide_langs) {
                            lang = new GuideLanguage();
                            lang.lang_code = guide_lang;
                            lang.lang_name = (string)obj["content"]["product"]["guideLangMap"][guide_lang];
                            langList.Add(lang);

                        }
                    } else {
                        lang = new GuideLanguage();
                        lang.lang_code = (string)obj["content"]["product"]["guideLang"];
                        lang.lang_name = (string)obj["content"]["product"]["guideLangMap"][(string)obj["content"]["product"]["guideLang"]];
                        langList.Add(lang);
                    }

                    product.guide_lang_list = langList;
                }

                //主要目的地 (地圖區)
                List<ArrivalMapInfo> arrList = new List<ArrivalMapInfo>();
                ArrivalMapInfo arr = null;
                if (obj["content"]["arrList"] != null) {
                    JArray items = (JArray)obj["content"]["arrList"];
                    foreach (var item in items) {
                        arr = new ArrivalMapInfo();
                        arr.photo_url = (string)item["latlong"]["imgUrl"];
                        arr.photo_desc = (string)item["latlong"]["photoDesc"];
                        arr.zoom = (int)item["latlong"]["zoomLv"];
                        arr.latitude = (string)item["latlong"]["latitude"];
                        arr.longitude = (string)item["latlong"]["longitude"];
                        arr.latlong_type = (string)item["latlong"]["latlongType"];
                        arr.latlong_desc = (string)item["latlong"]["latlongDesc"];
                        arr.latlong_xid = (int)item["latlong"]["latlongOid"];

                        arrList.Add(arr);

                    }

                    product.arr_map_info_list = arrList;
                }

                //圖片
                List<Images> imgList = new List<Images>();
                Images img = null;
                if (obj["content"]["imgList"] != null) {
                    JArray items = (JArray)obj["content"]["imgList"];

                    foreach (var item in items) {
                        img = new Images();
                        img.auth_name = (string)item["img"]["authName"];
                        img.is_main_img = (string)item["img"]["defaultImg"];
                        img.img_desc = (string)item["img"]["imgDesc"];
                        img.img_sort = (int)item["img"]["imgSeq"];
                        img.img_url = (string)item["img"]["imgUrl"];
                        img.img_kkday_url = (string)item["img"]["kkdayImgUrl"];
                        img.is_auth_cc = (string)item["img"]["isCcAuth"];
                        img.is_commerce = (string)item["img"]["isCommerce"];
                        img.share_type = (string)item["img"]["shareType"];

                        imgList.Add(img);
                    }
                    product.img_list = imgList;
                }




                //憑證類型
                //module api找出該商品的憑證類型
                JArray modules = (JArray)objModule["content"]["product"]["modules"];
                var voucher_module = modules.FirstOrDefault(jt => (string)jt["moduleType"] == "PMDL_EXCHANGE");

                if ((bool)voucher_module["moduleSetting"]["isRequired"]) {
                    //codeLang api找出exchangeType 與langValue 對應
                    var code = objExTypeLang["content"]["codeList"].FirstOrDefault(jt => (string)jt["code"]["langValue"] == (string)voucher_module["moduleSetting"]["setting"]["exchangeType"]);
                    //找出憑證類型敘述
                    product.voucher_desc = (string)code["code"]["langDesc"];
                } else {
                    product.voucher_desc = "";
                }
                //找出憑證領取地點資訊(名稱,地點,營業時間)
                product.voucher_locations = voucher_module["moduleSetting"]["setting"]["dataItems"]["locations"].ToObject<List<Location>>();


                //注意事項挖字處理
                product.remind_list = setRemInf(obj, queryRQ.locale_lang, null);

                var venue_module = modules.FirstOrDefault(jt => (string)jt["moduleType"] == "PMDL_VENUE");
                if ((string)venue_module["moduleSetting"]["setting"]["venueType"] == "01") {
                    MeetingPointMap meeting_point = new MeetingPointMap();
                    meeting_point.mapAddress = (string)venue_module["moduleSetting"]["setting"]["dataItems"]["meetingPointMap"]["mapAddress"];
                    meeting_point.latitude = (string)venue_module["moduleSetting"]["setting"]["dataItems"]["meetingPointMap"]["latitude"];
                    meeting_point.longitude = (string)venue_module["moduleSetting"]["setting"]["dataItems"]["meetingPointMap"]["longitude"];
                    meeting_point.zoomLevel = (string)venue_module["moduleSetting"]["setting"]["dataItems"]["meetingPointMap"]["zoomLevel"];
                    meeting_point.imageUrl = (string)venue_module["moduleSetting"]["setting"]["dataItems"]["meetingPointMap"]["imageUrl"];
                    product.meeting_point_map = meeting_point;
                    //product.meeting_point_map = venue_module["moduleSetting"]["setting"]["dataItems"]["meetingPointMap"].ToObject<MeetingPointMap>();
                }

                //List<Remind> remList = new List<Remind>();
                //Remind rem = null;

                //if (obj["content"]["remindList"] != null)
                //{
                //    JArray items = (JArray)obj["content"]["remindList"];
                //    int remindList = items.Count;
                //    for (int i = 0; i < remindList; i++)
                //    {
                //        rem = new Remind();
                //        rem.remind_desc = items[i]["remind"]["desc"].ToString();
                //        rem.remind_xid = (int)items[i]["remind"]["detailOid"];
                //        remList.Add(rem);
                //    }
                //    //疾病提醒
                //    if ((string)obj["content"]["product"]["diseaseRemind"] != "")
                //    {
                //        rem = new Remind();
                //        rem.remind_desc = obj["content"]["product"]["diseaseRemind"].ToString();
                //        rem.remind_xid = 0;
                //        remList.Add(rem);
                //    }

                //    product.remind_list = remList;
                //}

                //影片
                List<Video> videoList = new List<Video>();
                Video video = null;
                if (obj["content"]["videoList"] != null) {
                    JArray items = (JArray)obj["content"]["videoList"];

                    foreach (var item in items) {
                        video = new Video();
                        video.lang_code = (string)item["video"]["langCode"];
                        video.vidoe_url = (string)item["video"]["videoUrl"];
                        video.xid = (int)item["video"]["videoOid"];

                        videoList.Add(video);
                    }
                    product.video_list = videoList;
                }

                //費用包含與不包含
                List<CostDetail> detailList = new List<CostDetail>();
                CostDetail detail = null;
                if (obj["content"]["detailList"] != null) {
                    JArray items = (JArray)obj["content"]["detailList"];

                    foreach (var item in items) {
                        detail = new CostDetail();
                        detail.detail_desc = (string)item["detail"]["desc"];
                        detail.detail_type = (string)item["detail"]["detailType"];

                        detailList.Add(detail);
                    }
                    product.cost_detail_list = detailList;
                }

                //接機地點(地圖區)
                List<MeetingPoint> mpList = new List<MeetingPoint>();
                MeetingPoint mp = null;
                if (obj["content"]["meetingPointList"] != null) {
                    JArray items = (JArray)obj["content"]["meetingPointList"];

                    foreach (var item in items) {
                        mp = new MeetingPoint();
                        mp.terminal = (string)item["data"]["terminal"];
                        mp.meeting_point = (string)item["data"]["meeting"];
                        mp.airport_code = (string)item["data"]["airport"];
                        mp.img_url = (string)item["data"]["img"]["s3Url"];
                        mpList.Add(mp);
                    }
                    product.meeting_point_list = mpList;
                }

                //取國家,城市
                List<Country> countryList = new List<Country>();
                Country country = null;
                List<City> cityList = new List<City>();
                City city = null;

                if (obj["content"]["cityList"] != null) {
                    JArray items = (JArray)obj["content"]["cityList"];
                    country = new Country();
                    country.id = (string)items[0]["city"]["countryCd"];
                    country.name = (string)items[0]["city"]["countryName"];

                    foreach (var item in items) {
                        city = new City();
                        city.id = (string)item["city"]["cityCd"];
                        city.name = (string)item["city"]["cityName"];
                        cityList.Add(city);
                    }
                    country.cities = cityList;
                    countryList.Add(country);
                    product.countries = countryList;
                }

            } catch (Exception ex) {
                product.result = "10001";
                product.result_msg = $"Product ERROR:{ex.Message},{ex.StackTrace}";

                Website.Instance.logger.FatalFormat($"Product ERROR:{ex.Message},{ex.StackTrace}");
            }

            return product;
        }

        /// <summary>
        /// Gets the prod module.
        /// </summary>
        /// <returns>The prod module.</returns>
        /// <param name="queryRQ">Query rq.</param>
        public ProductModuleModel GetProdModule(QueryProductModel queryRQ) {
            ProductModuleModel module = new ProductModuleModel();
            JObject objModule = null, obj = null, objEvent = null, objCountryCode = null, objAreaCode = null, objAirport = null;

            try {

                string _moduleRedis = _redisCache.getRedis($"b2d:Product:module:{queryRQ.prod_no}_{queryRQ.pkg_no}");
                if (string.IsNullOrEmpty(_moduleRedis)) {

                    obj = ProdProxy.getProd(queryRQ);
                    objModule = ProdProxy.getModule(queryRQ);
                    objEvent = PackageProxy.getEvents(queryRQ);
                    objCountryCode = CommonProxy.getCodeCountry(queryRQ);
                    objAreaCode = CommonProxy.getCodeArea(queryRQ);
                    objAirport = CommonProxy.getProdAirport(queryRQ);

                    //挖字！！！！！！！
                    Dictionary<string, string> uikey = CommonRepository.getKlingon(_redisCache, "frontend", queryRQ.locale_lang);

                    if (objModule["content"]["result"].ToString() != "0000") {
                        module.result = objModule["content"]["result"].ToString();
                        module.result_msg = $"kkday module api response msg is not correct! {objModule["content"]["msg"].ToString()}";
                        throw new Exception($"kkday module api response msg is not correct! {objModule["content"]["msg"].ToString()}");
                    }
                    if (obj["content"]["result"].ToString() != "0000") {
                        module.result = objModule["content"]["result"].ToString();
                        module.result_msg = $"kkday product api response msg is not correct! {obj["content"]["msg"].ToString()}";
                        throw new Exception($"kkday product api response msg is not correct! {obj["content"]["msg"].ToString()}");
                    }


                    module.result = objModule["content"]["result"].ToString();
                    module.result_msg = objModule["content"]["msg"].ToString();

                    JArray jModules = (JArray)objModule["content"]["product"]["modules"];
                    module.module_type = jModules.Where(y => (bool)y["moduleSetting"]["isRequired"] == true).Select(x => (string)x["moduleType"]).ToList<string>();


                    //旅客資料
                    if (module.module_type.Where(x => x.Contains("PMDL_CUST_DATA")).Count() > 0) {
                        var objPmdlCustData = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_CUST_DATA");

                        CusData cus = new CusData();
                        cus.is_require = (bool)objPmdlCustData["moduleSetting"]["isRequired"];
                        cus.cus_type = (string)objPmdlCustData["moduleSetting"]["setting"]["customerDataType"];

                        EnglisName en = new EnglisName();
                        en.is_require_FirstName = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["englishName"]["isRequired"];
                        en.is_require_LastName = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["englishName"]["isRequired"];
                        cus.englis_name = en;

                        Gender gd = new Gender();
                        List<GenderType> gdType = new List<GenderType>();
                        gd.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["gender"]["isRequired"];
                        gdType.Add(new GenderType() {
                            type = "F",
                            type_name = uikey["common_female"]
                        });
                        gdType.Add(new GenderType() {
                            type = "M",
                            type_name = uikey["common_male"]
                        });
                        gd.gender_list = !gd.is_require ? null : gdType;
                        cus.gender = gd;

                        Nationality na = new Nationality();
                        NationInfo info = new NationInfo();
                        NationalityID naID = new NationalityID();
                        naID.is_require_TW = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["nationality"]["options"]["TWIdentityNumber"]["isRequired"];
                        naID.is_require_HKMO = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["nationality"]["options"]["HKMOIdentityNumber"]["isRequired"];
                        naID.is_require_MTP = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["nationality"]["options"]["MTPNumber"]["isRequired"];

                        na.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["nationality"]["isRequired"];
                        na.nation_list = !na.is_require ? null : ((JArray)objCountryCode["content"]["countryList"])
                                .Select(x => new NationInfo {
                                    country_code = (string)x["country"]["countryCd"],
                                    country_local_name = (string)x["country"]["countryName"]

                                }).ToList();

                        na.nationality_id = naID;
                        cus.nationality = na;

                        Birthday bhd = new Birthday();
                        bhd.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["birthday"]["isRequired"];
                        cus.birthday = bhd;

                        Passport pass = new Passport();
                        pass.is_require_PassprotNo = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["passportNo"]["isRequired"];
                        pass.is_require_PassprotExpDate = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["passportNo"]["options"]["passportExpDate"]["isRequired"];
                        cus.passport = pass;

                        LocalName ln = new LocalName();
                        ln.is_require_FirstName = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["localName"]["isRequired"];
                        ln.is_require_LastName = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["localName"]["isRequired"];
                        cus.local_name = ln;


                        High h = new High();
                        List<Unit> h_units = new List<Unit>();
                        h.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["height"]["isRequired"];
                        h_units.Add(new Unit() {
                            unit_code = "01",
                            unit_name = uikey["booking_step1_cust_data_height_unit_01"]

                        });
                        h_units.Add(new Unit() {
                            unit_code = "02",
                            unit_name = uikey["booking_step1_cust_data_height_unit_02"]
                        });
                        h.unit_list = !h.is_require ? null : h_units;
                        cus.high = h;

                        Weight w = new Weight();
                        List<Unit> w_units = new List<Unit>();
                        w.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["weight"]["isRequired"];
                        w_units.Add(new Unit() {
                            unit_code = "01",
                            unit_name = uikey["booking_step1_cust_data_weight_unit_01"]
                        });
                        w_units.Add(new Unit() {
                            unit_code = "02",
                            unit_name = uikey["booking_step1_cust_data_weight_unit_02"]
                        });
                        w.unit_list = !w.is_require ? null : w_units;
                        cus.weight = w;

                        ShoeSize shoe = new ShoeSize();
                        shoe.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["isRequired"];
                        if (shoe.is_require) {
                            Man man = new Man();
                            man.is_provided = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["man"]["isProvided"];
                            man.unit_list = !man.is_provided ? null : ((JArray)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["man"]["allowData"])
                                .Select(x => new Unit {
                                    unit_code = (string)x["unit"],
                                    unit_name = (string)x["unit"] == "01" ? "US" : (string)x["unit"] == "02" ? "EU" : "JP/CM",
                                    size_range_start = (string)x["min"],
                                    size_range_end = (string)x["max"]
                                }).ToList();

                            Woman woman = new Woman();
                            woman.is_provided = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["woman"]["isProvided"];
                            woman.unit_list = !woman.is_provided ? null : ((JArray)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["woman"]["allowData"])
                                .Select(x => new Unit {
                                    unit_code = (string)x["unit"],
                                    unit_name = (string)x["unit"] == "01" ? "US" : (string)x["unit"] == "02" ? "EU" : "JP/CM",
                                    size_range_start = (string)x["min"],
                                    size_range_end = (string)x["max"]
                                }).ToList();

                            Child child = new Child();
                            child.is_provided = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["child"]["isProvided"];
                            child.unit_list = !child.is_provided ? null : ((JArray)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["child"]["allowData"])
                                .Select(x => new Unit {
                                    unit_code = (string)x["unit"],
                                    unit_name = (string)x["unit"] == "01" ? "US" : (string)x["unit"] == "02" ? "EU" : "JP/CM",
                                    size_range_start = (string)x["min"],
                                    size_range_end = (string)x["max"]
                                }).ToList();
                            shoe.man = man;
                            shoe.woman = woman;
                            shoe.child = child;

                        }
                        cus.shoe_size = shoe;
                        Meal meal = new Meal();
                        ExcludeFood ex = new ExcludeFood();
                        AllergyFood allergy = new AllergyFood();

                        meal.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["meal"]["isRequired"];
                        meal.meal_list = !meal.is_require ? null : ((JArray)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["meal"]["options"]["meals"])
                            .Select(x => new MealType {
                                is_provided = (bool)x["isProvided"],
                                meal_type = (string)x["mealType"],
                                meal_type_name = (string)x["mealTypeName"]
                            }).OrderBy(y => y.meal_type).ToList();

                        ex.is_exclude = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["meal"]["options"]["excludeFood"]["isExcluded"];
                        ex.food_list = !ex.is_exclude ? null : ((JArray)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["meal"]["options"]["excludeFood"]["foods"])
                            .Select(x => new Food {
                                can_exclude = (bool)x["canExclude"],
                                food_type = (string)x["foodType"],
                                food_type_name = (string)x["foodTypeName"]
                            }).ToList();
                        allergy.is_require_FoodAllergy = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["meal"]["options"]["excludeFood"]["isExcluded"];
                        ex.allergy_food = allergy;
                        meal.exclude_food = ex;
                        cus.meal = meal;

                        GlassDiopter glass = new GlassDiopter();
                        glass.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["glassDiopter"]["isRequired"];
                        glass.degree_range_start = (string)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["glassDiopter"]["diopterRangeStart"];
                        glass.degree_range_end = (string)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["glassDiopter"]["diopterRangeEnd"];
                        cus.glass_degree = glass;

                        module.module_cust_data = cus;
                    }

                    //旅遊期間聯絡人
                    if (module.module_type.Where(x => x.Contains("PMDL_CONTACT_DATA")).Count() > 0) {
                        var objPmdlContactData = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_CONTACT_DATA");
                        ContactData contact = new ContactData();
                        contact.is_require = (bool)objPmdlContactData["moduleSetting"]["isRequired"];

                        ContactName name = new ContactName();
                        name.is_require_FirstName = (bool)objPmdlContactData["moduleSetting"]["setting"]["dataItems"]["contactName"]["isRequired"];
                        name.is_require_LastName = (bool)objPmdlContactData["moduleSetting"]["setting"]["dataItems"]["contactName"]["isRequired"];
                        contact.contact_name = name;

                        ContactTel tel = new ContactTel();
                        tel.is_require_TelCountryCode = (bool)objPmdlContactData["moduleSetting"]["setting"]["dataItems"]["contactTel"]["isRequired"];
                        tel.is_require_TelNumber = (bool)objPmdlContactData["moduleSetting"]["setting"]["dataItems"]["contactTel"]["isRequired"];
                        tel.tel_code_list = !tel.is_require_TelCountryCode ? null : ((JArray)objCountryCode["content"]["countryList"])
                                .Select(x => new NationInfo {
                                    country_tel_code = (string)x["country"]["telArea"],
                                    country_tel_info = $"{(string)x["country"]["countryEngName"]}({(string)x["country"]["countryName"]})"
                                }).ToList();

                        contact.contact_tel = tel;

                        ContactApp app = new ContactApp();
                        app.is_require = (bool)objPmdlContactData["moduleSetting"]["setting"]["dataItems"]["contactApp"]["isRequired"];
                        app.is_require_AppAccount = (bool)objPmdlContactData["moduleSetting"]["setting"]["dataItems"]["contactApp"]["isRequired"];
                        app.app_type_list = ((JArray)objPmdlContactData["moduleSetting"]["setting"]["dataItems"]["contactApp"]["apps"])
                            .Select(x => new App {
                                is_supported = (bool)x["isSupported"],
                                app_type = (string)x["appType"],
                                app_name = (string)x["appTypeName"]
                            }).ToList();
                        contact.contact_app = app;
                        module.module_contact_data = contact;
                    }

                    //其他資料
                    if (module.module_type.Where(x => x.Contains("PMDL_SIM_WIFI")).Count() > 0) {
                        var objPmdlSimWifi = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_SIM_WIFI");
                        SimWifi simWifi = new SimWifi();

                        simWifi.is_require = (bool)objPmdlSimWifi["moduleSetting"]["isRequired"];
                        MobileModelNumber no = new MobileModelNumber();
                        MobileIMEI imei = new MobileIMEI();
                        ActivationDate date = new ActivationDate();
                        no.is_require = (bool)objPmdlSimWifi["moduleSetting"]["setting"]["dataItems"]["mobileModelNumber"]["isRequired"];
                        imei.is_require = (bool)objPmdlSimWifi["moduleSetting"]["setting"]["dataItems"]["mobileIMEI"]["isRequired"];
                        date.is_require = (bool)objPmdlSimWifi["moduleSetting"]["setting"]["dataItems"]["activationDate"]["isRequired"];

                        simWifi.mobile_model_no = no;
                        simWifi.mobile_IMEI = imei;
                        simWifi.activation_date = date;
                        module.module_sim_wifi = simWifi;
                    }

                    //乘客資料
                    if (module.module_type.Where(x => x.Contains("PMDL_CAR_PSGR")).Count() > 0) {
                        var objPmdlCarPsgr = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_CAR_PSGR");

                        CarPasgr carPasgr = new CarPasgr();
                        carPasgr.is_require = (bool)objPmdlCarPsgr["moduleSetting"]["isRequired"];

                        AdultQty adult = new AdultQty();
                        ChildQty child = new ChildQty();
                        InfantQty infant = new InfantQty();
                        ChildSeat cseat = new ChildSeat();
                        InfantSeat iseat = new InfantSeat();
                        CarryLuggageQty carry = new CarryLuggageQty();
                        CheckedLuggageQty check = new CheckedLuggageQty();
                        adult.is_require = (bool)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyAdult"]["isRequired"];
                        adult.age_range_start = (int?)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyAdult"]["ageLimitStart"];
                        adult.age_range_end = (int?)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyAdult"]["ageLimitEnd"];

                        child.is_require = (bool)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyChild"]["isRequired"];
                        child.age_range_start = (int?)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyChild"]["ageLimitStart"];
                        child.age_range_end = (int?)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyChild"]["ageLimitEnd"];

                        infant.is_require = (bool)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyInfant"]["isRequired"];
                        infant.age_range_start = (int?)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyInfant"]["ageLimitStart"];
                        infant.age_range_end = (int?)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyInfant"]["ageLimitEnd"];

                        cseat.is_require_supplierProvided = (bool)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyChildSeat"]["isRequired"];
                        cseat.is_require_selfProvided = (bool)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyChildSeat"]["isRequired"];
                        cseat.age_range_start = (int?)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyChildSeat"]["ageLimitStart"];
                        cseat.age_range_end = (int?)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyChildSeat"]["ageLimitEnd"];

                        iseat.is_require_supplierProvided = (bool)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyInfantSeat"]["isRequired"];
                        iseat.is_require_selfProvided = (bool)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyInfantSeat"]["isRequired"];
                        iseat.age_range_start = (int?)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyInfantSeat"]["ageLimitStart"];
                        iseat.age_range_end = (int?)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyInfantSeat"]["ageLimitEnd"];

                        carry.is_require = (bool)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyCarryLuggage"]["isRequired"];
                        check.is_require = (bool)objPmdlCarPsgr["moduleSetting"]["setting"]["dataItems"]["qtyCheckedLuggage"]["isRequired"];

                        carPasgr.adul_qty = adult;
                        carPasgr.child_qty = child;
                        carPasgr.infant_qty = infant;
                        carPasgr.child_safety_seat = cseat;
                        carPasgr.infant_safety_seat = iseat;
                        carPasgr.carry_luggage_qty = carry;
                        carPasgr.checked_luggage_qty = check;

                        module.module_car_pasgr = carPasgr;

                    }

                    //班機資訊 
                    if (module.module_type.Where(x => x.Contains("PMDL_FLIGHT_INFO")).Count() > 0) {
                        var objPmdlFlightInfo = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_FLIGHT_INFO");
                        FlightInfo flight = new FlightInfo();
                        flight.is_require = (bool)objPmdlFlightInfo["moduleSetting"]["isRequired"];

                        Arrival arr = new Arrival();
                        Departure dep = new Departure();

                        List<FlightType> ft_list = new List<FlightType>();
                        ft_list.Add(new FlightType() {
                            type = "01",
                            type_name = uikey["booking_step1_flight_info_domestic_routes"]
                        });
                        ft_list.Add(new FlightType() {
                            type = "02",
                            type_name = uikey["booking_step1_flight_info_international_routes"]
                        });

                        List<Airport> airports = new List<Airport>();
                        if (objAirport["content"]["airportList"] != null) {
                            airports = ((JArray)objAirport["content"]["airportList"])
                              .Select(x => new Airport {
                                  airport_code = (string)x["airportCode"],
                                  airport_name = (string)x["airportName"],
                                  area_code = (string)x["areaCd"]
                              }).ToList();
                        }


                        arr.is_require_FlightType = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["flightType"]["isRequired"];
                        arr.flight_type_list = !arr.is_require_FlightType ? null : ft_list;
                        arr.is_require_Date = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["arrivalDatetime"]["isRequired"];
                        arr.is_require_Hour = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["arrivalDatetime"]["isRequired"];
                        arr.is_require_Minute = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["arrivalDatetime"]["isRequired"];
                        arr.is_require_Airport = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["airport"]["isRequired"];
                        arr.airport_list = !arr.is_require_Airport ? null : airports;
                        arr.is_require_Airline = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["airline"]["isRequired"];
                        arr.is_require_FlightNo = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["flightNo"]["isRequired"];
                        arr.is_require_TerminalNo = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["terminalNo"]["isRequired"];
                        arr.is_need_ApplyVisa = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["isNeedToApplyVisa"]["isRequired"];

                        dep.is_require_FlightType = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["flightType"]["isRequired"];
                        dep.flight_type_list = !dep.is_require_FlightType ? null : ft_list;
                        dep.is_require_Date = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["departureDatetime"]["isRequired"];
                        dep.is_require_Hour = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["departureDatetime"]["isRequired"];
                        dep.is_require_Minute = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["departureDatetime"]["isRequired"];
                        dep.is_require_Airport = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["airport"]["isRequired"];
                        dep.airport_list = !dep.is_require_Airport ? null : airports;
                        dep.is_require_Airline = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["airline"]["isRequired"];
                        dep.is_require_FlightNo = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["flightNo"]["isRequired"];
                        dep.is_require_TerminalNo = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["terminalNo"]["isRequired"];
                        //dep.is_require_HaveBeenInCountry = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["haveBeenInCountry"]["isRequired"];

                        flight.arrival = arr;
                        flight.departure = dep;

                        module.module_flight_info = flight;
                    }

                    //寄送資料
                    if (module.module_type.Where(x => x.Contains("PMDL_SEND_DATA")).Count() > 0) {
                        var objPmdlSendData = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_SEND_DATA");
                        SendData send = new SendData();
                        send.is_require = (bool)objPmdlSendData["moduleSetting"]["isRequired"];

                        ReceiverName recname = new ReceiverName();
                        recname.is_require_LastName = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["receiverName"]["isRequired"];
                        recname.is_require_FirstName = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["receiverName"]["isRequired"];
                        send.receiver_name = recname;

                        ReceiverTel rectel = new ReceiverTel();
                        rectel.is_require_TelCountryCode = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["receiverTel"]["isRequired"];
                        rectel.is_require_TelNumber = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["receiverTel"]["isRequired"];
                        rectel.tel_code_list = !rectel.is_require_TelCountryCode ? null : ((JArray)objCountryCode["content"]["countryList"])
                                .Select(x => new NationInfo {
                                    country_tel_code = (string)x["country"]["telArea"],
                                    country_tel_info = $"{(string)x["country"]["countryEngName"]}({(string)x["country"]["countryName"]})"
                                }).ToList();
                        send.receiver_tel = rectel;

                        ReceiverAddress recadd = new ReceiverAddress();
                        recadd.is_require_Country = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToCountry"]["isRequired"];
                        recadd.is_require_City = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToCountry"]["isRequired"];
                        recadd.is_require_ZipCode = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToCountry"]["isRequired"];
                        recadd.is_require_Address = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToCountry"]["isRequired"];


                        JArray items = (JArray)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToCountry"]["countries"];
                        List<Country> countryList = new List<Country>();
                        if (items != null) {
                            //moudle來的country 資訊
                            foreach (var item in items) {
                                Country country = new Country();
                                List<City> cityList = new List<City>();

                                //ex:A01-003
                                string area_code = item["countryCode"].ToString().Split('-')[0];
                                string country_code = item["countryCode"].ToString();

                                //對應另外一隻api的area country 資訊
                                var area_items = objAreaCode["content"]["areaList"].FirstOrDefault(x => x["continent"].ToString().Contains(area_code));
                                var country_items = area_items["countrys"].FirstOrDefault(x => x["country"].ToString().Contains(country_code));

                                country.id = country_items["country"].ToString().Split("|||")[1];
                                country.name = country_items["country"].ToString().Split("|||")[0];
                                country.cities = ((JArray)country_items["citys"])
                                    .Select(x => new City {
                                        id = x.ToString().Split("|||")[1],
                                        name = x.ToString().Split("|||")[0]

                                    }).ToList();
                                countryList.Add(country);
                            }
                        }


                        recadd.country_list = countryList;
                        send.receiver_address = recadd;

                        SendToHotel hotel = new SendToHotel();
                        SendToHotelInfo info = new SendToHotelInfo();
                        hotel.is_provided = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["isProvided"];
                        info.is_require_HotelName = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["hotelName"]["isRequired"];
                        info.is_require_HotelAddress = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["hotelAddress"]["isRequired"];
                        info.is_require_HotelTel = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["hotelTel"]["isRequired"];
                        info.is_require_BuyerPassportEnglishLastName = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["buyerPassportEnglishName"]["isRequired"];
                        info.is_require_BuyerPassportEnglishFirstName = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["buyerPassportEnglishName"]["isRequired"];
                        info.is_require_BuyerLocalFirstName = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["buyerLocalName"]["isRequired"];
                        info.is_require_BuyerLocalLastName = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["buyerLocalName"]["isRequired"];
                        info.is_require_BookingOrderNo = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["bookingOrderNo"]["isRequired"];
                        info.is_require_CheckInDate = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["checkInDate"]["isRequired"];
                        info.is_require_CheckOutDate = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["checkOutDate"]["isRequired"];
                        info.is_require_BookingWebsite = (bool)objPmdlSendData["moduleSetting"]["setting"]["dataItems"]["sendToHotel"]["options"]["bookingWebsite"]["isRequired"];

                        hotel.send_to_hotel_info = info;
                        send.send_to_hotel = hotel;
                        module.module_send_data = send;

                    }

                    //租車(取車/還車)＋接送資料
                    if (module.module_type.Where(x => x.Contains("PMDL_RENT_CAR")).Count() > 0) {
                        var objPmdlRentCar = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_RENT_CAR");
                        RentCar car = new RentCar();
                        car.is_require = (bool)objPmdlRentCar["moduleSetting"]["isRequired"];
                        car.rent_type = (string)objPmdlRentCar["moduleSetting"]["setting"]["rentCarType"];
                        car.is_require_SDate = true;
                        car.is_require_EDate = true;

                        //去租車公司取車
                        if ((string)objPmdlRentCar["moduleSetting"]["setting"]["rentCarType"] != "03") {
                            RentOffice office = new RentOffice();
                            BusinessHour bissTime = new BusinessHour();
                            office.is_ProvidedFreeGPS = (bool)objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["rentCar"]["isProvidedFreeGPS"];
                            office.is_ProvidedFreeWiFi = (bool)objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["rentCar"]["isProvidedFreeWiFi"];
                            office.is_require_SLocation = true;
                            office.is_require_ELocation = true;
                            office.office_list = ((JArray)objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["rentCar"]["offices"])
                                .Select(x => new Office {
                                    sort = (int)x["sort"],
                                    id = (string)x["id"],
                                    area_code = (string)x["areaCode"],
                                    office_name = (string)x["officeName"],
                                    address_eng = (string)x["addressEng"],
                                    address_local = (string)x["addressLocal"],
                                    drop_off_interval = (int)x["dropOffInterval"],
                                    business_hour = x["businessHours"].ToObject<BusinessHour>()

                                }).ToList();
                            car.rent_office = office;
                        }
                        //司機接送 或是 客人指定>>接送資料
                        else {
                            car.is_require_EDate = false; //客人指定不用還車日期

                            DriverShuttle driver = new DriverShuttle();
                            CharterRoute charter = new CharterRoute();
                            RouteCustomized customized = new RouteCustomized();
                            customized.is_require = (bool)objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["driverShuttle"]["charterRoute"]["customRoute"]["isAllowCustom"];
                            customized.is_require_Location = (bool)objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["driverShuttle"]["charterRoute"]["customRoute"]["isAllowCustom"];
                            customized.route_limit = (int?)objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["driverShuttle"]["charterRoute"]["customRoute"]["routeLimit"];
                            charter.is_require = (bool)objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["driverShuttle"]["charterRoute"]["isRequired"];
                            charter.route_list = objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["driverShuttle"]["charterRoute"]["routes"].ToObject<List<Routes>>();
                            charter.route_custom = customized;
                            driver.charterRoute = charter;

                            car.driver_shuttle = driver;
                        }
                        module.module_rent_car = car;

                    }

                    //接送資料(包車/指定)
                    if (module.module_type.Where(x => x.Contains("PMDL_VENUE")).Count() > 0) {
                        var objPmdlVenue = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_VENUE");
                        if ((string)objPmdlVenue["moduleSetting"]["setting"]["venueType"] == "03" || (string)objPmdlVenue["moduleSetting"]["setting"]["venueType"] == "04") {
                            VenueInfo venue = new VenueInfo();
                            venue.is_require = (bool)objPmdlVenue["moduleSetting"]["isRequired"];
                            venue.is_require_Date = (bool)objPmdlVenue["moduleSetting"]["isRequired"];
                            venue.venue_type = (string)objPmdlVenue["moduleSetting"]["setting"]["venueType"];
                            venue.designated_location_list = ((JArray)objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedLocation"]["locations"])
                                .Select(x => new DesignatedLocation {
                                    id = (string)x["id"],
                                    sort = (int)x["sort"],
                                    location_name = (string)x["locationName"],
                                    location_address = (string)x["locationAddress"],
                                    image_url = (string)x["imageUrl"],
                                    time_range_start = (string)x["timeRange"]["from"]["hour"] + ":" + (string)x["timeRange"]["from"]["minute"],
                                    time_range_end = (string)x["timeRange"]["to"]["hour"] + ":" + (string)x["timeRange"]["to"]["minute"]
                                }).ToList();


                            //objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedLocation"]["locations"].ToObject<List<DesignatedLocation>>();

                            DesignatedByCustomer byCustomer = new DesignatedByCustomer();
                            PipickUp up = new PipickUp();
                            PickUpTime upTime = new PickUpTime();
                            TimeCustomized cusTime = new TimeCustomized();

                            DropOff off = new DropOff();
                            off.is_require_Location = (bool)objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedByCustomer"]["pickUpLocation"]["isRequired"];
                            byCustomer.drop_off = off;

                            upTime.is_require = (bool)objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedByCustomer"]["pickUpLocation"]["options"]["pickUpTime"]["isRequired"];
                            upTime.time_list = objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedByCustomer"]["pickUpLocation"]["options"]["pickUpTime"]["times"].ToObject<List<Time>>();

                            cusTime.is_allow = (bool)objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedByCustomer"]["pickUpLocation"]["options"]["pickUpTime"]["customTime"]["isAllowCustom"];
                            string hour_from = (string)objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedByCustomer"]["pickUpLocation"]["options"]["pickUpTime"]["customTime"]["timeRange"]["from"]["hour"];
                            string min_from = (string)objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedByCustomer"]["pickUpLocation"]["options"]["pickUpTime"]["customTime"]["timeRange"]["from"]["minute"];
                            string hour_to = (string)objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedByCustomer"]["pickUpLocation"]["options"]["pickUpTime"]["customTime"]["timeRange"]["to"]["hour"];
                            string min_to = (string)objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedByCustomer"]["pickUpLocation"]["options"]["pickUpTime"]["customTime"]["timeRange"]["to"]["minute"];
                            cusTime.time_range_start = !cusTime.is_allow ? null : $"{hour_from}:{min_from}";
                            cusTime.time_range_end = !cusTime.is_allow ? null : $"{hour_to}:{min_to}";

                            upTime.custom = cusTime;

                            up.is_require_Location = (bool)objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedByCustomer"]["pickUpLocation"]["isRequired"];
                            up.time = upTime;
                            byCustomer.pick_up = up;

                            venue.designated_by_customer = byCustomer;
                            module.module_venue_info = venue;
                        } else {
                            module.module_type.Remove("PMDL_VENUE");
                        }

                    }

                    //憑證領取地點
                    if (module.module_type.Where(x => x.Contains("PMDL_EXCHANGE")).Count() > 0) {
                        var objPmdlExchange = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_EXCHANGE");
                        if ((string)objPmdlExchange["moduleSetting"]["setting"]["exchangeType"] == "05") {
                            List<Location> locations = ((JArray)objPmdlExchange["moduleSetting"]["setting"]["dataItems"]["locations"])
                                .Select(x => new Location {
                                    id = (string)x["id"],
                                    name = (string)x["name"]
                                }).ToList();

                            module.module_exchange_location_list = locations;
                        } else {
                            module.module_type.Remove("PMDL_EXCHANGE");
                        }
                    }

                    //導覽語系
                    if (obj["content"]["product"]["guideLang"] != null && (string)obj["content"]["product"]["guideLang"] != "") {
                        List<GuideLanguage> langList = new List<GuideLanguage>();
                        GuideLanguage lang = null;

                        module.module_type.Add("GUIDE_LANGUAGE");

                        string[] guide_langs = ((string)obj["content"]["product"]["guideLang"]).Split(',');
                        foreach (string guide_lang in guide_langs) {
                            lang = new GuideLanguage();
                            lang.lang_code = guide_lang;
                            lang.lang_name = (string)obj["content"]["product"]["guideLangMap"][guide_lang];
                            langList.Add(lang);

                        }
                        module.module_guide_lang_list = langList;
                    }

                    //場次
                    if (objEvent["content"]["eventData"] != null) {
                        module.module_type.Add("PACKAGE_EVENT_BACKUP");
                        List<Event> et = new List<Event>();

                        module.module_event_list = ((JArray)objEvent["content"]["eventData"][0]["events"])
                                .Select(x => new Event {
                                    day = (string)x["day"],
                                    event_times = (string)x["eventTimes"]

                                }).ToList();
                    }

                    //WMS旅規 塞入redis
                    string module_data = JsonConvert.SerializeObject(module);
                    _redisCache.SetRedis(module_data, $"b2d:Product:module:{queryRQ.prod_no}_{queryRQ.pkg_no}", 180);

                } else {
                    module = JsonConvert.DeserializeObject<ProductModuleModel>(_moduleRedis);
                }

            } catch (Exception ex) {
                module.result = "10001";
                module.result_msg = $"Module ERROR :{ex.Message},{ex.StackTrace}";
                module.module_type = null;
                Website.Instance.logger.FatalFormat($"Module ERROR :{ex.Message},{ex.StackTrace}");

            }

            return module;
        }

        //提醒事項的挖字處理
        public List<Remind> setRemInf(JObject obj, string locale_lan, Dictionary<string, string> uikey) {
            //注意事項  diseaseRemind   php:gen_prod_tips_html
            List<Remind> remList = new List<Remind>();
            Remind rem = null;
            uikey = CommonRepository.getKlingon(_redisCache, "frontend", locale_lan);

            //票券商品提示M05
            if (obj["content"]["product"]["mainCat"].ToString().Equals("M05")) //如果是票要設order_show_tkt_1 ~5
            {
                rem = new Remind();
                var expTp = obj["content"]["tkExpSetting"]["expTp"].ToString();
                var expNum = obj["content"]["tkExpSetting"]["expNum"].ToString();
                var expSt = obj["content"]["tkExpSetting"]["expSt"].ToString();
                var expEd = obj["content"]["tkExpSetting"]["expEd"].ToString();

                //"product_index_tkt_1": "指定效期區間 %s ~ %s ，逾期失效。",
                //"product_index_tkt_2": "自開票日算起%s日之內有效，逾期失效。",
                //"product_index_tkt_3": "自開票日算起%s年之內有效，逾期失效。",
                //"product_index_tkt_4": "自開票日算起%s月之內有效，逾期失效。",
                //"product_index_tkt_5": "需要按照預訂日期及當天開放時間內使用，逾期失效。",

                if (expTp == "1") {
                    string show = uikey["product_index_tkt_1"].ToString();
                    rem.remind_desc = show.Replace("%s ~", expSt).Replace("%s", expEd);
                } else if (expTp == "5") {
                    rem.remind_desc = uikey["product_index_tkt_5"].ToString(); ;
                } else {
                    string show = uikey[$"product_index_tkt_{expTp}"].ToString(); ;
                    rem.remind_desc = show.Replace("%s", expNum);
                }

                remList.Add(rem);
            }
            //一般提醒 共用資料pubRemind
            if (obj["content"]["pubRemind"].ToString() != "") {
                if (obj["content"]["pubRemind"]["A"]["checked"].ToString() == "Y") {
                    string show = uikey["common_pub_remind_1"].ToString(); // "當參加人數未達最少出團人數之%s人時，將於使用日前%s天發出取消旅遊的email通知。";
                    int vLength = show.IndexOf("%s");
                    show = show.Substring(0, vLength) + obj["content"]["pubRemind"]["A"]["value"] + show.Substring(vLength + 2, show.Length - vLength - 2);
                    show = show.Replace("%s", obj["content"]["pubRemind"]["B"]["value"].ToString());
                    rem = new Remind();
                    rem.remind_desc = show;
                    remList.Add(rem);
                }

                if (obj["content"]["pubRemind"]["C"].ToString() == "Y") {
                    rem = new Remind();
                    rem.remind_desc = uikey["common_pub_remind_2"].ToString();
                    remList.Add(rem);
                }
                if (obj["content"]["pubRemind"]["D"]["checked"].ToString() == "Y") {
                    string show = uikey["common_pub_remind_3"].ToString(); // "當參加人數未達最少出團人數之%s人時，將於使用日前%s天發出取消旅遊的email通知。";
                    int vLength = show.IndexOf("%s");
                    show = show.Substring(0, vLength) + obj["content"]["pubRemind"]["D"]["value"] + show.Substring(vLength + 2, show.Length - vLength - 2);
                    show = show.Replace("%s", obj["content"]["pubRemind"]["D"]["value2"].ToString());
                    rem = new Remind();
                    rem.remind_desc = show;
                    remList.Add(rem);
                }
            }
            //疾病提醒
            if (obj["content"]["product"]["diseaseRemind"].ToString() != "") {
                string[] disRem = obj["content"]["product"]["diseaseRemind"].ToString().Split(',');
                string show = "";
                foreach (string s in disRem) {
                    rem = new Remind();
                    show = show + uikey["common_disease_" + s].ToString() + ",";
                }

                show = uikey["common_disease_remind_desc"] + show.Substring(0, show.Length - 1);

                rem = new Remind();
                rem.remind_desc = show;
                remList.Add(rem);
            }
            //溫馨提醒 
            if (obj["content"]["remindList"].ToString() != "") {
                JArray items = (JArray)obj["content"]["remindList"];
                int remindList = items.Count;
                for (int i = 0; i < remindList; i++) {
                    rem = new Remind();
                    rem.remind_desc = items[i]["remind"]["desc"].ToString();
                    remList.Add(rem);
                }
            }

            return remList;
        }

        //預設標題挖字處理
        public static ProdTitleModel getProdTitle(Dictionary<string, string> uiKey) {
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


        #region --套餐--
        static string _SEARCH_TYPE_PKG = "PACKAGE";

        /// <summary>
        /// Gets the package lst.
        /// </summary>
        /// <returns>The package lst.</returns>
        /// <param name="rq">Rq.</param>
        //取得套餐列表
        public PackageModel GetPkgLst(QueryProductModel rq) {

            PackageModel pkg = new PackageModel();
            List<PkgDetailModel> pkgLst = new List<PkgDetailModel>();

            DataModel.Discount.DiscountRuleModel disc = null;
            PkgPriceModel pkg_price = new PkgPriceModel();
            DiscountRepository dis = new DiscountRepository(_redisCache);
            try {

                //商品黑名單過濾
                //抓商品是否為黑名單
                bool isBlack = dis.GetProdBlackWhite(rq.prod_no);

                if (isBlack) {
                    pkg.result = "10";
                    pkg.result_msg = $"Bad Request:Product-{rq.prod_no} is not available";
                    return pkg;
                }


                JObject obj = PackageProxy.getPkgLst(rq);
                JObject objProd = ProdProxy.getProd(rq);

                if (obj["content"]["result"].ToString() != "0000") {
                    pkg.result = obj["content"]["result"].ToString();
                    pkg.result_msg = $"kkday package api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception($"kkday package api response msg is not correct! {obj["content"]["msg"].ToString()}");
                }

                if (objProd["content"]["result"].ToString() != "0000") {
                    pkg.result = obj["content"]["result"].ToString();
                    pkg.result_msg = $"kkday product api response msg is not correct! {objProd["content"]["msg"].ToString()}";
                    throw new Exception($"kkday product api response msg is not correct! {objProd["content"]["msg"].ToString()}");
                }

                #region --1.取回傳資料是否成功的訊息、一般資訊--

                pkg.result = obj["content"]["result"].ToString();
                pkg.result_msg = obj["content"]["msg"].ToString();
                pkg.cost_calc_type = obj["content"]["costCalcMethod"].ToString();

                #endregion

                #region --2.從傑森物件取『套餐列表』--
                JArray jPkglst = (JArray)obj["content"]["packageList"];

                pkg_price.pkgs = new List<pkgs>(); // 初始化陣列
                pkg_price.currency = rq.current_currency;

                for (int i = 0; i < jPkglst.Count; i++) {

                    var model = new PkgDetailModel();
                    var price_model = new pkgs(); // pkg_price 用

                    model.pkg_no = jPkglst[i]["productPkg"]["pkgOid"].ToString();
                    model.pkg_name = jPkglst[i]["productPkg"]["pkgName"].ToString();
                    model.online_s_date = jPkglst[i]["productPkg"]["begValidDt"].ToString();
                    model.online_e_date = jPkglst[i]["productPkg"]["endValidDt"].ToString();
                    model.weekDays = jPkglst[i]["productPkg"]["weekDays"].ToString();

                    model.is_unit_pirce = jPkglst[i]["productPkg"]["priceType"].ToString();

                    model.price1 = dis.GetCompanyDiscPrice(Int64.Parse(rq.company_xid), rq.current_currency, (double)jPkglst[i]["productPkg"]["price1"], rq.prod_no, objProd["content"]["product"]["mainCat"].ToString(), _SEARCH_TYPE_PKG, $"{jPkglst[i]["productPkg"]["pkgOid"].ToString()}_price1", ref disc);//分銷價
                    model.price1_org = (double?)jPkglst[i]["productPkg"]["price1Org"] ?? 0;
                    model.price1_org_net = (double?)jPkglst[i]["productPkg"]["price1NetOrg"] ?? 0;
                    model.price1_profit_rate = (double?)jPkglst[i]["productPkg"]["price1GrossRate"] ?? 0;
                    model.price1_comm_rate = (double?)jPkglst[i]["productPkg"]["price1CommRate"] ?? 0;
                    model.price1_age_range = jPkglst[i]["productPkg"]["price1BegOld"].ToString() + "~" +
                                             jPkglst[i]["productPkg"]["price1EndOld"].ToString();
                    model.price1_b2c = (double?)jPkglst[i]["productPkg"]["price1Sale"] ?? 0;
                    // model.price1_net = (double)jPkglst[i]["productPkg"][""];
                    //  model.price1_list = (double)jPkglst[i]["productPkg"][""];

                    model.price2 = (double?)jPkglst[i]["productPkg"]["price2"] == null ? 0 : dis.GetCompanyDiscPrice(Int64.Parse(rq.company_xid), rq.current_currency, (double)jPkglst[i]["productPkg"]["price2"], rq.prod_no, objProd["content"]["product"]["mainCat"].ToString(), _SEARCH_TYPE_PKG, $"{jPkglst[i]["productPkg"]["pkgOid"].ToString()}_price2", ref disc);//分銷價
                    model.price2_org = (double?)jPkglst[i]["productPkg"]["price2Org"] ?? 0;
                    model.price2_org_net = (double?)jPkglst[i]["productPkg"]["price2NetOrg"] ?? 0;
                    model.price2_profit_rate = (double?)jPkglst[i]["productPkg"]["price2GrossRate"] ?? 0;
                    model.price2_comm_rate = (double?)jPkglst[i]["productPkg"]["price2CommRate"] ?? 0;
                    model.price2_age_range = jPkglst[i]["productPkg"]["price2BegOld"].ToString() + "~" +
                                             jPkglst[i]["productPkg"]["price2EndOld"].ToString();
                    model.price2_b2c = (double?)jPkglst[i]["productPkg"]["price2Sale"] ?? 0;
                    // model.price2_net = (double)jPkglst[i]["productPkg"][""];
                    //  model.price2_list = (double)jPkglst[i]["productPkg"][""];

                    model.price3 = (double?)jPkglst[i]["productPkg"]["price3"] == null ? 0 : dis.GetCompanyDiscPrice(Int64.Parse(rq.company_xid), rq.current_currency, (double)jPkglst[i]["productPkg"]["price3"], rq.prod_no, objProd["content"]["product"]["mainCat"].ToString(), _SEARCH_TYPE_PKG, $"{jPkglst[i]["productPkg"]["pkgOid"].ToString()}_price3", ref disc);//分銷價
                    model.price3_org = (double?)jPkglst[i]["productPkg"]["price3Org"] ?? 0;
                    model.price3_org_net = (double?)jPkglst[i]["productPkg"]["price3NetOrg"] ?? 0;
                    model.price3_profit_rate = (double?)jPkglst[i]["productPkg"]["price3GrossRate"] ?? 0;
                    model.price3_comm_rate = (double?)jPkglst[i]["productPkg"]["price3CommRate"] ?? 0;
                    model.price3_age_range = jPkglst[i]["productPkg"]["price3BegOld"].ToString() + "~" +
                                             jPkglst[i]["productPkg"]["price3EndOld"].ToString();
                    model.price3_b2c = (double?)jPkglst[i]["productPkg"]["price3Sale"] ?? 0;
                    // model.price3_net = (double)jPkglst[i]["productPkg"][""];
                    //  model.price3_list = (double)jPkglst[i]["productPkg"][""];

                    model.price4 = (double?)jPkglst[i]["productPkg"]["price4"] == null ? 0 : dis.GetCompanyDiscPrice(Int64.Parse(rq.company_xid), rq.current_currency, (double)jPkglst[i]["productPkg"]["price4"], rq.prod_no, objProd["content"]["product"]["mainCat"].ToString(), _SEARCH_TYPE_PKG, $"{jPkglst[i]["productPkg"]["pkgOid"].ToString()}_price4", ref disc);//分銷價
                    model.price4_org = (double?)jPkglst[i]["productPkg"]["price4Org"] ?? 0;
                    model.price4_org_net = (double?)jPkglst[i]["productPkg"]["price4NetOrg"] ?? 0;
                    model.price4_profit_rate = (double?)jPkglst[i]["productPkg"]["price4GrossRate"] ?? 0;
                    model.price4_comm_rate = (double?)jPkglst[i]["productPkg"]["price4CommRate"] ?? 0;
                    model.price4_age_range = jPkglst[i]["productPkg"]["price4BegOld"].ToString() + "~" +
                                             jPkglst[i]["productPkg"]["price4EndOld"].ToString();
                    model.price4_b2c = (double?)jPkglst[i]["productPkg"]["price4Sale"] ?? 0;
                    // model.price4_net = (double)jPkglst[i]["productPkg"][""];
                    //  model.price4_list = (double)jPkglst[i]["productPkg"][""];

                    model.status = jPkglst[i]["productPkg"]["status"].ToString();
                    model.norank_min_book_qty = (int)jPkglst[i]["productPkg"]["minOrderNum"];
                    model.norank_max_book_qty = (int)jPkglst[i]["productPkg"]["maxOrderNum"];
                    model.rank_min_book_qty = (int)jPkglst[i]["productPkg"]["minOrderQty"];
                    model.min_overage_qty = (int)jPkglst[i]["productPkg"]["minOrderAdultQty"];

                    model.isMultiple = jPkglst[i]["productPkg"]["isMultiple"].ToString();
                    model.book_qty = jPkglst[i]["productPkg"]["orderQty"].ToString();
                    model.unit = jPkglst[i]["productPkg"]["unit"].ToString();

                    model.unit_txt = jPkglst[i]["productPkg"]["unitTxt"].ToString();
                    model.unit_qty = (int)jPkglst[i]["productPkg"]["unitQty"];
                    model.pickupTp = jPkglst[i]["productPkg"]["pickupTp"].ToString();
                    model.pickupTpTxt = jPkglst[i]["productPkg"]["pickupTpTxt"].ToString();
                    model.is_hl = jPkglst[i]["productPkg"]["isBackUp"].ToString();
                    model.is_event = jPkglst[i]["productPkg"]["hasEvent"].ToString();

                    var d = jPkglst[i]["productPkg"]["pkgDesc"];
                    if (d.FirstOrDefault() != null) {
                        //取各套餐內的各個敘述
                        List<DescItem> desc = (d["descItems"][0]["content"])
                            .Select(x => new DescItem {

                                id = (string)x["id"],
                                desc = (string)x["desc"]

                            }).ToList();
                        model.desc_items = desc;
                    }

                    //組moduleSetting
                    var moduleSet = jPkglst[i]["productPkg"]["moduleSetting"];

                    if (moduleSet.FirstOrDefault() != null) {

                        FlightInfoType fit = new FlightInfoType() {
                            value =
                                moduleSet["flightInfoType"]["value"].ToString()

                        };

                        SendInfoType sit = new SendInfoType() {
                            value =
                                moduleSet["sendInfoType"]["value"].ToString(),
                            country_code =
                                moduleSet["sendInfoType"]["countryCode"].ToString()

                        };

                        VoucherValidInfo vi = new VoucherValidInfo();

                        if (moduleSet["voucherValidInfo"] != null && moduleSet["voucherValidInfo"].Any()) {

                            vi.valid_period_type =
                                  moduleSet["voucherValidInfo"]["validPeriodType"].ToString();

                            vi.before_specific_date =
                                  moduleSet["voucherValidInfo"]["beforeSpecificDate"].ToString();

                            if (moduleSet["voucherValidInfo"]["afterOrderDate"] != null && moduleSet["voucherValidInfo"]["afterOrderDate"].Any()) {
                                AfterOrderDate aod = new AfterOrderDate() {
                                    qty = (int?)moduleSet["voucherValidInfo"]["afterOrderDate"]["qty"],
                                    unit = moduleSet["voucherValidInfo"]["afterOrderDate"]["unit"].ToString()
                                };

                                vi.after_order_date = aod;
                            }
                        }

                        ModuleSetting ms = new ModuleSetting() {
                            flight_info_type = fit,
                            send_info_type = sit,
                            voucher_valid_info = vi
                        };

                        model.module_setting = ms;

                    }

                    pkgLst.Add(model);


                    price_model.pkg_no = model.pkg_no;
                    price_model.price1 = model.price1;
                    price_model.price1_b2c = model.price1_b2c;
                    price_model.price2 = model.price2;
                    price_model.price2_b2c = model.price2_b2c;
                    price_model.price3 = model.price3;
                    price_model.price3_b2c = model.price3_b2c;
                    price_model.price4 = model.price4;
                    price_model.price4_b2c = model.price4_b2c;
                    pkg_price.pkgs.Add(price_model);

                }

                pkg.pkgs = pkgLst;
                pkg.discount_rule = disc;
                pkg.guid = Guid.NewGuid().ToString();

                pkg_price.discount_rule = disc;

                _redisCache.SetRedis(JsonConvert.SerializeObject(pkg_price), "b2d:pkgsPrice:" + pkg.guid, 1440); // 將 pkg_price 存入redis 


                //依套餐取回『可售日期』
                pkg.sale_dates = (PkgSaleDateModel)GetPkgSaleDate(rq); ;

                #endregion


            } catch (Exception ex) {

                pkg.result = "10001";
                pkg.result_msg = $"Package ERROR :{ex.Message},{ex.StackTrace}";
                Website.Instance.logger.FatalFormat($"Package ERROR :{ex.Message},{ex.StackTrace}");
            }

            return pkg;
        }
        /// <summary>
        /// Gets the package sale date.
        /// </summary>
        /// <returns>The package sale date.</returns>
        /// <param name="rq">Rq.</param>
        //取得套餐可售日期
        public static PkgSaleDateModel GetPkgSaleDate(QueryProductModel rq) {

            PkgSaleDateModel pkgSdt = new PkgSaleDateModel();
            List<SaleDt> dt = new List<SaleDt>();

            try {

                JObject obj = PackageProxy.getSaleDate(rq);

                if (obj["content"]["result"].ToString() != "0000") {
                    pkgSdt.result = obj["content"]["result"].ToString();
                    pkgSdt.result_msg = $"kkday saleDate api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception($"kkday saleDate api response msg is not correct! {obj["content"]["msg"].ToString()}");
                }

                #region --1.取回傳資料是否成功的訊息--

                pkgSdt.result = obj["content"]["result"].ToString();
                pkgSdt.result_msg = obj["content"]["msg"].ToString();

                #endregion

                #region --2.從傑森物件取『套餐可售日期列表』--

                if (pkgSdt.result.ToString() == "0000") {

                    JArray jDt = (JArray)obj["content"]["saleDt"];

                    for (int i = 0; i < jDt.Count; i++) {



                        var model = new SaleDt();

                        model.pkg_no = jDt[i]["pkgOidObj"].ToString();
                        model.sale_day = jDt[i]["day"].ToString();
                        dt.Add(model);

                    }

                    pkgSdt.saleDt = dt;
                }


                #endregion
            } catch (Exception ex) {
                pkgSdt.result = "10001";
                pkgSdt.result_msg = $"SaleDate ERROR :{ex.Message},{ex.StackTrace}";
                Website.Instance.logger.FatalFormat($"SaleDate ERROR :{ex.Message},{ex.StackTrace}");

            }

            return pkgSdt;
        }

        /// <summary>
        /// Gets the package events.
        /// </summary>
        /// <returns>The package events.</returns>
        /// <param name="rq">Rq.</param>
        //取得套餐場次
        public PkgEventsModel GetPkgEvents(QueryProductModel rq) {

            PkgEventsModel pkgEvnt = new PkgEventsModel();
            List<Event> et = new List<Event>();

            try {

                JObject obj = PackageProxy.getEvents(rq);

                if (obj["content"]["result"].ToString() != "0000") {
                    pkgEvnt.result = obj["content"]["result"].ToString();
                    pkgEvnt.result_msg = $"kkday event api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception($"kkday event api response msg is not correct! {obj["content"]["msg"].ToString()}");
                }

                #region --1.取回傳資料是否成功的訊息--

                pkgEvnt.result = obj["content"]["result"].ToString();
                pkgEvnt.result_msg = obj["content"]["msg"].ToString();
                pkgEvnt.pkg_no = (int)obj["content"]["eventData"][0]["pkgOid"];
                pkgEvnt.is_hl = obj["content"]["eventData"][0]["isBackup"].ToString();

                #endregion

                #region --2.從傑森物件取『套餐場次列表』--
                JArray jEt = (JArray)obj["content"]["eventData"][0]["events"];

                for (int i = 0; i < jEt.Count; i++) {

                    var model = new Event();

                    model.day = jEt[i]["day"].ToString();
                    model.event_times = jEt[i]["eventTimes"].ToString();
                    et.Add(model);

                }

                pkgEvnt.events = et;

                #endregion
            } catch (Exception ex) {

                pkgEvnt.result = "10001";
                pkgEvnt.result_msg = $"Events ERROR :{ex.Message},{ex.StackTrace}";
                Website.Instance.logger.FatalFormat($"Events ERROR:{ex.Message},{ex.StackTrace}");
            }

            return pkgEvnt;


        }
        #endregion
    }
}
