using System;
using KKday.Web.B2D.EC.Models.Model.Booking;
using System.Collections.Generic;
namespace KKday.Web.B2D.EC.Models.Repostory.Booking
{
    public class OrderRepostory
    {
        public OrderRepostory()
        {
        }


        public OrderModel setOrderModel(DataModel data)
        {
            OrderModel book = new OrderModel();

            book.productOid = data.productOid;
            book.packageOid = data.packageOid;
            book.memberUuid = data.memberUuid;
            book.contactFirstname = data.contactFirstname;// "昶宇";
            book.contactLastname = data.contactLastname;// "林";
            book.contactEmail =data.contactEmail;//"paul.lin@kkday.com";
            book.telCountryCd = data.telCountryCd;// "886";
            book.contactTel = data.contactTel;// "934233332";
            book.contactCountryCd = data.contactCountryCd;// "TW";
            book.lstGoDt = data.lstGoDt;// "20180906";
            book.eventOid = data.eventOid;
            book.eventBackupData = data.eventBackupData==null ?"" : data.eventBackupData.Replace("-","");
            book.price1Qty = data.price1Qty;//;
            book.price2Qty = data.price2Qty;
            book.price3Qty = data.price3Qty;// 0;
            book.price4Qty =data.price4Qty;
            book.payMethod = data.payMethod;// "ONLINE_CITI";
            book.deviceId = data.deviceId;// "6ed75d896bbef9575563bfe33ab21e07";
            book.tokenKey = data.tokenKey;// "6f0748b65c09999b8818abfb3cf1846a";//"46da9fc1c7a765d20957c431d498c88f";
            book.crtDevice = data.crtDevice;// "Macintosh";
            book.crtBrowser = data.crtBrowser;// "Chrome";
            book.crtBrowserVersion =data.crtBrowserVersion ;//"68.0.3440.106";
            //book.couponUuid = data.couponUuid;
            //book.priceCoupon = data.priceCoupon;
            book.adCampaign = data.adCampaign;
            book.sourceCode = data.sourceCode;//"WEB";
            book.sourceParam1 = data.sourceParam1;// "4026";
            //book.sourceParam2 = "";
            //book.sourceParam3 = "";
            book.guideLang = data.guideLang;// "zh-tw";
            book.note = data.note;// "this is order note";
            book.hasRank = data.hasRank;// "Y";
            book.multipricePlatform = data.multipricePlatform;// "01";
            book.currency =data.currency;//"TWD";
            book.currPriceTotal = data.currPriceTotal.ToString();// "500";

            //book.card = new cardInfo() { cardNo = "" }; //不確定有沒有..
            //book.asiaMileMemberNo = "";//不確定有沒有..
            //book.asiaMileMemberFirstName = "";//不確定有沒有..
            //book.asiaMileMemberLastName = "";//不確定有沒有..

            book.orderCusList = data.travelerData;

            List<modules> ListM = new List<modules>();


           // BookingRepostory_3 dd = new BookingRepostory_3();
            ListM.Add(this.getFlightInfo2(data));
            ListM.Add(this.getShuttleInfo2(data));
            ListM.Add(this.getRendCar2(data));
            ListM.Add(this.getPsgData2(data));
            ListM.Add(this.getSendData2(data));
            ListM.Add(this.getOtherData2(data));
            ListM.Add(this.getContactData2(data));

            book.modules = ListM;

            book.productUrlOid = data.productUrlOid;// 17379;
            book.productName = data.productName;// "浪漫水金九";
            //string[] c = new string[1]; c[0] = "A01-001-00001";
            book.productCity = data.productCity;// c;
            //string[] c2 = new string[1]; c2[0] = "A01-001";
            book.productCountry = data.productCountry;// c2;
            book.productMainCat = data.productMainCat;// "M07";
            book.productOrderHandler = data.productOrderHandler;// "KKDAY";
            book.payPmchOid = data.payPmchOid;// "1";
            book.couponFailureCode = data.couponFailureCode;// null;
            //string[] c3 = new string[1];
            book.allowedCardNumberArray = data.allowedCardNumberArray;// c3;
            book.alsoUpdateMember = data.alsoUpdateMember;// true;
            book.riskStatus = data.riskStatus;// "03";

            Pmch pmchObj = new Pmch();

            pmchObj.pmchOid = "1";
            pmchObj.pmchCode = "CITI_CREDITCARD";
            pmchObj.pmchPayURL = "citi/payment/auth";
            pmchObj.is3D = "0";
            pmchObj.acctdocReceiveMethod = "ONLINE_CITI";

            InterfaceSetting interSetting = new InterfaceSetting();
            interSetting.isNeedCardInput = "true";

            List<LogoList> loglList = new List<LogoList>();
            LogoList logo = new LogoList();
            logo.logoName = "JCB";
            logo.logoUrl = "JCB_URL";
            loglList.Add(logo);

            logo = new LogoList();
            logo.logoName = "MASTER";
            logo.logoUrl = "MASTER_URL";
            loglList.Add(logo);

            logo = new LogoList();
            logo.logoName = "VISA";
            logo.logoUrl = "VISA_URL";
            loglList.Add(logo);

            interSetting.logoList = loglList;

            List<string> acceptedCardTypeListStr = new List<string>();
            acceptedCardTypeListStr.Add("MASTERCARD");
            acceptedCardTypeListStr.Add("JCB");
            acceptedCardTypeListStr.Add("VISA");
            interSetting.acceptedCardTypeList = acceptedCardTypeListStr;

            interSetting.acceptedCurrency = "TWD";

            List<string> otherInfoList = new List<string>();
            otherInfoList.Add("01");

            interSetting.otherInfoList = otherInfoList;

            pmchObj.interfaceSetting = interSetting;
            pmchObj.title = "信用卡付款";
            pmchObj.bg_class = null;
            List<string> card_classList = new List<string>();
            card_classList.Add("jcbloggero");
            card_classList.Add("masterloggero");
            card_classList.Add("visaloggero");

            pmchObj.card_class_list = card_classList;

            book.pmch = pmchObj;
            return book;
        }

        public omdlFlightInfo getFlightInfo2(DataModel data)
        {
            omdlFlightInfo flight = new omdlFlightInfo();
            flight.moduleType = data.modules.flightInfoData.moduleType;// "OMDL_FLIGHT_INFO";

            //OMDL_FLIGHT_INFO flight = new OMDL_FLIGHT_INFO();

            FilghtInfoModules fm = new FilghtInfoModules();
            fm.arrival = new arrivalInfo()
            {
                flightType = data.modules.flightInfoData.moduleData.arrival.flightType,
                airport = data.modules.flightInfoData.moduleData.arrival.airport,
                terminalNo = data.modules.flightInfoData.moduleData.arrival.terminalNo,
                airline = data.modules.flightInfoData.moduleData.arrival.airline,
                flightNo = data.modules.flightInfoData.moduleData.arrival.flightNo,
                arrivalDatetime = new arrivalDatetimeInfo()
                {
                    date = data.modules.flightInfoData.moduleData.arrival.arrivalDatetime.date,
                    hour = data.modules.flightInfoData.moduleData.arrival.arrivalDatetime.hour,
                    minute = data.modules.flightInfoData.moduleData.arrival.arrivalDatetime.minute
                },
                isNeedToApplyVisa = data.modules.flightInfoData.moduleData.arrival.isNeedToApplyVisa
            };

            fm.departure = new departureInfo()
            {
                flightType = data.modules.flightInfoData.moduleData.departure.flightType,
                airport = data.modules.flightInfoData.moduleData.departure.airport,
                terminalNo = data.modules.flightInfoData.moduleData.departure.terminalNo,
                airline = data.modules.flightInfoData.moduleData.departure.airline,
                flightNo = data.modules.flightInfoData.moduleData.departure.flightNo,
                departureDatetime = new departureDatetimeInfo() { 
                    date = data.modules.flightInfoData.moduleData.departure.departureDatetime.date, 
                    hour = data.modules.flightInfoData.moduleData.departure.departureDatetime.hour, 
                    minute = data.modules.flightInfoData.moduleData.departure.departureDatetime.minute },
                haveBeenInCountry = false
            };

            flight.moduleData = fm;
            return flight;
        }

        public omdlShuttleDate getShuttleInfo2(DataModel data) //接送 
        {
            omdlShuttleDate shuttle = new omdlShuttleDate();

            ShuttleModules sm = new ShuttleModules();

            shuttle.moduleType = data.modules.shuttleData.moduleType;// "OMDL_SHUTTLE";
            sm.shuttleDate = data.modules.shuttleData.moduleData.shuttleDate;// "2018-09-09";
            sm.designatedLocation = new designatedLocationInfo() { locationID = data.modules.shuttleData.moduleData.designatedLocation.locationID };
            sm.designatedByCustomer = new designatedByCustomerInfo()
            {
                pickUp = new pickUpInfo()
                {
                    time = new timeInfo() { 
                        isCustom = data.modules.shuttleData.moduleData.designatedByCustomer.pickUp.time.isCustom, 
                        timeID = data.modules.shuttleData.moduleData.designatedByCustomer.pickUp.time.timeID,
                        hour = data.modules.shuttleData.moduleData.designatedByCustomer.pickUp.time.hour, 
                        minute = data.modules.shuttleData.moduleData.designatedByCustomer.pickUp.time.minute },
                    location = data.modules.shuttleData.moduleData.designatedByCustomer.pickUp.location
                },
                dropOff = new dropOffInfo() { location = data.modules.shuttleData.moduleData.designatedByCustomer.dropOff.location }

            };

            //string[] a = new string[0];
            //a[0] = "北門"; a[1] = "東門"; a[2] = "西門";
            sm.charterRoute = new charterRouteInfo()
            {
                isCustom = data.modules.shuttleData.moduleData.charterRoute.isCustom ,
                routesID = data.modules.shuttleData.moduleData.charterRoute.routesID,
                customRoutes = data.modules.shuttleData.moduleData.charterRoute.customRoutes
            };

            shuttle.moduleData = sm;

            return shuttle;
        }

        public omdlRentCar getRendCar2(DataModel data) //租車
        {

            omdlRentCar rendCar = new omdlRentCar();
            rendCar.moduleType = data.modules.carRentingData.moduleType;// "OMDL_RENT_CAR";

            RentCarModules rm = new RentCarModules();

            pickUpInfo_forCar p = new pickUpInfo_forCar();
            p.officeID = data.modules.carRentingData.moduleData.pickUp.officeID;// null;

            dateTimeInfo dt1 = new dateTimeInfo();
            dt1.date = data.modules.carRentingData.moduleData.pickUp.datetime.date;//  "2018-09-06";
            dt1.hour = data.modules.carRentingData.moduleData.pickUp.datetime.hour; //8;
            dt1.minute = data.modules.carRentingData.moduleData.pickUp.datetime.minute;// 40;
            p.datetime = dt1;

            rm.pickup = p;

            dropOffInfo_forCar d = new dropOffInfo_forCar();
            d.officeID = data.modules.carRentingData.moduleData.dropOff.officeID;//  null;
            dt1 = new dateTimeInfo();
            dt1.date = data.modules.carRentingData.moduleData.dropOff.datetime.date;
            dt1.hour = data.modules.carRentingData.moduleData.dropOff.datetime.hour;//null;
            dt1.minute = data.modules.carRentingData.moduleData.dropOff.datetime.minute;// null;
            d.datetime = dt1;

            rm.dropff = d;
            rm.isNeedFreeGPS = data.modules.carRentingData.moduleData.isNeedFreeGPS;
            rm.isNeedFreeWiFi = data.modules.carRentingData.moduleData.isNeedFreeWiFi;// null;

            rendCar.moduleData = rm;
            return rendCar;
        }

        public omdlPsgrData getPsgData2(DataModel data)
        {
            omdlPsgrData psg = new omdlPsgrData();
            psg.moduleType = data.modules.passengerData.moduleType;// "OMDL_PSGR_DATA";

            PsgDataModules pm = new PsgDataModules();

            pm.qtyAdult = data.modules.passengerData.moduleData.qtyAdult;// 1;
            pm.qtyChild = data.modules.passengerData.moduleData.qtyChild;
            pm.qtyInfant = data.modules.passengerData.moduleData.qtyInfant;

            qtyChildSeatInfo qch = new qtyChildSeatInfo();
            qch.supplierProvided = data.modules.passengerData.moduleData.qtyChildSeat.supplierProvided==null?0: data.modules.passengerData.moduleData.qtyChildSeat.supplierProvided;
            qch.selfProvided = data.modules.passengerData.moduleData.qtyChildSeat.selfProvided==null? 0: data.modules.passengerData.moduleData.qtyChildSeat.selfProvided;// 0;
            pm.qtyChildSeat = qch;

            qtyInfantSeatInfo qin = new qtyInfantSeatInfo();
            qin.supplierProvided = data.modules.passengerData.moduleData.qtyInfantSeat.supplierProvided==null?0 : data.modules.passengerData.moduleData.qtyInfantSeat.supplierProvided;
            qin.selfProvided = data.modules.passengerData.moduleData.qtyInfantSeat.selfProvided==null? 0: data.modules.passengerData.moduleData.qtyInfantSeat.selfProvided;// 0;
            pm.qtyInfantSeat = qin;

            pm.qtyCarryLuggage = data.modules.passengerData.moduleData.qtyCarryLuggage;
            pm.qtyCheckedLuggage = data.modules.passengerData.moduleData.qtyCheckedLuggage;

            psg.moduleData = pm;
            return psg;
        }

        public omdlSendData getSendData2(DataModel data)
        {
            omdlSendData send = new omdlSendData();

            send.moduleType = data.modules.sendData.moduleType;// "OMDL_SEND_DATA";

            SendDataModules sm = new SendDataModules();
            receiverNameInfo rec = new receiverNameInfo();
            rec.firstName = data.modules.sendData.moduleData.receiverName.firstName;// null;
            rec.lastName = data.modules.sendData.moduleData.receiverName.lastName;//null;
            sm.receiverName = rec;

            receiverTelInfo recTel = new receiverTelInfo();
            recTel.telCountryCode = data.modules.sendData.moduleData.receiverTel.telCountryCode;// "886";
            recTel.telNumber = data.modules.sendData.moduleData.receiverTel.telNumber;//"934233332";
            sm.receiverTel = recTel;

            sendToCountryInfo sendCountry = new sendToCountryInfo();
            receiveAddressInfo recAdd = new receiveAddressInfo();
            recAdd.countryCode = data.modules.sendData.moduleData.sendToCountry.receiveAddress.countryCode;// null;
            recAdd.cityCode = data.modules.sendData.moduleData.sendToCountry.receiveAddress.cityCode;// null;
            recAdd.zipCode = data.modules.sendData.moduleData.sendToCountry.receiveAddress.zipCode;//null;
            recAdd.address = data.modules.sendData.moduleData.sendToCountry.receiveAddress.address;//null;
            sendCountry.receiveAddress = recAdd;
            sm.sendToCountry = sendCountry;

            sendToHotelInfo sendHtl = new sendToHotelInfo();
            sendHtl.hotelName = data.modules.sendData.moduleData.sendToHotel.hotelName;// null;
            sendHtl.hotelAddress = data.modules.sendData.moduleData.sendToHotel.hotelAddress ;//null;
            sendHtl.hotelTel = data.modules.sendData.moduleData.sendToHotel.hotelTel;// null;
            buyerPassportEnglishNameInfo b = new buyerPassportEnglishNameInfo();
            b.firstName = data.modules.sendData.moduleData.sendToHotel.buyerPassportEnglishName.firstName;
            b.lastName = data.modules.sendData.moduleData.sendToHotel.buyerPassportEnglishName.lastName;
            sendHtl.buyerPassportEnglishName = b;

            buyerLocalNameInfo b1 = new buyerLocalNameInfo();
            b1.firstName = data.modules.sendData.moduleData.sendToHotel.buyerLocalName.firstName;
            b1.lastName = data.modules.sendData.moduleData.sendToHotel.buyerLocalName.lastName;
            sendHtl.buyerLocalName = b1;

            sendHtl.bookingOrderNo = data.modules.sendData.moduleData.sendToHotel.bookingOrderNo;
            sendHtl.bookingWebsite = data.modules.sendData.moduleData.sendToHotel.bookingWebsite;// null;
            sendHtl.checkInDate = data.modules.sendData.moduleData.sendToHotel.checkInDate;//null;
            sendHtl.checkOutDate = data.modules.sendData.moduleData.sendToHotel.checkOutDate;// null;

            sm.sendToHotel = sendHtl;
            shipInfoInfo s = new shipInfoInfo();
            s.shipDate = data.modules.sendData.moduleData.shipInfo.shipDate;// null;
            s.trackingNumber = data.modules.sendData.moduleData.shipInfo.trackingNumber;// null;

            sm.shipInfo = s;
            send.moduleData = sm;
            return send;
        }

        public omdlOtherData getOtherData2(DataModel data)
        {
            omdlOtherData other = new omdlOtherData();

            other.moduleType = data.modules.otherData.moduleType;// "OMDL_OTHER_DATA";

            OtherDataModules om = new OtherDataModules();

            om.mobileModelNumber = data.modules.otherData.moduleData.mobileModelNumber;// "M5555";
            om.mobileIMEI = data.modules.otherData.moduleData.mobileIMEI;// null;
            om.activationDate = data.modules.otherData.moduleData.activationDate;//null;
            om.exchangeLocationID = data.modules.otherData.moduleData.exchangeLocationID;//"20180704_giewf";

            other.moduleData = om;
            return other;

        }

        public omdlContactData getContactData2(DataModel data)
        {
            omdlContactData contact = new omdlContactData();
            contact.moduleType = data.modules.contactData.moduleType;// "OMDL_CONTACT_DATA";

            ContactDataModules cm = new ContactDataModules();
            contactNameInfo c = new contactNameInfo();
            c.firstName = data.modules.contactData.moduleData.contactName.firstName ;//"Ming Ming";
            c.lastName = data.modules.contactData.moduleData.contactName.lastName;//"Chen";
            cm.contactName = c;

            contactTelInfo ct = new contactTelInfo();
            ct.haveTel = data.modules.contactData.moduleData.contactTel.haveTel;// true;
            ct.telCountryCode = data.modules.contactData.moduleData.contactTel.telCountryCode;//  "886";
            ct.telNumber = data.modules.contactData.moduleData.contactTel.telNumber;//  "0912344555";
            cm.contactTel = ct;

            contactAppInfo ca = new contactAppInfo();
            ca.haveApp = data.modules.contactData.moduleData.contactApp.haveApp;
            ca.appType = data.modules.contactData.moduleData.contactApp.appType ;//null;
            ca.appAccount = data.modules.contactData.moduleData.contactApp.appAccount;// null;
            cm.contactApp = ca;

            contact.moduleData = cm;
            return contact;

        }
    }
}
