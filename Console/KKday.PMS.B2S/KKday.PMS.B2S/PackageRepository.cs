using System;
using System.Linq;
using System.Collections.Generic;
using log4net;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.Models.Package;
using Newtonsoft.Json;
using ObjectsComparer;

namespace KKday.PMS.B2S.PackageRepository
{
    public class PackageRepository
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(PackageRepository));

        public void Main()
        {
            try
            {
                //initial log4net
                CommonTool.LoadLog4netConfig();

                var get = CommonTool.GetData("https://api.rezdy.com/latest/availability?apiKey=0b3d137cc1db4108a92c309fa7d7f6da&productCode=PVVRFE&startTimeLocal=2018-11-01 00:00:00&endTimeLocal=2019-12-31 00:00:00");
                //RezdyPackageModel

                var obj1 = RezdyPackageModel.FromJson(get);
                var obj2 = RezdyPackageModel.FromJson(get);

                obj2.Sessions[10].ProductCode = "dadad";

                obj2.Sessions[17].Id = 123;
                obj2.Sessions[17].PriceOptions[0].Price = 123;

                #region Model Comparison
                ObjectsComparer.IComparer<RezdyPackageModel> _comparer = new ObjectsComparer.Comparer<RezdyPackageModel>(
                new ComparisonSettings
                {
                    //Null and empty error lists are equal
                    EmptyAndNullEnumerablesEqual = true
                });

                //Do not compare ProductCode
                _comparer.AddComparerOverride(() => new Session().ProductCode, DoNotCompareValueComparer.Instance);

                var isEqual = _comparer.Compare(obj1, obj2, out var differences);

                if (!isEqual)
                {
                    if (differences.Any())
                    {
                        foreach (Difference difference in differences)
                        {
                            Console.WriteLine(difference.MemberPath);
                            Console.WriteLine($"{difference.Value1}->{difference.Value2}");
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
            }
        }


    }
}
