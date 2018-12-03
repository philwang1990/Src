using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace KKday.Consoles.B2S.Rezdy.AppCode
{
    public class CommonTool
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(Program));

        public static void LoadLog4netConfig()
        {
            var repository = LogManager.CreateRepository(
                                Assembly.GetEntryAssembly(),
                                typeof(log4net.Repository.Hierarchy.Hierarchy)
                            );
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

            //_log.Info("Application Start ------");
        }
    }
}
