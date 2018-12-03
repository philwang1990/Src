using System;
using log4net;
using log4net.Config;

namespace KKday.Consoles.B2S.Rezdy.ModuleRepository
{
    public class ModuleRepository
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(ModuleRepository));

        public void Main()
        {
            try
            {
                //initial log4net
                KKday.Consoles.B2S.Rezdy.AppCode.CommonTool.LoadLog4netConfig();

                //do something
            }
            catch (Exception ex)
            {
                _log.Debug(ex.ToString());
            }
        }
    }
}
