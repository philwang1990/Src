using System;
using System.Collections.Generic;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.Models.DataModel.Product;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

namespace KKday.API.WMS.Models.Repository.Product
{
    public class ProductRepository
    {
        public ProductRepository()
        {
        }

        public static ProductModel GetProdDtl(QueryProductModel queryRQ)
        {
            ProductModel product = new ProductModel();

            try
            {
                JObject obj = ProdProxy.getProd(queryRQ);
                product.prod_no = obj["content"]["product"]["prodOid"].ToString();
                product.prod_name = obj["content"]["product"]["productName"].ToString();
                List<Remind> reminds = null;
                JArray aa = (JArray)obj["content"]["remindList"];
                for (int i = 0; i < aa.Count; i++)
                {
                    JArray axx = (JArray)aa[i]["remind"];
                    reminds = axx
                                .Select(x => new Remind
                                {
                                    remind_xid = (int)x["detailOid"],
                                    remind_desc = (string)x["desc"]
                                }).ToList();
                }

                //foreach (var ttt in aa)
                //{
                //    reminds = ttt["remind"]
                //                .Select(x => new Remind
                //                {
                //                    remind_xid = (int)x["detailOid"],
                //                    remind_desc = (string)x["desc"]
                //                }).ToList();

                //}
                product.remind_list = reminds;

                List<Remind> remList = new List<Remind>();
                Remind rem = null;

                if (obj["content"]["remindList"].ToString() != "")
                {
                    JArray items = (JArray)obj["content"]["remindList"];
                    int remindList = items.Count;
                    for (int i = 0; i < remindList; i++)
                    {
                        rem = new Remind();
                        rem.remind_desc = items[i]["remind"]["desc"].ToString();
                        rem.remind_xid = (int)items[i]["remind"]["detailOid"];
                        remList.Add(rem);
                    }
                    product.remind_list = remList;
                }

            }
            catch (Exception ex)
            {
                var aa = ex.Message;
            }

          

            return product;
        }
    }
}
