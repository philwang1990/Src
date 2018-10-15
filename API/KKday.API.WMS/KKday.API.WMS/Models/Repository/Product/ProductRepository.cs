using System;
using System.Collections.Generic;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.Models.DataModel.Product;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;
using KKday.API.WMS.Models.DataModel.Package;
using KKday.Web.B2D.EC.AppCode;

namespace KKday.API.WMS.Models.Repository.Product
{
    public class ProductRepository
    {
        public ProductRepository()
        {
        }
        //code lang type
        private static string _VOUCHER_EXCHANGE_TYPE = "VOUCHER_EXCHANGE_TYPE";

        /// <summary>
        /// Gets the prod dtl.
        /// </summary>
        /// <returns>The prod dtl.</returns>
        /// <param name="queryRQ">Query rq.</param>
        public static ProductModel GetProdDtl(QueryProductModel queryRQ)
        {
            ProductModel product = new ProductModel();
            JObject obj = null, objModule = null, objExTypeLang = null;
            try
            {
                obj = ProdProxy.getProd(queryRQ);
                objModule = ProdProxy.getModule(queryRQ);
                objExTypeLang = CommonProxy.getCodeLang(queryRQ, ProductRepository._VOUCHER_EXCHANGE_TYPE);


                if (obj["content"]["result"].ToString() != "0000")
                {
                    product.reasult = obj["content"]["result"].ToString();
                    product.reasult_msg = $"kkday product api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception("kkday product api response msg is not correct!");
                }

                product.reasult = obj["content"]["result"].ToString();
                product.reasult_msg = obj["content"]["msg"].ToString();
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
                product.prod_desction = obj["content"]["product"]["productDesc"].ToString();
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
                comment.comment_record  = obj["content"]["prodUrlInfo"]["countRec"].ToString();
                comment.keyword = obj["content"]["prodUrlInfo"]["keyword"].ToString();
                comment.sales_qty = obj["content"]["prodUrlInfo"]["orderNum"].ToString();
                comment.prod_url_oid= obj["content"]["prodUrlInfo"]["prodUrlOid"].ToString();
                product.prod_comment_info = comment;

                product.b2c_price = (double)obj["content"]["product"]["minSalePrice"]; 
                product.b2d_price = (double)obj["content"]["product"]["minPrice"]; 

                product.order_email = obj["content"]["product"]["orderEmail"].ToString();

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
                if (obj["content"]["policyList"] != null)
                {
                    JArray policy_items = (JArray)obj["content"]["policyList"];

                    foreach (var item in policy_items)
                    {
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
                if (obj["content"]["scheList"] != null)
                {
                    JArray sche_items = (JArray)obj["content"]["scheList"];


                    foreach (var item in sche_items)
                    {
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
                if (obj["content"]["scheMealList"] != null)
                {
                    JArray meal_items = (JArray)obj["content"]["scheMealList"];

                    foreach (var item in meal_items)
                    {
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
                if (obj["content"]["product"]["guideLang"] != null && (string)obj["content"]["product"]["guideLang"] !="")
                {
                    //多筆
                    if (((string)obj["content"]["product"]["guideLang"]).Contains(','))
                    {
                        string[] guide_langs = ((string)obj["content"]["product"]["guideLang"]).Split(',');
                        foreach(string guide_lang in guide_langs)
                        {
                            lang = new GuideLanguage();
                            lang.lang_code = guide_lang;
                            lang.lang_name = (string)obj["content"]["product"]["guideLangMap"][guide_lang];
                            langList.Add(lang);

                        }
                    }
                    else
                    {
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
                if (obj["content"]["arrList"] != null)
                {
                    JArray items = (JArray)obj["content"]["arrList"];
                    foreach (var item in items)
                    {
                        arr = new ArrivalMapInfo();
                        arr.photo_url = (string)item["latlong"]["imgUrl"];
                        arr.photo_desc= (string)item["latlong"]["photoDesc"];
                        arr.zoom = (int)item["latlong"]["zoomLv"];
                        arr.latitude = (string)item["latlong"]["latitude"];
                        arr.longitude = (string)item["latlong"]["longitude"];
                        arr.latlong_type = (string)item["latlong"]["latlongType"];
                        arr.latlong_desc = (string)item["latlong"]["latlongDesc"];  

                        arrList.Add(arr);

                    }

                    product.arr_map_info_list = arrList;
                }

                //圖片
                List<Images> imgList = new List<Images>();
                Images img = null;
                if (obj["content"]["imgList"] != null)
                {
                    JArray items = (JArray)obj["content"]["imgList"];

                    foreach (var item in items)
                    {
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


                //注意事項挖字處理
                product.remind_list = setRemInf(obj, queryRQ.locale_lang, null);
               
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
                if (obj["content"]["videoList"] != null)
                {
                    JArray items = (JArray)obj["content"]["videoList"];

                    foreach (var item in items)
                    {
                        video = new Video();
                        video.lang_code = (string)item["video"]["langCode"];
                        video.vidoe_url = (string)item["video"]["videoUrl"];


                        videoList.Add(video);
                    }
                    product.video_list = videoList;
                }

                //費用包含與不包含
                List<CostDetail> detailList = new List<CostDetail>();
                CostDetail detail = null;
                if (obj["content"]["detailList"] != null)
                {
                    JArray items = (JArray)obj["content"]["detailList"];

                    foreach (var item in items)
                    {
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
                if (obj["content"]["meetingPointList"] != null)
                {
                    JArray items = (JArray)obj["content"]["meetingPointList"];

                    foreach (var item in items)
                    {
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

                if (obj["content"]["cityList"] != null)
                {
                    JArray items = (JArray)obj["content"]["cityList"];
                    country = new Country();
                    country.id = (string)items[0]["city"]["countryCd"];
                    country.name = (string)items[0]["city"]["countryName"];

                    foreach (var item in items)
                    {
                        city = new City();
                        city.id = (string)item["city"]["cityCd"];
                        city.name = (string)item["city"]["cityName"];
                        cityList.Add(city);
                    }
                    country.cities = cityList;
                    countryList.Add(country);
                    product.countries= countryList;
                }


                //憑證類型
                //module api找出該商品的憑證類型
                JArray modules = (JArray)objModule["content"]["product"]["modules"];
                var voucher_module = modules.FirstOrDefault(jt => (string)jt["moduleType"] == "PMDL_EXCHANGE");

                if((bool)voucher_module["moduleSetting"]["isRequired"])
                {
                    //codeLang api找出exchangeType 與langValue 對應
                    var code = objExTypeLang["content"]["codeList"].FirstOrDefault(jt => (string)jt["code"]["langValue"] == (string)voucher_module["moduleSetting"]["setting"]["exchangeType"]);
                    //找出憑證類型敘述
                    product.voucher_desc = (string)code["code"]["langDesc"];
                }else
                {
                    product.voucher_desc = "";
                }
                //找出憑證領取地點資訊(名稱,地點,營業時間)
                product.voucher_locations = voucher_module["moduleSetting"]["setting"]["dataItems"]["locations"].ToObject<List<Location>>();



            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"Product System Error :{ex.Message},{ex.StackTrace}");
            }

            return product;
        }

        /// <summary>
        /// Gets the prod module.
        /// </summary>
        /// <returns>The prod module.</returns>
        /// <param name="queryRQ">Query rq.</param>
        public static ProductModuleModel GetProdModule(QueryProductModel queryRQ)
        {
            ProductModuleModel module = new ProductModuleModel();
            JObject objModule = null, obj = null, objEvent = null, objCountryCode = null,objAreaCode = null;

            try
            {
                obj = ProdProxy.getProd(queryRQ);
                objModule = ProdProxy.getModule(queryRQ);
                objEvent = PackageProxy.getEvents(queryRQ);
                objCountryCode = CommonProxy.getCodeCountry(queryRQ);
                objAreaCode = CommonProxy.getCodeArea(queryRQ);

                if (objModule["content"]["result"].ToString() != "0000")
                {
                    module.reasult = objModule["content"]["result"].ToString();
                    module.reasult_msg = $"kkday module api response msg is not correct! {objModule["content"]["msg"].ToString()}";
                    throw new Exception($"kkday module api response msg is not correct! {objModule["content"]["msg"].ToString()}");
                }
                if (obj["content"]["result"].ToString() != "0000")
                {
                    module.reasult = objModule["content"]["result"].ToString();
                    module.reasult_msg = $"kkday product api response msg is not correct! {obj["content"]["msg"].ToString()}";
                    throw new Exception($"kkday product api response msg is not correct! {obj["content"]["msg"].ToString()}");
                }
                if (objEvent["content"]["result"].ToString() != "0000")
                {
                    module.reasult = objModule["content"]["result"].ToString();
                    module.reasult_msg = $"kkday event api response msg is not correct! {objEvent["content"]["msg"].ToString()}";
                    throw new Exception($"kkday event api response msg is not correct! {objEvent["content"]["msg"].ToString()}");
                }

                module.reasult = objModule["content"]["result"].ToString();
                module.reasult_msg = objModule["content"]["msg"].ToString();

                JArray jModules = (JArray)objModule["content"]["product"]["modules"];
                module.module_type = jModules.Where(y => (bool)y["moduleSetting"]["isRequired"] == true).Select(x => (string)x["moduleType"]).ToList<string>();


                //旅客資料
                if (module.module_type.Where(x => x.Contains("PMDL_CUST_DATA")).Count() > 0)
                {
                    var objPmdlCustData = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_CUST_DATA");

                    //挖字！！！！！！！
                    RedisHelper rds = new RedisHelper();
                    Dictionary<string, string> uikey = rds.klingonGet("frontend", queryRQ.locale_lang);

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
                    gdType.Add(new GenderType()
                    {
                        type = "F",
                        type_name = uikey["common_female"]
                    });
                    gdType.Add(new GenderType()
                    {
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
                            .Select(x => new NationInfo
                            {
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
                    h_units.Add(new Unit()
                    { 
                        unit_code = "01",
                        unit_name = uikey["booking_step1_cust_data_height_unit_01"]

                    });
                    h_units.Add(new Unit()
                    {
                        unit_code = "02",
                        unit_name = uikey["booking_step1_cust_data_height_unit_02"]
                    });
                    h.unit_list = !h.is_require ? null : h_units;
                    cus.high = h;

                    Weight w = new Weight();
                    List<Unit> w_units = new List<Unit>();
                    w.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["weight"]["isRequired"];
                    w_units.Add(new Unit()
                    {
                        unit_code = "01",
                        unit_name = uikey["booking_step1_cust_data_weight_unit_01"]
                    });
                    w_units.Add(new Unit()
                    { 
                        unit_code = "02",
                        unit_name = uikey["booking_step1_cust_data_weight_unit_02"]
                    });
                    w.unit_list = !w.is_require ? null : w_units;
                    cus.weight = w;

                    ShoeSize shoe = new ShoeSize();
                    shoe.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["isRequired"];
                    if (shoe.is_require)
                    {
                        Man man = new Man();
                        man.is_provided = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["man"]["isProvided"];
                        man.unit_list = !man.is_provided ? null : ((JArray)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["man"]["allowData"])
                            .Select(x => new Unit
                            {
                                unit_code = (string)x["unit"],
                                unit_name = (string)x["unit"] == "01" ? "US" : (string)x["unit"] == "02" ? "EU" : "JP/CM",
                                size_range_start = (string)x["min"],
                                size_range_end = (string)x["max"]
                            }).ToList();

                        Woman woman = new Woman();
                        woman.is_provided = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["woman"]["isProvided"];
                        woman.unit_list = !woman.is_provided ? null : ((JArray)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["woman"]["allowData"])
                            .Select(x => new Unit
                            {
                                unit_code = (string)x["unit"],
                                unit_name = (string)x["unit"] == "01" ? "US" : (string)x["unit"] == "02" ? "EU" : "JP/CM",
                                size_range_start = (string)x["min"],
                                size_range_end = (string)x["max"]
                            }).ToList();

                        Child child = new Child();
                        child.is_provided = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["child"]["isProvided"];
                        child.unit_list = !child.is_provided ? null : ((JArray)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["shoeSize"]["options"]["child"]["allowData"])
                            .Select(x => new Unit
                            {
                                unit_code = (string)x["unit"],
                                unit_name = (string)x["unit"] == "01" ? "US" : (string)x["unit"] == "02" ? "EU" : "JP/CM",
                                size_range_start = (string)x["min"],
                                size_range_end = (string)x["max"]
                            }).ToList();
                        shoe.man = man;
                        shoe.wonman = woman;
                        shoe.child = child;

                    }
                    cus.shoe_size = shoe;
                    Meal meal = new Meal();
                    ExcludeFood ex = new ExcludeFood();
                    AllergyFood allergy = new AllergyFood();

                    meal.is_require = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["meal"]["isRequired"];
                    meal.meal_list = !meal.is_require ? null : ((JArray)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["meal"]["options"]["meals"])
                        .Select(x => new MealType
                        {
                            is_provided = (bool)x["isProvided"],
                            meal_type = (string)x["mealType"],
                            meal_type_name = (string)x["mealTypeName"]
                        }).ToList();
                    ex.is_exclude = (bool)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["meal"]["options"]["excludeFood"]["isExcluded"];
                    ex.food_list = !ex.is_exclude ? null : ((JArray)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["meal"]["options"]["excludeFood"]["foods"])
                        .Select(x => new Food
                        {
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
                    glass.diopter_range_start = (string)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["glassDiopter"]["diopterRangeStart"];
                    glass.diopter_range_end = (string)objPmdlCustData["moduleSetting"]["setting"]["dataItems"]["glassDiopter"]["diopterRangeEnd"];
                    cus.glass_dopter = glass;

                    module.module_cust_data = cus;


                }

                //旅遊期間聯絡人
                if (module.module_type.Where(x => x.Contains("PMDL_CONTACT_DATA")).Count() > 0)
                {
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
                            .Select(x => new NationInfo
                            {
                                country_tel_code = (string)x["country"]["telArea"],
                                country_tel_info = $"{(string)x["country"]["countryEngName"]}({(string)x["country"]["countryName"]})"
                             }).ToList();

                    contact.contact_tel = tel;

                    ContactApp app = new ContactApp();
                    app.is_require = (bool)objPmdlContactData["moduleSetting"]["setting"]["dataItems"]["contactApp"]["isRequired"];
                    app.is_require_AppAccount = (bool)objPmdlContactData["moduleSetting"]["setting"]["dataItems"]["contactApp"]["isRequired"];
                    app.app_type_list = ((JArray)objPmdlContactData["moduleSetting"]["setting"]["dataItems"]["contactApp"]["apps"])
                        .Select(x => new App
                        {
                            is_supported = (bool)x["isSupported"],
                            app_type = (string)x["appType"],
                            app_name = (string)x["appTypeName"]
                        }).ToList();
                    contact.contact_app = app;
                    module.module_contact_data = contact;
                }

                //其他資料
                if (module.module_type.Where(x => x.Contains("PMDL_SIM_WIFI")).Count() > 0)
                {
                    var objPmdlSimWifi = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_SIM_WIFI");
                    SimWifi simWifi = new SimWifi();

                    simWifi.is_require = (bool)objPmdlSimWifi["moduleSetting"]["isRequired"];
                    MobileModleNumber no = new MobileModleNumber();
                    MobileIMEI imei = new MobileIMEI();
                    ActivationDate date = new ActivationDate();
                    no.is_require = (bool)objPmdlSimWifi["moduleSetting"]["setting"]["dataItems"]["mobileModelNumber"]["isRequired"];
                    imei.is_require = (bool)objPmdlSimWifi["moduleSetting"]["setting"]["dataItems"]["mobileIMEI"]["isRequired"];
                    date.is_require = (bool)objPmdlSimWifi["moduleSetting"]["setting"]["dataItems"]["activationDate"]["isRequired"];

                    simWifi.mobile_modle_no = no;
                    simWifi.mobile_IMEI = imei;
                    simWifi.activation_date = date;
                    module.module_sim_wifi = simWifi;
                }

                //乘客資料
                if (module.module_type.Where(x => x.Contains("PMDL_CAR_PSGR")).Count() > 0)
                {
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
                if (module.module_type.Where(x => x.Contains("PMDL_FLIGHT_INFO")).Count() > 0)
                {
                    var objPmdlFlightInfo = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_FLIGHT_INFO");
                    FlightInfo flight = new FlightInfo();
                    flight.is_require = (bool)objPmdlFlightInfo["moduleSetting"]["isRequired"];

                    Arrival arr = new Arrival();
                    Departure dep = new Departure();
                    arr.is_require_FlightType = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["flightType"]["isRequired"];
                    arr.is_require_Date = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["arrivalDatetime"]["isRequired"];
                    arr.is_require_Hour = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["arrivalDatetime"]["isRequired"];
                    arr.is_require_Minute = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["arrivalDatetime"]["isRequired"];
                    arr.is_require_Airport = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["airport"]["isRequired"];
                    arr.is_require_Airline = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["airline"]["isRequired"];
                    arr.is_require_FlightNo = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["flightNo"]["isRequired"];
                    arr.is_require_TerminalNo = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["terminalNo"]["isRequired"];
                    arr.is_need_ApplyVisa = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["arrival"]["isNeedToApplyVisa"]["isRequired"];

                    dep.is_require_FlightType = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["flightType"]["isRequired"];
                    dep.is_require_Date = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["departureDatetime"]["isRequired"];
                    dep.is_require_Hour = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["departureDatetime"]["isRequired"];
                    dep.is_require_Minute = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["departureDatetime"]["isRequired"];
                    dep.is_require_Airport = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["airport"]["isRequired"];
                    dep.is_require_Airline = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["airline"]["isRequired"];
                    dep.is_require_FlightNo = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["flightNo"]["isRequired"];
                    dep.is_require_TerminalNo = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["terminalNo"]["isRequired"];
                    //dep.is_require_HaveBeenInCountry = (bool)objPmdlFlightInfo["moduleSetting"]["setting"]["dataItems"]["departure"]["haveBeenInCountry"]["isRequired"];

                    flight.arrival = arr;
                    flight.departure = dep;

                    module.module_flight_info = flight;
                }

                //寄送資料
                if (module.module_type.Where(x => x.Contains("PMDL_SEND_DATA")).Count() > 0)
                {
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
                            .Select(x => new NationInfo
                            {
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
                    if (items != null)
                    {
                        //moudle來的country 資訊
                        foreach (var item in items)
                        {
                            Country country = new Country();
                            List<City> cityList = new List<City>();

                            string area_code = item["countryCode"].ToString().Split('-')[0];
                            string country_code = item["countryCode"].ToString();

                            //對應另外一隻api的area country 資訊
                            var area_items = objAreaCode["content"]["areaList"].FirstOrDefault(x => x["continent"].ToString().Contains(area_code));
                            var country_items = area_items["countrys"].FirstOrDefault(x => x["country"].ToString().Contains(country_code));

                            country.id = country_items["country"].ToString().Split("|||")[1];
                            country.name = country_items["country"].ToString().Split("|||")[0];
                            country.cities = ((JArray)country_items["citys"])
                                .Select(x => new City
                                {
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
                if (module.module_type.Where(x => x.Contains("PMDL_RENT_CAR")).Count() > 0)
                {
                    var objPmdlRentCar = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_RENT_CAR");
                    RentCar car = new RentCar();
                    car.is_require = (bool)objPmdlRentCar["moduleSetting"]["isRequired"];
                    car.rent_type = (string)objPmdlRentCar["moduleSetting"]["setting"]["rentCarType"];

                    if ((string)objPmdlRentCar["moduleSetting"]["setting"]["rentCarType"] != "03")
                    {
                        RentOffice office = new RentOffice();
                        BusinessHour bissTime = new BusinessHour();
                        office.is_ProvidedFreeGPS = (bool)objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["rentCar"]["isProvidedFreeGPS"];
                        office.is_ProvidedFreeWiFi = (bool)objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["rentCar"]["isProvidedFreeWiFi"];
                        office.is_require_PickUp = true;
                        office.is_require_DropOff = car.rent_type == "01" ? true : false;
                        office.office_list = ((JArray)objPmdlRentCar["moduleSetting"]["setting"]["dataItems"]["rentCar"]["offices"])
                            .Select(x => new Office
                            {
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
                    else
                    {
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
                if (module.module_type.Where(x => x.Contains("PMDL_VENUE")).Count() > 0)
                {
                    var objPmdlVenue = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_VENUE");
                    if((string)objPmdlVenue["moduleSetting"]["setting"]["venueType"] =="03" || (string)objPmdlVenue["moduleSetting"]["setting"]["venueType"] =="04")
                    {
                        VenueInfo venue = new VenueInfo();
                        venue.is_require = (bool)objPmdlVenue["moduleSetting"]["isRequired"];
                        venue.is_require_Date = (bool)objPmdlVenue["moduleSetting"]["isRequired"];
                        venue.venue_type = (string)objPmdlVenue["moduleSetting"]["setting"]["venueType"];
                        venue.designated_location_list = objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedLocation"]["locations"].ToObject<List<DesignatedLocation>>();

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
                        cusTime.time_range_start = $"{hour_from}:{min_from}";
                        cusTime.time_range_end = $"{hour_to}:{min_to}";

                        upTime.custom = cusTime;

                        up.is_require_Location = (bool)objPmdlVenue["moduleSetting"]["setting"]["dataItems"]["designatedByCustomer"]["pickUpLocation"]["isRequired"];
                        up.time = upTime;
                        byCustomer.pick_up = up;

                        venue.designated_by_customer = byCustomer;
                        module.module_venue_info = venue;
                    }
                    else{
                        module.module_type.Remove("PMDL_VENUE");
                    }

                }

                //憑證領取地點
                if (module.module_type.Where(x => x.Contains("PMDL_EXCHANGE")).Count() > 0)
                {
                    var objPmdlExchange = jModules.FirstOrDefault(y => (string)y["moduleType"] == "PMDL_EXCHANGE");
                    if ((string)objPmdlExchange["moduleSetting"]["setting"]["exchangeType"] == "05")
                    {

                        List<Location> locations = ((JArray)objPmdlExchange["moduleSetting"]["setting"]["dataItems"]["locations"])
                            .Select(x => new Location
                            {
                                id = (string)x["id"],
                                name = (string)x["name"]

                            }).ToList();

                        module.module_exchange_location_list = locations;
                    }
                    else
                    {
                        module.module_type.Remove("PMDL_EXCHANGE");
                    }
                }

                //導覽語系
                if (obj["content"]["product"]["guideLang"] != null && (string)obj["content"]["product"]["guideLang"] != "")
                {
                    List<GuideLanguage> langList = new List<GuideLanguage>();
                    GuideLanguage lang = null;

                    module.module_type.Add("GUIDE_LANGAGE");

                    string[] guide_langs = ((string)obj["content"]["product"]["guideLang"]).Split(',');
                    foreach (string guide_lang in guide_langs)
                    {
                        lang = new GuideLanguage();
                        lang.lang_code = guide_lang;
                        lang.lang_name = (string)obj["content"]["product"]["guideLangMap"][guide_lang];
                        langList.Add(lang);

                    }
                    module.module_guide_lang_list = langList;
                }

                //場次
                if (objEvent["content"]["eventData"] != null)
                {
                    module.module_type.Add("PACKAGE_EVENT_BACKUP");
                    List<Event> et = new List<Event>();

                    module.module_event_list = ((JArray)objEvent["content"]["eventData"][0]["events"])
                            .Select(x => new Event
                            {
                                day = (string)x["day"],
                                event_times = (string)x["eventTimes"]

                            }).ToList();
                }


            }
            catch (Exception ex)
            {
                module.reasult = "10001";
                module.reasult_msg = ex.Message;
                module.module_type = null;
                Website.Instance.logger.FatalFormat($"Module System Error :{ex.Message},{ex.StackTrace}");
            }

            return module;
        }



        //提醒事項的挖字處理
        public static List<Remind> setRemInf(JObject obj, string locale_lan, Dictionary<string, string> uikey) {
            //注意事項  diseaseRemind   php:gen_prod_tips_html
            List<Remind> remList = new List<Remind>();
            Remind rem = null;
            RedisHelper rds = new RedisHelper();
            uikey = rds.klingonGet("frontend", locale_lan);
           
            //票券商品提示M05
            if (obj["content"]["product"]["mainCat"].ToString().Equals("M05")) //如果是票要設order_show_tkt_1 ~5
            {
                rem = new Remind();
                var expTp = obj["content"]["tkExpSetting"]["expTp"].ToString();
                var expNum = obj["content"]["tkExpSetting"]["expNum"].ToString();
                var expSt = obj["content"]["tkExpSetting"]["expSt"].ToString();
                var expEd = obj["content"]["tkExpSetting"]["expEd"].ToString();
                //"order_show_tkt_1": "指定效期區間 %s ~ %s ，逾期失效。",
                //"order_show_tkt_2": "自開票日算起%s日之內有效，逾期失效。",
                //"order_show_tkt_4": "自開票日算起%s年之內有效，逾期失效。",
                //"order_show_tkt_3": "自開票日算起%s月之內有效，逾期失效。",
                //"order_show_tkt_5": "需要按照預訂日期及當天開放時間內使用，逾期失效。",

                if (expTp == "1") {
                    string show = uikey["order_show_tkt_1"].ToString();
                    rem.remind_desc = show.Replace("%s ~", expSt).Replace("%s", expEd);
                } else if (expTp == "5") {
                    rem.remind_desc = uikey["order_show_tkt_1"].ToString(); ;
                } else {
                    string show = uikey[$"order_show_tkt_{expTp}"].ToString(); ;
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

    }
}
