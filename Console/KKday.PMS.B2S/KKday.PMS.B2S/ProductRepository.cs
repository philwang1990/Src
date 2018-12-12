using System;
using log4net;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.Models.Product;
using KKday.PMS.B2S.Models.Shared;
using KKday.PMS.B2S.Models.Shared.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KKday.PMS.B2S.ProductRepository
{
    public class ProductRepository
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(ProductRepository));

        public SupplierLoginRSModel setParameters(PMSSourse pms, string supplierName, string kkday_supplier_oid, string email, string password)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                SupplierLoginRQModel supplierLoginRQModel = new SupplierLoginRQModel();
                SupplierLoginRSModel supplierLoginRSModel = new SupplierLoginRSModel();

                supplierLoginRQModel.json = new SupplierLoginJson();
                supplierLoginRQModel.json.email = email;
                supplierLoginRQModel.json.password = password;
                supplierLoginRQModel.json.deviceId = System.Guid.NewGuid().ToString();
                supplierLoginRQModel.json.code = "";

                JObject supplierLogin = CommonTool.GetDataPost(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_supplierlogin), JsonConvert.SerializeObject(supplierLoginRQModel));

                if (supplierLogin["content"]["result"].ToString() != "0000")
                {
                    supplierLoginRSModel.result = supplierLogin["content"]["result"].ToString();
                    supplierLoginRSModel.msg = supplierLogin["content"]["msg"].ToString();
                    return supplierLoginRSModel;
                }

                supplierLoginRSModel.result = supplierLogin["content"]["result"].ToString();
                supplierLoginRSModel.msg = supplierLogin["content"]["msg"].ToString();
                supplierLoginRSModel.email = email;
                supplierLoginRSModel.password = password;
                supplierLoginRSModel.supplierUserUuid = new Guid(supplierLogin["content"]["supplierUserUuid"].ToString());
                supplierLoginRSModel.deviceId = supplierLoginRQModel.json.deviceId;
                supplierLoginRSModel.tokenKey = supplierLogin["content"]["tokenKey"].ToString();
                foreach (var i in supplierLogin["content"]["supplierList"])
                {
                    if (i["supplier"]["supplierOid"].ToString() == kkday_supplier_oid)
                    {
                        supplierLoginRSModel.supplierOid = (long)i["supplier"]["supplierOid"];
                        break;
                    }
                }



                return supplierLoginRSModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel getProductList(PMSSourse pms, ref RezdyProductListModel rezdyProductListModel, string supplierId, int offset)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();
                var get = CommonTool.GetData(string.Format(Startup.Instance.GetParameter(pms, ParameterType.Product), Startup.Instance.GetParameter(pms, ParameterType.ApiKey), supplierId) + "&limit=1&offset="+ offset);

                //RezdyProductListModel

                rezdyProductListModel = JsonConvert.DeserializeObject<RezdyProductListModel>(get);

                if (rezdyProductListModel.RequestStatus.success == false)
                {
                    rsModel.result = "0001";
                    rsModel.msg = "沒找到商品";

                    return rsModel;
                }

                rsModel.result = "0000";
                rsModel.msg = "正確";

                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel getProduct(PMSSourse pms, ref RezdyProductModel rezdyProductModel, string productCode)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();
                var get = CommonTool.GetData(string.Format(Startup.Instance.GetParameter(pms, ParameterType.ProductSearch), productCode, Startup.Instance.GetParameter(pms, ParameterType.ApiKey)));
                //RezdyProductModel

                rezdyProductModel = JsonConvert.DeserializeObject<RezdyProductModel>(get);

                if (rezdyProductModel.RequestStatus.success == false)
                {
                    rsModel.result = "0001";
                    rsModel.msg = "沒找到商品";

                    return rsModel;
                }

                rsModel.result = "0000";
                rsModel.msg = "正確";

                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }

        }

        public RSModel createProduct(SupplierLoginRSModel supplierLoginRSModel, ref long prodOid, RezdyProductModel rezdyProductModel)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();

                SCMProductModel scmModel = new SCMProductModel();
                scmModel.json = new ScmProductJson();
                scmModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                scmModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                scmModel.json.deviceId = supplierLoginRSModel.deviceId;
                scmModel.json.tokenKey = supplierLoginRSModel.tokenKey;

                scmModel.json.productName = rezdyProductModel.Product.name;
                scmModel.json.masterLang = scmModel.Locale;
                if (rezdyProductModel.Product.productType == "DAYTOUR")
                    scmModel.json.mainCat = "M01";
                else if (rezdyProductModel.Product.productType == "MULTIDAYTOUR")
                    scmModel.json.mainCat = "M02";
                else
                {
                    rsModel.result = "0001";
                    rsModel.msg = "此商品為:" + rezdyProductModel.Product.productType + " ,非DAYTOUR 也非MULTIDAYTOUR";
                    return rsModel;
                }


                JObject productNew = CommonTool.GetDataPost(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_productnew), JsonConvert.SerializeObject(scmModel));

                if (productNew["content"]["result"].ToString() != "0000")
                {
                    rsModel.result = productNew["content"]["result"].ToString();
                    rsModel.msg = productNew["content"]["msg"].ToString();
                    return rsModel;
                }

                prodOid = (long)productNew["content"]["product"]["prodOid"];
                rsModel.result = productNew["content"]["result"].ToString();
                rsModel.msg = productNew["content"]["msg"].ToString();

                return rsModel;


            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel setScmProduct(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel)
        {

            try
            {
                RSModel rsModel = new RSModel();
                string timezoneString = null;
                SCMProductModel scmModel = new SCMProductModel();
                scmModel.json = new ScmProductJson();
                scmModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                scmModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                scmModel.json.deviceId = supplierLoginRSModel.deviceId;
                scmModel.json.tokenKey = supplierLoginRSModel.tokenKey;

                setStep1(supplierLoginRSModel, prodOid, rezdyProductModel, scmModel, ref timezoneString); // 基本設定
                setStep2(supplierLoginRSModel, prodOid, rezdyProductModel, scmModel); // 商品分類
                setStep3(supplierLoginRSModel, prodOid, rezdyProductModel, scmModel, timezoneString); // 上架時間
                setStep4(supplierLoginRSModel, prodOid, rezdyProductModel, scmModel); // 憑證設定
                setStep5(supplierLoginRSModel, prodOid, rezdyProductModel, scmModel); // 行程說明
                //setStep6(supplierLoginRSModel, prodOid, rezdyProductModel, scmModel); // 照片及影片
                //setStep7(supplierLoginRSModel, prodOid, rezdyProductModel, scmModel); // 行程表
                //setStep8(supplierLoginRSModel, prodOid, rezdyProductModel, scmModel); // 集合地點
                //setStep9(supplierLoginRSModel, prodOid, rezdyProductModel, scmModel); // 費用包含細節
                //setStep10(supplierLoginRSModel, prodOid, rezdyProductModel, scmModel); // 兌換方式

                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel setStep1(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel, SCMProductModel scmModel, ref string timezoneString)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();
                JObject scmRSModel;
                JObject countryList;
                JObject cityList;
                JObject countryModifyRSModel;
                JObject setCostMethodRSModel;
                JObject timezone;
                AreaRQModel areaModel;
                CountryModifyRQModel countryModifyRQModelModel;
                SetCostMethodRQModel setCostMethodRQModel;

                areaModel = new AreaRQModel();
                areaModel.json = new AreaJson();
                areaModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                areaModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                areaModel.json.deviceId = supplierLoginRSModel.deviceId;
                areaModel.json.tokenKey = supplierLoginRSModel.tokenKey;

                countryModifyRQModelModel = new CountryModifyRQModel();
                countryModifyRQModelModel.json = new CountryModifyJson();
                countryModifyRQModelModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                countryModifyRQModelModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                countryModifyRQModelModel.json.deviceId = supplierLoginRSModel.deviceId;
                countryModifyRQModelModel.json.tokenKey = supplierLoginRSModel.tokenKey;

                scmModel.Currency = rezdyProductModel.Product.currency;
                scmModel.json.productName = rezdyProductModel.Product.name; // 商品名稱

                string country = null;
                string city = null;
                if (rezdyProductModel.Product.timezone != null)
                {
                    foreach (var i in rezdyProductModel.Product.timezone.Split('/'))
                    {
                        if (country == null)
                            country = i;
                        else
                            city = i;
                    }

                }


                foreach (var i in new string[] { "A01", "A03", "A04", "A04", "A05", "A06", "A07", "A08", "A09" })
                {
                    areaModel.json.parentAreaCd = i;
                    countryList = CommonTool.GetDataPost(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_area), JsonConvert.SerializeObject(areaModel));
                    foreach (var j in countryList["content"]["areaList"])
                    {
                        if (country == j["areaShortName"].ToString())
                        {
                            areaModel.json.parentAreaCd = j["areaCd"].ToString();
                            cityList = CommonTool.GetDataPost(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_area), JsonConvert.SerializeObject(areaModel));
                            foreach (var k in cityList["content"]["areaList"])
                            {
                                if (city == k["areaShortName"].ToString())
                                {
                                    countryModifyRQModelModel.json.opType = "UPDATE";
                                    countryModifyRQModelModel.json.cityCd = k["areaCd"].ToString(); // 區域（城巿）
                                    countryModifyRSModel = CommonTool.GetDataPost(string.Format(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_countrymodify), prodOid), JsonConvert.SerializeObject(countryModifyRQModelModel));
                                    if (countryModifyRSModel["content"]["result"].ToString() != "0000")
                                    {
                                        rsModel.result = countryModifyRSModel["content"]["result"].ToString();
                                        rsModel.msg = countryModifyRSModel["content"]["msg"].ToString();
                                        return rsModel;
                                    }

                                    break;

                                }
                            }

                            break;
                        }

                    }
                }


                timezone = CommonTool.GetDataPost(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_timezone), JsonConvert.SerializeObject(scmModel));
                foreach (var i in timezone["content"]["codeList"])
                {
                    if (i["code"]["dataName"].ToString().StartsWith(rezdyProductModel.Product.timezone) == true)
                    {
                        scmModel.json.timezone = i["code"]["dataCd"].ToString(); // 商品時區
                        timezoneString = scmModel.json.timezone;
                        break;
                    }
                }

                setCostMethodRQModel = new SetCostMethodRQModel();
                setCostMethodRQModel.json = new SetCostMethodJson();
                setCostMethodRQModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                setCostMethodRQModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                setCostMethodRQModel.json.deviceId = supplierLoginRSModel.deviceId;
                setCostMethodRQModel.json.tokenKey = supplierLoginRSModel.tokenKey;
                setCostMethodRQModel.json.costCalcMethod = "NET"; //成本計算方式
                setCostMethodRQModel.json.prodCurrCd = rezdyProductModel.Product.currency; //成本計算方式 幣別
                setCostMethodRSModel = CommonTool.GetDataPost(string.Format(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_setCostMethod), prodOid), JsonConvert.SerializeObject(setCostMethodRQModel));
                if (setCostMethodRSModel["content"]["result"].ToString() != "0000")
                {
                    rsModel.result = setCostMethodRSModel["content"]["result"].ToString();
                    rsModel.msg = setCostMethodRSModel["content"]["msg"].ToString();
                    return rsModel;
                }

                // 有可能是 en-au 但是scm均為en 所以要作轉換
                //for (int i = 0; i < rezdyProductModel.Product.Languages.Count; i++)
                //{
                //    if (rezdyProductModel.Product.Languages[i].StartsWith("en") == true)
                //        rezdyProductModel.Product.Languages[i] = "en";
                //}

                //scmModel.json.guideLang = rezdyProductModel.Product.Languages; // 提供解說服務
                scmModel.json.keyWord = ""; // 自訂關鍵字 用逗號分隔 共三格 11,22,33
                scmModel.json.orderEmail = supplierLoginRSModel.email; // 訂單通知 email 
                scmModel.json.supplierNote = rezdyProductModel.Product.productCode; // 備註(店家內部使用) 對應到對方的productCode

                //後面參數為model有null時 不顯示在model內
                scmRSModel = CommonTool.GetDataPost(string.Format(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_productmodify), prodOid), JsonConvert.SerializeObject(scmModel,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));
                if (scmRSModel["content"]["result"].ToString() != "0000")
                {
                    rsModel.result = scmRSModel["content"]["result"].ToString();
                    rsModel.msg = scmRSModel["content"]["msg"].ToString();
                    return rsModel;
                }

                rsModel.result = scmRSModel["content"]["result"].ToString();
                rsModel.msg = scmRSModel["content"]["msg"].ToString();


                return rsModel;

            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }

        }

        public RSModel setStep2(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel, SCMProductModel scmModel)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();
                JObject scmRSModel;

                //scmModel.json.tagCd = new List<string>();
                if (rezdyProductModel.Product.productType == "DAYTOUR")
                    scmModel.json.tagCd = new List<string>(new String[] { "TAG_4_4" }); // 商品分類 一日遊
                else if (rezdyProductModel.Product.productType == "MULTIDAYTOUR")
                    scmModel.json.tagCd = new List<string>(new String[] { "TAG_4_5" }); // 商品分類 多日遊

                //後面參數為model有null時 不顯示在model內
                scmRSModel = CommonTool.GetDataPost(string.Format(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_productmodify), prodOid), JsonConvert.SerializeObject(scmModel,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));
                if (scmRSModel["content"]["result"].ToString() != "0000")
                {
                    rsModel.result = scmRSModel["content"]["result"].ToString();
                    rsModel.msg = scmRSModel["content"]["msg"].ToString();
                    return rsModel;
                }

                rsModel.result = scmRSModel["content"]["result"].ToString();
                rsModel.msg = scmRSModel["content"]["msg"].ToString();

                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel setStep3(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel, SCMProductModel scmModel, string timezoneString)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();
                JObject scmRSModel;
                UpdateDateRQModel updateDateRQModel;
                JObject updateDateRSModel;


                scmModel.json.confirmHour = "48"; // 自訂確認時間
                //後面參數為model有null時 不顯示在model內
                scmRSModel = CommonTool.GetDataPost(string.Format(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_productmodify), prodOid), JsonConvert.SerializeObject(scmModel,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));
                if (scmRSModel["content"]["result"].ToString() != "0000")
                {
                    rsModel.result = scmRSModel["content"]["result"].ToString();
                    rsModel.msg = scmRSModel["content"]["msg"].ToString();
                    return rsModel;
                }

                updateDateRQModel = new UpdateDateRQModel();
                updateDateRQModel.json = new UpdateDateJson();
                updateDateRQModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                updateDateRQModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                updateDateRQModel.json.deviceId = supplierLoginRSModel.deviceId;
                updateDateRQModel.json.tokenKey = supplierLoginRSModel.tokenKey;
                updateDateRQModel.json.cutOfDay = 5; //訂購前置日
                updateDateRQModel.json.cutoffdayProcessTimezone = timezoneString; //每日結單時區
                updateDateRQModel.json.begSaleDt = $"{DateTime.Now.ToString("yyyyMMdd")}0000";//自動上架日期
                updateDateRQModel.json.endSaleDt = $"{DateTime.Now.AddYears(3).ToString("yyyyMMdd")}2359";//自動下架日期
                updateDateRSModel = CommonTool.GetDataPost(string.Format(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_updateDate), prodOid), JsonConvert.SerializeObject(updateDateRQModel));
                if (updateDateRSModel["content"]["result"].ToString() != "0000")
                {
                    rsModel.result = updateDateRSModel["content"]["result"].ToString();
                    rsModel.msg = updateDateRSModel["content"]["msg"].ToString();
                    return rsModel;
                }

                rsModel.result = updateDateRSModel["content"]["result"].ToString();
                rsModel.msg = updateDateRSModel["content"]["msg"].ToString();

                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel setStep4(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel, SCMProductModel scmModel)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();
                VoucherUpdateRQModel voucherUpdateRQModel;
                JObject voucherUpdateRSModel;

                voucherUpdateRQModel = new VoucherUpdateRQModel();
                voucherUpdateRQModel.json = new VoucherUpdateJson();
                voucherUpdateRQModel.json.moduleSetting = new ModuleSetting();
                voucherUpdateRQModel.json.moduleSetting.setting = new Setting();
                voucherUpdateRQModel.json.moduleSetting.setting.dataItems = new DataItems();
                voucherUpdateRQModel.json.moduleSetting.setting.dataItems.validOptions = new ValidOptions();
                voucherUpdateRQModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                voucherUpdateRQModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                voucherUpdateRQModel.json.deviceId = supplierLoginRSModel.deviceId;
                voucherUpdateRQModel.json.tokenKey = supplierLoginRSModel.tokenKey;
                voucherUpdateRQModel.json.moduleType = "PMDL_VOUCHER"; // 固定
                voucherUpdateRQModel.json.moduleSetting.isRequired = true; // 固定
                voucherUpdateRQModel.json.moduleSetting.setting.voucherType = "02"; // 憑證類型 供應商憑證

                //後面參數為model有null時 不顯示在model內
                voucherUpdateRSModel = CommonTool.GetDataPost(string.Format(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_voucherupdate),prodOid),
                                                              JsonConvert.SerializeObject(voucherUpdateRQModel));
                if (voucherUpdateRSModel["content"]["result"].ToString() != "0000")
                {
                    rsModel.result = voucherUpdateRSModel["content"]["result"].ToString();
                    rsModel.msg = voucherUpdateRSModel["content"]["msg"].ToString();
                    return rsModel;
                }

                rsModel.result = voucherUpdateRSModel["content"]["result"].ToString();
                rsModel.msg = voucherUpdateRSModel["content"]["msg"].ToString();
                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel setStep5(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel, SCMProductModel scmModel)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();
                JObject scmRSModel;

                scmModel.json.introduction = Regex.Replace(rezdyProductModel.Product.shortDescription, "<.*?>", "\n"); // 商品概述  將html tag 都取代成 換行
                scmModel.json.productDesc = Regex.Replace(rezdyProductModel.Product.description, "<.*?>", "\n"); // 詳細內容 將html tag 都取代成 換行

                //後面參數為model有null時 不顯示在model內
                scmRSModel = CommonTool.GetDataPost(string.Format(Startup.Instance.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_productmodify), prodOid), JsonConvert.SerializeObject(scmModel,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }));
                if (scmRSModel["content"]["result"].ToString() != "0000")
                {
                    rsModel.result = scmRSModel["content"]["result"].ToString();
                    rsModel.msg = scmRSModel["content"]["msg"].ToString();
                    return rsModel;
                }

                rsModel.result = scmRSModel["content"]["result"].ToString();
                rsModel.msg = scmRSModel["content"]["msg"].ToString();

                return rsModel;

            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel setStep6(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel, SCMProductModel scmModel)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();

                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel setStep7(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel, SCMProductModel scmModel)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();

                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel setStep8(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel, SCMProductModel scmModel)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();

                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel setStep9(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel, SCMProductModel scmModel)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();

                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }

        public RSModel setStep10(SupplierLoginRSModel supplierLoginRSModel, long prodOid, RezdyProductModel rezdyProductModel, SCMProductModel scmModel)
        {
            try
            {
                //Startup startup = new Startup();
                //startup.Initial();
                RSModel rsModel = new RSModel();

                return rsModel;
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }
    }
}
