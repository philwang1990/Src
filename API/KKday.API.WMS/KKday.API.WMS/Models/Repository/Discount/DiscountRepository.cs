using System;
using System.Collections.Generic;
using System.Linq;
using KKday.API.WMS.AppCode.DAL;
using KKday.API.WMS.Models.DataModel.Discount;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.Models.Repository.Discount {
    public class DiscountRepository {

        //1. 先過濾此商品是否存在黑名單
        public static bool GetProdBlackWhite(string prod_no) {

            var obj = new JObject();
            bool isBlack = false;

            try {
            
                //所有的黑名單
                obj = DiscountDAL.GetBlackList();                             
                      
                List<DiscountModel> dm = ((JArray)obj["Table"]).
                    Select(x => new DiscountModel {

                        black_prod_no = (string)x["prod_no"]

                    }).ToList();

                if (dm.Where(x => x.black_prod_no.Equals(prod_no)).Count() == 1) 
                {

                    //表示該商品存在於黑名單中  移除Search list裡的商品
                    isBlack = true;
                }

            } catch (Exception ex) {

                Website.Instance.logger.FatalFormat($"getBlackLst  Error :{ex.Message},{ex.StackTrace}");

                throw ex;
                }

            return isBlack;
        }

    }
}
