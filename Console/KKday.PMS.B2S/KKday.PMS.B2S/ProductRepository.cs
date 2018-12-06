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
                supplierLoginRQModel.json.deviceId = "f46ffaed630f6351127dd36d13b2f8be";
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
                supplierLoginRSModel.deviceId = "f46ffaed630f6351127dd36d13b2f8be";
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
                var get = CommonTool.GetData("https://api.rezdy.com/latest/products/PUP3Q0?apiKey=0b3d137cc1db4108a92c309fa7d7f6da&supplierId=21470");
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

        public RSModel New(SupplierLoginRSModel supplierLoginRSModel, ref long prodOid, RezdyProductModel rezdyProductModel)
        {
            try
            {

                //initial log4net
                CommonTool.LoadLog4netConfig();
                RSModel rsModel = new RSModel();

                SCMProductModel scmModel = new SCMProductModel();
                scmModel.json = new ScmProductJson();
                scmModel.json.supplierOid = supplierLoginRSModel.supplierOid.ToString();
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


    }
}
