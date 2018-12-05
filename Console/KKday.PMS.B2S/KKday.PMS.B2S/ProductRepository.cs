using System;
using log4net;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.Models.Product;
using Newtonsoft.Json;

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

        public RezdyProductModel New(ref string prodOid)
        {
            try
            {

                //initial log4net
                CommonTool.LoadLog4netConfig();

                var get = CommonTool.GetData("https://api.rezdy.com/latest/products/PUP3Q0?apiKey=0b3d137cc1db4108a92c309fa7d7f6da&supplierId=21470");
                //RezdyProductModel

                RezdyProductModel obj = JsonConvert.DeserializeObject<RezdyProductModel>(get);

                SCMProductModel scmModel = new SCMProductModel();
                scmModel.json.supplierOid = "4128";
                scmModel.json.deviceId = "dc1f2ee8d691e5571d29bbca8b826782";
                scmModel.json.tokenKey = "f3c61986193bcac4291d139bfeadc54b";
                scmModel.json.productName = obj.Products.name;
                scmModel.json.masterLang = scmModel.Locale;
                scmModel.json.mainCat = "M01";

                var get2 = CommonTool.GetData("https://api.sit.kkday.com/api/product/new");

                return obj;


            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }
    }
}
