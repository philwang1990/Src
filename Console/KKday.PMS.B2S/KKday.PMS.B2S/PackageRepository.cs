using System;
using System.Linq;
using System.Collections.Generic;
using log4net;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.Models.Package;
using KKday.PMS.B2S.Models.Package.SCMPackageModel;
using KKday.PMS.B2S.Models.Package.SCMPackageCalendarModel;
using KKday.PMS.B2S.Models.Package.SCMPackagePriceModel;
using Newtonsoft.Json;
using ObjectsComparer;
using KKday.PMS.B2S.Models.Shared.Enum;

namespace KKday.PMS.B2S.PackageRepository
{
    public class PackageRepository
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(PackageRepository));

        public void Main(PMSSourse pms, long prodOid, long supplierId, string productCode, Guid supplierUserUuid, string deviceId, string tokenKey)
        {
            try
            {
                //initial log4net
                CommonTool.LoadLog4netConfig();

                DateTime startDate = DateTime.Parse("2018-11-01 00:00:00");
                DateTime endDate = DateTime.Parse("2019-12-31 00:00:00");

                var get = CommonTool.GetDataNew("https://api.rezdy.com/latest/availability?" +
                    "apiKey=0b3d137cc1db4108a92c309fa7d7f6da&" +
                    $"productCode={productCode}&" +
                    $"startTimeLocal={startDate.ToString("yyyy-MM-dd")} 00:00:00&" +
                    $"endTimeLocal={endDate.ToString("yyyy-MM-dd")} 00:00:00");

                var rezdyPackageModel = RezdyPackageModel.FromJson(get.Result);

                if (rezdyPackageModel != null && rezdyPackageModel.RequestStatus.Success && rezdyPackageModel.Sessions != null && rezdyPackageModel.Sessions.Any())
                {
                    //start mapping
                    int index = 0;
                    foreach (var item in rezdyPackageModel.Sessions)
                    {
                        if (index > 0) break;
                        long packageOid = 0;
                        //create package
                        #region Create Package

                        ScmPackageModel scmPackageModel = new ScmPackageModel
                        {
                            Json = new Models.Package.SCMPackageModel.Json
                            {
                                SupplierOid = supplierId,
                                SupplierUserUuid = supplierUserUuid,
                                DeviceId = deviceId,
                                TokenKey = tokenKey,
                                PackageNm = $"{item.ProductCode}_{item.Id}", //待確認
                                PackageDesc = new PackageDesc
                                {
                                    DescItems = new List<DescItem> { new DescItem { Content = new List<Content> { new Content {
                                         Id = item.Id.ToString(), //待確認
                                         Desc = item.Id.ToString() //待確認
                                     } } } }
                                }
                            }
                        };

                        //Post
                        var newPackageResult = CommonTool.GetDataPost($"https://api.sit.kkday.com/api/product/updatePkg/{prodOid}", JsonConvert.SerializeObject(scmPackageModel));
                        if (newPackageResult["content"]["result"].ToString() != "0000")
                        {
                            throw new Exception("create package fail.");
                        }

                        packageOid = Convert.ToInt64(newPackageResult["content"]["packageOid"].ToString());
                        #endregion

                        //create calendar
                        #region Create Calendar
                        Newtonsoft.Json.Linq.JObject calendarInitialResult = null;
                        if (packageOid != 0)
                        {
                            ScmPackageCalendarModel scmPackageCalendarModel = new ScmPackageCalendarModel
                            {
                                Json = new Models.Package.SCMPackageCalendarModel.Json
                                {
                                    SupplierOid = supplierId,
                                    SupplierUserUuid = supplierUserUuid,
                                    DeviceId = deviceId,
                                    TokenKey = tokenKey,
                                    ProdOid = prodOid,
                                    PackageOid = packageOid,
                                    StartDt = Convert.ToInt64(startDate.ToString("yyyyMMdd")), //待確認
                                    EndDt = Convert.ToInt64(endDate.ToString("yyyyMMdd")), //待確認
                                    WeekDays = "1,2,3,4,5,6" //待確認
                                }
                            };
                            //Post
                            calendarInitialResult = CommonTool.GetDataPost($"https://api.sit.kkday.com/api/1.0/pkg/cal/extend", JsonConvert.SerializeObject(scmPackageCalendarModel));
                        }
                        #endregion

                        #region Creata Price
                        if (packageOid != 0 && calendarInitialResult["content"]["result"].ToString() == "0000" && item.PriceOptions != null && item.PriceOptions.Any())
                        {
                            KKday.PMS.B2S.Models.Package.SCMPackagePriceModel.Json price = new KKday.PMS.B2S.Models.Package.SCMPackagePriceModel.Json
                            {
                                SupplierOid = supplierId,
                                SupplierUserUuid = supplierUserUuid,
                                DeviceId = deviceId,
                                TokenKey = tokenKey,
                                ProdOid = prodOid,
                                PackageOid = packageOid,
                                PriceType = "RANK", //待確認
                                CostCalcMethod = "NET", //待確認
                                ProdCurrCd = "AUD", //待確認
                                MinOrderQty = 1,
                                MinOrderAdultQty = 0
                            };

                            foreach (var priceOption in item.PriceOptions)
                            {
                                var priceCondition = PassengerMapping(PMSSourse.Rezdy, priceOption.Label);

                                if (priceCondition == "price1")
                                {
                                    price.Price1BegOld = 12;
                                    price.Price1EndOld = 99;
                                    price.Price1NetOrg = priceOption.Price;
                                }
                                else if (priceCondition == "price2")
                                {
                                    price.Price2BegOld = 1;
                                    price.Price2EndOld = 11;
                                    price.Price2NetOrg = priceOption.Price;
                                }
                                else if (priceCondition == "price3")
                                {
                                    //price.Price3BegOld = priceOption.Price + 10;
                                    //price.Price3EndOld = priceOption.Price + 10;
                                    price.Price3NetOrg = priceOption.Price;
                                }
                                else //price4
                                {
                                    //price.Price4BegOld = priceOption.Price + 10;
                                    //price.Price4EndOld = priceOption.Price + 10;
                                    price.Price4NetOrg = priceOption.Price;
                                }
                            }

                            SCMPackagePriceModel scmPackagePriceModel = new SCMPackagePriceModel
                            {
                                Json = price
                            };

                            //Post
                            CommonTool.GetDataPost("https://api.sit.kkday.com/api/1.0/pkg/price/update", JsonConvert.SerializeObject(scmPackagePriceModel));
                        }
                        #endregion

                        index++;
                    }
                }

                #region Model Comparison
                //ObjectsComparer.IComparer<RezdyPackageModel> _comparer = new ObjectsComparer.Comparer<RezdyPackageModel>(
                //new ComparisonSettings
                //{
                //    //Null and empty error lists are equal
                //    EmptyAndNullEnumerablesEqual = true
                //});

                ////Do not compare ProductCode
                //_comparer.AddComparerOverride(() => new Session().ProductCode, DoNotCompareValueComparer.Instance);

                //var isEqual = _comparer.Compare(obj1, obj2, out var differences);

                //if (!isEqual)
                //{
                //    if (differences.Any())
                //    {
                //        foreach (Difference difference in differences)
                //        {
                //            Console.WriteLine(difference.MemberPath);
                //            Console.WriteLine($"{difference.Value1}->{difference.Value2}");
                //        }
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
            }
        }

        private string PassengerMapping(PMSSourse pms, string passengerType)
        {
            try
            {
                switch (pms)
                {
                    case PMSSourse.Rezdy:
                        switch (passengerType)
                        {
                            case "Adult":
                                return "price1";
                            case "Child":
                                return "price2";
                            case "Infant":
                                return "price3";
                            case "Concession":
                                return "price4";
                            default:
                                return "";
                        }
                    default:
                        return "";
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
                throw ex;
            }
        }
    }
}
