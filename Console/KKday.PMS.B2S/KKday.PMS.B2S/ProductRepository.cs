using System;
using log4net;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.Models.Product;
using KKday.PMS.B2S.Models.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KKday.PMS.B2S.ProductRepository
{
    public class ProductRepository
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(ProductRepository));

        public void Main()
        {
            try
            {
                //initial log4net
                CommonTool.LoadLog4netConfig();

                var get = CommonTool.GetData("https://api.rezdy.com/latest/products/marketplace?apiKey=0b3d137cc1db4108a92c309fa7d7f6da&limit=1&supplierId=21470&productCode=P0KEEN");
                //RezdyProductModel

                RezdyProductModel obj = JsonConvert.DeserializeObject<RezdyProductModel>(get);

                SCMProductModel scmModel = new SCMProductModel();
                scmModel.json.deviceId = "1";

                //do something
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
            }
        }

        public SupplierLoginRSModel setParameters(string supplierName, string email, string password)
        {
            try
            {
                SupplierLoginRQModel supplierLoginRQModel = new SupplierLoginRQModel();
                SupplierLoginRSModel supplierLoginRSModel = new SupplierLoginRSModel();

                supplierLoginRQModel.json = new SupplierLoginJson();
                supplierLoginRQModel.json.email = email;
                supplierLoginRQModel.json.password = password;
                supplierLoginRQModel.json.deviceId = System.Guid.NewGuid().ToString();
                supplierLoginRQModel.json.code = "";

                JObject supplierLogin = CommonTool.GetDataPost("https://api.sit.kkday.com/api/supplier/login", JsonConvert.SerializeObject(supplierLoginRQModel));

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
                    if (i["supplier"]["supplierName"].ToString() == supplierName)
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

        public RSModel getProduct(ref RezdyProductModel rezdyProductModel)
        {
            try
            {
                RSModel rsModel = new RSModel();
                var get = CommonTool.GetData("https://api.rezdy.com/latest/products/PSSPVU?apiKey=0b3d137cc1db4108a92c309fa7d7f6da");
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

                //initial log4net
                CommonTool.LoadLog4netConfig();
                RSModel rsModel = new RSModel();

                SCMProductModel scmModel = new SCMProductModel();
                scmModel.json = new ScmProductJson();
                scmModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                scmModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid ;
                scmModel.json.deviceId = supplierLoginRSModel.deviceId;
                scmModel.json.tokenKey = supplierLoginRSModel.tokenKey;

                scmModel.json.productName = rezdyProductModel.Product.name;
                scmModel.json.masterLang = scmModel.Locale;
                if(rezdyProductModel.Product.productType == "DAYTOUR")
                  scmModel.json.mainCat = "M01";
                else if (rezdyProductModel.Product.productType == "MULTIDAYTOUR")
                    scmModel.json.mainCat = "M02";
                else
                {
                    rsModel.result = "0001";
                    rsModel.msg = "此商品為:"+ rezdyProductModel.Product.productType + " ,非DAYTOUR 也非MULTIDAYTOUR";
                    return rsModel;
                }


                JObject productNew = CommonTool.GetDataPost("https://api.sit.kkday.com/api/product/new", JsonConvert.SerializeObject(scmModel));

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
                JObject scmRSModel;
                JObject countryList;
                JObject cityList;
                JObject countryModifyRSModel;
                JObject setCostMethodRSModel;
                SCMProductModel scmModel = new SCMProductModel();
                scmModel.json = new ScmProductJson();
                scmModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                scmModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                scmModel.json.deviceId = supplierLoginRSModel.deviceId;
                scmModel.json.tokenKey = supplierLoginRSModel.tokenKey;

                AreaRQModel areaModel = new AreaRQModel();
                areaModel.json = new AreaJson();
                areaModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                areaModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                areaModel.json.deviceId = supplierLoginRSModel.deviceId;
                areaModel.json.tokenKey = supplierLoginRSModel.tokenKey;

                CountryModifyRQModel countryModifyRQModelModel = new CountryModifyRQModel();
                countryModifyRQModelModel.json = new CountryModifyJson();
                countryModifyRQModelModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                countryModifyRQModelModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                countryModifyRQModelModel.json.deviceId = supplierLoginRSModel.deviceId;
                countryModifyRQModelModel.json.tokenKey = supplierLoginRSModel.tokenKey;

                scmModel.Currency = rezdyProductModel.Product.currency;
                //scmModel.json.productName = rezdyProductModel.Product.name; // 商品名稱

                string country = null;
                string city = null;
                if(rezdyProductModel.Product.timezone != null) 
                { 
                   foreach( var i in rezdyProductModel.Product.timezone.Split('/'))
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
                    countryList = CommonTool.GetDataPost("https://api.sit.kkday.com/api/area", JsonConvert.SerializeObject(areaModel));
                    foreach (var j in countryList["content"]["areaList"])
                    {
                        if (country == j["areaShortName"].ToString())
                        {
                            areaModel.json.parentAreaCd = j["areaCd"].ToString();
                            cityList = CommonTool.GetDataPost("https://api.sit.kkday.com/api/area", JsonConvert.SerializeObject(areaModel));
                            foreach (var k in cityList["content"]["areaList"])
                            {
                                if (city == k["areaShortName"].ToString())
                                {
                                    countryModifyRQModelModel.json.opType = "UPDATE";
                                    countryModifyRQModelModel.json.cityCd = k["areaCd"].ToString();
                                    countryModifyRSModel = CommonTool.GetDataPost("https://api.sit.kkday.com/api/product/country/modify/" + prodOid, JsonConvert.SerializeObject(countryModifyRQModelModel));
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


                JObject timezone = CommonTool.GetDataPost("https://api.sit.kkday.com/api/comm/TIMEZONE", JsonConvert.SerializeObject(scmModel));
                foreach(var i in timezone["content"]["codeList"])
                {
                    if (i["code"]["dataName"].ToString().StartsWith(rezdyProductModel.Product.timezone) == true)
                    {
                        scmModel.json.timezone = i["code"]["dataCd"].ToString(); // 商品時區
                        break;
                    }
                }

                SetCostMethodRQModel setCostMethodRQModel = new SetCostMethodRQModel();
                setCostMethodRQModel.json = new SetCostMethodJson();
                setCostMethodRQModel.json.supplierOid = supplierLoginRSModel.supplierOid;
                setCostMethodRQModel.json.supplierUserUuid = supplierLoginRSModel.supplierUserUuid;
                setCostMethodRQModel.json.deviceId = supplierLoginRSModel.deviceId;
                setCostMethodRQModel.json.tokenKey = supplierLoginRSModel.tokenKey;
                setCostMethodRQModel.json.costCalcMethod = "NET";
                setCostMethodRQModel.json.prodCurrCd = "USD";
                setCostMethodRSModel = CommonTool.GetDataPost("https://api.sit.kkday.com/api/product/setCostMethod/" + prodOid, JsonConvert.SerializeObject(setCostMethodRQModel));
                if (setCostMethodRSModel["content"]["result"].ToString() != "0000")
                {
                    rsModel.result = setCostMethodRSModel["content"]["result"].ToString();
                    rsModel.msg = setCostMethodRSModel["content"]["msg"].ToString();
                    return rsModel;
                }

                // 有可能是 en-au 但是scm均為en 所以要作轉換
                for ( int i = 0; i< rezdyProductModel.Product.Languages.Count;i++ )
                {
                    if (rezdyProductModel.Product.Languages[i].StartsWith("en") == true)
                        rezdyProductModel.Product.Languages[i] = "en";
                }

                scmModel.json.guideLang = rezdyProductModel.Product.Languages ; // 提供解說服務
                scmModel.json.keyWord = ""; // 自訂關鍵字 用逗號分隔 共三格 11,22,33
                scmModel.json.orderEmail = supplierLoginRSModel.email; // 訂單通知 email 
                scmModel.json.supplierNote = rezdyProductModel.Product.productCode; // 備註(店家內部使用) 對應到對方的productCode

                //後面參數為model有null時 不顯示在model內
                scmRSModel = CommonTool.GetDataPost("https://api.sit.kkday.com/api/product/modify/"+ prodOid, JsonConvert.SerializeObject(scmModel,
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


    }
}
