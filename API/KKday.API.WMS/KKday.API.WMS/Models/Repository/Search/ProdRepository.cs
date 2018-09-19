using System;
using System.Collections.Generic;
using System.Data;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.Models.DataModel.Search;
using KKday.API.WMS.Models.Search;

namespace KKday.API.WMS.Models.Repository {
    public class ProdRepository {

        //1.取得商品列表
        public static List<ProdtLisModel> GetProdList(SearchRQModel list_rq) 
        {
            List<ProdtLisModel> prod_list = new List<ProdtLisModel>();

            try {
                DataSet ds = SearchProxy.GetProductAsync(list_rq);

                foreach (DataRow dr in ds.Tables[0].Rows) {
                    var model = new ProdtLisModel();

                    model.prod_no = dr.ToStringEx("PROD_NO");
                   




                    prod_list.Add(model);
                }




            }
             catch (Exception ex) {
              //  Website.Instance.logger.FatalFormat("{0},{1}", ex.Message, ex.StackTrace);
            }

            return prod_list;
        }

    }
}
