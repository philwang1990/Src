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

        public RSModel New(ref long prodOid)
        {
            try
            {

                //initial log4net
                CommonTool.LoadLog4netConfig();
                RSModel rsModel = new RSModel();

                var get = CommonTool.GetData("https://api.rezdy.com/latest/products/PUP3Q0?apiKey=0b3d137cc1db4108a92c309fa7d7f6da&supplierId=21470");
                //RezdyProductModel

                RezdyProductModel obj = JsonConvert.DeserializeObject<RezdyProductModel>(get);

                SCMProductModel scmModel = new SCMProductModel();
                scmModel.json = new Json();
                scmModel.json.supplierOid = "807";
                scmModel.json.supplierUserUuid = "4c529bc6-af3c-47c4-986c-eef30cdaa1f0";
                scmModel.json.deviceId = "11b501a87f4cf456f271e27395eb924b";
                scmModel.json.tokenKey = "02991db50e2ee8d7e4ae87be81f5ebc7";
                scmModel.json.productName = obj.Product.name;
                scmModel.json.masterLang = scmModel.Locale;
                scmModel.json.mainCat = "M01";

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
