using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace KKday.Consoles.B2S.Rezdy.ProductRepository
{
    public class ProductRepository
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(ProductRepository));

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
