using System;
using System.Collections.Generic;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S.Models.Module
{
    public class SCMPmdlModel : ScmBaseModel
    {
        public ScmPmdlJson json { get; set; }
    }

    public class ScmPmdlJson
    {
        public long supplierOid { get; set; }
        public Guid supplierUserUuid { get; set; }
        public string deviceId { get; set; }
        public string tokenKey { get; set; }
        public string moduleType { get; set; }
    }

    #region PMDL_CUST_DATA
    public class ScmCustDataJson : ScmPmdlJson
    {
        public CustData moduleSetting { get; set; }
    }
    public class CustData
    {
        public bool isRequired { get; set; }
        public CusSetting setting { get; set; }
    }
    public class CusSetting
    {
        public string customerDataType { get; set; } = "02";
        public CusDataItems dataItems { get; set; }
    }
    public class CusDataItems
    {
        public EnglishName englishName { get; set; }
        public Gender gender { get; set; }
        public Nationality nationality { get; set; }
        public Birthday birthday { get; set; }
        public PassportNo passportNo { get; set; }
        public LocalName localName { get; set; }
        public Height height { get; set; }
        public Weight weight { get; set; }
        public ShoeSize shoeSize { get; set; }
        public Meal meal { get; set; }
        public GlassDiopter glassDiopter { get; set; }
    }
    public class EnglishName
    {
        public bool isRequired { get; set; }
    }
    public class Gender
    {
        public bool isRequired { get; set; }
    }
    public class Nationality
    {
        public bool isRequired { get; set; }
        public nationalityOption options { get; set; }
    }
    public class TWID
    {
        public bool isRequired { get; set; }
    }
    public class HKMOID
    {
        public bool isRequired { get; set; }
    }
    public class MTP
    {
        public bool isRequired { get; set; }
    }
    public class Birthday
    {
        public bool isRequired { get; set; }
    }
    public class PassportNo
    {
        public bool isRequired { get; set; }
        public passportNoOption options { get; set; }
    }
    public class PassportRxpDate
    {
        public bool isRequired { get; set; }
    }
    public class LocalName
    {
        public bool isRequired { get; set; }
    }
    public class Height
    {
        public bool isRequired { get; set; }
        public string unit { get; set; } = "01";
        public string unitName { get; set; } = "cm";
    }
    public class Weight
    {
        public bool isRequired { get; set; }
        public string unit { get; set; } = "01";
        public string unitName { get; set; } = "kg";
    }
    public class ShoeSize
    {
        public bool isRequired { get; set; }
        public shoeSizeOption options { get; set; }
    }
    public class Meal
    {
        public bool isRequired { get; set; }
        public mealOption options { get; set; }
    }
    public class GlassDiopter
    {
        public bool isRequired { get; set; }
        public string diopterRangeStart { get; set; } = null;
        public string diopterRangeEnd { get; set; } = null;
    }
    public class nationalityOption
    {
        //nationality
        public TWID TWIdentityNumber { get; set; }
        public HKMOID HKMOIdentityNumber { get; set; }
        public MTP MTPNumber { get; set; }
    }
    public class passportNoOption
    {
        //passportNo
        public PassportRxpDate passportExpDate { get; set; }
    }
    public class shoeSizeOption
    {
        //shoeSize
        public SizeInfo man { get; set; }
        public SizeInfo woman { get; set; }
        public SizeInfo child { get; set; }
    }
    public class mealOption
    {
        //meal
        public List<MealInfo> meals { get; set; }
        public ExcludeFood excludeFood { get; set; }
    }
    public class ExcludeFood
    {
        public List<FoodInfo> foods { get; set; }
        public bool isExcluded { get; set; }
        public FoodAllergy foodAllergy { get; set; }
    }
    public class FoodInfo
    {
        public string foodType { get; set; }
        public bool canExclude { get; set; }
    }
    public class FoodAllergy
    {
        public bool canExclude { get; set; }
    }
    public class MealInfo
    {
        public string mealType { get; set; } 
        public bool isProvided { get; set; } 
    }
    public class SizeInfo
    {
        public string unit { get; set; } = "01";
        public bool isProvided { get; set; }
        public string sizeRangeStart { get; set; } = null;
        public string sizeRangeEnd { get; set; } = null;
    }

    #endregion

    #region PMDL_CUST_DATA
    public class ScmContactDataJson : ScmPmdlJson
    {
        public ContactData moduleSetting { get; set; }
    }
    public class ContactData
    {
        public bool isRequired { get; set; }
        public ContactSetting setting { get; set; }
    }
    public class ContactSetting
    {
        public ContactDataItems dataItems { get; set; }
    }
    public class ContactDataItems
    {
        public ContactName contactName { get; set; }
        public ContactTel contactTel { get; set; }
        public ContactApp contactApp { get; set; }
    }
    public class ContactName
    {
        public bool isRequired { get; set; }
    }

    public class ContactTel
    {
        public bool isRequired { get; set; }
    }
    public class ContactApp
    {
        public bool isRequired { get; set; }
        public List<AppInfo> apps { get; set; }
    }
    public class AppInfo
    {
        public string appType { get; set; }
        public bool isSupported { get; set; }
    }
    #endregion

    #region PMDL_VENUE
    public class ScmVenueJson : ScmPmdlJson
    {
        public VenueData moduleSetting { get; set; }
    }
    public class VenueData
    {
        public bool isRequired { get; set; }
        public VenueSetting setting { get; set; }
    }
    public class VenueSetting
    {
        public string venueType { get; set; } = "03";
        public VenueItems dataItems { get; set; }
    }
    public class VenueItems
    {
        public string meetingPointMap { get; set; } = null; //打SCM可以直接給 null module不完整
        public string meetingPointImage { get; set; } = null;//打SCM可以直接給 null module不完整
        public DesignatedLocation designatedLocation { get; set; }
        public DesignatedByCustomer designatedByCustomer { get; set; }
    }

    public class DesignatedLocation
    {
        public List<LocationInfo> locations { get; set; }
    }
    public class LocationInfo
    {
        public string id { get; set; }
        public string locationName { get; set; }
        public string locationAddress { get; set; }
        public string imageUrl { get; set; }
        public TimeRange timeRange { get; set; }
        public string sort { get; set; }
    }
    public class TimeRange
    {
        public Time from { get; set; }
        public Time to { get; set; }
    }
    public class Time
    {
        public string hour { get; set; }
        public string minute { get; set; }
    }



    public class DesignatedByCustomer
    {
        public PickUpLocation pickUpLocation { get; set; }
        public DropOffLocation dropOffLocation { get; set; }
    }
    public class PickUpLocation
    {
        public bool isRequired { get; set; }
        public VenuOption options { get; set; }
    }
    public class VenuOption
    {
        public PickUpTime pickUpTime { get; set; }
    }
    public class PickUpTime
    {
        public bool isRequired { get; set; }
        public List<string> times { get; set; }
        public CustomTime customTime { get; set; }
    }
    public class CustomTime
    {
        public bool isAllowCustom { get; set; }
        public TimeRange timeRange { get; set; }
    }
    public class DropOffLocation
    {
        public bool isRequired { get; set; }
    }

    #endregion

    #region 其他PMDL 在 Rezdy用不到(此PMDL Model內容不完整) moduleSetting isRequired=false

    public class ScmOtherDataJson : ScmPmdlJson
    {
        public OteherData moduleSetting { get; set; }
    }
    public class OteherData
    {
        public bool isRequired { get; set; } = false;
    }
    #endregion















}
