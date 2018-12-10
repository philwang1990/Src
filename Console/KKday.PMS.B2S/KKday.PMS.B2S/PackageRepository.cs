using System;
using System.Linq;
using System.Collections.Generic;
using log4net;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.Models.Package;
using KKday.PMS.B2S.Models.Package.SCMPackageModel;
using KKday.PMS.B2S.Models.Package.SCMPackageCalendarModel;
using KKday.PMS.B2S.Models.Package.SCMPackageCalendarModifyModel;
using KKday.PMS.B2S.Models.Package.SCMPackagePriceModel;
using Newtonsoft.Json;
using ObjectsComparer;
using KKday.PMS.B2S.Models.Shared.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace KKday.PMS.B2S
{
    public class PackageRepository
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(PackageRepository));

        public void Main(PMSSourse pms, long prodOid, long supplierId, string productCode, Guid supplierUserUuid, string deviceId, string tokenKey)
        {
            try
            {
                _log.Info("PackageRepository start..");

                Startup startup = new Startup();
                startup.Initial();

                DateTime startDate = DateTime.Now; //待確認
                DateTime endDate = DateTime.Now.AddYears(3); //待確認

                var get = CommonTool.GetDataNew(string.Format(startup.GetParameter(pms, ParameterType.Availability),
                                                startup.GetParameter(pms, ParameterType.ApiKey),
                                                productCode,
                                                $"{startDate.ToString("yyyy-MM-dd")} 00:00:00",
                                                $"{endDate.ToString("yyyy-MM-dd")} 23:59:00"));

                Console.WriteLine("GetAvailabilityData..");
                //_log.Info(get.Result);

                var rezdyPackageModel = RezdyPackageModel.FromJson(get.Result);

                if (rezdyPackageModel != null && rezdyPackageModel.RequestStatus.Success && rezdyPackageModel.Sessions != null && rezdyPackageModel.Sessions.Any())
                {
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
                            PackageNm = $"{rezdyPackageModel.Sessions.First().ProductCode}", //待確認
                            PackageDesc = new PackageDesc
                            {
                                DescItems = new List<DescItem> { new DescItem { Content = new List<Content> { new Content {
                                         Id = rezdyPackageModel.Sessions.First().Id.ToString(), //待確認
                                         Desc = rezdyPackageModel.Sessions.First().Id.ToString() //待確認
                                     } } } }
                            }
                        }
                    };

                    //Post
                    var newPackageResult = CommonTool.GetDataPost(string.Format(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_updatepkg), prodOid), JsonConvert.SerializeObject(scmPackageModel));
                    if (newPackageResult["content"]["result"].ToString() != "0000")
                    {
                        throw new Exception("create package fail.");
                    }

                    packageOid = Convert.ToInt64(newPackageResult["content"]["packageOid"].ToString());
                    #endregion

                    Newtonsoft.Json.Linq.JObject calendarInitialResult = null;

                    #region Create Calendar
                    if (packageOid != 0)
                    {
                        var availableDate = rezdyPackageModel.Sessions.Select(x => x.StartTimeLocal).OrderBy(date => date).ToList();

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
                                StartDt = Convert.ToInt64(availableDate.Min().ToString("yyyyMMdd")), //待確認
                                EndDt = Convert.ToInt64(availableDate.Max().ToString("yyyyMMdd")), //待確認
                                WeekDays = "1,2,3,4,5,6,7" //待確認
                            }
                        };

                        //test missing date
                        availableDate.RemoveAt(10);
                        availableDate.RemoveAt(11);
                        availableDate.RemoveAt(12);
                        availableDate.RemoveAt(24);
                        availableDate.RemoveAt(65);

                        var missingDates = new List<DateTimeOffset>();
                        for (int i = 0; i + 1 < availableDate.Count; i++)
                        {
                            var diff = (availableDate[i + 1] - availableDate[i]).TotalDays;
                            for (int iToComplete = 1; iToComplete < diff; iToComplete++)
                            {
                                missingDates.Add(availableDate[i].AddDays(iToComplete));
                            }
                        }


                        //Post
                        calendarInitialResult = CommonTool.GetDataPost(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_calendarextend), JsonConvert.SerializeObject(scmPackageCalendarModel));


                        foreach (var missingDate in missingDates)
                        {
                            ScmPackageCalendarModifyModel scmPackageCalendarModifyModel = new ScmPackageCalendarModifyModel
                            {
                                Json = new Models.Package.SCMPackageCalendarModifyModel.Json
                                {
                                    SupplierOid = supplierId,
                                    SupplierUserUuid = supplierUserUuid,
                                    DeviceId = deviceId,
                                    TokenKey = tokenKey,
                                    ProdOid = prodOid,
                                    PackageOid = packageOid,
                                    GoDt = Convert.ToInt64(missingDate.ToString("yyyyMMdd")),
                                    IsUse = "N"
                                }
                            };
                            //Post
                            calendarInitialResult = CommonTool.GetDataPost(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_calendarmodify), JsonConvert.SerializeObject(scmPackageCalendarModifyModel));
                        }
                    }
                    #endregion

                    #region Creata Price
                    if (packageOid != 0 && calendarInitialResult["content"]["result"].ToString() == "0000" && 
                    rezdyPackageModel.Sessions.First().PriceOptions != null && rezdyPackageModel.Sessions.First().PriceOptions.Any()) //待確認
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

                        foreach (var priceOption in rezdyPackageModel.Sessions.First().PriceOptions) //待確認
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
                            //else if (priceCondition == "price3")
                            //{
                            //    //price.Price3BegOld = priceOption.Price + 10;
                            //    //price.Price3EndOld = priceOption.Price + 10;
                            //    price.Price3NetOrg = priceOption.Price;
                            //}
                            //else //price4
                            //{
                            //    //price.Price4BegOld = priceOption.Price + 10;
                            //    //price.Price4EndOld = priceOption.Price + 10;
                            //    price.Price4NetOrg = priceOption.Price;
                            //}
                        }

                        SCMPackagePriceModel scmPackagePriceModel = new SCMPackagePriceModel
                        {
                            Json = price
                        };

                        //Post
                        CommonTool.GetDataPost(startup.GetParameter(PMSSourse.KKday, ParameterType.KKdayApi_priceupdate), JsonConvert.SerializeObject(scmPackagePriceModel));
                    }
                    #endregion
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
