using System;
using log4net;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.Models.Package;
using Newtonsoft.Json;
using KKday.PMS.B2S.Models.Shared;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Product;
using KKday.PMS.B2S.Models.Shared.Enum;
using KKday.PMS.B2S.Models.Module;
using System.Linq;

namespace KKday.PMS.B2S.ModuleRepository
{
    public class ModuleRepository
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(ModuleRepository));

        //module type
        private static string _PMDL_CUST_DATA = "PMDL_CUST_DATA";
        private static string _PMDL_RENT_CAR = "PMDL_RENT_CAR";
        private static string _PMDL_CAR_PSGR = "PMDL_CAR_PSGR";
        private static string _PMDL_SEND_DATA = "PMDL_SEND_DATA";
        private static string _PMDL_SIM_WIFI = "PMDL_SIM_WIFI";
        private static string _PMDL_CONTACT_DATA = "PMDL_CONTACT_DATA";
        private static string _PMDL_FLIGHT_INFO = "PMDL_FLIGHT_INFO";
        private static string _PMDL_VENUE = "PMDL_VENUE";
        //private static string _PMDL_EXCHANGE = "PMDL_EXCHANGE";


        public void Main(SupplierLoginRSModel supplier, List<bookingFields> bookingField,long prodOid ,string prodCode,string dateInput)
        {
            try
            {
                //initial log4net
                CommonTool.LoadLog4netConfig();

                //url帶入
                Startup startup = new Startup();
                startup.Initial();

                //Rezdy Data
                var getPickup = CommonTool.GetData(string.Format(startup.GetParameter(PMSSourse.Rezdy, ParameterType.Pickups),
                                                                prodCode,
                                                                startup.GetParameter(PMSSourse.Rezdy, ParameterType.ApiKey)
                                                                ));

                RezdyPickupModel obj = JsonConvert.DeserializeObject<RezdyPickupModel>(getPickup);

                //打SCM Java Api
                //1.PMDL_CUST_DATA
                CustData cust_data = new CustData();
                CusSetting cusSetting = new CusSetting();
                cusSetting.customerDataType = "02";
                CusDataItems dataItems = new CusDataItems();

                EnglishName en = new EnglishName();
                en.isRequired = bookingField.Any(y => y.label == "First Name" && y.requiredPerParticipant == true);
                dataItems.englishName = en;

                Gender gn = new Gender();
                gn.isRequired = bookingField.Any(y => y.label == "Gender" && y.requiredPerParticipant == true); 
                dataItems.gender = gn;

                Nationality na = new Nationality();
                nationalityOption na_option = new nationalityOption();
                TWID tw = new TWID();
                tw.isRequired = false;
                na_option.TWIdentityNumber = tw;
                HKMOID hkmo = new HKMOID();
                hkmo.isRequired = false;
                na_option.HKMOIdentityNumber = hkmo;
                MTP mtp = new MTP();
                mtp.isRequired = false;
                na_option.MTPNumber = mtp;
                na.isRequired = bookingField.Any(y => y.label == "Country" && y.requiredPerParticipant == true); 
                na.options = na_option;
                dataItems.nationality = na;

                Birthday brd = new Birthday();
                brd.isRequired = bookingField.Any(y => y.label == "Date of birth" && y.requiredPerParticipant == true); 
                dataItems.birthday = brd;

                PassportNo pass = new PassportNo();
                pass.isRequired = false;
                passportNoOption pass_option = new passportNoOption();
                PassportRxpDate expdate = new PassportRxpDate();
                expdate.isRequired = false;
                pass_option.passportExpDate = expdate;
                pass.options = pass_option;
                dataItems.passportNo = pass;

                LocalName localName = new LocalName();
                localName.isRequired = false;
                dataItems.localName = localName;

                Height h = new Height();
                h.isRequired = false;
                dataItems.height = h;

                Weight w = new Weight();
                w.isRequired = false;
                dataItems.weight = w;

                ShoeSize shoe = new ShoeSize();
                shoe.isRequired = false;
                shoeSizeOption shoe_option = new shoeSizeOption();
                SizeInfo sizeInfo = new SizeInfo();
                sizeInfo.isProvided = false;
                shoe_option.man = sizeInfo;
                shoe_option.woman = sizeInfo;
                shoe_option.child = sizeInfo;
                shoe.options = shoe_option;
                dataItems.shoeSize = shoe;

                Models.Module.Meal me = new Models.Module.Meal();
                me.isRequired = false;
                mealOption meal_option = new mealOption();
                MealInfo mInfo = null;
                List<MealInfo> mealInfos = new List<MealInfo>();
                for (int i = 1; i <= 4; i++)
                {
                    mInfo = new MealInfo();
                    mInfo.isProvided = false;
                    mInfo.mealType = $"000{i}";
                    mealInfos.Add(mInfo);
                }
                meal_option.meals = mealInfos;
                ExcludeFood exclude = new ExcludeFood();
                FoodInfo fInfo = null;
                List<FoodInfo> foodInfos = new List<FoodInfo>();
                for (int i = 1; i <= 8; i++)
                {
                    fInfo = new FoodInfo();
                    fInfo.canExclude = false;
                    fInfo.foodType = $"000{i}";
                    foodInfos.Add(fInfo);
                }
                FoodAllergy allergy = new FoodAllergy();
                allergy.canExclude = false;
                exclude.foodAllergy = allergy;
                exclude.isExcluded = false;
                exclude.foods = foodInfos;
                meal_option.excludeFood = exclude;
                me.options = meal_option;
                dataItems.meal = me;

                GlassDiopter glass = new GlassDiopter();
                glass.isRequired = false;
                dataItems.glassDiopter = glass;
                cusSetting.dataItems = dataItems;

                cust_data.isRequired = true;
                cust_data.setting = cusSetting;

                SCMPmdlModel scmCustomModel = new SCMPmdlModel
                {
                    json = new ScmCustDataJson
                    {
                        supplierOid = supplier.supplierOid,
                        supplierUserUuid = supplier.supplierUserUuid,
                        deviceId = supplier.deviceId,
                        tokenKey = supplier.tokenKey,
                        moduleType = ModuleRepository._PMDL_CUST_DATA,
                        moduleSetting = cust_data
                    }
                };
                var custDataResult = CommonTool.GetDataPost(string.Format(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_custdata), prodOid), JsonConvert.SerializeObject(scmCustomModel));
                if (custDataResult["content"]["result"].ToString() != "0000")
                {
                    throw new Exception("create booking fields custom data fail.");
                }


                //2. PMDL_CUST_DATA
                ContactData contact_data = new ContactData();
                ContactSetting contactSetting = new ContactSetting();
                ContactDataItems ContactdataItems = new ContactDataItems();

                ContactName name = new ContactName();
                name.isRequired = false;
                ContactdataItems.contactName = name;

                ContactTel tel = new ContactTel();
                tel.isRequired = bookingField.Any(y => y.label == "Phone" && y.requiredPerBooking == true); 
                ContactdataItems.contactTel = tel;

                ContactApp app = new ContactApp();
                AppInfo aInfo = null;
                List<AppInfo> appInfos = new List<AppInfo>();
                for (int i = 1; i <= 8; i++)
                {
                    aInfo = new AppInfo();
                    aInfo.isSupported= false;
                    aInfo.appType = $"000{i}";
                    appInfos.Add(aInfo);
                }
                app.apps = appInfos;
                app.isRequired = false;
                ContactdataItems.contactApp = app;
                contactSetting.dataItems = ContactdataItems;

                contact_data.isRequired = (tel.isRequired || name.isRequired);
                contact_data.setting = contactSetting;

                SCMPmdlModel scmContactModel = new SCMPmdlModel
                {
                    json = new ScmContactDataJson
                    {
                        supplierOid = supplier.supplierOid,
                        supplierUserUuid = supplier.supplierUserUuid,
                        deviceId = supplier.deviceId,
                        tokenKey = supplier.tokenKey,
                        moduleType = ModuleRepository._PMDL_CONTACT_DATA,
                        moduleSetting = contact_data
                    }
                };
                var contactDataResult = CommonTool.GetDataPost(string.Format(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_contactdata), prodOid), JsonConvert.SerializeObject(scmContactModel));
                if (contactDataResult["content"]["result"].ToString() != "0000")
                {
                    throw new Exception("create booking fields contact data fail.");
                }

                //3. PMDL_VENUE
                if (obj.requestStatus.success && obj.pickupLocations.Count > 0)
                {
                    dateInput = "2018-12-13 00:00";
                    DateTime parsedDate = DateTime.Parse(dateInput);

                    VenueData venue_data = new VenueData();
                    VenueSetting venueSetting = new VenueSetting();
                    venueSetting.venueType = "03";
                    VenueItems VenuedataItems = new VenueItems();

                    DesignatedLocation location = new DesignatedLocation();

                    LocationInfo loInfo = null;
                    List<LocationInfo> loInfos = new List<LocationInfo>();
                    int sort = 0;
                    //塞入接送資訊
                    foreach (var pickup in obj.pickupLocations)
                    {
                        parsedDate = parsedDate.AddMinutes(-pickup.minutesPrior);

                        loInfo = new LocationInfo();
                        loInfo.id = $"{DateTime.Now.ToString("yyyyMMdd")}_{Guid.NewGuid().ToString("N").Substring(0, 5)}";
                        loInfo.imageUrl = null;
                        loInfo.sort = sort.ToString();
                        loInfo.locationName = pickup.locationName;
                        loInfo.locationAddress = pickup.address;
                        TimeRange lorange = new TimeRange();
                        Time lotime = new Time();
                        lotime.hour = parsedDate.Hour.ToString();
                        lotime.minute = parsedDate.Minute.ToString();
                        lorange.from = lotime;
                        lorange.to = lotime;
                        loInfo.timeRange = lorange;
                        loInfos.Add(loInfo);
                        sort++;

                    }
                    location.locations = loInfos;
                    VenuedataItems.designatedLocation = location;


                    DesignatedByCustomer customer = new DesignatedByCustomer();
                    PickUpLocation pickUp = new PickUpLocation();
                    VenuOption venu_option = new VenuOption();
                    CustomTime customTime = new CustomTime();
                    customTime.isAllowCustom = false;
                    TimeRange range = new TimeRange();
                    Time time = new Time();
                    time.hour = null;
                    time.minute = null;
                    range.from = time;
                    range.to = time;
                    customTime.timeRange = range;
                    PickUpTime upTime = new PickUpTime();
                    upTime.isRequired = false;
                    upTime.times = new List<string>{};
                    upTime.customTime = customTime;
                    venu_option.pickUpTime = upTime;
                    pickUp.isRequired = false;
                    pickUp.options = venu_option;
                    customer.pickUpLocation = pickUp;

                    DropOffLocation dropOff = new DropOffLocation();
                    dropOff.isRequired = false;
                    customer.dropOffLocation = dropOff;

                    VenuedataItems.designatedByCustomer = customer;
                    venueSetting.dataItems = VenuedataItems;

                    venue_data.isRequired = true;
                    venue_data.setting = venueSetting;

                    SCMPmdlModel scmVenueModel = new SCMPmdlModel
                    {
                        json = new ScmVenueJson
                        {
                            supplierOid = supplier.supplierOid,
                            supplierUserUuid = supplier.supplierUserUuid,
                            deviceId = supplier.deviceId,
                            tokenKey = supplier.tokenKey,
                            moduleType = ModuleRepository._PMDL_VENUE,
                            moduleSetting = venue_data
                        }
                    };

                    var venuDataResult = CommonTool.GetDataPost(string.Format(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_venue), prodOid), JsonConvert.SerializeObject(scmVenueModel));
                    if (venuDataResult["content"]["result"].ToString() != "0000")
                    {
                        throw new Exception("create booking fields venue data fail.");
                    }

                }


                //4. 其他
                OteherData oteher = new OteherData();
                oteher.isRequired = false;

                SCMPmdlModel scmOtherFlightModel = new SCMPmdlModel
                {
                    json = new ScmOtherDataJson
                    {
                        supplierOid = supplier.supplierOid,
                        supplierUserUuid = supplier.supplierUserUuid,
                        deviceId = supplier.deviceId,
                        tokenKey = supplier.tokenKey,
                        moduleType = ModuleRepository._PMDL_FLIGHT_INFO,
                        moduleSetting = oteher
                    }
                };
                var flightDataResult = CommonTool.GetDataPost(string.Format(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_flightinfo), prodOid), JsonConvert.SerializeObject(scmOtherFlightModel));
                if (flightDataResult["content"]["result"].ToString() != "0000")
                {
                    throw new Exception("create booking fields flightinfo data fail.");
                }

                SCMPmdlModel scmOtherCarPsgModel = new SCMPmdlModel
                {
                    json = new ScmOtherDataJson
                    {
                        supplierOid = supplier.supplierOid,
                        supplierUserUuid = supplier.supplierUserUuid,
                        deviceId = supplier.deviceId,
                        tokenKey = supplier.tokenKey,
                        moduleType = ModuleRepository._PMDL_CAR_PSGR,
                        moduleSetting = oteher
                    }
                };
                var carPsgDataResult = CommonTool.GetDataPost(string.Format(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_catpsgr), prodOid), JsonConvert.SerializeObject(scmOtherCarPsgModel));
                if (carPsgDataResult["content"]["result"].ToString() != "0000")
                {
                    throw new Exception("create booking fields carpsg data fail.");
                }

                SCMPmdlModel scmOtherRentModel = new SCMPmdlModel
                {
                    json = new ScmOtherDataJson
                    {
                        supplierOid = supplier.supplierOid,
                        supplierUserUuid = supplier.supplierUserUuid,
                        deviceId = supplier.deviceId,
                        tokenKey = supplier.tokenKey,
                        moduleType = ModuleRepository._PMDL_RENT_CAR,
                        moduleSetting = oteher
                    }
                };
                var rentDataResult = CommonTool.GetDataPost(string.Format(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_rentcar), prodOid), JsonConvert.SerializeObject(scmOtherRentModel));
                if (rentDataResult["content"]["result"].ToString() != "0000")
                {
                    throw new Exception("create booking fields rentcar data fail.");
                }

                SCMPmdlModel scmOtherSendModel = new SCMPmdlModel
                {
                    json = new ScmOtherDataJson
                    {
                        supplierOid = supplier.supplierOid,
                        supplierUserUuid = supplier.supplierUserUuid,
                        deviceId = supplier.deviceId,
                        tokenKey = supplier.tokenKey,
                        moduleType = ModuleRepository._PMDL_SEND_DATA,
                        moduleSetting = oteher
                    }
                };
                var sendDataResult = CommonTool.GetDataPost(string.Format(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_senddata), prodOid), JsonConvert.SerializeObject(scmOtherSendModel));
                if (sendDataResult["content"]["result"].ToString() != "0000")
                {
                    throw new Exception("create booking fields send data fail.");
                }
                SCMPmdlModel scmOtherSimWifiModel = new SCMPmdlModel
                {
                    json = new ScmOtherDataJson
                    {
                        supplierOid = supplier.supplierOid,
                        supplierUserUuid = supplier.supplierUserUuid,
                        deviceId = supplier.deviceId,
                        tokenKey = supplier.tokenKey,
                        moduleType = ModuleRepository._PMDL_SIM_WIFI,
                        moduleSetting = oteher
                    }
                };
                var simwifiDataResult = CommonTool.GetDataPost(string.Format(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_simwifi), prodOid), JsonConvert.SerializeObject(scmOtherSimWifiModel));
                if (simwifiDataResult["content"]["result"].ToString() != "0000")
                {
                    throw new Exception("create booking fields simwifi data fail.");
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
            }
        }
    }
}
