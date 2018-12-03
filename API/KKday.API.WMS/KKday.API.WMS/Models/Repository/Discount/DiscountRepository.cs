using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using KKday.API.WMS.AppCode;
using KKday.API.WMS.AppCode.DAL;
using KKday.API.WMS.Models.DataModel.Discount;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.Models.Repository.Discount {
    public class DiscountRepository {

        static RedisHelper rds = new RedisHelper();
        //private static RedisHelper rds;

        //1. 先過濾此商品是否存在黑名單
        public static bool GetProdBlackWhite(string prod_no) {

            var obj = new JObject();
            bool isBlack = false;

            try
            {
                //黑名單規則塞入redis
                string _blackRedis = rds.getRedis($"b2d:discount:blcakList:{prod_no}");
                if (string.IsNullOrEmpty(_blackRedis) || _blackRedis == "{}")
                {
                    obj = DiscountDAL.GetBlackList();
                    rds.SetRedis(obj.ToString(), $"b2d:discount:blcakList:{prod_no}", 1440);
                }
                else
                {
                    obj = JObject.Parse(_blackRedis);
                }

                //所有的黑名單
                List<DiscountModel> dm = ((JArray)obj["Table"]).
                    Select(x => new DiscountModel
                    {
                        black_prod_no = (string)x["prod_no"]

                    }).ToList();

                if (dm.Where(x => x.black_prod_no.Equals(prod_no)).Count() > 0)
                {
                    //表示該商品存在於黑名單中  移除Search list裡的商品
                    isBlack = true;
                }

            }
            catch (Exception ex)
            {

                Website.Instance.logger.FatalFormat($"getBlackLst  Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return isBlack;
        }


        //2. 2.1固定價 >> 2.2套價規則
        public static double GetCompanyDiscPrice(Int64 company_xid,string company_currency, double b2d_price,string prod_no ,string prod_type,string search_type,string price_cond, ref DiscountRuleModel disc )
        {
            var objRules = new JObject();
            var objFixed = new JObject();
            List<DataModel.Discount.Rule> ruList = new List<DataModel.Discount.Rule>();
            double fixedPrice = 0.0;

            //prod_no = "2246";
            //prod_type = "M06";

            try
            {

                //固定價規則塞入redis
                string _fixedRedis = rds.getRedis($"b2d:discount:fixedPriceList:{company_xid}_{prod_no}");
                if (string.IsNullOrEmpty(_fixedRedis) || _fixedRedis == "{}")
                {
                    objFixed = DiscountDAL.GetFixedPriceList(company_xid, prod_no);
                    rds.SetRedis(objFixed.ToString(), $"b2d:discount:fixedPriceList:{company_xid}_{prod_no}", 1440);
                }
                else
                {
                    objFixed = JObject.Parse(_fixedRedis);
                }

                //2.1固定價 中就直接給固定價 不用算套價disc.isRule = false;
                if (objFixed["Table"] != null)
                {
                    //searchType= search,product 找出pkg price(排除0)最低價
                    //searchType= package price_cond 找出價錢 若0則 套回原來的價錢
                    var _lowPrice = objFixed["Table"].Where(y => ((double)y["price"]) > 0.0).Select(z => new { price = (double)z["price"] }).OrderBy(x => x.price).FirstOrDefault().price;
                    switch (search_type)
                    {
                        case "SEARCH":
                            fixedPrice = Convert.ToDouble(_lowPrice) > 0.0 ? Convert.ToDouble(_lowPrice) : b2d_price;
                            break;
                        case "PRODUCT":
                            fixedPrice = Convert.ToDouble(_lowPrice) > 0.0 ? Convert.ToDouble(_lowPrice) : b2d_price;
                            break;
                        case "PACKAGE":
                            var _lowPricePkg = objFixed["Table"].Where(y => (string)y["price_cond"] == price_cond.Split('_')[1] && (string)y["pkg_no"] == price_cond.Split('_')[0]).Select(z => new { price = (double)z["price"] }).OrderBy(x => x.price).FirstOrDefault().price;
                            fixedPrice = Convert.ToDouble(_lowPricePkg) > 0.0 ? Convert.ToDouble(_lowPricePkg) : b2d_price;
                            break;
                    }

                    disc = new DiscountRuleModel();
                    disc.isRule = false;
                    disc.disc_xid = null;
                    disc.disc_name = null;
                    disc.disc_percent = null;
                    disc.amt = null;
                    disc.currency = null;
                    disc.disc_dtl_xid = null;

                    return fixedPrice;
                }


                //2.2套價規則
                DataModel.Discount.Rule rule = null;

                //當初最原始牌價
                rule = new DataModel.Discount.Rule();
                rule.disc_price = b2d_price;
                rule.mst_xid = null;
                rule.disc_name = null;
                ruList.Add(rule);


                //套價規塞入redis
                string _ruleRedis = rds.getRedis($"b2d:discount:ruleList:{company_xid}_{company_currency}");
                if (string.IsNullOrEmpty(_ruleRedis) || _ruleRedis =="{}")
                {
                    objRules = DiscountDAL.GetDiscRuleList(company_xid, company_currency);
                    rds.SetRedis(objFixed.ToString(), $"b2d:discount:ruleList:{company_xid}_{company_currency}", 1440);
                }
                else
                {
                    objRules = JObject.Parse(_ruleRedis);
                }

                //此分銷商的折扣規則
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
                        rule.disc_type = (string)item["disc_type"];
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
                        rule.disc_type = (string)item["disc_type"];
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
                        disc.disc_type = null;
                        disc.disc_price = 0;
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
                        disc.disc_type = ruList[0].disc_type;
                        disc.disc_price = ruList[0].disc_price;
                    }
                        
                }

                Website.Instance.logger.Info($"B2D套價規則 COMPANY_XID:{company_xid},PROD_NO:,{prod_no},XID:{ruList[0].mst_xid},DISC_PERCENT:{ruList[0].disc_percent},DISC_AMT:{ruList[0].amt}");


            }
            catch (Exception ex)
            {
                Website.Instance.logger.FatalFormat($"getDiscPrice Error :{ex.Message},{ex.StackTrace}");
                throw ex;
            }

            return ruList[0].disc_price;
        }

    }
}
