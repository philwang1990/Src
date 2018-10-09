using System;
using System.Collections.Generic;
using System.Linq;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.Models.DataModel.Package;
using KKday.API.WMS.Models.DataModel.Product;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.Models.Repository.Package {
    /// <summary>
    /// Package repository.
    /// </summary>
    public class PackageRepository {
        /// <summary>
        /// Gets the package lst.
        /// </summary>
        /// <returns>The package lst.</returns>
        /// <param name="rq">Rq.</param>
        //取得套餐列表
        public static PackageModel GetPkgLst(QueryProductModel rq) {

            PackageModel pkg = new PackageModel();
            List<PkgDetailModel> pkgLst = new List<PkgDetailModel>();

            try {

                JObject obj = PackageProxy.getPkgLst(rq);

                #region --1.取回傳資料是否成功的訊息、一般資訊--

                pkg.result = obj["content"]["result"].ToString();
                pkg.result_msg = obj["content"]["msg"].ToString();
                pkg.cost_calc_type = obj["content"]["costCalcMethod"].ToString();

                #endregion

                #region --2.從傑森物件取『套餐列表』--
                JArray jPkglst = (JArray)obj["content"]["packageList"];

                for (int i = 0; i < jPkglst.Count; i++) {

                    var model = new PkgDetailModel();

                    model.pkg_no = jPkglst[i]["productPkg"]["pkgOid"].ToString();
                    model.pkg_name = jPkglst[i]["productPkg"]["pkgName"].ToString();
                    model.online_s_date = jPkglst[i]["productPkg"]["begValidDt"].ToString();
                    model.online_e_date = jPkglst[i]["productPkg"]["endValidDt"].ToString();
                    model.weekDays = jPkglst[i]["productPkg"]["weekDays"].ToString();

                    model.is_unit_pirce = jPkglst[i]["productPkg"]["pkgName"].ToString();

                    model.price1 = (double?)jPkglst[i]["productPkg"]["price1"];
                    model.price1_org = (double?)jPkglst[i]["productPkg"]["price1Org"];
                    model.prcie1_org_net = (double?)jPkglst[i]["productPkg"]["price1NetOrg"];
                    model.prcie1_profit_rate = (double?)jPkglst[i]["productPkg"]["price1GrossRate"];
                    model.prcie1_comm_rate = (double?)jPkglst[i]["productPkg"]["price1CommRate"];
                    model.prcie1_age_range = jPkglst[i]["productPkg"]["price1BegOld"].ToString() + "~" +
                                             jPkglst[i]["productPkg"]["price1EndOld"].ToString();
                    // model.price1_net = (double)jPkglst[i]["productPkg"][""];
                    //  model.price1_list = (double)jPkglst[i]["productPkg"][""];

                    model.price2 = (double?)jPkglst[i]["productPkg"]["price2"];
                    model.price2_org = (double?)jPkglst[i]["productPkg"]["price2Org"];
                    model.prcie2_org_net = (double?)jPkglst[i]["productPkg"]["price2NetOrg"];
                    model.prcie2_profit_rate = (double?)jPkglst[i]["productPkg"]["price2GrossRate"];
                    model.prcie2_comm_rate = (double?)jPkglst[i]["productPkg"]["price2CommRate"];
                    model.prcie2_age_range = jPkglst[i]["productPkg"]["price2BegOld"].ToString() + "~" +
                                             jPkglst[i]["productPkg"]["price2EndOld"].ToString();
                    // model.price2_net = (double)jPkglst[i]["productPkg"][""];
                    //  model.price2_list = (double)jPkglst[i]["productPkg"][""];

                    model.price3 = (double?)jPkglst[i]["productPkg"]["price3"];
                    model.price3_org = (double?)jPkglst[i]["productPkg"]["price3Org"];
                    model.prcie3_org_net = (double?)jPkglst[i]["productPkg"]["price3NetOrg"];
                    model.prcie3_profit_rate = (double?)jPkglst[i]["productPkg"]["price3GrossRate"];
                    model.prcie3_comm_rate = (double?)jPkglst[i]["productPkg"]["price3CommRate"];
                    model.price3_age_range = jPkglst[i]["productPkg"]["price3BegOld"].ToString() + "~" +
                                             jPkglst[i]["productPkg"]["price3EndOld"].ToString();
                    // model.price3_net = (double)jPkglst[i]["productPkg"][""];
                    //  model.price3_list = (double)jPkglst[i]["productPkg"][""];

                    model.price4 = (double?)jPkglst[i]["productPkg"]["price4"];
                    model.price4_org = (double?)jPkglst[i]["productPkg"]["price4Org"];
                    model.prcie4_org_net = (double?)jPkglst[i]["productPkg"]["price4NetOrg"];
                    model.prcie4_profit_rate = (double?)jPkglst[i]["productPkg"]["price4GrossRate"];
                    model.prcie4_comm_rate = (double?)jPkglst[i]["productPkg"]["price4CommRate"];
                    model.price4_age_range = jPkglst[i]["productPkg"]["price4BegOld"].ToString() + "~" +
                                             jPkglst[i]["productPkg"]["price4EndOld"].ToString();
                    // model.price4_net = (double)jPkglst[i]["productPkg"][""];
                    //  model.price4_list = (double)jPkglst[i]["productPkg"][""];

                    model.status = jPkglst[i]["productPkg"]["status"].ToString();
                    model.min_book_qty = (int)jPkglst[i]["productPkg"]["minOrderNum"];
                    model.max_book_qty = (int)jPkglst[i]["productPkg"]["maxOrderNum"];
                    model.isMultiple = jPkglst[i]["productPkg"]["isMultiple"].ToString();
                    model.book_qty = jPkglst[i]["productPkg"]["orderQty"].ToString();
                    model.unit = jPkglst[i]["productPkg"]["unit"].ToString();

                    model.unit_txt = jPkglst[i]["productPkg"]["unitTxt"].ToString();
                    model.unit_qty = (int)jPkglst[i]["productPkg"]["unitQty"];
                    model.pickupTp = jPkglst[i]["productPkg"]["pickupTp"].ToString();
                    model.pickupTpTxt = jPkglst[i]["productPkg"]["pickupTpTxt"].ToString();
                    model.is_hl = jPkglst[i]["productPkg"]["isBackUp"].ToString();
                    model.is_event = jPkglst[i]["productPkg"]["hasEvent"].ToString();

                    var d = jPkglst[i]["productPkg"]["pkgDesc"];
                    if (d.FirstOrDefault() != null) {
                        //取各套餐內的各個敘述
                        List<DescItem> desc = (d["descItems"][0]["content"])
                            .Select(x => new DescItem {

                                id = (string)x["id"],
                                desc = (string)x["desc"]

                            }).ToList();
                        model.desc_items = desc;
                    }

                    //組moduleSetting
                    var moduleSet = jPkglst[i]["productPkg"]["moduleSetting"];

                    if (moduleSet.FirstOrDefault() != null) {

                        FlightInfoType fit = new FlightInfoType() {
                            value =
                                moduleSet["flightInfoType"]["value"].ToString()

                        };

                        SendInfoType sit = new SendInfoType() {
                            value =
                                moduleSet["sendInfoType"]["value"].ToString(),
                            country_code =
                                moduleSet["sendInfoType"]["countryCode"].ToString()

                        };

                        VoucherValidInfo vi = new VoucherValidInfo();

                        if (moduleSet["voucherValidInfo"] != null && moduleSet["voucherValidInfo"].Any()) {

                            vi.valid_period_type =
                                  moduleSet["voucherValidInfo"]["validPeriodType"].ToString();

                            vi.before_specific_date =
                                  moduleSet["voucherValidInfo"]["beforeSpecificDate"].ToString();

                            if (moduleSet["voucherValidInfo"]["afterOrderDate"] != null && moduleSet["voucherValidInfo"]["afterOrderDate"].Any()) {
                                AfterOrderDate aod = new AfterOrderDate() {
                                    qty = (int)moduleSet["voucherValidInfo"]["afterOrderDate"]["qty"],
                                    unit = moduleSet["voucherValidInfo"]["afterOrderDate"]["unit"].ToString()
                                };

                                vi.after_order_date = aod;
                            }
                        }

                        ModuleSetting ms = new ModuleSetting() {
                            flight_info_type = fit,
                            send_info_type = sit,
                            voucher_valid_info = vi
                        };

                        model.module_setting = ms;

                        }
                                      
                    pkgLst.Add(model);
                }

                pkg.pkgs = pkgLst;

                //依套餐取回『可售日期』
                pkg.sale_dates = (PkgSaleDateModel)GetPkgSaleDate(rq); ;

                #endregion
                            

            } catch (Exception ex) {

                Website.Instance.logger.FatalFormat($"getPkg  Error :{ex.Message},{ex.StackTrace}");

                throw ex;
              
            }

            return pkg;
        }
        /// <summary>
        /// Gets the package sale date.
        /// </summary>
        /// <returns>The package sale date.</returns>
        /// <param name="rq">Rq.</param>
        //取得套餐可售日期
        public static PkgSaleDateModel GetPkgSaleDate(QueryProductModel rq) {

            PkgSaleDateModel pkgSdt = new PkgSaleDateModel();
            List<SaleDt> dt = new List<SaleDt>();

            try {

                JObject obj = PackageProxy.getSaleDate(rq);

                #region --1.取回傳資料是否成功的訊息--

                pkgSdt.result = obj["content"]["result"].ToString();
                pkgSdt.result_msg = obj["content"]["msg"].ToString();

                #endregion

                #region --2.從傑森物件取『套餐可售日期列表』--
                if (pkgSdt.result.ToString() == "0000" ) {

                    JArray jDt = (JArray)obj["content"]["saleDt"];

                    for (int i = 0; i < jDt.Count; i++) {

                        var model = new SaleDt();

                        model.pkg_no = jDt[i]["pkgOidObj"].ToString();
                        model.sale_day = jDt[i]["day"].ToString();
                        dt.Add(model);

                    }

                    pkgSdt.saleDt = dt;
                }
               

                #endregion
            } catch (Exception ex) {

                Website.Instance.logger.FatalFormat($"getPkg  Error :{ex.Message},{ex.StackTrace}");

                throw ex;
              }

            return pkgSdt;
        }

        /// <summary>
        /// Gets the package events.
        /// </summary>
        /// <returns>The package events.</returns>
        /// <param name="rq">Rq.</param>
        //取得套餐場次
        public static PkgEventsModel GetPkgEvents(QueryProductModel rq) {

            PkgEventsModel pkgEvnt = new PkgEventsModel();
            List<Event> et = new List<Event>();

            try {

                JObject obj = PackageProxy.getEvents(rq);

                #region --1.取回傳資料是否成功的訊息--

                pkgEvnt.result = obj["content"]["result"].ToString();
                pkgEvnt.result_msg = obj["content"]["msg"].ToString();
                pkgEvnt.pkg_no = (int)obj["content"]["eventData"][0]["pkgOid"];
                pkgEvnt.is_hl = obj["content"]["eventData"][0]["isBackup"].ToString();

                #endregion

                #region --2.從傑森物件取『套餐場次列表』--
                JArray jEt = (JArray)obj["content"]["eventData"][0]["events"];

                for (int i = 0; i < jEt.Count; i++) {

                    var model = new Event();

                    model.day = jEt[i]["day"].ToString();
                    model.event_times = jEt[i]["eventTimes"].ToString();
                    et.Add(model);

                }

                pkgEvnt.events = et;

                #endregion
            } catch (Exception ex) {

                Website.Instance.logger.FatalFormat($"getPkg  Error :{ex.Message},{ex.StackTrace}");

                throw ex;
             
            }

            return pkgEvnt;
        }
    }
}



