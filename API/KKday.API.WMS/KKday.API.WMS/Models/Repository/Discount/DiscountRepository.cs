using System;
using System.Collections.Generic;
using System.Data;
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


        //2. 套價規則
        public static double GetCompanyDiscPrice(Int64 company_xid,double b2d_price,string prod_no ,string prod_type, ref DiscountRuleModel disc )
        {
            var objRules = new JObject();
            List<DataModel.Discount.Rule> ruList = new List<DataModel.Discount.Rule>();

            //prod_no = "2246";
            //prod_type = "M06";

            try
            {

                DataModel.Discount.Rule rule = null;

                //此分銷商的折扣規則
                objRules = DiscountDAL.GetDiscRuleList(company_xid);

                //當初最原始牌價
                rule = new DataModel.Discount.Rule();
                rule.disc_price = b2d_price;
                rule.mst_xid = null;
                rule.disc_name = null;
                ruList.Add(rule);


                if (objRules["Table"] != null)
                {
                    //找出不限的規則
                    var all_list = objRules["Table"].Where(y => y["rule_status"].ToString() == "00");

                    foreach (var item in all_list)
                    {
                        rule = new DataModel.Discount.Rule();
                        rule.mst_xid = (string)item["xid"];
                        rule.disc_percent = (double)item["disc_percent"];
                        rule.amt = (double)item["amt"];
                        rule.disc_price = System.Math.Round((b2d_price * (1 + rule.disc_percent / 100)) + rule.amt, MidpointRounding.AwayFromZero);
                        rule.disc_name = (string)item["disc_name"];
                        ruList.Add(rule);
                    }

                    //找出有規定黑白名單條件的規則
                    var rules = objRules["Table"].Where(y => y["rule_status"].ToString() == "01" &&
                                                        (y["main_cat_wb"].ToString().Contains(prod_type + "^0") || (y["main_cat_wb"].ToString().Contains("^1") && !y["main_cat_wb"].ToString().Contains(prod_type + "^1"))) &&//(符合白名單)|| (確定是黑名單＆＆不符合黑名單)  也算白名單
                                                        (y["prod_no_wb"].ToString().Contains(prod_no + "^0") || (y["prod_no_wb"].ToString().Contains("^1") && !y["prod_no_wb"].ToString().Contains(prod_no + "^1"))) //符合白名單 不符合黑名單 也算白名單
                                                                      //!y["main_cat_wb"].ToString().Contains(prod_type + "^1") && !y["prod_no_wb"].ToString().Contains(prod_no + "^1")//符合黑名單 不符合白名單 也算黑名單
                                                                      );
                    foreach (var item in rules)
                    {
                        rule = new DataModel.Discount.Rule();
                        rule.mst_xid = (string)item["xid"];
                        rule.disc_percent = (double)item["disc_percent"];
                        rule.amt = (double)item["amt"];
                        rule.disc_price = System.Math.Round((b2d_price * (1 + rule.disc_percent / 100)) + rule.amt, MidpointRounding.AwayFromZero);
                        rule.disc_name = (string)item["disc_name"];
                        rule.currency = (string)item["currency"];
                        rule.disc_dtl_xid = (string)item["disc_dtl_xid"];
                        ruList.Add(rule);
                    }

                }
                //套價規則排序 取最低價
                ruList = ruList.OrderBy(x => x.disc_price).ToList();

                if (disc == null) // 第一次進入才需要紀錄 折扣資訊 避免 price1 price2 price3 price4 記錄了４次
                {
                    disc = new DiscountRuleModel();

                    if (ruList[0].mst_xid == null) 
                    {
                        disc.isRule = false;
                        disc.disc_xid = null;
                        disc.disc_name = null;
                        disc.disc_percent = null;
                        disc.amt = null;
                        disc.currency = null;
                        disc.disc_dtl_xid = null;
                    }
                        
                    else 
                    {
                        disc.isRule = true;
                        disc.disc_xid = ruList[0].mst_xid;
                        disc.disc_name = ruList[0].disc_name;
                        disc.disc_percent = ruList[0].disc_percent;
                        disc.amt = ruList[0].amt;
                        disc.currency = ruList[0].currency ;
                        disc.disc_dtl_xid = ruList[0].disc_dtl_xid ;
                    }
                        
                }

                Website.Instance.logger.Info($"B2D套價規則 COMPANY_XID:{company_xid},PROD_NO:,{prod_no},XID:{ruList[0].mst_xid},DISC_PERCENT:{ruList[0].disc_percent},DISC_AMT:{ruList[0].amt}");


            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"getDiscPrice  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return ruList[0].disc_price;
        }

    }
}
