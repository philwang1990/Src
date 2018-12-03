using System;
using KKday.Consoles.B2S.Rezdy.ProductRepository;
using KKday.Consoles.B2S.Rezdy.PackageRepository;
using KKday.Consoles.B2S.Rezdy.ModuleRepository;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

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
                product.Main();

                //initial log4net
                KKday.Consoles.B2S.Rezdy.AppCode.CommonTool.LoadLog4netConfig();

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
