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
                //product.prod_name_main_lang = obj["content"]["product"]["productNameMaster"].ToString();
                product.prod_type = obj["content"]["product"]["mainCat"].ToString();
                product.prod_type_name = obj["content"]["product"]["mainCatStr"].ToString();
                product.tag = obj["content"]["product"]["tagCd"].ToString();
                product.tag_name = obj["content"]["product"]["tagCd"].ToString();
                product.main_lang = obj["content"]["product"]["masterLang"].ToString();
                product.introduction = obj["content"]["product"]["introduction"].ToString();
                product.prod_desction = obj["content"]["product"]["productDesc"].ToString();
                product.prod_tips = obj["content"]["product"]["productTips"].ToString();
                product.prod_map_note = obj["content"]["product"]["gatherNote"].ToString();
                product.is_search = obj["content"]["product"]["isSearch"].ToString();
                product.apply_status = obj["content"]["product"]["applyStatus"].ToString();
                product.status = obj["content"]["product"]["saleStatus"].ToString();
                product.policy_no = obj["content"]["product"]["policyNo"].ToString();  //1:不扣手續費，退全額（包含當天取消者） 2:取消訂單，將收取所有實際產生費用 3:取消時間依照商品時區決定
               
                //product 之外的list 或 object
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
