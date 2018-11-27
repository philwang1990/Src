using System;
using System.Collections.Generic;
using System.Linq;
using KKday.API.WMS.AppCode.DAL;
using KKday.API.WMS.AppCode.Proxy;
using KKday.API.WMS.Models.DataModel.Common;
using KKday.API.WMS.Models.DataModel.Order;
using KKday.API.WMS.Models.DataModel.Product;
using KKday.API.WMS.Models.Repository.Common;
using KKday.API.WMS.AppCode;
using Newtonsoft.Json.Linq;

namespace KKday.API.WMS.Models.Repository.Order
{
    public class OrderRepository
    {
        private static RedisHelper rds;

        public static OrderListModel GetOrders(QueryOrderModel queryRQ)
        {
            queryRQ.option.orders = new List<string>();

            OrderListModel orderList = new OrderListModel();
            orderList.order = new List<DataModel.Order.Order>();
            DataModel.Order.Order order = new DataModel.Order.Order();

            string[] orders = queryRQ.option.kkday_orders;
            string orderMid = "", order_no = "";
            JObject obj = null;

            //step1.b2d order_no 與 kkday order mapping 驗證是否為此分銷商的訂單
            //用b2d kkday orderMid 找出 order_no
            //foreach (string item in orders)
            //{
            //    bool isOrder = OrderDAL.CheckOrder(queryRQ.company_xid, item,ref order_no);
            //    if (!isOrder)
            //    {
            //        orderList.result = "10";
            //        orderList.result_msg = $"Bad Request:OrderNo-{item} is not available";
            //        return orderList;
            //    }

            //    queryRQ.option.orders.Add(order_no);
            //}

            try
            {
                obj = OrderProxy.getOrders(queryRQ);
                if (obj["content"]["result"].ToString() != "0000")
                {
                    orderList.result = obj["content"]["result"].ToString();
                    orderList.result_msg = $"kkday order api response msg is not correct! {obj["content"]["msg"].ToString()}";

                    return orderList;
                }

                orderList.result = obj["content"]["result"].ToString();
                orderList.result_msg = obj["content"]["msg"].ToString();
                orderList.order_qty = (int)obj["content"]["size"];
                orderList.current_page = (int)obj["content"]["currentPage"];
                JArray jOrders = (JArray)obj["content"]["orderList"];

                foreach(var jOrder in jOrders)
                {
                    //用kkday orderMid 找出 b2d orderNo
                    bool isOrder = OrderDAL.CheckOrder(queryRQ.company_xid, ref order_no, (string)jOrder["order"]["orderMid"]);
                    if (!isOrder)
                    {
                        orderList.result = "10";
                        orderList.result_msg = $"Bad Request:OrderNo is not mapping";
                        return orderList;
                    }

                    var objOrder = jOrder["order"].ToObject<DataModel.Order.Order>();
                    objOrder.orderNo = order_no;
                    orderList.order.Add(objOrder);

                }

            }
            catch (Exception ex)
            {
                orderList.result = "10001";
                orderList.result_msg = ex.Message;
                Website.Instance.logger.FatalFormat($"OrderList ERROR :{ex.Message},{ex.StackTrace}");
            }

            return orderList;
        }

        public static OrderInfoModel GetOrderInfo(QueryOrderModel queryRQ,string orderMid)
        {
            OrderInfoModel info = new OrderInfoModel();
            info.order_modules = new List<modules>();
            info.order_cusList = new List<CusDataInfo>();

            var obj = new JObject();

            string order_no = "";

            try
            {
                //step1.b2d order_no 與 kkday order mapping 驗證是否為此分銷商的訂單
                bool isOrder = OrderDAL.CheckOrder(queryRQ.company_xid, ref order_no ,orderMid);
                if (!isOrder)
                {
                    info.result = "10";
                    info.result_msg = $"Bad Request:OrderNo-{orderMid} is not available";
                    return info;
                }

                //step2.JAVA API 查詢訂單資料
                obj = OrderProxy.getOrderInfo(queryRQ, orderMid);

                //RedisHelper rds = new RedisHelper();
                Dictionary<string, string> uikey = rds.klingonGet("frontend", queryRQ.locale_lang);

                if (obj["content"]["result"].ToString() != "0000")
                {
                    info.result = obj["content"]["result"].ToString();
                    info.result_msg = $"kkday order api response msg is not correct! {obj["content"]["msg"].ToString()}";

                    return info;
                }


                info.order_handler = obj["content"]["orderHandler"].ToString();

                //此訂單的OMDL
                JArray jModules = (JArray)obj["content"]["orderModuleData"];
                foreach (var items in jModules)
                {
                    if (items["moduleType"].ToString() == "OMDL_CUST_DATA")
                    {
                        var objOmdl = jModules.FirstOrDefault(y => (string)y["moduleType"] == "OMDL_CUST_DATA");
                        var objOmdls = (JArray)objOmdl["moduleData"];

                        foreach (var item in objOmdls)
                        {
                            CusDataInfo cus = new CusDataInfo();

                            englishNameInfo en = new englishNameInfo();
                            en.lastName = (string)item["englishName"]["lastName"];
                            en.firstName = (string)item["englishName"]["firstName"];
                            cus.englishName = en;

                            cus.gender = (string)item["gender"] == "M" ? uikey["common_male"] : (string)item["gender"] == "F" ? uikey["common_female"] : null;

                            cus.birthday = (string)item["birthday"];

                            nationalityInfo nation = new nationalityInfo();
                            nation.nationalityCode = (string)item["nationality"]["nationalityCode"];
                            nation.HKMOIdentityNumber = (string)item["nationality"]["HKMOIdentityNumber"];
                            nation.TWIdentity_number = (string)item["nationality"]["TWIdentityNumber"];
                            nation.MTPNumber = (string)item["nationality"]["MTPNumber"];
                            cus.nationality = nation.nationalityCode == null ? null : nation;

                            passportInfo pas = new passportInfo();
                            pas.passportNo = (string)item["passport"]["passportNo"];
                            pas.passportExpDate = (string)item["passport"]["passportExpDate"];
                            cus.passport = pas.passportNo == null ? null : pas;

                            localNameInfo lo = new localNameInfo();
                            lo.lastName = (string)item["localName"]["lastName"];
                            lo.firstName = (string)item["localName"]["firstName"];
                            cus.localName = lo.lastName == null ? null : lo;

                            heightInfo height = new heightInfo();
                            height.unit = (string)item["height"]["unit"] == "01" ? uikey["booking_step1_cust_data_height_unit_01"] : (string)item["height"]["unit"] == "02" ? uikey["booking_step1_cust_data_height_unit_02"] : null;
                            height.value = (double?)item["height"]["value"];
                            cus.height = height.unit == null ? null : height;

                            weightInfo weight = new weightInfo();
                            weight.unit = (string)item["weight"]["unit"] == "01" ? uikey["booking_step1_cust_data_weight_unit_01"] : (string)item["weight"]["unit"] == "02" ? uikey["booking_step1_cust_data_weight_unit_02"] : null;
                            weight.value = (double?)item["weight"]["value"];
                            cus.weight = weight.unit == null ? null : weight;

                            shoeSizeInfo shoe = new shoeSizeInfo();
                            shoe.unit = (string)item["shoeSize"]["unit"] == "01" ? "US" : (string)item["shoeSize"]["unit"] == "02" ? "EU" : (string)item["shoeSize"]["unit"] == "03" ? "JP/CM" : null;
                            shoe.value = (double?)item["shoeSize"]["value"];
                            cus.shoeSize = shoe.unit == null ? null : shoe;

                            mealInfo meal = new mealInfo();
                            foodAllergyInfo allergy = new foodAllergyInfo();
                            if (item["meal"]["mealType"].Type != JTokenType.Null)
                            {
                                meal.mealName = (string)item["meal"]["mealType"]["typeName"];
                                meal.excludeFoodType = item["meal"]["excludeFoodType"].Cast<string>().ToArray();
                                allergy.isFoodAllergy = (bool?)item["meal"]["foodAllergy"]["isFoodAllergy"];
                                allergy.allergenList = (string)item["meal"]["foodAllergy"]["allergenList"];
                                meal.foodAllergy = allergy;
                                cus.meal = meal;
                            }

                            cus.glassDiopter = (double?)item["glassDiopter"];

                            info.order_cusList.Add(cus);
                        }


                    }

                    if (items["moduleType"].ToString() == "OMDL_SHUTTLE")
                    {
                        var objOmdl = jModules.FirstOrDefault(y => (string)y["moduleType"] == "OMDL_SHUTTLE");

                        ShuttleModules shuttle = new ShuttleModules();
                        shuttle.shuttleDate = objOmdl["moduleData"]["shuttleDate"].ToString();

                        designatedLocationInfo designated = new designatedLocationInfo();
                        if (objOmdl["moduleData"]["designatedLocation"]["locationID"].Type != JTokenType.Null)
                        {
                            designated.locationName = objOmdl["moduleData"]["designatedLocation"]["orderProdSetting"]["locationName"].ToString();
                            designated.locationAddress = objOmdl["moduleData"]["designatedLocation"]["orderProdSetting"]["locationAddress"].ToString();
                            designated.timeRangeStart = objOmdl["moduleData"]["designatedLocation"]["orderProdSetting"]["timeRange"]["from"] == null ? null :
                                objOmdl["moduleData"]["designatedLocation"]["orderProdSetting"]["timeRange"]["from"]["hour"].ToString().PadLeft(2, '0') + ":" + objOmdl["moduleData"]["designatedLocation"]["orderProdSetting"]["timeRange"]["from"]["minute"].ToString().PadLeft(2, '0');
                            designated.timeRangeEnd = objOmdl["moduleData"]["designatedLocation"]["orderProdSetting"]["timeRange"]["to"] == null ? null :
                                objOmdl["moduleData"]["designatedLocation"]["orderProdSetting"]["timeRange"]["to"]["hour"].ToString().PadLeft(2, '0') + ":" + objOmdl["moduleData"]["designatedLocation"]["orderProdSetting"]["timeRange"]["to"]["minute"].ToString().PadLeft(2, '0');
                            shuttle.designatedLocation = designated;
                        }

                        designatedByCustomerInfo byCustomer = new designatedByCustomerInfo();
                        pickUpInfo pick = new pickUpInfo();
                        dropOffInfo drop = new dropOffInfo();
                        if (objOmdl["moduleData"]["designatedByCustomer"]["pickUp"]["location"].Type != JTokenType.Null)
                        {
                            pick.location = objOmdl["moduleData"]["designatedByCustomer"]["pickUp"]["location"].ToString();
                            pick.time = (bool)objOmdl["moduleData"]["designatedByCustomer"]["pickUp"]["time"]["isCustom"] ?
                                objOmdl["moduleData"]["designatedByCustomer"]["pickUp"]["time"]["hour"].ToString().PadLeft(2, '0') + ":" + objOmdl["moduleData"]["designatedByCustomer"]["pickUp"]["time"]["minute"].ToString().PadLeft(2, '0')
                                : objOmdl["moduleData"]["designatedByCustomer"]["pickUp"]["time"]["orderProdSetting"]["hour"].ToString().PadLeft(2, '0') + ":" + objOmdl["moduleData"]["designatedByCustomer"]["pickUp"]["time"]["orderProdSetting"]["minute"].ToString().PadLeft(2, '0');
                            drop.location = objOmdl["moduleData"]["designatedByCustomer"]["dropOff"]["location"].ToString();
                            byCustomer.pickUp = pick;
                            byCustomer.dropOff = drop;
                            shuttle.designatedByCustomer = byCustomer;
                        }

                        charterRouteInfo charter = new charterRouteInfo();
                        if (objOmdl["moduleData"]["charterRoute"]["routesID"].Type != JTokenType.Null)
                        {
                            charter.isCustom = (bool)objOmdl["moduleData"]["charterRoute"]["isCustom"];
                            charter.customRoutes = objOmdl["moduleData"]["charterRoute"]["customRoutes"].Cast<string>().ToArray();
                            charter.routeLocal = (string)objOmdl["moduleData"]["charterRoute"]["orderProdSetting"]["routeLocal"];
                            shuttle.charterRoute = charter;
                        }
                        omdlShuttleDate date = new omdlShuttleDate();
                        date.module_type = "OMDL_SHUTTLE";
                        date.module_data = shuttle;
                        info.order_modules.Add(date);
                    }

                    if (items["moduleType"].ToString() == "OMDL_CONTACT_DATA")
                    {
                        var objOmdl = jModules.FirstOrDefault(y => (string)y["moduleType"] == "OMDL_CONTACT_DATA");
                        ContactDataModules contact = new ContactDataModules();
                        contactNameInfo name = new contactNameInfo();
                        contactTelInfo tel = new contactTelInfo();
                        contactAppInfo app = new contactAppInfo();
                        name.firstName = (string)objOmdl["moduleData"]["contactName"]["firstName"];
                        name.lastName = (string)objOmdl["moduleData"]["contactName"]["lastName"];
                        contact.contactName = name;

                        if ((bool)objOmdl["moduleData"]["contactTel"]["haveTel"])
                        {
                            tel.telCountryCode = (string)objOmdl["moduleData"]["contactTel"]["telCountryCode"];
                            tel.telNumber = (string)objOmdl["moduleData"]["contactTel"]["telNumber"];
                            contact.contactTel = tel;
                        }

                        if ((bool)objOmdl["moduleData"]["contactApp"]["haveApp"])
                        {
                            app.appName = (string)objOmdl["moduleData"]["contactApp"]["appType"]["typeName"];
                            app.appAccount = (string)objOmdl["moduleData"]["contactApp"]["appAccount"];
                            contact.contactApp = app;
                        }

                        if(name.firstName != null)
                        {
                            omdlContactData date = new omdlContactData();
                            date.module_type = "OMDL_CONTACT_DATA";
                            date.module_data = contact;
                            info.order_modules.Add(date);
                        }

                    }

                    if (items["moduleType"].ToString() == "OMDL_PSGR_DATA")
                    {
                        var objOmdl = jModules.FirstOrDefault(y => (string)y["moduleType"] == "OMDL_PSGR_DATA");
                        PsgDataModules psg = new PsgDataModules();
                        qtyChildSeatInfo c_seat = new qtyChildSeatInfo();
                        qtyInfantSeatInfo i_seat = new qtyInfantSeatInfo();

                        psg.qtyAdult = (int?)objOmdl["moduleData"]["qtyAdult"];
                        psg.qtyChild = (int?)objOmdl["moduleData"]["qtyChild"];
                        psg.qtyInfant = (int?)objOmdl["moduleData"]["qtyInfant"];
                        psg.qtyCarryLuggage = (int?)objOmdl["moduleData"]["qtyCarryLuggage"];
                        psg.qtyCheckedLuggage = (int?)objOmdl["moduleData"]["qtyCheckedLuggage"];

                        if(objOmdl["moduleData"]["qtyChildSeat"]["supplierProvided"].Type != JTokenType.Null && objOmdl["moduleData"]["qtyChildSeat"]["selfProvided"].Type!=JTokenType.Null)
                        {
                            c_seat.supplierProvided = (int?)objOmdl["moduleData"]["qtyChildSeat"]["supplierProvided"];
                            c_seat.selfProvided = (int?)objOmdl["moduleData"]["qtyChildSeat"]["selfProvided"];
                            psg.qtyChildSeat = c_seat;

                        }

                        if (objOmdl["moduleData"]["qtyInfantSeat"]["supplierProvided"].Type != JTokenType.Null && objOmdl["moduleData"]["qtyInfantSeat"]["selfProvided"].Type != JTokenType.Null)
                        {
                            i_seat.supplierProvided = (int?)objOmdl["moduleData"]["qtyInfantSeat"]["supplierProvided"];
                            i_seat.selfProvided = (int?)objOmdl["moduleData"]["qtyInfantSeat"]["selfProvided"];
                            psg.qtyInfantSeat = i_seat;
                        }
                            

                        //if (objOmdl["moduleData"]["qtyChildSeat"]["supplierProvided"].Type != JTokenType.Null)
                        //{
                        //    c_seat.supplierProvided = (int?)objOmdl["moduleData"]["qtyChildSeat"]["supplierProvided"];
                        //    c_seat.selfProvided = (int?)objOmdl["moduleData"]["qtyChildSeat"]["selfProvided"];
                        //    psg.qtyChildSeat = c_seat;
                        //}

                        omdlPsgrData date = new omdlPsgrData();
                        date.module_type = "OMDL_PSGR_DATA";
                        date.module_data = psg;
                        info.order_modules.Add(date);

                    }

                    if (items["moduleType"].ToString() == "OMDL_FLIGHT_INFO")
                    {
                        var objOmdl = jModules.FirstOrDefault(y => (string)y["moduleType"] == "OMDL_FLIGHT_INFO");
                        FilghtInfoModules filght = new FilghtInfoModules();
                        arrivalInfo arrival = new arrivalInfo();
                        departureInfo departure = new departureInfo();
                        departureDatetimeInfo depTime = new departureDatetimeInfo();
                        arrivalDatetimeInfo arrTime = new arrivalDatetimeInfo();

                        arrival.airline = (string)objOmdl["moduleData"]["arrival"]["airline"];
                        arrival.airport = (string)objOmdl["moduleData"]["arrival"]["airport"];
                        arrTime.date = (string)objOmdl["moduleData"]["arrival"]["arrivalDatetime"]["date"];
                        arrTime.hour = (int?)objOmdl["moduleData"]["arrival"]["arrivalDatetime"]["hour"];
                        arrTime.minute = (int?)objOmdl["moduleData"]["arrival"]["arrivalDatetime"]["minute"];
                        arrival.arrivalDatetime = arrTime;
                        arrival.terminalNo = (string)objOmdl["moduleData"]["arrival"]["terminalNo"];
                        arrival.isNeedToApplyVisa = (bool?)objOmdl["moduleData"]["arrival"]["isNeedToApplyVisa"];
                        filght.arrival = arrival;

                        departure.airline = (string)objOmdl["moduleData"]["departure"]["airline"];
                        departure.airport = (string)objOmdl["moduleData"]["departure"]["airport"];
                        depTime.date = (string)objOmdl["moduleData"]["departure"]["departureDatetime"]["date"];
                        depTime.hour = (int?)objOmdl["moduleData"]["departure"]["departureDatetime"]["hour"];
                        depTime.minute = (int?)objOmdl["moduleData"]["departure"]["departureDatetime"]["minute"];
                        departure.departureDatetime = depTime;
                        departure.terminalNo = (string)objOmdl["moduleData"]["departure"]["terminalNo"];
                        departure.haveBeenInCountry = (bool?)objOmdl["moduleData"]["departure"]["haveBeenInCountry"];
                        filght.departure = departure;

                        if (filght.arrival.airline != null || filght.departure.airline != null)
                        {
                            omdlFlightInfo date = new omdlFlightInfo();
                            date.module_type = "OMDL_FLIGHT_INFO";
                            date.module_data = filght;
                            info.order_modules.Add(date);
                        }

                    }

                    if (items["moduleType"].ToString() == "OMDL_RENT_CAR")
                    {
                        var objOmdl = jModules.FirstOrDefault(y => (string)y["moduleType"] == "OMDL_RENT_CAR");
                        RentCarModules rent = new RentCarModules();

                        pickUpInfo_forCar pick = new pickUpInfo_forCar();
                        pick.officeName = (string)objOmdl["moduleData"]["pickUp"]["orderProdSetting"]["officeName"];
                        pick.addressEng = (string)objOmdl["moduleData"]["pickUp"]["orderProdSetting"]["addressEng"];
                        pick.addressLocal = (string)objOmdl["moduleData"]["pickUp"]["orderProdSetting"]["addressLocal"];
                        pick.datetime = $"{(string)objOmdl["moduleData"]["pickUp"]["datetime"]["date"]} {(string)objOmdl["moduleData"]["pickUp"]["datetime"]["hour"].ToString().PadLeft(2, '0')}:{(string)objOmdl["moduleData"]["pickUp"]["datetime"]["minute"].ToString().PadLeft(2, '0')}";

                        rent.pickup = pick;

                        dropOffInfo_forCar drop = new dropOffInfo_forCar();
                        drop.officeName = (string)objOmdl["moduleData"]["dropOff"]["orderProdSetting"]["officeName"];
                        drop.addressEng = (string)objOmdl["moduleData"]["dropOff"]["orderProdSetting"]["addressEng"];
                        drop.addressLocal = (string)objOmdl["moduleData"]["dropOff"]["orderProdSetting"]["addressLocal"];
                        drop.datetime = $"{(string)objOmdl["moduleData"]["dropOff"]["datetime"]["date"]} {(string)objOmdl["moduleData"]["pickUp"]["datetime"]["hour"].ToString().PadLeft(2, '0')}:{(string)objOmdl["moduleData"]["pickUp"]["datetime"]["minute"].ToString().PadLeft(2, '0')}";

                        rent.dropff = drop;

                        rent.isNeedFreeGPS = (bool?)objOmdl["moduleData"]["isNeedFreeGPS"];
                        rent.isNeedFreeWiFi = (bool?)objOmdl["moduleData"]["isNeedFreeWiFi"];
                        omdlRentCar date = new omdlRentCar();
                        date.module_type = "OMDL_RENT_CAR";
                        date.module_data = rent;
                        info.order_modules.Add(date);
                    }

                    if (items["moduleType"].ToString() == "OMDL_OTHER_DATA")
                    {
                        var objOmdl = jModules.FirstOrDefault(y => (string)y["moduleType"] == "OMDL_OTHER_DATA");
                        OtherDataModules other = new OtherDataModules();

                        other.activationDate = (string)objOmdl["moduleData"]["activationDate"];
                        other.mobileIMEI = (string)objOmdl["moduleData"]["mobileIMEI"];
                        other.mobileModelNumber = (string)objOmdl["moduleData"]["mobileModelNumber"];
                        other.exchangeLocationID = (string)objOmdl["moduleData"]["exchangeLocationID"];
                        other.exchangeLocationName = (string)objOmdl["moduleData"]["orderProdSetting"]["name"];
                        other.exchangeLocationAddress = (string)objOmdl["moduleData"]["orderProdSetting"]["address"];
                        other.exchangeLocationNote = (string)objOmdl["moduleData"]["orderProdSetting"]["note"];

                        omdlOtherData date = new omdlOtherData();
                        date.module_type = "OMDL_OTHER_DATA";
                        date.module_data = other;
                        info.order_modules.Add(date);
                    }

                    if (items["moduleType"].ToString() == "OMDL_SEND_DATA")
                    {
                        var objOmdl = jModules.FirstOrDefault(y => (string)y["moduleType"] == "OMDL_SEND_DATA");
                        SendDataModules send = new SendDataModules();

                        receiverNameInfo name = new receiverNameInfo();
                        receiverTelInfo tel = new receiverTelInfo();
                        receiveAddressInfo address = new receiveAddressInfo();
                        name.firstName = (string)objOmdl["moduleData"]["receiverName"]["firstName"];
                        name.lastName = (string)objOmdl["moduleData"]["receiverName"]["lastName"];
                        send.receiverName = name;

                        tel.telCountryCode = (string)objOmdl["moduleData"]["receiverTel"]["telCountryCode"];
                        tel.telNumber = (string)objOmdl["moduleData"]["receiverTel"]["telNumber"];
                        send.receiverTel = tel;

                        address.address = (string)objOmdl["moduleData"]["receiveAddress"]["address"];
                        address.cityName = (string)objOmdl["moduleData"]["receiveAddress"]["cityName"];
                        address.zipCode = (string)objOmdl["moduleData"]["receiveAddress"]["zipCode"];
                        address.countryName = (string)objOmdl["moduleData"]["receiveAddress"]["countryName"];

                        sendToHotelInfo hotel = new sendToHotelInfo();
                        buyerPassportEnglishNameInfo buyer = new buyerPassportEnglishNameInfo();
                        buyer.firstName = (string)objOmdl["moduleData"]["buyerPassportEnglishName"]["firstName"];
                        buyer.lastName = (string)objOmdl["moduleData"]["buyerPassportEnglishName"]["lastName"];
                        hotel.buyerPassportEnglishName = buyer;

                        buyerLocalNameInfo buyerLocalName = new buyerLocalNameInfo();
                        buyerLocalName.firstName = (string)objOmdl["moduleData"]["buyerLocalName"]["firstName"];
                        buyerLocalName.lastName = (string)objOmdl["moduleData"]["buyerLocalName"]["lastName"];
                        hotel.buyerLocalName = buyerLocalName;

                        hotel.bookingOrderNo = (string)objOmdl["moduleData"]["sendToHotel"]["bookingOrderNo"];
                        hotel.bookingWebsite = (string)objOmdl["moduleData"]["sendToHotel"]["bookingWebsite"];
                        hotel.checkInDate = (string)objOmdl["moduleData"]["sendToHotel"]["checkInDate"];
                        hotel.checkOutDate = (string)objOmdl["moduleData"]["sendToHotel"]["checkOutDate"];
                        hotel.hotelName = (string)objOmdl["moduleData"]["sendToHotel"]["hotelName"];
                        hotel.hotelAddress = (string)objOmdl["moduleData"]["sendToHotel"]["hotelAddress"];
                        hotel.hotelTel = (string)objOmdl["moduleData"]["sendToHotel"]["hotelTel"];
                        send.sendToHotel = hotel;

                        shipInfoInfo ship = new shipInfoInfo();
                        ship.shipDate = (string)objOmdl["moduleData"]["shipInfo"]["shipDate"];
                        ship.trackingNumber = (string)objOmdl["moduleData"]["shipInfo"]["trackingNumber"];
                        send.shipInfo = ship;

                        omdlSendData date = new omdlSendData();
                        date.module_type = "OMDL_SEND_DATA";
                        date.module_data = send;
                        info.order_modules.Add(date);
                    }

                }

                //此訂單的Detail
                OrderDtl order = new OrderDtl();
                order = obj["content"]["order"]["order"].ToObject<OrderDtl>();

                //導覽語言轉換
                GuideLanguageModel gudies = new GuideLanguageModel();
                gudies = CommonRepository.GetGuideLanguage();
                var guide = gudies.lang_list.FirstOrDefault(jt => (string)jt.langCd == order.guideLang);
                order.guideLang = guide.langName.ToString();
                order.orderNo = order_no;
                info.order_info = order;

            }
            catch (Exception ex)
            {
                info.result = "10001";
                info.result_msg = ex.Message;

                Website.Instance.logger.FatalFormat($"OrderInfo ERROR :{ex.Message},{ex.StackTrace}");
            }

            return info;
        }
    }
}
