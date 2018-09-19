using System;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.Models.DataModel.Product;

namespace KKday.API.WMS.Models.Repository.Product
{
    public class ProductRepository
    {
        public ProductRepository()
        {
        }

        public static ProductResponseModel GetProdDtl(QueryProdRquestModel queryRQ)
        {
            ProductResponseModel product = new ProductResponseModel();
            var kkapi_prod = ProdProxy.getProd(queryRQ);


            return product;
        }
    }
}
