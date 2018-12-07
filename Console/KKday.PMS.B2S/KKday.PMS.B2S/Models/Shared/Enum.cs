using System;

namespace KKday.PMS.B2S.Models.Shared.Enum
{
    public enum PMSSourse
    {
        Rezdy,
        KKday,
    }

    public enum Step
    {
        //基本設定
        BasicSetting = 0,

        //商品分類
        ProductCategory = 1,

        //上架時間設定
        OnlineTime = 2,

        //憑證設定
        VoucherSetting = 3,

        //行程說明
        TourDescription = 4,

        //照片與影片
        Photograph = 5,

        //行程表
        Timetable = 6,

        //集合地點
        MettingPoint = 7,

        //費用包含細節
        PriceDetail = 8,

        //兌換方式
        Redeem = 9,

        //取消費用設定
        CancellationFee = 10,

        //套餐
        ProductOption = 11,

        //旅客資料
        PassengerInfo = 12,

        //旅客聯絡資料
        PassengerContact = 13
    }

    public enum ParameterType
    {
        KKdayApi_updatepkg,
        KKdayApi_calendarextend,
        KKdayApi_calendarmodify,
        KKdayApi_priceupdate,
        KKdayApi_voucherupdate,
        ApiKey,
        Product,
        Pickups,
        Availability
    }
}
