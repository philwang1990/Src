using System;
using System.Collections.Generic;
using KKday.Web.B2D.EC.Models.Model.Booking;

namespace KKday.Web.B2D.EC.Models.Repostory.Booking
{
    public static class DataSettingRepostory
    {
        public static DataModel fakeDataModel(DataModel data)
        {
            data.productOid = "20159";
            data.packageOid = "77568";
            data.contactFirstname = "飛兒";
            data.contactLastname = "張";
            data.contactEmail = "phil651105@gmail.com";
            data.telCountryCd = "886";
            data.contactTel = "0939650299";
            data.contactCountryCd = "TW";
            data.lstGoDt = "20181025";
            data.eventBackupData = "";
            data.price1Qty = 1;
            data.price2Qty = 1;
            data.price3Qty = 0;
            data.price4Qty = 0;
            data.payMethod = "ONLINE_CITI";
            data.guideLang = "zh-tw";
            data.note = "test訂單備註";
            data.hasRank = "Y";
            data.productUrlOid = 20159;
            data.productName = "浪漫水金九 （正）";
            data.productCity = new string[] { "A01-001-00006" };
            data.productCountry = "A01-001";
            data.productMainCat = "M01";
            data.productOrderHandler = "KKDAY";
            data.payPmchOid = "1";
            data.allowedCardNumberArray = new string[]{};
            data.alsoUpdateMember = true;
            data.currency = "TWD";
            data.currPriceTotal = 210;
            data.crtDevice = "Macintosh";
            data.crtBrowser = "Safari";
            data.crtBrowserVersion = "12.0";
            data.memberUuid = "de619322-9810-4bd4-aea9-1dd211974da9";
            data.riskStatus = "02";
            data.tokenKey = "15785a3e81944707e1012f1741be2e1a";
            data.deviceId = "f5dd4ce1b24548adf178aa066b67f93f";
            data.multipricePlatform = "01";
            data.sourceCode = "WEB";
            data.sourceParam1 = "";

            data.couponUuid = "";
            data.priceCoupon = 0;
            data.priceCouponUSD = 0;

            data.travelerData = null;
            List<CusDataInfo> cus = new List<CusDataInfo>();
            cus.Add(getCus());
            cus.Add(getCus2());
            data.travelerData = cus;

            //data =chkModules(data);


            return data;
        }

        public static CusDataInfo getCus()
        {
            CusDataInfo cus = new CusDataInfo();
            cus.isSave = true;
            cus.englishName = new englishNameInfo() { firstName = "phil", lastName = "chang" };

            cus.gender = "M";
            cus.nationality = new nationalityInfo()
            {
                nationalityCode = null,
                TWIdentityNumber = null,
                HKMOIdentityNumber = null,
                MTPNumber = null
            };

            cus.birthday = null;// "1976-01-01";
            cus.passport = new passportInfo()
            {
                passportNo = null,
                passportExpDate = null
            };

            cus.localName = new localNameInfo()
            {
                firstName = null,
                lastName = null
            };
            cus.height = new heightInfo() { unit = null, value = null };
            cus.weight = new weightInfo() { unit = null, value = null };
            cus.shoeSize = new shoeSizeInfo() { type = null, unit = null, value = null };
            string[] ex = new string[1]; ex[0] = null;// ex[1] = "0002";
            cus.meal = new mealInfo()
            {
                mealType = null,
                excludeFoodType = ex,
                foodAllergy = new foodAllergyInfo { allergenList = null, isFoodAllergy = false }
            };
            cus.glassDiopter = null;
            cus.isSave = true;
            cus.friendOid = null;
            return cus;
        }

        public static CusDataInfo getCus2()
        {
            CusDataInfo cus = new CusDataInfo();
            cus.isSave = true;
            cus.englishName = new englishNameInfo() { firstName = "nini", lastName = "chang" };

            cus.gender = "F";
            cus.nationality = new nationalityInfo()
            {
                nationalityCode = null,
                TWIdentityNumber = null,
                HKMOIdentityNumber = null,
                MTPNumber = null
            };

            cus.birthday = null;// "1976-01-01";
            cus.passport = new passportInfo()
            {
                passportNo = null,
                passportExpDate = null
            };

            cus.localName = new localNameInfo()
            {
                firstName = null,
                lastName = null
            };
            cus.height = new heightInfo() { unit = null, value = null };
            cus.weight = new weightInfo() { unit = null, value = null };

            cus.shoeSize = new shoeSizeInfo() { type = null, unit = null, value = null };

            string[] ex = new string[1]; ex[0] = null;// ex[1] = "0002";
            cus.meal = new mealInfo()
            {
                mealType = null,
                excludeFoodType = ex,
                foodAllergy = new foodAllergyInfo { allergenList = null, isFoodAllergy = false }
            };
            cus.glassDiopter = null;
            cus.isSave = true;
            cus.friendOid = null;
            return cus;
        }


        public static DataModel chkModules(DataModel data)
        {
            data.modules.shuttleData.moduleData.shuttleDate = "2018-10-25";
            data.modules.shuttleData.moduleData.designatedLocation.locationID = "20181004_9hcgx";

            data.modules.carRentingData.moduleData.pickUp.datetime.date = "2018-10-25";

            data.modules.passengerData.moduleData.qtyAdult = 1;
            data.modules.passengerData.moduleData.qtyChild = 1;
            data.modules.passengerData.moduleData.qtyInfant = 0;
            data.modules.passengerData.moduleData.qtyChildSeat.supplierProvided = 0;
            data.modules.passengerData.moduleData.qtyChildSeat.selfProvided = 0;

            data.modules.passengerData.moduleData.qtyInfantSeat.supplierProvided = 0;
            data.modules.passengerData.moduleData.qtyInfantSeat.selfProvided = 0;
            data.modules.passengerData.moduleData.qtyCarryLuggage = 0;
            data.modules.passengerData.moduleData.qtyCheckedLuggage = 0;

            data.modules.sendData.moduleData.receiverTel.telCountryCode = "886";
            data.modules.sendData.moduleData.receiverTel.telNumber = "0939650299";

            data.modules.contactData.moduleData.contactTel.telCountryCode = "886";

            return data;
        }

        //假聯絡人
        //public static distributorInfo fakeContact()
        //{

        //    distributorInfo fake = new distributorInfo()
        //    {
        //        companyXid = "1",
        //        channelOid = "111",
        //        userid = "2",
        //        firstName = "sharon",
        //        lastName = "chang",
        //        areatel = "886",
        //        tel = "3939889",
        //        email = "bid@kkday.com",
        //        countryCd = "TW",
        //        lang = "zh-tw",
        //        currency = "TWD",
        //        state="TW",
        //        memberUuid = "051794b8-db2a-4fe7-939f-31ab1ee2c719",
        //        tokenKey = "897af29c45ed180451c2e6bfa81333b6",
        //        deviceId = "3c2ab71448224d1d7148350f7972e96e"
        //    };

        //    return fake;
        //}

        //假國攷
        //public static List<Country> fakeCountry()
        //{
        //    Country c1 = new Country() { countryCd = "TW", countryName = "台灣", countryEngName = "TAIWAN" };
        //    Country c2 = new Country() { countryCd = "HK", countryName = "香港", countryEngName = "HONG KONG" };
        //    Country c3 = new Country() { countryCd = "MO", countryName = "澳門", countryEngName = "MO" };
        //    Country c4 = new Country() { countryCd = "CN", countryName = "中國", countryEngName = "CN" };
        //    Country c5 = new Country() { countryCd = "US", countryName = "美國", countryEngName = "US" };

        //    List<Country> lstCountry = new List<Country>();
        //    lstCountry.Add(c1);
        //    lstCountry.Add(c2);
        //    lstCountry.Add(c3);
        //    lstCountry.Add(c4);
        //    lstCountry.Add(c5);

        //    return lstCountry;
        //}

        //單純的目的是產出前台可以使用的object string
        public static DataModel getDefaultDataModel(int qty,string guidNo) 
        {

            DataModel d = new DataModel();

            d.guidNo = guidNo;

            modulesData modules = new modulesData();

            otherDataM other = new otherDataM();
            other.moduleType = "OMDL_OTHER_DATA";
            moduleData_otherData mo = new moduleData_otherData();
            other.moduleData = mo;
            modules.otherData = other;

            contactDataM contact = new contactDataM();
            contact.moduleType = "OMDL_CONTACT_DATA";
            moduleData_contactData mcontract = new moduleData_contactData();
            contactNameInfo cni = new contactNameInfo();
            contactTelInfo cti = new contactTelInfo();
            contactAppInfo cai = new contactAppInfo();
            mcontract.contactName = cni;
            mcontract.contactTel = cti;
            mcontract.contactApp = cai;
            contact.moduleData = mcontract;


            modules.contactData = contact;
             
            sendDataM send = new sendDataM();
            send.moduleType = "OMDL_SEND_DATA";
            moduleData_sendData msend = new moduleData_sendData();
            receiverNameInfo receiverName = new receiverNameInfo();
            receiverTelInfo receiverTel = new receiverTelInfo();
            sendToCountryInfo sendToCountry = new sendToCountryInfo();

            receiveAddressInfo receiveAdd = new receiveAddressInfo();
            sendToCountry.receiveAddress = receiveAdd;

            sendToHotelInfo sendToHotel = new sendToHotelInfo();
            buyerPassportEnglishNameInfo buyerPassportEnglishName = new buyerPassportEnglishNameInfo();
            buyerLocalNameInfo buyerLocalName = new buyerLocalNameInfo();
            sendToHotel.buyerPassportEnglishName = buyerPassportEnglishName;
            sendToHotel.buyerLocalName = buyerLocalName;
            shipInfoInfo shipInfo = new shipInfoInfo();
            msend.receiverName = receiverName;
            msend.receiverTel = receiverTel;
            msend.sendToCountry = sendToCountry;
            msend.sendToHotel = sendToHotel;
            msend.shipInfo = shipInfo;
            send.moduleData = msend;
            modules.sendData = send;

            passengerDataM passenger = new passengerDataM();
            passenger.moduleType = "OMDL_PSGR_DATA";
            moduleData_passenger mp = new moduleData_passenger();
            qtyChildSeatInfo qtyChildSeat = new qtyChildSeatInfo();
            qtyInfantSeatInfo qtyInfantSeat = new qtyInfantSeatInfo();
            mp.qtyChildSeat = qtyChildSeat;
            mp.qtyInfantSeat = qtyInfantSeat;
            passenger.moduleData = mp;
            modules.passengerData = passenger;

            carRentingDataM carRentingData = new carRentingDataM();
            carRentingData.moduleType = "OMDL_RENT_CAR";
            moduleData_CarRent cm = new moduleData_CarRent();
            pickUpInfo_forCar pickUp = new pickUpInfo_forCar();

            dateTimeInfo t = new dateTimeInfo();
            pickUp.datetime = t;

            dropOffInfo_forCar dropOff = new dropOffInfo_forCar();
            dropOff.datetime = t;
            cm.pickUp = pickUp;
            cm.dropOff = dropOff;
            carRentingData.moduleData = cm;
            modules.carRentingData = carRentingData;

            shuttleDataM shuttle = new shuttleDataM();
            shuttle.moduleType = "OMDL_SHUTTLE";
            moduleData_Shuttle ms = new moduleData_Shuttle();
            designatedLocationInfo designatedLocation = new designatedLocationInfo();
            timeInfo time = new timeInfo();
            pickUpInfo pp = new pickUpInfo();
            pp.time = time;
            dropOffInfo dd = new dropOffInfo();
            designatedByCustomerInfo designatedByCustomer = new designatedByCustomerInfo();
            designatedByCustomer.pickUp = pp;
            designatedByCustomer.dropOff = dd;
            charterRouteInfo charterRoute = new charterRouteInfo();
            ms.designatedLocation = designatedLocation;
            ms.designatedByCustomer = designatedByCustomer;
            ms.charterRoute = charterRoute;
            shuttle.moduleData = ms;
            modules.shuttleData = shuttle;

            flightInfoDataM flight = new flightInfoDataM();
            flight.moduleType = "OMDL_FLIGHT_INFO";
            moduleData_FlightInfo mf = new moduleData_FlightInfo();
            arrivalInfo arrival = new arrivalInfo();

            arrivalDatetimeInfo atime = new arrivalDatetimeInfo();
            arrival.arrivalDatetime = atime;
            departureInfo departure = new departureInfo();
            departureDatetimeInfo dtime = new departureDatetimeInfo();
            departure.departureDatetime = dtime;

            mf.arrival = arrival;
            mf.departure = departure;
            flight.moduleData = mf;
            modules.flightInfoData = flight;

            d.modules = modules;


            List<CusDataInfo> LstCus= new List<CusDataInfo>();

            for (int i = 0; i < qty;i++)
            {
                CusDataInfo cus = new CusDataInfo();

                nationalityInfo nation = new nationalityInfo();
                cus.nationality = nation;
                passportInfo ppt = new passportInfo();
                cus.passport = ppt;

                englishNameInfo en = new englishNameInfo();
                cus.englishName = en;
                localNameInfo localName = new localNameInfo();
                cus.localName = localName;
                weightInfo weight = new weightInfo();
                cus.weight = weight;
                heightInfo height = new heightInfo();
                cus.height = height;
                shoeSizeInfo shoes = new shoeSizeInfo();
                cus.shoeSize = shoes;

                foodAllergyInfo food = new foodAllergyInfo();
                mealInfo meal = new mealInfo();
                meal.foodAllergy = food;
                meal.excludeFoodType = new string[] { };

                cus.meal = meal;
                LstCus.Add(cus);
            }
            d.travelerData = LstCus;

            cardInfo card = new cardInfo();
            d.card = card;

            return d;

        }
    }
}
