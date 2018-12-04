using System;
using log4net;
using KKday.Consoles.B2S.Rezdy.AppCode;
using KKday.Consoles.B2S.Rezdy.Models.Produvt;
using Newtonsoft.Json;

namespace KKday.Consoles.B2S.Rezdy.ProductRepository
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

                //do something
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
            }
        }
    }
}
