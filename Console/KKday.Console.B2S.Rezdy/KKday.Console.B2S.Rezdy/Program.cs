using System;
using KKday.Consoles.B2S.Rezdy.ProductRepository;
using KKday.Consoles.B2S.Rezdy.PackageRepository;
using KKday.Consoles.B2S.Rezdy.ModuleRepository;
using log4net;
using KKday.Consoles.B2S.Rezdy.AppCode;

namespace KKday.Consoles.B2S.Rezdy
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
                //initial log4net
                CommonTool.LoadLog4netConfig();

                var accounts = new[] { new { id = 1, name = "thisisname" } };
                var suppliers = new[] { new { id = 1, suppliername = "rezdy" } };

                foreach (var account in accounts)
                {
                    foreach (var supplier in suppliers)
                    {
                        //商品明細
                        product.Main();

                        //套餐
                        package.Main();

                        //旅規
                        module.Main();
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
