using System;
using KKday.PMS.B2S.ProductRepository;
using KKday.PMS.B2S.PackageRepository;
using KKday.PMS.B2S.ModuleRepository;
using log4net;
using KKday.PMS.B2S.AppCode;
using KKday.PMS.B2S.Models.Shared;

namespace KKday.PMS.B2S
{
    class Program
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(Program));
        private static ProductRepository.ProductRepository product = new ProductRepository.ProductRepository();
        private static PackageRepository.PackageRepository package = new PackageRepository.PackageRepository();
        private static ModuleRepository.ModuleRepository module = new ModuleRepository.ModuleRepository();

        static void Main(string[] args)
        {
            try
            {
                //待補
                long prodOid = 20442;
                SupplierLoginRSModel supplierLoginRSModel = new SupplierLoginRSModel();
                //initial log4net
                CommonTool.LoadLog4netConfig();

                var accounts = new[] { new { id = 1, name = "thisisname" } };
                var suppliers = new[] { new { id = 1, suppliername = "rezdy" } };

                foreach (var account in accounts)
                {
                    foreach (var supplier in suppliers)
                    {
                        //prodOid = 0;
                        long supplierId = 807;

                        //待補
                        //"supplierOid":807,
                        //"supplierUserUuid":"4c529bc6-af3c-47c4-986c-eef30cdaa1f0",
                        //"deviceId":"11b501a87f4cf456f271e27395eb924b",
                        //"tokenKey":"02991db50e2ee8d7e4ae87be81f5ebc7"

                        // 設定參數
                        supplierLoginRSModel = product.setParameters("AAT Kings Tours", "op-llh@kkday.com", "123456");

                        //建立商品
                        product.New(supplierLoginRSModel, ref prodOid);

                        //商品明細
                        //product.Main();

                        //套餐
                        package.Main(
                        Models.Shared.Enum.PMSSourse.Rezdy,
                        prodOid,
                        supplierId,
                        "PVVRFE",
                        Guid.Parse("4c529bc6-af3c-47c4-986c-eef30cdaa1f0"),
                        "11b501a87f4cf456f271e27395eb924b",
                        "02991db50e2ee8d7e4ae87be81f5ebc7");

                        //旅規
                        //module.Main();
                    }
                }

                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                //error log
                _log.Error(ex.ToString());
            }
        }
    }
}
