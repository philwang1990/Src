using System;
using KKday.Web.B2D.EC.Models.Model.Product;
using System.Linq;
using System.Collections.Generic;
using KKday.Web.B2D.EC.AppCode;
using KKday.Web.B2D.EC.Models.Model.Booking;
using KKday.Web.B2D.EC.Models.Model.Pmch;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using KKday.Web.B2D.EC.Models.Model.Booking.api;
using KKday.Web.B2D.EC.Models.Model.Account;
using KKday.Web.B2D.EC.Models.Model.UserAgent;
using KKday.Web.B2D.BE.App_Code;

namespace KKday.Web.B2D.EC.Models.Repostory.Booking
{
    public static class BookingRepostory
    {

        public static DataModel setDefaultBookingInfo(string memUuid,UserAgent ua,DataModel data, ProductModel prod, PkgDetailModel pkg, confirmPkgInfo confirm, B2dAccount  UserData, Pmgw pmgw)
        {
            data.productOid = confirm.prodOid;
            data.packageOid = confirm.pkgOid;
            data.contactFirstname = UserData.NAME_FIRST;
            data.contactLastname = UserData.NAME_LAST;
            data.contactEmail = UserData.EMAIL;
            data.telCountryCd = UserData.TEL_AREA;
            data.contactTel = UserData.TEL;
            data.contactCountryCd = UserData.COUNRTY_CODE;
            data.lstGoDt = confirm.selDate;
            if (confirm.pkgEvent != null) data.eventOid = confirm.pkgEvent;

            data.price1Qty = confirm.price1Qty;
            data.price2Qty = confirm.price2Qty == null ? 0 : confirm.price2Qty;
            data.price3Qty = confirm.price3Qty == null ? 0 : confirm.price3Qty;
            data.price4Qty = confirm.price4Qty == null ? 0 : confirm.price4Qty;
            data.payMethod = pmgw.acctdocReceiveMethod;
            data.hasRank = pkg.is_unit_pirce == "RANK" ? "Y" : "N";
            //data.productUrlOid = 
            data.productName = prod.prod_name;
            string[] citys = new string[prod.countries[0].cities.Count];

            int i = 0;
            foreach (City c in prod.countries[0].cities)
            {
                citys[i] = c.id;
                i++;
            }

            data.productCity = citys;
            data.productCountry = prod.countries[0].id;
            data.productMainCat = prod.prod_type;
            data.productOrderHandler = prod.prod_hander;
            data.payPmchOid = "1";
            data.currency = UserData.CURRENCY;

            data.currPriceTotal = ((pkg.price1 * confirm.price1Qty) + (pkg.price2 * confirm.price2Qty) + (pkg.price3 * confirm.price3Qty) + (pkg.price4 * confirm.price4Qty));// 263;// (pkg.price1 * confirm.price1Qty) +(pkg.price2 * confirm.price2Qty) +(pkg.price3 * confirm.price3Qty) + (pkg.price4 * confirm.price4Qty);
            data.crtDevice = ua.OS.Name;// "Macintosh";
            data.crtBrowser = ua.Browser.Name;// "Safari";
            data.crtBrowserVersion = ua.Browser.Version;// "12.0";
            data.memberUuid = memUuid;
            data.deviceId = data.guidNo;
            data.tokenKey = MD5Tool.GetMD5(memUuid + data.deviceId + Website.Instance.Configuration["kkdayKey:memuuidToken"].ToString());// "897af29c45ed180451c2e6bfa81333b6";
            data.riskStatus = "01";

            data.multipricePlatform = "01";
            data.sourceCode = "WEB";
            data.sourceParam1 = "";
            data.allowedCardNumberArray = new string[] { };

            //senddata
            data.modules.sendData.moduleData.receiverTel.telCountryCode = UserData.TEL_AREA;
            data.modules.sendData.moduleData.receiverTel.telNumber = UserData.TEL;

            //contact
            data.modules.contactData.moduleData.contactTel.telCountryCode = UserData.TEL_AREA;

            //rendCar 寫在 js

            return data;
        }

        //舊版
        public static PmchSslRequest setPaymentInfo(ProductModel prod, OrderModel orderModel, string orderMid)
        {
            PmchSslRequest pmch = new PmchSslRequest();

            pmch.apiKey = "kkdayapi";
            pmch.userOid = "1";
            pmch.ver = "1.0.1";
            pmch.ipaddress = "127.0.0.1";

            CallJsonPay json = new CallJsonPay();

            json.pmchOid = orderModel.payPmchOid;
            json.is3D = "0";
            json.payCurrency = orderModel.currency;
            json.payAmount = Convert.ToDouble(orderModel.currPriceTotal);
            json.returnURL = "https://localhost:5001/Final/Success/" + orderMid;
            json.cancelURL = "https://localhost:5001/Final/Cancel/" + orderMid;
            json.userLocale = "zh-tw";
            json.paymentParam1 = "";
            json.paymentParam2 = "";

            PaymentSourceInfo pay = new PaymentSourceInfo();
            pay.sourceType = "KKDAY";
            pay.orderMid = orderMid;

            json.paymentSourceInfo = pay;

            CreditCardInfo credit = new CreditCardInfo();
            credit.cardHolder = "phil";
            credit.cardNo = GibberishAES.OpenSSLEncrypt("", Website.Instance.Configuration["kkdayKey:cardNo"].ToString());
            credit.cardType = "VISA";
            credit.cardCvv = "143";
            credit.cardExp = "202310";

            json.creditCardInfo = credit;

            PayerInfo payer = new PayerInfo();
            payer.firstName = "ming";
            payer.lastName = "chen";
            payer.phone = "0939650222";
            payer.email = "phil.chang@kkday.com";

            json.payerInfo = payer;

            PayProductInfo prodInfo = new PayProductInfo();
            prodInfo.prodName = prod.prod_name;
            prodInfo.prodOid = prod.prod_no.ToString();

            json.productInfo = prodInfo;

            PayMember member = new PayMember();
            member.memberUuid = orderModel.memberUuid;
            member.riskStatus = "01";

            json.member = member;
            pmch.json = json;

            return pmch;// JsonConvert.SerializeObject(pmch);
        }

        //新版
        public static PmchSslRequest3 setPaymentInfo2(ProductModel prod, DataModel data, string orderMid, B2dAccount UserData, Pmgw pmgw, string memUuid ,string ip)
        {
            PmchSslRequest3 pmch = new PmchSslRequest3();

            pmch.api_key = "kkdayapi";
            pmch.user_oid = "1";
            pmch.ver = "1.0.1";
            pmch.lang_code =UserData.LOCALE;
            pmch.ipaddress = ip;

            CallJsonPay2 json = new CallJsonPay2();

            json.pmch_oid = pmgw.pmchOid;
            json.is_3d = "0";
            json.pay_currency = data.currency;
            json.pay_amount = Convert.ToDouble(data.currPriceTotal);
            json.return_url = Website.Instance.Configuration["payRtnUrl:returnUrl"].ToString() + orderMid;
            json.cancel_url = Website.Instance.Configuration["payRtnUrl:cancelUrl"].ToString() + orderMid;
            json.user_locale = UserData.LOCALE;// "zh-tw";
            json.paymentParam1 = "";
            json.paymentParam2 = "";

            if (prod.img_list.Count > 0)
            {
                json.logo_url = Website.Instance.Configuration["kkUrl:imgUrl"].ToString() + prod.img_list[0].img_kkday_url;
            }
            else
            {
                json.logo_url = "";
            }

            payment_source_info pay = new payment_source_info();
            pay.source_type = "KKDAY";
            pay.order_mid = orderMid;

            json.payment_source_info = pay;
            credit_card_info credit = new credit_card_info();
            credit.card_holder = data.card.cardHolder;
            credit.card_no = data.card.cardNo.Replace(" ", "");
            credit.card_type = data.card.cardType;//"VISA";
            credit.card_cvv = data.card.cardCvv;
            data.card.expiry = data.card.expiry.Replace(" ", "").Replace("/", "");
            credit.card_exp = "20" + data.card.expiry.Substring(2, 2) + data.card.expiry.Substring(0, 2);// "202312";

            json.credit_card_info = credit;

            payer_info payer = new payer_info();
            payer.first_name = UserData.NAME_LAST;
            payer.last_name = UserData.NAME_LAST;
            payer.phone = UserData.TEL;
            payer.email = UserData.EMAIL;

            json.payer_info = payer;

            product_info prodInfo = new product_info();
            prodInfo.prod_name = prod.prod_name;
            prodInfo.prod_oid = prod.prod_no.ToString();

            json.product_info = prodInfo;

            member member = new member();
            member.member_uuid = memUuid;
            member.risk_status = "01";
            member.ip = "127.0.0.1";

            json.member = member;
            pmch.json = json;

            return pmch;// JsonConvert.SerializeObject(pmch);
        }


        public static void setPayDtltoRedis(DataModel data, string orderMid, string memUuid, IRedisHelper rds)
        {
            PaymentDtl payDtl = new PaymentDtl();

            payDtl.currency = data.currency;
            payDtl.orderMid = orderMid;
            payDtl.payMethod = data.payMethod;
            payDtl.currTotalPrice = Convert.ToDouble(data.currPriceTotal);
            payDtl.paymentToken = MD5Tool.GetMD5(orderMid + memUuid + Website.Instance.Configuration["kkdayKey:payDtl"].ToString());

            string payDtlStr = JsonConvert.SerializeObject(payDtl);
            rds.SetRedis(payDtlStr, "b2d:ec:payDtl:" + orderMid, 60);
        }

        //組出booking 頁右邊顯示的內容
        public static BookingShowProdModel setBookingShowProd(ProductModel prod, PkgDetailModel pkg, confirmPkgInfo confirm, string currency, PkgEventsModel pkgEvent, ProdTitleModel prodTitle)
        {
            BookingShowProdModel prodShow = new BookingShowProdModel();

            prodShow.prodOid = prod.prod_no.ToString();
            prodShow.prodName = prod.prod_name;
            prodShow.currency = currency;
            prodShow.sDate = DateTimeTool.yyyy_mm_dd(confirm.selDate);
            prodShow.price1Qty = confirm.price1Qty;
            prodShow.price2Qty = confirm.price2Qty;
            prodShow.price3Qty = confirm.price3Qty;
            prodShow.price4Qty = confirm.price4Qty;
            prodShow.price1 = pkg.price1;
            prodShow.price2 = pkg.price2;
            prodShow.price3 = pkg.price3;
            prodShow.price4 = pkg.price4;
            prodShow.eventOid = confirm.pkgEvent;
            if (prod.img_list.Count > 0)
            {
                prodShow.photoUrl = Website.Instance.Configuration["kkUrl:imgUrl"].ToString() + prod.img_list[0].img_kkday_url;
            }

            prodShow.isRank = pkg.is_unit_pirce == "RANK" ? true : false;
            prodShow.pkgOid = pkg.pkg_no;
            prodShow.pkgName = pkg.pkg_name;
            prodShow.totoalPrice = (prodShow.price1Qty * prodShow.price1) + (prodShow.price2Qty * prodShow.price2) +
                (prodShow.price3Qty * prodShow.price3) + (prodShow.price4Qty * prodShow.price4);

            prodShow.unitText = pkg.unit_txt;

            if (pkgEvent != null)
            {
                var eTemp = pkgEvent.events.Where(x => x.day.Equals(confirm.selDate));


                foreach (Event e in eTemp)
                {
                    string[] times = e.event_times.Split(",");

                    foreach (string s in times)
                    {
                        string id = s.Split("_")[0];
                        if (id.Equals(confirm.pkgEvent))
                        {
                            prodShow.eventTime = s.Split("_")[1];
                            break;
                        }
                    }
                }
            }

            //設定回覆確試時間
            if (prod.confirm_order_time == 0)
            {
                prodShow.confirm_order_time = prodTitle.common_imm_confirm;
            }
            else
            {
                prodShow.confirm_order_time = prodTitle.booking_step3_check_confirm_hour.Replace("%d", prod.confirm_order_time.ToString());
            }

            return prodShow;
        }


        //組出價格別與年齡->前端js判斷calender用
        public static CusAgeRange getCusAgeRange(confirmPkgInfo confirm, PkgDetailModel pkgsTemp)
        {
            CusAgeRange cus = new CusAgeRange();
            cus.price1Qty = 0;
            cus.price2Qty = 0;
            cus.price3Qty = 0;
            cus.price4Qty = 0;

            if (confirm.price1Qty > 0)
            {
                cus.price1Qty = confirm.price1Qty;

                cus.price1sAge = Convert.ToInt32(pkgsTemp.price1_age_range.Split('~')[0]);
                cus.price1eAge = Convert.ToInt32(pkgsTemp.price1_age_range.Split('~')[1]);
            }
            if (confirm.price2Qty > 0)
            {
                cus.price2Qty = confirm.price2Qty;

                cus.price2sAge = Convert.ToInt32(pkgsTemp.price2_age_range.Split('~')[0]);
                cus.price2eAge = Convert.ToInt32(pkgsTemp.price2_age_range.Split('~')[1]);
            }
            if (confirm.price3Qty > 0)
            {
                cus.price3Qty = confirm.price3Qty;

                cus.price3sAge = Convert.ToInt32(pkgsTemp.price3_age_range.Split('~')[0]);
                cus.price3eAge = Convert.ToInt32(pkgsTemp.price3_age_range.Split('~')[1]);
            }
            if (confirm.price4Qty > 0)
            {
                cus.price4Qty = confirm.price4Qty;

                cus.price4sAge = Convert.ToInt32(pkgsTemp.price4_age_range.Split('~')[0]);
                cus.price4eAge = Convert.ToInt32(pkgsTemp.price4_age_range.Split('~')[1]);
            }

            return cus;
        }


        //套餐日期
        public static String getPkgEventDate(PkgEventsModel pkgEvent, string inPkgOi, int? bookintQty)
        {
            //event 要有位控且位控>=訂購數
            string dayTemp = "";
            foreach (Event e in pkgEvent.events)
            {
                string[] times = e.event_times.Split(",");

                foreach (string s in times)
                {
                    int qty = Convert.ToInt32(s.Split("_")[2]);
                    DateTime day = DateTimeTool.yyyyMMdd2DateTime(e.day);
                    if (qty >= bookintQty) dayTemp = dayTemp + day.ToString("yyyy-MM-dd") + ",";
                    break;
                }
            }

            dayTemp = dayTemp.Substring(0, dayTemp.Length - 1);
            return dayTemp;
        }

        //組出排除的餐食
        public static DataModel exculdeFood(ProductModel prod, DataModel dataModel, ProductModuleModel module)
        {
            CusData cus = module.module_cust_data;
            List<string> excludeFood = new List<string>();

            if (cus != null)
            {
                if (cus.is_require == true)
                {
                    if (cus.meal.is_require == true)
                    {
                        ExcludeFood excluede = cus.meal.exclude_food;

                        foreach (CusDataInfo travelerData in dataModel.travelerData)
                        {
                            if (!string.IsNullOrEmpty(travelerData.meal.mealType))
                            {
                                //'0002': ['0001', '0002', '0003', '0004', '0005', '0006'], //素食
                                //'0003': ['0002'], //猶太餐
                                //'0004': ['0002', '0005'] //穆斯林餐
                                if (travelerData.meal.mealType == "0002")
                                {
                                    foreach (MealType meal in cus.meal.meal_list)
                                    {
                                        if (meal.is_provided == true)
                                        {
                                            if (meal.meal_type == "0001" || meal.meal_type == "0002" ||
                                               meal.meal_type == "0003" || meal.meal_type == "0004" || meal.meal_type == "0005" || meal.meal_type == "0006")
                                            {
                                                excludeFood.Add(meal.meal_type);
                                            }
                                        }
                                    }

                                }
                                else if (travelerData.meal.mealType == "0003")
                                {
                                    var mealType = cus.meal.meal_list.Where(x => x.meal_type.Equals("0002"));
                                    if (mealType != null)
                                    {
                                        foreach (MealType m in mealType)
                                        {
                                            excludeFood.Add(m.meal_type);
                                        }
                                    }
                                }
                                else if (travelerData.meal.mealType == "0004")
                                {
                                    var mealType = cus.meal.meal_list.Where(x => x.meal_type.Equals("0002") || x.meal_type.Equals("0003"));
                                    if (mealType != null)
                                    {
                                        foreach (MealType m in mealType)
                                        {
                                            excludeFood.Add(m.meal_type);
                                        }
                                    }
                                }

                            }
                            travelerData.meal.excludeFoodType = excludeFood.ToArray();

                        }
                    }
                }
            }

            return dataModel;

        }

        public static object orderNew(DataModel data, ProdTitleModel title)
        {
            try
            {
                object result = ApiHelper.orderNew(data, title);
                return result;
            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"bookingStep1_orderNewErr:{ JsonConvert.SerializeObject(ex.ToString())}");
                throw new Exception(ex.Message.ToString());
            }
        }

        //成立b2d 訂單
        public static string insB2dOrder(ProdTitleModel title, ProductModel prod, PkgDetailModel pkg, confirmPkgInfo confirm, DataModel dataModel, B2dAccount UserData, DiscountRuleModel discRule)
        {
            try
            {
                B2dOrderModel order = new B2dOrderModel();

                order.connect_mail = dataModel.contactEmail;
                order.order_date = DateTime.Now;
                order.order_type = "B2D";
                order.order_status = "NW";
                order.order_amt = Convert.ToDouble(dataModel.currPriceTotal);
                order.order_b2c_amt = Convert.ToDouble(dataModel.currPriceTotal); //要重算
                order.connect_name = dataModel.asiaMileMemberLastName + " " + dataModel.contactFirstname;
                order.connect_tel = dataModel.contactTel;
                order.order_note = dataModel.note;

                OrderDiscountRule rule = new OrderDiscountRule();

                if (discRule.isRule == true)
                {
                    double discAmt = 0;
                    if (confirm.price1Qty > 0) discAmt = discAmt + Convert.ToDouble((pkg.price1_org - pkg.price1)*confirm.price1Qty);
                    if (confirm.price2Qty > 0) discAmt = discAmt + Convert.ToDouble((pkg.price2_org - pkg.price2) * confirm.price2Qty);
                    if (confirm.price3Qty > 0) discAmt = discAmt + Convert.ToDouble((pkg.price3_org - pkg.price3) * confirm.price2Qty);
                    if (confirm.price4Qty > 0) discAmt = discAmt + Convert.ToDouble((pkg.price4_org - pkg.price4) * confirm.price3Qty);

                    rule.disc_amt = discAmt;
                    rule.disc_currency = UserData.CURRENCY;
                    rule.disc_name = discRule.disc_name;
                    rule.disc_note = "";
                    order.order_discount_rule = rule;
                }
                else
                {
                    order.order_discount_rule = rule;
                }

                //Source source = new Source();
                //source.booking_type = "WEB";
                //source.company_xid = UserData.COMPANY_XID;
                //source.channel_oid = UserData.KKDAY_CHANNEL_OID;
                //source.connect_tel = dataModel.contactTel;
                //source.connect_mail = dataModel.contactEmail;
                //source.connect_name = dataModel.asiaMileMemberLastName + " " + dataModel.contactFirstname;
                //source.order_note = dataModel.note;
                //source.client_ip = "127.0.0.1";
                //source.crt_time = DateTime.Now;

                //order.source = source;

                //List<OrderCus> cusList = new List<OrderCus>();
                //List<OrderLst> lstList = new List<OrderLst>();

                //NORANK 且 （只有一個代表人 或 不要代表人）    ->只塞一筆order_lst
                //NORANK 且要填所有旅客資料->只塞1~*筆order_lst

                //RANK 且 （只有一個代表人 或 不要代表人）    ->只塞1~*筆order_lst
                //RANK 且要填所有旅客資料     ->只塞1~*筆order_lst

                //int? cusSeqno = 1;
                //int lstSeqno = 1;

                //string priceType = "";
                //int ii = 0;
                ////滿足cus
                //foreach (CusDataInfo cus in dataModel.travelerData)
                //{
                //    if (ii < confirm.price1Qty) { priceType = "price1"; }
                //    else if (ii < (confirm.price1Qty + confirm.price2Qty)) { priceType = "price2"; }
                //    else if (ii < (confirm.price1Qty + confirm.price2Qty + confirm.price3Qty)) { priceType = "price3"; }
                //    else if (ii < (confirm.price1Qty + confirm.price2Qty + confirm.price3Qty + confirm.price4Qty)) { priceType = "price4"; }
                //    OrderCus cusTemp = new OrderCus();
                //    //cusTemp.cus_seqno = Convert.ToInt32(cusSeqno);
                //    cusTemp.cus_type = priceType;
                //    cusTemp.cus_mail = "";
                //    cusTemp.cus_name_e_first = cus.englishName.firstName;
                //    cusTemp.cus_name_e_last = cus.englishName.lastName;
                //    cusTemp.cus_sex = cus.gender;
                //    cusTemp.cus_tel = "";

                //    cusList.Add(cusTemp);
                //    cusSeqno = cusSeqno + 1;
                //    ii = ii + 1;
                //}

                //if (dataModel.travelerData.Count == 0)
                //{
                //    cusSeqno = null;
                //}
                //else
                //{
                //    cusSeqno = 1;
                //}

                //if (dataModel.travelerData.Count == 1)
                //{
                //    //依priceTeype寫入
                //    if (confirm.price1Qty > 0) lstList.Add(insOrderListTemp(prod, pkg, confirm, dataModel, UserData, "price1", lstSeqno, 1, Convert.ToInt32(confirm.price1Qty), discRule));
                //    lstSeqno = lstSeqno + 1;
                //    if (confirm.price2Qty > 0) lstList.Add(insOrderListTemp(prod, pkg, confirm, dataModel, UserData, "price2", lstSeqno, 1, Convert.ToInt32(confirm.price2Qty), discRule));
                //    lstSeqno = lstSeqno + 1;
                //    if (confirm.price3Qty > 0) lstList.Add(insOrderListTemp(prod, pkg, confirm, dataModel, UserData, "price3", lstSeqno, 1, Convert.ToInt32(confirm.price3Qty), discRule));
                //    lstSeqno = lstSeqno + 1;
                //    if (confirm.price4Qty > 0) lstList.Add(insOrderListTemp(prod, pkg, confirm, dataModel, UserData, "price4", lstSeqno, 1, Convert.ToInt32(confirm.price4Qty), discRule));
                //}
                //else
                //{
                //    //依每一個row寫入
                //    for (ii = 0; ii < dataModel.travelerData.Count; ii++)
                //    {
                //        if (ii < confirm.price1Qty) { priceType = "price1"; }
                //        else if (ii < (confirm.price1Qty + confirm.price2Qty)) { priceType = "price2"; }
                //        else if (ii < (confirm.price1Qty + confirm.price2Qty + confirm.price3Qty)) { priceType = "price3"; }
                //        else if (ii < (confirm.price1Qty + confirm.price2Qty + confirm.price3Qty + confirm.price4Qty)) { priceType = "price4"; }
                //        lstList.Add(insOrderListTemp(prod, pkg, confirm, dataModel, UserData, priceType, lstSeqno, cusSeqno, 1, discRule));
                //        lstSeqno = lstSeqno + 1;
                //        cusSeqno = cusSeqno + 1;
                //    }
                //}

                //order.order_cus = cusList;
                //order.order_lst = lstList;

                Website.Instance.logger.Debug($"bookingStep1_insB2dOrder:{ JsonConvert.SerializeObject(order)}");

                insB2dOrderResult result = ApiHelper.insB2dOrder(order, title);
                if (result.result == "0000")
                {

                    Website.Instance.logger.Debug($"bookingStep1_insB2dOrderResult:{ JsonConvert.SerializeObject(result)}");
                    return result.order_no;
                }
                else
                {
                    Website.Instance.logger.Debug($"bookingStep1_insB2dOrderResult:{ JsonConvert.SerializeObject(result)}");
                    throw new Exception(result.result_msg);
                }

            }
            catch (Exception ex)
            {
                Website.Instance.logger.Debug($"bookingStep1_insB2dOrderErr:{ JsonConvert.SerializeObject(ex.ToString())}");
                throw new Exception(ex.Message.ToString());
            }
        }

        //UPDb2d 訂單

        public static Boolean updB2dOrder(long companyXid,string orderOid,string orderMid ,string b2bOrderNo,ProdTitleModel title)
        {
            UpdateB2dOrderModel order = new UpdateB2dOrderModel();

            order.company_xid = companyXid.ToString();
            order.order_no = b2bOrderNo;
            order.order_mid = orderMid;
            order.order_oid = orderOid;

            try
            {
                Website.Instance.logger.Debug($"bookingStep1_insB2dOrder:{ JsonConvert.SerializeObject(order)}");

                updB2dOrderResult result = ApiHelper.updB2dOrder(order, title);
                if (result.result == "0000")
                {

                    Website.Instance.logger.Debug($"bookingStep1_insB2dOrderResult:{ JsonConvert.SerializeObject(result)}");
                    return true;
                }
                else
                {
                    Website.Instance.logger.Debug($"bookingStep1_insB2dOrderResult:{ JsonConvert.SerializeObject(result)}");
                    throw new Exception(result.result_msg);
                }
            }
            catch(Exception ex)
            {
                Website.Instance.logger.Debug($"bookingStep1_insB2dOrderErr:{ JsonConvert.SerializeObject(ex.ToString())}");
                throw new Exception(ex.Message.ToString());
            }
        }

        //public static OrderLst insOrderListTemp(ProductModel prod, PkgDetailModel pkg, confirmPkgInfo confirm, DataModel dataModel, B2dAccount UserData, string priceType, int lstSeqno, int? cusSeqno, int prodQty, DiscountRuleModel discRule)
        //{
        //    OrderLst lstTemp = new OrderLst();
        //    //lstTemp.lst_seqno = lstSeqno;
        //    //lstTemp.cus_seqno = cusSeqno;
        //    lstTemp.prod_no = prod.prod_no.ToString();
        //    lstTemp.prod_amt = Convert.ToDouble(priceType == "price1" ? pkg.price1 : priceType == "price2" ? pkg.price2 : priceType == "price3" ? pkg.price3 : pkg.price4);
        //    lstTemp.prod_name = prod.prod_name;
        //    lstTemp.prod_b2c_amt = Convert.ToDouble(priceType == "price1" ? pkg.price1_b2c : priceType == "price2" ? pkg.price2_b2c : priceType == "price3" ? pkg.price3_b2c : pkg.price4_b2c);
        //    lstTemp.prod_currency = UserData.CURRENCY;
        //    lstTemp.prod_cond1 = priceType;
        //    lstTemp.prod_cond2 = pkg.unit;
        //    lstTemp.events = confirm.pkgEvent;
        //    lstTemp.pkg_date = confirm.selDate;
        //    //lstTemp.discount_xid = 0;
        //    lstTemp.pkg_no = pkg.pkg_no;
        //    lstTemp.pkg_name = pkg.pkg_name;
        //    lstTemp.prod_qty = prodQty;

        //    OrderDiscountRule rule = new OrderDiscountRule();

        //    if (discRule.isRule == true)
        //    {
        //        double discAmt = 0;
        //        if (priceType == "price1") discAmt = pkg.price1_org - pkg.price1;
        //        if (priceType == "price2") discAmt = pkg.price2_org - pkg.price2;
        //        if (priceType == "price3") discAmt = Convert.ToDouble(pkg.price3_org - pkg.price3);
        //        if (priceType == "price4") discAmt = Convert.ToDouble(pkg.price4_org - pkg.price4);

        //        rule.disc_amt = discAmt;
        //        rule.disc_currency = UserData.CURRENCY;
        //        rule.disc_name = discRule.disc_name;
        //        rule.disc_note = "";
        //        //rule.lst_seqno = lstSeqno;
        //        lstTemp.order_discount_rule = rule;
        //    }
        //    else
        //    {
        //        lstTemp.order_discount_rule = rule;
        //    }
        //    return lstTemp;
        //}


        //卡號先加密
        public static DataModel setCardEncrypt(DataModel data)
        {
            if (data.card != null)
            {
                if (data.card.cardNo != null)
                {
                    //卡別
                    int cardNum = Convert.ToInt32(data.card.cardNo.Substring(0, 3));
                    string cardType = data.card.cardNo.Substring(0, 1) == "4" ? "VISA" : data.card.cardNo.Substring(0, 1) == "5" ? "MASTER" :
                                      data.card.cardNo.Substring(0, 1) == "1" && data.card.cardNo.Substring(0, 4) == "1800" ? "JCB" :
                                      data.card.cardNo.Substring(0, 1) == "2" && data.card.cardNo.Substring(0, 4) == "2131" ? "JCB" :
                                      data.card.cardNo.Substring(0, 1) == "3" && cardNum >= 300 && cardNum <= 399 ? "JCB" : "";

                    data.card.cardType = cardType;
                    data.card.cardNo = GibberishAES.OpenSSLEncrypt(data.card.cardNo, Website.Instance.Configuration["kkdayKey:cardNo"].ToString());
                }
            }

            return data;
        }
    }
}
